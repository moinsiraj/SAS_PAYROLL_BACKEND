using BLL.Interfaces.Manager.EmployeeTransfer;
using BLL.Utility;
using BOL.Models;
using Microsoft.AspNetCore.Hosting;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Implementation.Manager.EmployeeTransfer
{
    public class EmployeeTransferManager : IEmployeeTransferManager
    {
        private readonly Dg_Common _dgCommon;
        private readonly SqlConnection _payCon;
        private readonly IWebHostEnvironment _webHost;
        public EmployeeTransferManager(Dg_Common dg_Common, IWebHostEnvironment webHost)
        {
            _dgCommon = dg_Common;
            _payCon = new SqlConnection(Getway.Dg_Payroll);
            _webHost = webHost;
        }

        public async Task<ReturnObject> SaveEmployeeTransfer(EmployeeTransferModel obj)
        {
            var result = new ReturnObject();
            try
            {
                if (obj.transferType== "External" && obj.toCompanyID==0 && obj.newEmployeeID==0 && obj.newProxid=="" && obj.salaryCatID == 0)
                {
                    result.IsSuccess = false;
                    result.Message = "New Company Or New Employee Number Or New Proxid Is Required !!";
                    return result;
                }
                var dtMsg = await _dgCommon.get_InformationDataTableAsync("Dg_Pay_Employee_Transfer", _payCon, obj);
                if (dtMsg.Rows[0]["msgType"].ToString() == "success")
                {
                    result.IsSuccess = true;
                    result.Message = dtMsg.Rows[0]["msg"].ToString();
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = dtMsg.Rows[0]["msg"].ToString();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                ex.ToString();
                result.IsSuccess = false;
                result.Message = "Something Went Wrong !!";
            }
            return result;
        }
        public async Task<DataTable> GetEmployeeTransferList(int companyID, int employeeID)
        {
            var data = await _dgCommon.get_InformationDataTableAsync("Dg_Pay_GetEmpTransferInfo "+ companyID + ","+ employeeID, _payCon);
            return data;
        }
        public async Task<DataTable> GetTransferToCompany(int fCompid)
        {
            var data = await _dgCommon.get_InformationDataTableAsync("select com_id,com_name FROM dg_pay_company where com_id !="+ fCompid, _payCon);
            return data;
        }
        public async Task<ReturnObject> ApproveEmployeeTransfer(int transID, string userName)
        {
            var result = new ReturnObject();
            if (transID != 0)
            {
                bool isUpdate = await _dgCommon.saveChangesAsync("update dg_emp_transfer_info set [Status]=1,confirmedby='" + userName + "',confirmed_date=getdate() where trans_id=" + transID, _payCon);
                if (isUpdate)
                {
                    result.IsSuccess = true;
                    result.Message = "Employee Transfer Approved Successfully !!";
                    var dtInfo = await _dgCommon.get_InformationDataTableAsync("select trans_type,From_company,toconpany,pre_emp_no,emp_no_new from dg_emp_transfer_info where trans_id=" + transID, _payCon);
                    string trans_type = dtInfo.Rows.Count > 0 && !string.IsNullOrEmpty(dtInfo.Rows[0]["trans_type"].ToString()) ? dtInfo.Rows[0]["trans_type"].ToString() : string.Empty;                   
                    if (trans_type == "External")
                    {
                        int From_company = dtInfo.Rows.Count > 0 && !string.IsNullOrEmpty(dtInfo.Rows[0]["From_company"].ToString()) ? int.Parse(dtInfo.Rows[0]["From_company"].ToString()) : 0;
                        int toconpany = dtInfo.Rows.Count > 0 && !string.IsNullOrEmpty(dtInfo.Rows[0]["toconpany"].ToString()) ? int.Parse(dtInfo.Rows[0]["toconpany"].ToString()) : 0;
                        int pre_emp_no = dtInfo.Rows.Count > 0 && !string.IsNullOrEmpty(dtInfo.Rows[0]["pre_emp_no"].ToString()) ? int.Parse(dtInfo.Rows[0]["pre_emp_no"].ToString()) : 0;
                        int emp_no_new = dtInfo.Rows.Count > 0 && !string.IsNullOrEmpty(dtInfo.Rows[0]["emp_no_new"].ToString()) ? int.Parse(dtInfo.Rows[0]["emp_no_new"].ToString()) : 0;                       
                        var imgSourcePath = Path.Combine(_webHost.WebRootPath, "EmployeeImage", From_company.ToString(), string.Concat(pre_emp_no, ".jpg"));
                        if (File.Exists(imgSourcePath))
                        {
                            string emgDir = Path.Combine(_webHost.WebRootPath, "EmployeeImage", toconpany.ToString());
                            if (!Directory.Exists(emgDir))
                            {
                                Directory.CreateDirectory(emgDir);
                            }
                            var imgDestinationPath = Path.Combine(emgDir, string.Concat(emp_no_new, ".jpg"));
                            File.Copy(imgSourcePath, imgDestinationPath, true);
                        }

                        var signSourcePath = Path.Combine(_webHost.WebRootPath, "EmployeeSignature", From_company.ToString(), string.Concat(pre_emp_no, ".png"));
                        if (File.Exists(signSourcePath))
                        {
                            string signDir = Path.Combine(_webHost.WebRootPath, "EmployeeSignature", toconpany.ToString());
                            if (!Directory.Exists(signDir))
                            {
                                Directory.CreateDirectory(signDir);
                            }
                            var signDestinationPath = Path.Combine(signDir, string.Concat(emp_no_new, ".png"));
                            File.Copy(signSourcePath, signDestinationPath, true);
                        }

                        var nidSourcePath = Path.Combine(_webHost.WebRootPath, "Emp_Nid", From_company.ToString(), string.Concat(pre_emp_no, ".pdf"));
                        if (File.Exists(nidSourcePath))
                        {
                            string nidDir = Path.Combine(_webHost.WebRootPath, "Emp_Nid", toconpany.ToString());
                            if (!Directory.Exists(nidDir))
                            {
                                Directory.CreateDirectory(nidDir);
                            }
                            var nidDestinationPath = Path.Combine(nidDir, string.Concat(emp_no_new, ".pdf"));
                            File.Copy(nidSourcePath, nidDestinationPath, true);
                        }

                        var nomineSourcePath = Path.Combine(_webHost.WebRootPath, "NomineeImage", From_company.ToString(), string.Concat(pre_emp_no, ".jpg"));
                        if (File.Exists(nomineSourcePath))
                        {
                            string nomoneDir = Path.Combine(_webHost.WebRootPath, "NomineeImage", toconpany.ToString());
                            if (!Directory.Exists(nomoneDir))
                            {
                                Directory.CreateDirectory(nomoneDir);
                            }
                            var nominDestinationPath = Path.Combine(nomoneDir, string.Concat(emp_no_new, ".jpg"));
                            File.Copy(nomineSourcePath, nominDestinationPath, true);
                        }

                        var nomineNidSourcePath = Path.Combine(_webHost.WebRootPath, "NomineeNid", From_company.ToString(), string.Concat(pre_emp_no, ".pdf"));
                        if (File.Exists(nomineNidSourcePath))
                        {
                            string nomineNidDir = Path.Combine(_webHost.WebRootPath, "NomineeNid", toconpany.ToString());
                            if (!Directory.Exists(nomineNidDir))
                            {
                                Directory.CreateDirectory(nomineNidDir);
                            }
                            var nomineNidDestinationPath = Path.Combine(nomineNidDir, string.Concat(emp_no_new, ".pdf"));
                            File.Copy(nomineNidSourcePath, nomineNidDestinationPath, true);
                        }


                    }
                }
                else
                {
                    result.Message = "Employee Transfer Approved Failed !!";
                }
            }
            else
            {
                result.Message = "Employee Transfer Not Approved !!";
            }
            return result;
        }
        public async Task<ReturnObject> DeleteEmployeeTransfer(int transID)
        {
            var result = new ReturnObject();
            try
            {
                if (transID !=0)
                {
                    bool isDelete = await _dgCommon.saveChangesAsync("delete dg_emp_transfer_info where [Status]=0 and trans_id="+ transID, _payCon);
                    if (isDelete)
                    {
                        result.IsSuccess = true;
                        result.Message = "Employee Transfer Delete Successfully !!";
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = "Employee Transfer Delete Failed !!";
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = "Employee Transfer Not Deleted !!";
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                ex.ToString();
                result.IsSuccess = false;
                result.Message = "Something Went Wrong !!";
            }
            return result;
        }
    }
}