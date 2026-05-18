using BLL.Interfaces.Manager.Login;
using BLL.Utility;
using BOL.Models;
using DAL.Data;
using DAL.Implementation.Repository.Login;
using EF.Core.Repository.Manager;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;

namespace DAL.Implementation.Manager.Login
{
    public class loginManager : CommonManager<User_DbModel>,ILoginManager
    {
        private readonly Dg_Common _dgCommon;
        private readonly SqlConnection _dgParollCon;
        private readonly SqlConnection _dgSpecFo;
        private readonly string _key;
        public loginManager(dg_hrpayrollContext context, Dg_Common dgCommon, IConfiguration config) : base(new LoginRepository(context))
        {
            _dgCommon = dgCommon;
            _dgParollCon = new SqlConnection(Getway.Dg_Payroll);
            _dgSpecFo = new SqlConnection(Getway.SpecFoCon);
            _key = config["DgEncryptKey:Key"];
        }

        public async Task<User_DbModel> GetMasterUserTableInfo(string userID,string password)
        {
            string passEnct = _dgCommon.EncryptText(_key, password);
            //var dtUser = await _dgCommon.get_InformationDataTableAsync("dg_pay_GetMasterUserTableInfo '" + userID + "','" + password + "'", _dgParollCon);
            var dtUser = await _dgCommon.get_InformationDataTableAsync("dg_pay_GetMasterUserTableInfo_2 '"+ userID + "','"+ passEnct + "'", _dgParollCon);
            var result = _dgCommon.GetSingleListObject<User_DbModel>(dtUser);
            return result;
        }
        public async Task<bool> UpdateUserPassBatch()
        {
            bool flag = false;
            var dt = await _dgCommon.get_InformationDataTableAsync("select nUserID,cUserName,cPassWord from Smt_Users", _dgSpecFo);
            foreach (DataRow row in dt.Rows)
            {
                int nUserID = int.Parse(row["nUserID"].ToString());
                string pass = row["cPassWord"].ToString();
                string nPass = _dgCommon.EncryptText(_key, pass);
                flag = await _dgCommon.saveChangesAsync($"update Smt_Users set cPassWord_enct='{nPass}' where nUserID={nUserID}", _dgSpecFo);
            }
            return flag;
        }
        public async Task<string> SaveUserPasswordChange(int compID, string loginID, string confPassword, string newPassword)
        {
            string result = string.Empty;
            try
            {
                if (confPassword == newPassword)
                {
                    //bool isSave = await _dgCommon.saveChangesAsync("update Tbl_User set [Password]='"+ newPassword + "' where CompId="+ compID + " and RTRIM(FullName)='" + loginID + "'", _dgParollCon);
                    string passEnct = _dgCommon.EncryptText(_key, newPassword);
                    bool isSave = await _dgCommon.saveChangesAsync("dg_pay_ChangeMasterUserPassword " + compID + ",'" + loginID + "','" + newPassword + "','" + passEnct + "'", _dgParollCon);
                    if (isSave)
                    {
                        result = "Password Change Successfully !!";
                    }
                    else
                    {
                        result = "Password Not Change !!";
                    }
                }
                else
                {
                    result = "Password Mismatch !!";
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                result = "Something went wrong !!";
            }
            return result;
        }
        public async Task<ReturnObject> SendPasswordResetCode(UserPasswordResetPayload obj)
        {
            var result = new ReturnObject();
            var dt = await _dgCommon.get_InformationDataTableAsync($"select Email from Smt_Users where cUserName='{obj.userId}'", _dgSpecFo);
            if (dt.Rows.Count > 0)
            {
                string userEmailId = !string.IsNullOrEmpty(dt.Rows[0]["Email"].ToString()) ? dt.Rows[0]["Email"].ToString() : string.Empty;
                if (string.IsNullOrEmpty(userEmailId.Trim()))
                {
                    result.Message = "Email Id Not Found,Contact With Administrator !!";
                }
                else
                {
                    string ramadomCode = Generate6DigitCode();
                    await SendEmail(userEmailId.Trim(), ramadomCode.Trim());
                    await _dgCommon.saveChangesAsync($"insert into Smt_EmailOtp(em_userId,em_Code,em_ExpiryTime) values('{obj.userId}','{ramadomCode.Trim()}','{DateTime.Now.AddSeconds(60)}')", _dgSpecFo);
                    result.IsSuccess = true;
                    result.Message = $"Please Check, Code Send Your Email({userEmailId}) !!";
                }
            }
            else
            {
                result.Message = "User Id Not Vaild !!";
            }
            return result;
        }
        public async Task<ReturnObject> CheckResetCodeValidity(string userId, string vCode)
        {
            var result = new ReturnObject();
            var dt = await _dgCommon.get_InformationDataTableAsync($"select top 1 nCompanyID,em_Id,em_Code,em_ExpiryTime from Smt_EmailOtp inner join Smt_Users on em_userId=cUserName where em_userId='{userId}' and em_Code='{vCode}' and em_IsUsed=0 order by em_ExpiryTime desc", _dgSpecFo);
            if (dt.Rows.Count > 0)
            {
                int vId = int.Parse(dt.Rows[0]["em_Id"].ToString());
                int compid = int.Parse(dt.Rows[0]["nCompanyID"].ToString());
                string Code = dt.Rows[0]["em_Code"].ToString();
                DateTime expDate = DateTime.Parse(dt.Rows[0]["em_ExpiryTime"].ToString());
                if (expDate < DateTime.Now)
                {
                    result.Message = "Verification Code Expired !!";
                }
                else
                {
                    if (vCode == Code)
                    {
                        await _dgCommon.saveChangesAsync($"update Smt_EmailOtp set em_IsUsed=1 where em_Id={vId}", _dgSpecFo);
                        result.IsSuccess = true;
                        result.Message = "Verification Successful !!";
                        result.dataTable = new {compID = compid };
                    }
                    else
                    {
                        result.Message = "Verification Code Not Match !!";
                    }
                }
                return result;
            }
            result.Message = "Verification Code Not Valid !!";
            return result;
        }
        public async Task<object> CheckUserEmailSet(UserPasswordResetPayload obj)
        {
            var dt = await _dgCommon.get_InformationDataTableAsync($"select Email from Smt_Users where cUserName='{obj.userId}' and (Email is not null or Email<>'')", _dgSpecFo);
            var result = new
            {
                isSuccess = dt.Rows.Count > 0 && !string.IsNullOrEmpty(dt.Rows[0]["Email"].ToString()),
                emailID = dt.Rows.Count > 0 && !string.IsNullOrEmpty(dt.Rows[0]["Email"].ToString()) ? dt.Rows[0]["Email"].ToString() : string.Empty,
                statusCode = 200
            };
            return result;
        }
        public async Task<ReturnObject> SetupUserEmailId(UserEmailSetPayload obj)
        {
            var result = new ReturnObject();
            if (string.IsNullOrEmpty(obj.userId.Trim()))
            {
                result.Message = "Please Enter User Id !!";
            }
            else if (string.IsNullOrEmpty(obj.emailId.Trim()))
            {
                result.Message = "Please Enter Email Id !!";
            }
            else
            {
                bool isUpdate = await _dgCommon.saveChangesAsync($"update Smt_Users set Email='{obj.emailId}' where cUserName='{obj.userId}'", _dgSpecFo);
                if (isUpdate)
                {
                    result.IsSuccess = true;
                    result.Message = "Email Setup Successfully !!";
                }
                else
                {
                    result.Message = "Email Setup Fail,Try Again !!";
                }
            }
            return result;
        }

        //For Check Function
        public async Task<List<object>> GetPermitedMenuList2(List<MenuList2> obj)
        {
            var lstMenu = new List<object>();           
            var dt = await _dgCommon.get_InformationDataTableAsync("select Permission_Status,nUgroup from Smt_Users where cUserName='" + obj[0].userName.Trim() + "'", _dgSpecFo);
            if (dt.Rows.Count > 0)
            {
                int nUgroup = int.Parse(dt.Rows[0]["nUgroup"].ToString().Trim());
                if (nUgroup != 1)
                {
                    string x = dt.Rows[0]["Permission_status"].ToString().Trim();
                    for (int i = 0; i < obj.Count; i++)
                    {
                        var dtMenuChk = new DataTable();
                        string frmName = obj[i].menuText.Trim();
                        string menuUrl = obj[i].menuUrl.Trim();
                        string userName = obj[i].userName.Trim();
                        if (x == "U")
                            dtMenuChk = await _dgCommon.get_InformationDataTableAsync("select Form_Name from Smt_UserPermittedform where User_ID='" + userName + "' and Permission_Status='" + x + "' and Form_Name='" + frmName + "' and Url='" + menuUrl + "'", _dgSpecFo);
                        else
                            dtMenuChk = await _dgCommon.get_InformationDataTableAsync("select Form_Name from Smt_UserPermittedform where nUgroup=" + nUgroup + " and Form_Name='" + frmName + "' and Url='" + menuUrl + "'", _dgSpecFo);
                        if (dtMenuChk.Rows.Count < 1)
                        {
                            var dtSepCount = x == "U" ? await _dgCommon.get_InformationDataTableAsync("select count(pSep_name) as pSep_name from Smt_UserPermittedform where User_ID='" + userName + "' and Permission_Status='" + x + "' and pSep_name='" + obj[i].separatorName + "'", _dgSpecFo)
                                : await _dgCommon.get_InformationDataTableAsync("select count(pSep_name) as pSep_name from Smt_UserPermittedform where nUgroup=" + nUgroup + " and pSep_name='" + obj[i].separatorName + "'", _dgSpecFo);
                            var LiID = new
                            {
                                mainMenuName = obj[i].mainMenuName,
                                MenuText = frmName,
                                MenuUrl = menuUrl,
                                separatorName = obj[i].separatorName,
                                isShowseparator = int.Parse(dtSepCount.Rows[0]["pSep_name"].ToString()) > 0,
                            };
                            lstMenu.Add(LiID);
                        }
                    }
                }               
            }            
            return lstMenu;
        }
        public async Task<List<object>> GetPermitedMenuList(List<MenuList> obj)
        {
            var lstMenu = new List<object>();           
            var dt = await _dgCommon.get_InformationDataTableAsync("select Permission_Status,nUgroup from Smt_Users where cUserName='" + obj[0].userName + "'", _dgSpecFo);
            if (dt.Rows.Count > 0)
            {
                int nUgroup = int.Parse(dt.Rows[0]["nUgroup"].ToString().Trim());
                if (nUgroup != 1)
                {
                    string x = dt.Rows[0]["Permission_status"].ToString().Trim();
                    for (int i = 0; i < obj.Count; i++)
                    {
                        var dtMenuChk = new DataTable();
                        string frmName = obj[i].menuText.Trim();
                        string userName = obj[i].userName.Trim();
                        if (x == "U")
                            dtMenuChk = await _dgCommon.get_InformationDataTableAsync("select Form_Name from Smt_UserPermittedform where User_ID='" + userName + "' and Permission_Status='" + x + "' and Form_Name='" + frmName + "'", _dgSpecFo);
                        else
                            dtMenuChk = await _dgCommon.get_InformationDataTableAsync("select Form_Name from Smt_UserPermittedform where nUgroup=" + nUgroup + " and Form_Name='" + frmName + "'", _dgSpecFo);
                        if (dtMenuChk.Rows.Count < 1)
                        {
                            var LiID = new
                            {
                                MenuText = frmName,
                            };
                            lstMenu.Add(LiID);
                        }
                    }
                }
            }
            return lstMenu;
        }
        public async Task<List<object>> SetBtnPermission(List<ButtonList> obj)
        {
            List<object> lst = new List<object>();
            if (obj.Count > 0)
            {
                for (int i = 0; i < obj.Count; i++)
                {
                    string btnName = obj[i].buttonName;
                    string controller = obj[i].pageName;
                    var dtbtn = await _dgCommon.get_InformationDataTableAsync("select ButtonName from tst_permitterbtn where UserName='" + obj[0].userName + "' and FormName='" + controller + "' and ButtonName='" + btnName + "'", _dgSpecFo);
                    if (dtbtn.Rows.Count > 0)
                    {
                        var btnList = new ButtonList
                        {
                            isShow = true,
                            buttonName = obj[i].buttonName
                        };
                        lst.Add(btnList);
                    }
                    else
                    {
                        var btnList = new ButtonList
                        {
                            isShow = false,
                            buttonName = obj[i].buttonName
                        };
                        lst.Add(btnList);
                    }
                }
            }
            return lst;
        }

        private async Task SendEmail(string toEmail, string vfCode)
        {
            var smtp = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("debonairinfosys@gmail.com", "sfmr xvpf aeie lxaf"),
                EnableSsl = true
            };
            var mail = new MailMessage
            {
                From = new MailAddress("debonairinfosys@gmail.com"),
                Subject = "ERP Login Password Reset",
                Body = $"Verification Code: <b>{vfCode}</b>",
                IsBodyHtml = true
            };
            mail.To.Add(toEmail);
            await smtp.SendMailAsync(mail);
        }
        private string Generate6DigitCode()
        {
            byte[] bytes = new byte[4];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }
            int value = BitConverter.ToInt32(bytes, 0) & 0x7fffffff;
            int code = value % 1000000;

            return code.ToString("D6");
        }
    }
}
