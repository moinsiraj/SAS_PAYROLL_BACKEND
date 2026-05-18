using BLL.Interfaces.Manager.Employees;
using BLL.Utility;
using BOL.Models;
using DAL.Data;
using DAL.Implementation.Repository.Employees;
using EF.Core.Repository.Manager;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Reporting.NETCore;
using QRCoder;
using SharpCompress.Archives;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO.Compression;

namespace DAL.Implementation.Manager.Employees
{
    public class EmployeesManager : CommonManager<Employee_DbModel>, IEmployeesManager
    {
        private readonly Dg_Common _dgCommon;
        private readonly SqlConnection _connection;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public EmployeesManager(dg_hrpayrollContext context, Dg_Common dgCommon, IWebHostEnvironment webHostEnvironment) : base(new EmployeesRepository(context))
        {
            _dgCommon = dgCommon;
            _connection = new SqlConnection(Getway.Dg_Payroll);
            _webHostEnvironment = webHostEnvironment;
        }

        public void QR_Generator(string[] QrInfo, string[] FilePath)
        {
            string filepath = FilePath[0];
            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }
            string fullPath = string.Concat(FilePath);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
            string InfoFull = string.Concat(QrInfo);
            QRCodeGenerator QrGenerator = new QRCodeGenerator();
            QRCodeData QrCodeInfo = QrGenerator.CreateQrCode(InfoFull, QRCodeGenerator.ECCLevel.Q);
            QRCode QrCode = new QRCode(QrCodeInfo);
            var bmp = QrCode.GetGraphic(60);
            bmp.Save(fullPath, ImageFormat.Jpeg);
        }
        public async Task<string[]> Employee_active_status(int Emp_serial, int Active_status, string active_Date, string Inactive_Date, string in_active_Resion, string userName)
        {
            var data = await _dgCommon.get_InformationDataTableAsync("Active_status " + Emp_serial + "," + Active_status + ",'" + active_Date + "','" + Inactive_Date + "','" + in_active_Resion + "','" + userName + "'", _connection);
            string[] result = new string[] { data.Rows[0]["msgType"].ToString(), data.Rows[0]["msg"].ToString() };
            return result;
        }
        public async Task<ReturnObject> GetActiveInactiveHistory(int empSerial)
        {
            var result = new ReturnObject();
            try
            {
                var data = await _dgCommon.get_InformationDataTableAsync("Dg_active_inactive_history " + empSerial, _connection);
                if (data.Rows.Count > 0)
                {
                    result.IsSuccess = true;
                    result.Message = "Data Loaded !!";
                    result.dataTable = data;
                }
                else
                {
                    result.Message = "Data Not Loaded !!";
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                result.Message = "Something Went Wrong !!";
            }
            return result;
        }
        public async Task<DataSet> textfile(int nempSerial, int Emp_ID, int ngroupid, int ncompid, DateTime nDate, string ntime)
        {
            var data = await _dgCommon.get_InformationDtasetAsync("dg_pay_Att_Insert_Textfile " + nempSerial + "," + Emp_ID + "," + ngroupid + "," + ncompid + ",'" + nDate + "','" + ntime + "'", _connection);
            return data;
        }
        
        public async Task<DataSet> employee_ShiftChange(int? Compid = null, int? Department = null, int? section = null, int?
            Building = null, int? Floor = null, int? Line = null, int? Shift = null, int? Grade = null, int?
            salcat = null, int? Newshift = null, DateTime? EffectDate = null, string user = null)
        {
            var data = await _dgCommon.get_InformationDtasetAsync("Emp_filtering_shiftchange " + Compid + "," + Department + "," + section + "," + Building + "," + Floor + "," + Line + "," + Shift + "," + Grade + "," + salcat + "," + Newshift + ",'" + EffectDate + "','" + user + "'", _connection);
            return data;
        }
        public async Task<DataSet> get()
        {
            var data = await _dgCommon.get_InformationDtasetAsync("textfile_datetime_position", _connection);
            return data;
        }
        public async Task<DataSet> employee_Info(int emp_serial)
        {
            var data = await _dgCommon.get_InformationDtasetAsync("Emp_filtering_individual " + emp_serial, _connection);
            return data;
        }
        public async Task<DataSet> employee_Info()
        {
            var data = await _dgCommon.get_InformationDtasetAsync("Id_card_bangla", _connection);
            return data;
        }
        public async Task<DataSet> GetCompanyName()
        {
            var data = await _dgCommon.get_InformationDtasetAsync("D_company", _connection);
            return data;
        }
        public async Task<DataSet> GetdivisionName()
        {
            var data = await _dgCommon.get_InformationDtasetAsync("D_Division", _connection);
            return data;
        }
        public async Task<DataSet> GetDistict(int di_id)
        {
            var data = await _dgCommon.get_InformationDtasetAsync("D_district " + di_id, _connection);
            return data;
        }
        public async Task<DataSet> GetThana(int th_id)
        {
            var data = await _dgCommon.get_InformationDtasetAsync("D_Thana " + th_id, _connection);
            return data;
        }
        public async Task<DataSet> Getpostoffice(int ThanaID)
        {
            var data = await _dgCommon.get_InformationDtasetAsync("D_Postoffice " + ThanaID, _connection);
            return data;
        }
        public async Task<DataSet> Getvillage(int thanaID)
        {
            var data = await _dgCommon.get_InformationDtasetAsync("D_village " + thanaID, _connection);
            return data;
        }
        public async Task<DataSet> Companywiseemployee(int compid)
        {
            var data = await _dgCommon.get_InformationDtasetAsync("D_employee_companywise " + compid, _connection);
            return data;
        }
        public async Task<DataSet> Employee_ID(int compid)
        {
            var data = await _dgCommon.get_InformationDtasetAsync("D_employee_id_companywise " + compid, _connection);
            return data;
        }
        public async Task<DataSet> singleEmployee(int comId, int EmpId)
        {
            var data = await _dgCommon.get_InformationDtasetAsync("D_employee_companywise_and_Idwise " + comId + "," + EmpId + "", _connection);
            return data;
        }
        public async Task<DataSet> formeternity(int compid)
        {
            var data = await _dgCommon.get_InformationDtasetAsync("D_employee_id_companywise_formeternity " + compid, _connection);
            return data;
        }
        public async Task<DataSet> Proxmity_ID(int compid)
        {
            var data = await _dgCommon.get_InformationDtasetAsync("D_employee_proxid_companywise " + compid, _connection);
            return data;
        }
        public async Task<DataSet> GetDesignation()
        {
            var data = await _dgCommon.get_InformationDtasetAsync("D_Designation", _connection);
            return data;
        }
        public async Task<DataSet> Department(int compid)
        {
            var data = await _dgCommon.get_InformationDtasetAsync("D_Departmennt " + compid, _connection);
            return data;
        }
        public async Task<DataSet> GetSection(int Department)
        {
            var data = await _dgCommon.get_InformationDtasetAsync("D_Section " + Department, _connection);
            return data;
        }
        public async Task<DataSet> Getbuilding(int compid)
        {
            var data = await _dgCommon.get_InformationDtasetAsync("D_Building " + compid, _connection);
            return data;
        }
        public async Task<DataSet> GetFloor(int compid, int Building)
        {
            var data = await _dgCommon.get_InformationDtasetAsync("D_Floor " + compid + "," + Building + "", _connection);
            return data;
        }
        public async Task<DataSet> GetLine(int compid, int Building, int Floor)
        {
            var data = await _dgCommon.get_InformationDtasetAsync("D_Line " + compid + "," + Building + "," + Floor + "", _connection);
            return data;
        }
        public async Task<DataSet> Getsalarycategory(int compID)
        {
            var data = await _dgCommon.get_InformationDtasetAsync("D_salary_category " + compID, _connection);
            return data;
        }
        public async Task<DataSet> GetShift(int compid)
        {
            var data = await _dgCommon.get_InformationDtasetAsync("D_Shift " + compid, _connection);
            return data;
        }
        public async Task<DataSet> GetGrade()
        {
            var data = await _dgCommon.get_InformationDtasetAsync("D_Grade", _connection);
            return data;
        }
        public async Task<DataSet> GetBank()
        {
            var data = await _dgCommon.get_InformationDtasetAsync("D_Bank", _connection);
            return data;
        }
        public async Task<DataTable> GetCompanyAddress(int compID)
        {
            var data = await _dgCommon.get_InformationDataTableAsync("select com_name,com_ad1 from dg_pay_company where com_id=" + compID, _connection);
            return data;
        }


        private async Task<bool> DeleteOldReportFilterData(string TableName)
        {
            bool flag = false;
            try
            {
                await _connection.OpenAsync();
                SqlCommand cmd = new SqlCommand("truncate table " + TableName + "", _connection);
                cmd.CommandType = CommandType.Text;
                await cmd.ExecuteNonQueryAsync();
                flag = true;
            }
            catch (Exception ex)
            {
                ex.ToString();
                flag = false;
            }
            finally
            {
                await _connection.CloseAsync();
            }
            return flag;
        }
        public async Task<bool> EmpLeaveGenerate(DgPayEmployee obj)
        {
            bool flag = false;
            try
            {
                SqlCommand cmd = new SqlCommand("Auto_casual_medical_leavegenarate", _connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@lev_compid", obj.Compid);
                cmd.Parameters.AddWithValue("@emp_no", obj.EmpNo);
                await _connection.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                flag = true;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            finally
            {
                await _connection.CloseAsync();
            }
            return flag;
        }

        //New Version
        public async Task<bool> UploadEmpInfoImage(IFormFile fileEmp, IFormFile fileNominee, IFormFile fileSign, IFormFile fileEmpNid, IFormFile fileNomineeNid, string compid, int empid)
        {
            bool flag = true;
            try
            {
                if (fileEmp != null)
                {
                    string filepathEmp = $"{_webHostEnvironment.WebRootPath}\\EmployeeImage\\" + compid + "\\";
                    if (!Directory.Exists(filepathEmp))
                    {
                        Directory.CreateDirectory(filepathEmp);
                    }
                    string[] fileArr = new string[] { filepathEmp, null, null };
                    fileArr[1] = empid.ToString();
                    fileArr[2] = ".jpg";
                    string fileFull = string.Concat(fileArr);
                    if (File.Exists(fileFull))
                    {
                        File.Delete(fileFull);
                    }
                    using (var stream = new FileStream(fileFull, FileMode.Create))
                    {
                        await fileEmp.CopyToAsync(stream);
                        flag = true;
                    }
                    //using var image = Image.Load(fileEmp.OpenReadStream());
                    //image.Mutate(x => x.Resize(300, 300));
                    //await image.SaveAsync(fileFull);
                }
                if (fileNominee != null)
                {
                    string filepathNominee = $"{_webHostEnvironment.WebRootPath}\\NomineeImage\\" + compid + "\\";
                    if (!Directory.Exists(filepathNominee))
                    {
                        Directory.CreateDirectory(filepathNominee);
                    }
                    string[] fileArr = new string[] { filepathNominee, null, null };
                    fileArr[1] = empid.ToString();
                    fileArr[2] = ".jpg";
                    string fileFull = string.Concat(fileArr);
                    if (File.Exists(fileFull))
                    {
                        File.Delete(fileFull);
                    }
                    using (var stream = new FileStream(fileFull, FileMode.Create))
                    {
                        await fileNominee.CopyToAsync(stream);
                        flag = true;
                    }
                }
                if (fileSign != null)
                {
                    string filepathSign = $"{_webHostEnvironment.WebRootPath}\\EmployeeSignature\\" + compid + "\\";
                    if (!Directory.Exists(filepathSign))
                    {
                        Directory.CreateDirectory(filepathSign);
                    }
                    string[] fileArr = new string[] { filepathSign, null, null };
                    fileArr[1] = empid.ToString();
                    fileArr[2] = ".png";
                    string fileFull = string.Concat(fileArr);
                    if (File.Exists(fileFull))
                    {
                        File.Delete(fileFull);
                    }
                    using (var stream = new FileStream(fileFull, FileMode.Create))
                    {
                        await fileSign.CopyToAsync(stream);
                        flag = true;
                    }
                }
                if (fileEmpNid != null)
                {
                    string filepathEmp = $"{_webHostEnvironment.WebRootPath}\\Emp_Nid\\" + compid + "\\";
                    if (!Directory.Exists(filepathEmp))
                    {
                        Directory.CreateDirectory(filepathEmp);
                    }
                    string[] fileArr = new string[] { filepathEmp, null, null };
                    fileArr[1] = empid.ToString();
                    fileArr[2] = ".pdf";
                    string fileFull = string.Concat(fileArr);
                    if (File.Exists(fileFull))
                    {
                        File.Delete(fileFull);
                    }
                    using (var stream = new FileStream(fileFull, FileMode.Create))
                    {
                        await fileEmpNid.CopyToAsync(stream);
                        flag = true;
                    }
                }
                if (fileNomineeNid != null)
                {
                    string filepathNominee = $"{_webHostEnvironment.WebRootPath}\\NomineeNid\\" + compid + "\\";
                    if (!Directory.Exists(filepathNominee))
                    {
                        Directory.CreateDirectory(filepathNominee);
                    }
                    string[] fileArr = new string[] { filepathNominee, null, null };
                    fileArr[1] = empid.ToString();
                    fileArr[2] = ".pdf";
                    string fileFull = string.Concat(fileArr);
                    if (File.Exists(fileFull))
                    {
                        File.Delete(fileFull);
                    }
                    using (var stream = new FileStream(fileFull, FileMode.Create))
                    {
                        await fileNomineeNid.CopyToAsync(stream);
                        flag = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                flag = false;
            }
            return flag;
        }
        public ReturnObject viewEmpDocFile(int compid, int empid, string vFrom)
        {
            var result = new ReturnObject();
            if (vFrom == "empNID")
            {
                string filepathEmp = $"{_webHostEnvironment.WebRootPath}\\Emp_Nid\\" + compid + "\\" + empid + ".pdf";
                if (!File.Exists(filepathEmp))
                {
                    result.Message = "File Not found !!";
                    return result;
                }
                var fileBytes = File.ReadAllBytes(filepathEmp);
                result.IsSuccess = true;
                result.dataTable = fileBytes;
                return result;
            }
            else if (vFrom == "nomineeNID")
            {
                string filepathEmp = $"{_webHostEnvironment.WebRootPath}\\NomineeNid\\" + compid + "\\" + empid + ".pdf";
                if (!File.Exists(filepathEmp))
                {
                    result.Message = "File Not found !!";
                    return result;
                }
                var fileBytes = File.ReadAllBytes(filepathEmp);
                result.IsSuccess = true;
                result.dataTable = fileBytes;
                return result;
            }
            else
            {
                result.Message = "View Type Not Found !!";

                return result;
            }
        }
        public async Task<ReturnObject> UploadEmployeeImageFolder(IFormFile zipOrRarFile, int compid, string imgType)
        {
            var result = new ReturnObject();
            if (zipOrRarFile != null)
            {
                string fileExtension = Path.GetExtension(zipOrRarFile.FileName).ToLower().Trim();
                if (fileExtension == ".zip" || fileExtension == ".rar")
                {
                    string destPath = string.Empty;
                    if (imgType == "EmployeeImage")
                    {
                        destPath = $"{_webHostEnvironment.WebRootPath}\\EmployeeImage\\" + compid + "\\";
                        var empImgRes = await this.UploadImageZipOrRar(zipOrRarFile, compid, 300, 300, destPath, imgType);
                        if (empImgRes.IsSuccess)
                        {
                            result.IsSuccess = empImgRes.IsSuccess;
                            result.Message = empImgRes.Message;
                            return result;
                        }
                        result.Message = empImgRes.Message;
                        return result;
                    }
                    else if (imgType == "EmployeeSignature")
                    {
                        destPath = $"{_webHostEnvironment.WebRootPath}\\EmployeeSignature\\" + compid + "\\";
                        var empImgRes = await this.UploadImageZipOrRar(zipOrRarFile, compid, 250, 70, destPath, imgType);
                        if (empImgRes.IsSuccess)
                        {
                            result.IsSuccess = empImgRes.IsSuccess;
                            result.Message = empImgRes.Message;
                            return result;
                        }
                        result.Message = empImgRes.Message;
                        return result;
                    }
                }
                result.Message = "File Format Not Valid Input Zip Or Rar File !!";
                return result;
            }
            result.Message = "Please Upload File First !!";
            return result;
        }
        /*public async Task<string> AddOrEditEmpPersonalInfo(Employee_Personal_Info obj)
        {
            string result = string.Empty;
            if (obj.emp_serial == 0)
            {
                bool isExists = this.EmployeeNoExists(int.Parse(obj.compid.ToString()), int.Parse(obj.emp_no.ToString()));
                if (!isExists)
                {
                    var dtChkProxid = _dgCommon.get_InformationDataTable(string.Format("select rtrim(emp_proxid) as emp_proxid from dg_pay_Employee where compid={0} and emp_proxid='{1}' group by emp_proxid", obj.compid, obj.emp_proxid.Trim()), _connection);
                    if (dtChkProxid.Rows.Count > 0)
                    {
                        return result = "Employee Proxid Already Exists !!";
                    }
                    
                    //if (!string.IsNullOrEmpty(obj.pi_nic) && obj.pi_nic !="0")
                    //{
                    //    var dtNID = _dgCommon.get_InformationDataTable(string.Format("select rtrim(pi_nic) as pi_nic from dg_pay_Employee where pi_nic='{0}' and (oi_transfer=0 or oi_transfer is null) group by pi_nic", obj.pi_nic.Trim()), _connection);
                    //    if (dtNID.Rows.Count > 0)
                    //    {
                    //        return result = "Employee NID Number Already Exists !!";
                    //    }                       
                    //}                    
                    //if (!string.IsNullOrEmpty(obj.pi_birth_certificate_no) && obj.pi_birth_certificate_no !="0")
                    //{
                    //    var dtBCertificate = _dgCommon.get_InformationDataTable(string.Format("select rtrim(pi_birth_certificate_no) as pi_birth_certificate_no from dg_pay_Employee where pi_birth_certificate_no='{0}' and (oi_transfer=0 or oi_transfer is null) group by pi_nic", obj.pi_birth_certificate_no.Trim()), _connection);
                    //    if (dtBCertificate.Rows.Count > 0)
                    //    {
                    //        return result = "Employee Birth Certificate Already Exists !!";
                    //    }                       
                    //}
                    bool isSave = await _dgCommon.saveChangesAsync("dg_Pay_InsertOrUpdate_EmpPersonalInfo", _connection, obj);
                    return result = isSave ? "Save Successfully !!" : "Something is wrong,Data not save !!";                   
                }
                return result = "Employee numner(" + obj.emp_no + ") already exists !!";
            }
            else
            {
                var dtEmpNo = _dgCommon.get_InformationDataTable("select emp_no from dg_pay_Employee where emp_serial=" + obj.emp_serial, _connection);
                int empNo = dtEmpNo.Rows.Count > 0 ? int.Parse(dtEmpNo.Rows[0]["emp_no"].ToString()) : 0;
                if (empNo != obj.emp_no)
                {
                    return result = "Employee(" + empNo + ") Id Mismatch !!";
                }
                var dtChkProxid_u = _dgCommon.get_InformationDataTable(string.Format("select rtrim(emp_proxid) as emp_proxid from dg_pay_Employee where compid={0} and emp_proxid='{1}' and emp_no<>{2} group by emp_proxid", obj.compid, obj.emp_proxid.Trim(), empNo), _connection);
                if (dtChkProxid_u.Rows.Count > 0)
                {
                    return result = "Employee Proxid Already Exists !!";
                }
                //if (!string.IsNullOrEmpty(obj.pi_nic) && obj.pi_nic != "0")
                //{
                //    var dtNID_u = _dgCommon.get_InformationDataTable(string.Format("select rtrim(pi_nic) as pi_nic from dg_pay_Employee where pi_nic='{0}' and (oi_transfer=0 or oi_transfer is null) and emp_no<>{1} group by pi_nic", obj.pi_nic.Trim(), empNo), _connection);
                //    if (dtNID_u.Rows.Count > 0)
                //    {
                //        return result = "Employee NID Number Already Exists !!";
                //    }                   
                //}               
                //if (!string.IsNullOrEmpty(obj.pi_birth_certificate_no) && obj.pi_birth_certificate_no != "0")
                //{
                //    var dtBCertificate_u = _dgCommon.get_InformationDataTable(string.Format("select rtrim(pi_birth_certificate_no) as pi_birth_certificate_no from dg_pay_Employee where pi_birth_certificate_no='{0}' and (oi_transfer=0 or oi_transfer is null) and emp_no<>{1} group by pi_nic", obj.pi_birth_certificate_no.Trim(), empNo), _connection);
                //    if (dtBCertificate_u.Rows.Count > 0)
                //    {
                //        return result = "Employee Birth Certificate Already Exists !!";
                //    }                   
                //}
                bool isUpdate = await _dgCommon.saveChangesAsync("dg_Pay_InsertOrUpdate_EmpPersonalInfo", _connection, obj);
                return result = isUpdate ? "Update Successfully !!" : "Something is wrong,Data not Update !!";
            }
        }*/

        public async Task<ReturnObject> AddOrEditEmpPersonalInfo(Employee_Personal_Info obj)
        {
            var result = new ReturnObject();
            if (obj.emp_serial == 0)
            {
                bool isExists = this.EmployeeNoExists(int.Parse(obj.compid.ToString()), int.Parse(obj.emp_no.ToString()));
                if (!isExists)
                {
                    var dtChkProxid = await _dgCommon.get_InformationDataTableAsync($"select rtrim(emp_proxid) as emp_proxid from dg_pay_Employee where compid={obj.compid} and emp_proxid='{obj.emp_proxid.Trim()}' group by emp_proxid", _connection);
                    if (dtChkProxid.Rows.Count > 0)
                    {
                        result.Message = "Employee Proxid Already Exists !!";
                        return result;
                    }

                    if (!string.IsNullOrEmpty(obj.pi_nic) && obj.pi_nic != "0" && !obj.isNidDuplicate)
                    {
                        var dtNID = await _dgCommon.get_InformationDataTableAsync($"select rtrim(pi_nic) as pi_nic from dg_pay_Employee where pi_nic='{obj.pi_nic.Trim()}' group by pi_nic", _connection);
                        if (dtNID.Rows.Count > 0)
                        {
                            result.Message = $"Employee NID Number({obj.pi_nic}) Already Exists !!";
                            result.dataTable = await _dgCommon.get_InformationDataTableAsync($"select comp_name,emp_no,pi_fullname,pi_nic,oi_active from dg_pay_Employee where pi_nic='{obj.pi_nic.Trim()}'", _connection);
                            return result;
                        }
                    }
                    if (!string.IsNullOrEmpty(obj.pi_birth_certificate_no) && obj.pi_birth_certificate_no != "0" && !obj.isBirthNoDuplicate)
                    {
                        var dtBCertificate = _dgCommon.get_InformationDataTable($"select rtrim(pi_birth_certificate_no) as pi_birth_certificate_no,oi_active from dg_pay_Employee where pi_birth_certificate_no='{obj.pi_birth_certificate_no.Trim()}' group by pi_birth_certificate_no", _connection);
                        if (dtBCertificate.Rows.Count > 0)
                        {
                            result.Message = $"Employee Birth Certificate No({obj.pi_birth_certificate_no}) Already Exists !!";
                            result.dataTable = await _dgCommon.get_InformationDataTableAsync($"select comp_name,emp_no,pi_fullname,pi_birth_certificate_no from dg_pay_Employee where pi_birth_certificate_no='{obj.pi_birth_certificate_no}'", _connection);
                            return result;
                        }
                    }
                    bool isSave = await _dgCommon.saveChangesAsync("dg_Pay_InsertOrUpdate_EmpPersonalInfo", _connection, obj);
                    if (isSave)
                    {
                        result.IsSuccess = true;
                        result.Message = "Save Successfully !!";
                        return result;
                    }
                    result.Message = "Something Is Wrong,Data Not Save !!";
                    return result;
                }
                result.Message = $"Employee Numner({obj.emp_no}) Already Exists !!";
                return result;
            }
            else
            {
                var dtEmpNo = _dgCommon.get_InformationDataTable($"select emp_no from dg_pay_Employee where emp_serial={obj.emp_serial}", _connection);
                int empNo = dtEmpNo.Rows.Count > 0 ? int.Parse(dtEmpNo.Rows[0]["emp_no"].ToString()) : 0;
                if (empNo != obj.emp_no)
                {
                    result.Message = $"Employee({empNo}) Id Mismatch !!";
                    return result;
                }
                var dtChkProxid_u = _dgCommon.get_InformationDataTable($"select rtrim(emp_proxid) as emp_proxid from dg_pay_Employee where compid={obj.compid} and emp_proxid='{obj.emp_proxid.Trim()}' and emp_serial<>{obj.emp_serial} group by emp_proxid", _connection);
                if (dtChkProxid_u.Rows.Count > 0)
                {
                    result.Message = "Employee Proxid Already Exists !!";
                    return result;
                }
                if (!string.IsNullOrEmpty(obj.pi_nic) && obj.pi_nic != "0" && !obj.isNidDuplicate)
                {
                    var dtNID_u = _dgCommon.get_InformationDataTable($"select rtrim(pi_nic) as pi_nic from dg_pay_Employee where pi_nic='{obj.pi_nic.Trim()}' and emp_serial<>{obj.emp_serial} group by pi_nic", _connection);
                    if (dtNID_u.Rows.Count > 0)
                    {
                        result.Message = $"Employee NID Number({obj.pi_nic}) Already Exists !!";
                        result.dataTable = await _dgCommon.get_InformationDataTableAsync($"select comp_name,emp_no,pi_fullname,pi_nic,oi_active from dg_pay_Employee where pi_nic='{obj.pi_nic.Trim()}'", _connection);
                        return result;
                    }
                }
                if (!string.IsNullOrEmpty(obj.pi_birth_certificate_no) && obj.pi_birth_certificate_no != "0" && !obj.isBirthNoDuplicate)
                {
                    var dtBCertificate_u = _dgCommon.get_InformationDataTable($"select rtrim(pi_birth_certificate_no) as pi_birth_certificate_no from dg_pay_Employee where pi_birth_certificate_no='{obj.pi_birth_certificate_no.Trim()}' and emp_serial<>{obj.emp_serial} group by pi_birth_certificate_no", _connection);
                    if (dtBCertificate_u.Rows.Count > 0)
                    {
                        result.Message = $"Employee Birth Certificate No({obj.pi_birth_certificate_no}) Already Exists !!";
                        result.dataTable = await _dgCommon.get_InformationDataTableAsync($"select comp_name,emp_no,pi_fullname,pi_birth_certificate_no,oi_active from dg_pay_Employee where pi_birth_certificate_no='{obj.pi_birth_certificate_no}'", _connection);
                        return result;
                    }
                }
                bool isUpdate = await _dgCommon.saveChangesAsync("dg_Pay_InsertOrUpdate_EmpPersonalInfo", _connection, obj);
                if (isUpdate)
                {
                    result.IsSuccess = true;
                    result.Message = "Update Successfully !!";
                    return result;
                }
                result.Message = "Something Is Wrong,Data Not Update !!";
                return result;
            }
        }

        public async Task<Employee_Personal_Info_View> GetEmpPersonalInfo_ByEmpNo(int compid, int emp_no)
        {
            var dataTable = await _dgCommon.get_InformationDataTableAsync("dg_Pay_GetEmpPersonalInfo_ByEmpNo " + compid + "," + emp_no, _connection);
            var data = _dgCommon.GetSingleListObject<Employee_Personal_Info_View>(dataTable);
            return data;
        }
        public async Task<string[]> UpdateEmpOfficialInfo(Employee_Office_Info obj)
        {
            obj.oi_grossalaryBnWords = NumberConverter.BanglaWords(int.Parse(obj.oi_grossalary.ToString()));
            var dtBgtMsg = await _dgCommon.get_InformationDataTableAsync("dg_Pay_Update_EmpOfficialInfo", _connection, obj);
            string[] result = new string[] { dtBgtMsg.Rows[0]["msgType"].ToString(), dtBgtMsg.Rows[0]["msg"].ToString() };
            return result;
        }
        public async Task<Employee_Office_Info_View> GetEmpOfficialInfo_ByEmpSerial(int emp_serial)
        {
            var data = await _dgCommon.get_InformationDataTableAsync("dg_Pay_EmpOfficialInfo_ByEmpSerial " + emp_serial, _connection);
            var newData = _dgCommon.GetSingleListObject<Employee_Office_Info_View>(data);
            return newData;
        }
        public async Task<bool> SaveEmployeeAttenData(int emp_serial)
        {
            bool flag = await _dgCommon.saveChangesAsync("dg_pay_InsertMenualAttRow " + emp_serial, _connection);
            return flag;
        }
        public async Task<string> UpdateEmpNomineeInfo(Employee_Nominee_Info obj)
        {
            string result = string.Empty;
            bool isSave = await _dgCommon.saveChangesAsync("dg_Pay_Update_EmpNomineeInfo", _connection, obj);
            if (isSave)
            {
                return result = "Save Successfully !!";
            }
            return result = "Something is wrong,Data not save !!";
        }
        public async Task<Employee_Nominee_Info_View> GetEmpNomineeInfo_ByEmpSerial(int emp_serial)
        {
            var data = await _dgCommon.get_InformationDataTableAsync("dg_Pay_EmpNomineeInfo_ByEmpSerial " + emp_serial, _connection);
            var newData = _dgCommon.GetSingleListObject<Employee_Nominee_Info_View>(data);
            return newData;
        }
        public async Task<string> UpdateEmpEducationInfo(Employee_Education_Info obj)
        {
            string result = string.Empty;
            bool isSave = await _dgCommon.saveChangesAsync("dg_Pay_InsertOrUpdate_EmpEducationInfo", _connection, obj);
            if (isSave)
            {
                return result = "Save Successfully !!";
            }
            return result = "Something is wrong,Data not save !!";
        }
        public async Task<Employee_Education_Info_View> GetEmpEducationInfo_ByEmpNo(int compid, int emp_no)
        {
            var data = await _dgCommon.get_InformationDataTableAsync("dg_Pay_EducationInfo_ByEmpNo " + compid + "," + emp_no, _connection);
            var newData = _dgCommon.GetSingleListObject<Employee_Education_Info_View>(data);
            return newData;
        }
        public async Task<string> addOrEditEmpExperince_Info(Employee_Experince_Info obj)
        {
            string result = string.Empty;
            bool isSave = await _dgCommon.saveChangesAsync("dg_Pay_InsertOrUpdate_ExperinceInfo", _connection, obj);
            if (isSave)
            {
                return result = "Save Successfully !!";
            }
            return result = "Something is wrong,Data not save !!";
        }
        public async Task<Employee_Experince_Info_View> GetEmpExperinceInfo_ByEmpNo(int compid, int emp_no)
        {
            var data = await _dgCommon.get_InformationDataTableAsync("dg_Pay_ExperinceInfo_ByEmpNo " + compid + "," + emp_no, _connection);
            var newData = _dgCommon.GetSingleListObject<Employee_Experince_Info_View>(data);
            return newData;
        }
        public async Task<Employee_FullName_View> GetEmployeeFullName(int compid, int emp_no)
        {
            var data = await _dgCommon.get_InformationDataTableAsync(string.Format("dg_pay_GetEmployeeInfo_WithImage {0},{1}", compid, emp_no), _connection);
            if (data.Rows.Count > 0)
            {
                using (HttpClient client = new HttpClient())
                {
                    string emp_image = data.Rows[0]["emp_image"].ToString();
                    string emp_signature_image = data.Rows[0]["emp_signature_image"].ToString();
                    var responseEmp_image = await client.GetAsync(emp_image);
                    var response_emp_signature_image = await client.GetAsync(emp_signature_image);
                    data.Rows[0]["emp_image"] = responseEmp_image.IsSuccessStatusCode ? emp_image : string.Empty;
                    data.Rows[0]["emp_signature_image"] = response_emp_signature_image.IsSuccessStatusCode ? emp_signature_image : string.Empty;
                }
            }           
            var newData = _dgCommon.GetSingleListObject<Employee_FullName_View>(data);
            return newData;
        }
        public async Task<DataTable> GetAllDegree_Title()
        {
            var data = await _dgCommon.get_InformationDataTableAsync("select * from dg_exam_and_degree_title order by degree_title", _connection);
            return data;
        }
        public async Task<DataTable> GetAllExam_Board()
        {
            var data = await _dgCommon.get_InformationDataTableAsync("select * from dg_exam_board order by eb_id desc", _connection);
            return data;
        }
        public async Task<DataTable> GetEmployeeNo(int compid)
        {
            var data = await _dgCommon.get_InformationDataTableAsync("select emp_serial,emp_no,pi_fullname from dg_pay_Employee where oi_active=1 and compid=" + compid, _connection);
            return data;
        }
        public async Task<ReturnObject> GetCompanyWiseEmployee_ForPMS(int companyID)
        {
            var result = new ReturnObject();
            try
            {
                var dtEmpInfo = await _dgCommon.get_InformationDataTableAsync("employee_Info_company_wise " + companyID, _connection);
                if (dtEmpInfo.Rows.Count > 0)
                {
                    result.IsSuccess = true;
                    result.Message = "Employee Information Loaded !!";
                    result.dataTable = dtEmpInfo;
                }
                else
                {
                    result.Message = "Employee Information Not Loaded !!";
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                result.Message = "Something Went Wrong !!";
            }
            return result;
        }
        private bool EmployeeNoExists(int compid, int emp_no)
        {
            var checkData = _dgCommon.get_InformationDataTable("SELECT emp_no FROM dg_pay_Employee WHERE compid=" + compid + " AND emp_no=" + emp_no, _connection);
            if (checkData.Rows.Count > 0)
            {
                return true;
            }
            return false;
        }
        public async Task<ReturnObject> GetEmployeeInfoFilter(EmployeeFilterPayload obj)
        {
            var result = new ReturnObject();
            try
            {
                string departmentId = string.Empty;
                string sectionId = string.Empty;
                string floorId = string.Empty;
                string lineId = string.Empty;
                string salCatId = string.Empty;

                if (obj.compid != 0)
                {
                    if (obj.department.Length > 0)
                    {
                        var dptArr = new List<int>();
                        foreach (var dpt in obj.department)
                        {
                            dptArr.Add(dpt);
                        }
                        departmentId = " and oi_department in(" + string.Join(",", dptArr) + ")";
                    }
                    if (obj.section.Length > 0)
                    {
                        var sectionArr = new List<int>();
                        foreach (var section in obj.section)
                        {
                            sectionArr.Add(section);
                        }
                        sectionId = " and oi_section in(" + string.Join(",", sectionArr) + ")";
                    }
                    if (obj.floor.Length > 0)
                    {
                        var floorArr = new List<int>();
                        foreach (var floor in obj.floor)
                        {
                            floorArr.Add(floor);
                        }
                        floorId = " and oi_floor in(" + string.Join(",", floorArr) + ")";
                    }
                    if (obj.line.Length > 0)
                    {
                        var lineArr = new List<int>();
                        foreach (var line in obj.line)
                        {
                            lineArr.Add(line);
                        }
                        lineId = " and oi_line in(" + string.Join(",", lineArr) + ")";
                    }
                    if (obj.salCat.Length > 0)
                    {
                        var salCatArr = new List<int>();
                        foreach (var salCat in obj.salCat)
                        {
                            salCatArr.Add(salCat);
                        }
                        salCatId = " and oi_salcategory in(" + string.Join(",", salCatArr) + ")";
                    }
                    var dtEmplist = await _dgCommon.get_InformationDataTableAsync("dg_Emp_filtering_Multi '" + obj.compid + "','" + departmentId + "','" + sectionId + "','" + obj.building + "','" + floorId + "','" + lineId + "','"+ salCatId + "'", _connection);
                    if (dtEmplist.Rows.Count > 0)
                    {
                        result.IsSuccess = true;
                        result.Message = "Data Loaded !!";
                        result.dataTable = dtEmplist;
                        return result;
                    }
                    result.Message = "No Data Available !!";
                    return result;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return result;
        }
        public async Task<ReturnObject> GetEmployeeInfoLeaveFilter(EmployeeLeaveFilterPayload obj)
        {
            var result = new ReturnObject();
            try
            {
                string departmentId = string.Empty;
                string sectionId = string.Empty;
                string floorId = string.Empty;
                string lineId = string.Empty;
                string salCatId = string.Empty;

                if (obj.compid != 0)
                {
                    if (obj.department.Length > 0)
                    {
                        var dptArr = new List<int>();
                        foreach (var dpt in obj.department)
                        {
                            dptArr.Add(dpt);
                        }
                        departmentId = " and oi_department in(" + string.Join(",", dptArr) + ")";
                    }
                    if (obj.section.Length > 0)
                    {
                        var sectionArr = new List<int>();
                        foreach (var section in obj.section)
                        {
                            sectionArr.Add(section);
                        }
                        sectionId = " and oi_section in(" + string.Join(",", sectionArr) + ")";
                    }
                    if (obj.floor.Length > 0)
                    {
                        var floorArr = new List<int>();
                        foreach (var floor in obj.floor)
                        {
                            floorArr.Add(floor);
                        }
                        floorId = " and oi_floor in(" + string.Join(",", floorArr) + ")";
                    }
                    if (obj.line.Length > 0)
                    {
                        var lineArr = new List<int>();
                        foreach (var line in obj.line)
                        {
                            lineArr.Add(line);
                        }
                        lineId = " and oi_line in(" + string.Join(",", lineArr) + ")";
                    }
                    if (obj.salCat.Length > 0)
                    {
                        var salCatArr = new List<int>();
                        foreach (var salCat in obj.salCat)
                        {
                            salCatArr.Add(salCat);
                        }
                        salCatId = " and oi_salcategory in(" + string.Join(",", salCatArr) + ")";
                    }

                    var dtEmplist = new DataTable();
                    using (var cmd = new SqlCommand("dg_Emp_filtering_for_leave", _connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Compid", obj.compid);
                        cmd.Parameters.AddWithValue("@DepartmentID", departmentId);
                        cmd.Parameters.AddWithValue("@sectionID", sectionId);
                        cmd.Parameters.AddWithValue("@Building", obj.building);
                        cmd.Parameters.AddWithValue("@FloorID", floorId ?? "");
                        cmd.Parameters.AddWithValue("@LineID", lineId ?? "");
                        cmd.Parameters.AddWithValue("@SalCatID", salCatId ?? "");
                        cmd.Parameters.AddWithValue("@leave_type", obj.leave_type ?? "");
                        cmd.Parameters.AddWithValue("@leave_days", obj.leave_days ?? "");
                        cmd.Parameters.AddWithValue("@fromDate", obj.fromDate);
                        cmd.Parameters.AddWithValue("@toDate", obj.toDate);
                        if (_connection.State == ConnectionState.Closed || _connection.State == ConnectionState.Broken)
                        {
                            _connection.Open();
                        }
                        dtEmplist.Load(await cmd.ExecuteReaderAsync());
                    }

                    if (dtEmplist.Rows.Count > 0)
                    {
                        result.IsSuccess = true;
                        result.Message = "Data Loaded !!";
                        result.dataTable = dtEmplist;
                        return result;
                    }
                    result.Message = "No Data Available !!";
                    return result;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return result;
        }
        public byte[] Dg_EmployeeSingleDetailedInformation(int emp_serial, string userName, string reportType)
        {
            var data = _dgCommon.get_InformationDataTable("Dg_Rep_emp_CV_Single " + emp_serial + ",'"+ userName + "'", _connection);
            string dataset = "rpt_emp_CV";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Dg_EmployeeInfo.rdlc";
            string imgPath = new Uri($"{_webHostEnvironment.WebRootPath}\\EmployeeImage\\").AbsoluteUri;
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("EmpImagePath",imgPath),
                new ReportParameter("PrintUser",userName)
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            return reportBytes;
        }

        private async Task<ReturnObject> UploadImageZipOrRar(IFormFile zipFile, int compid, int imageWidth, int imageHeight, string uploadPath, string imgType)
        {
            var result = new ReturnObject();
            string tempExtractFolder = string.Empty;
            string tempZipPath = string.Empty;
            string upFileExt = Path.GetExtension(zipFile.FileName).ToLower().Trim();
            if (zipFile == null || zipFile.Length == 0 || (upFileExt != ".zip" && upFileExt != ".rar"))
            {
                result.Message = "Invalid file. Please upload a zip or rar file";
                return result;
            }
            try
            {
                //uploadPath = $"{_webHostEnvironment.WebRootPath}\\EmployeeSignature\\" + compid + "\\";
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }
                string fileNameTrim = string.Concat(Path.GetFileNameWithoutExtension(zipFile.FileName).Trim(), Path.GetExtension(zipFile.FileName));
                tempZipPath = Path.Combine(Path.GetTempPath(), fileNameTrim);
                using (var fileStream = new FileStream(tempZipPath, FileMode.Create))
                {
                    await zipFile.CopyToAsync(fileStream);
                }
                tempExtractFolder = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(zipFile.FileName).Trim());
                if (upFileExt == ".zip")
                {
                    ZipFile.ExtractToDirectory(tempZipPath, tempExtractFolder);
                }
                else if (upFileExt == ".rar")
                {
                    ExtractRar(tempZipPath, tempExtractFolder);
                }
                int totalUploadFile = Directory.GetFiles(tempExtractFolder, "*.*", SearchOption.AllDirectories).Count();
                var empNo = _dgCommon.get_InformationDataTable("select emp_no from dg_pay_Employee where compid=" + compid, _connection)
                    .Rows.OfType<DataRow>().Select(k => k[0].ToString().Trim()).ToArray();

                var validImages = new List<string>();
                if (imgType == "EmployeeImage")
                {
                    validImages = Directory.GetFiles(tempExtractFolder, "*.*", SearchOption.AllDirectories)
                    .Where(file => (Path.GetExtension(file).ToLower().Trim() == ".jpg")
                    && empNo.Any(emp => emp.Equals(Path.GetFileNameWithoutExtension(file).Trim(), StringComparison.OrdinalIgnoreCase))).ToList();
                }
                else if(imgType == "EmployeeSignature")
                {
                    validImages = Directory.GetFiles(tempExtractFolder, "*.*", SearchOption.AllDirectories)
                    .Where(file => (Path.GetExtension(file).ToLower().Trim() == ".png")
                    && empNo.Any(emp => emp.Equals(Path.GetFileNameWithoutExtension(file).Trim(), StringComparison.OrdinalIgnoreCase))).ToList();
                }

                if (validImages.Count > 0)
                {
                    int oldFileCount = 0;
                    foreach (var imagePath in validImages)
                    {
                        var fileName = Path.GetFileName(imagePath);
                        var destPath = Path.Combine(uploadPath, fileName);
                        if (File.Exists(destPath))
                        {
                            File.Delete(destPath);
                            oldFileCount++;
                        }
                        if (IsValidImage(imagePath, imageWidth, imageHeight))
                        {
                            File.Move(imagePath, destPath);
                        }
                        else
                        {
                            var imageFormat = imgType == "EmployeeImage" ? ImageFormat.Jpeg : ImageFormat.Png;
                            var imageCrop = _dgCommon.CustomImageCrop(imagePath, imageWidth, imageHeight, imageFormat);
                            imageCrop.Save(destPath);
                            imageCrop = null;
                        }
                    }
                    result.IsSuccess = true;
                    result.Message = string.Format("Total Upload File({0}) Overwrite Upload({1}) New Upload({2}) !!", totalUploadFile, oldFileCount, validImages.Count - oldFileCount);
                }
                else
                {
                    result.Message = "Valid File Not Found !!";
                }
                Directory.Delete(tempExtractFolder, true);
                File.Delete(tempZipPath);
                return result;
            }
            catch (Exception ex)
            {
                ex.ToString();
                Directory.Delete(tempExtractFolder, true);
                File.Delete(tempZipPath);
            }
            return result;
        }
        private bool IsValidImage(string filePath, int maxWidth, int maxHeight)
        {
            using var image = Image.FromFile(filePath.Trim());
            if (image.Width <= maxWidth && image.Height <= maxHeight)
            {
                return true;
            }
            return false;
        }
        private void ExtractRar(string rarFilePath, string extractFolder)
        {
            using var archive = SharpCompress.Archives.Rar.RarArchive.Open(rarFilePath);
            foreach (var entry in archive.Entries.Where(entry => !entry.IsDirectory))
            {
                var destinationPath = Path.Combine(extractFolder, entry.Key);
                Directory.CreateDirectory(Path.GetDirectoryName(destinationPath)!);
                using var fileStream = File.Create(destinationPath);
                entry.WriteTo(fileStream);
            }
        }
    }
}