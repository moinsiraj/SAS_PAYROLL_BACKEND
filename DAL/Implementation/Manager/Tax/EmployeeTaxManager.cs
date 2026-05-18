using BLL.Interfaces.Manager.Tax;
using BLL.Utility;
using BOL.Models;
using DAL.Data;
using DAL.Implementation.Repository.Tax;
using EF.Core.Repository.Manager;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Reporting.NETCore;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Implementation.Manager.Tax
{
    public class EmployeeTaxManager : CommonManager<BankBranch>, IEmployeeTaxManager
    {
        private readonly Dg_Common _dgCommon;
        private readonly SqlConnection _payCon;
        private readonly IWebHostEnvironment _webHost;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public EmployeeTaxManager(dg_hrpayrollContext context, Dg_Common dgCommon, IWebHostEnvironment webHost, IHttpContextAccessor httpContextAccessor) : base(new EmployeeTaxRepository(context))
        {
            _dgCommon = dgCommon;
            _payCon = new SqlConnection(Getway.Dg_Payroll);
            _webHost = webHost;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ReturnObject> GetCompanyForTax(string userName)
        {
            var result = new ReturnObject();
            var dtCompany = await _dgCommon.get_InformationDataTableAsync(string.Format("User_List_from_access_table '{0}'", userName), _payCon);
            if (dtCompany.Rows.Count > 0)
            {
                result.IsSuccess = true;
                result.dataTable = dtCompany;
                return result;
            }
            result.Message = "No Data Available !!";
            return result;
        }
        public async Task<ReturnObject> GetTaxYearInfo()
        {
            var result = new ReturnObject();
            var dttaxYear = await _dgCommon.get_InformationDataTableAsync("select txYear_id,txYear_name,txYear_amt from Dg_Tax_Year order by txYear_id asc", _payCon);
            if (dttaxYear.Rows.Count > 0)
            {
                result.IsSuccess = true;
                result.dataTable = dttaxYear;
                return result;
            }
            result.Message = "No Data Available !!";
            return result;
        }
        public async Task<ReturnObject> GetEmployeeInfoForTax(int compid,string empPrefix)
        {
            var result = new ReturnObject();
            //var domain = string.Concat($"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}", "/EmployeeImage/");
            string domain = "http://203.202.240.228:8085/EmployeeImage/";
            var dtEmpInfo = await _dgCommon.get_InformationDataTableAsync(string.Format("Dg_Pay_Tax_GetEmployeeInfo {0},'{1}'", compid, empPrefix), _payCon);
            if (dtEmpInfo.Rows.Count > 0)
            {
                using (HttpClient client = new HttpClient())
                {
                    foreach (DataRow row in dtEmpInfo.Rows)
                    {
                        string imgPath = string.Concat(domain, row["compid"].ToString(), "/", row["emp_no"].ToString(), ".jpg");
                        var response = await client.GetAsync(imgPath);
                        row["empImage"] = response.IsSuccessStatusCode ? imgPath : string.Empty;
                    }
                }               
                result.IsSuccess = true;
                result.dataTable = dtEmpInfo;
                return result;
            }
            result.Message = "No Data Available !!";
            return result;
        }
        public async Task<ReturnObject> GetExistingTaxInfo(int taxYear,int empSL)
        {
            var result = new ReturnObject();
            var dtInputDtl = await _dgCommon.get_InformationDataTableAsync(string.Format("select top 1 dtl_invstment,dtl_invstment_title,dtl_advcTax,dtl_TaxOnCR_invstment,dtl_ttlTaxAble_Inc,dtl_ttlTax_Inc,dtl_ttlTaxPay_inc,dtl_monthly_tax,dtl_interestBnk_inc from Dg_Tax_Summary_Info where dtl_emp_serial={0} and dtl_year={1} order by dtl_id desc", empSL, taxYear), _payCon);
            if (dtInputDtl.Rows.Count > 0)
            {
                result.IsSuccess = true;
                result.dataTable = new
                {
                    benefitsInfo = await _dgCommon.get_InformationDataTableAsync(string.Format("select txbf_id,txbf_type,txbf_title,txbf_amt from Dg_Tax_Benefits where txbf_empSerial={0} and txbf_year={1}", empSL, taxYear), _payCon),
                    inputDetails = dtInputDtl
                };
                return result;
            }
            result.Message = "No Data Available !!";
            return result;
        }
        public async Task<ReturnObject> SaveEmployeeTaxInfo(EmployeeTaxModel obj)
        {
            var result = new ReturnObject();
            if (obj.dtl_monthly_tax==0)
            {
                result.Message = "Tax Monthly Is Empty !!";
                return result;
            }
            var queryRes = await _dgCommon.saveChangesAsyncNew("Dg_Pay_Tax_SaveEmployeeTaxSummaryInfo", _payCon, obj);
            if (queryRes.isExecute)
            {
                result.IsSuccess = queryRes.isExecute;
                result.Message = queryRes.ExecuteNotify;
                if (obj.taxBenefits.Count > 0)
                {
                    await _dgCommon.saveChangesAsync(string.Format("delete Dg_Tax_Benefits where txbf_compid={0} and txbf_empSerial={1} and txbf_year={2}", obj.compid, obj.emp_serial, obj.txYear_id), _payCon);
                    obj.taxBenefits.ToList().ForEach(row =>
                    {
                        var dbObj = EmployeeTaxBenefits.TaxBenefitsQuery(obj, row);
                        _dgCommon.saveChanges("Dg_Pay_Tax_SaveEmployeeTaxBenefitsInfo", _payCon, dbObj);
                    });
                }               
                return result;
            }
            result.Message = queryRes.ExecuteNotify;
            return result;
        }
        public async Task<EmployeeTaxReportRender> GetEmployeeTaxInfoReport(EmployeeTaxReport obj)
        {
            var result = new EmployeeTaxReportRender();
            if (string.IsNullOrEmpty(obj.compid.ToString()))
            {
                result.message = "Company Is Required !!";
                return result;
            }
            var dtRepData = await _dgCommon.get_InformationDataTableAsync(string.Format("Dg_Pay_Tax_GetEmployeeTaxCalInfoRPT {0},{1},{2}", obj.compid, obj.emp_serial, obj.taxYear), _payCon);
            string dataset = "empTaxInfo";
            string path = $"{_webHost.WebRootPath}\\Tax_Report\\Dg_EmpTaxInfoReport.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser", obj.userName)
            };
            result.reportByte = _dgCommon.GenerateReport(dtRepData, dataset, path, obj.reportType, reportParameters);
            return result;
        }

        public async Task<ReturnObject> GetEmployeeInfoFromTax(int compid,int taxYear, string empPrefix)
        {
            var result = new ReturnObject();
            //var domain = string.Concat($"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}", "/EmployeeImage/");
            string domain = "http://203.202.240.228:8085/EmployeeImage/";
            var dtEmpInfo = await _dgCommon.get_InformationDataTableAsync(string.Format("Dg_Pay_Tax_GetEmployeeInfoFromTax {0},{1},'{2}'", compid, taxYear, empPrefix), _payCon);
            if (dtEmpInfo.Rows.Count > 0)
            {
                using (HttpClient client = new HttpClient())
                {
                    foreach (DataRow row in dtEmpInfo.Rows)
                    {
                        string imgPath = string.Concat(domain, row["compid"].ToString(), "/", row["emp_no"].ToString(), ".jpg");
                        var response = await client.GetAsync(imgPath);
                        row["empImage"] = response.IsSuccessStatusCode ? imgPath : string.Empty;
                    }
                }
                result.IsSuccess = true;
                result.dataTable = dtEmpInfo;
                return result;
            }
            result.Message = "No Data Available !!";
            return result;
        }
        public async Task<ReturnObject> GetBankInfoForTax()
        {
            var result = new ReturnObject();
            var dtBank = await _dgCommon.get_InformationDataTableAsync("select Bank_Code,Bank_Name from dg_pay_BankInfo order by Bank_Name", _payCon);
            if (dtBank.Rows.Count > 0)
            {
                result.IsSuccess = true;
                result.dataTable = dtBank;
                return result;
            }
            result.Message = "No Data Available !!";
            return result;
        }
        public async Task<ReturnObject> GetBankInfoBranchForTax(int bankID)
        {
            var result = new ReturnObject();
            var dtBankBranch = await _dgCommon.get_InformationDataTableAsync(string.Format("select bnc_id,bnc_name from dg_pay_BankInfo_branch where bnk_id={0}", bankID), _payCon);
            if (dtBankBranch.Rows.Count > 0)
            {
                result.IsSuccess = true;
                result.dataTable = dtBankBranch;
                return result;
            }
            result.Message = "No Data Available !!";
            return result;
        }
        public async Task<ReturnObject> SaveTaxChallan(EmployeeTaxChallan obj)
        {
            var result = new ReturnObject();
            if (!string.IsNullOrEmpty(obj.challanNo) && !string.IsNullOrEmpty(obj.amt.ToString()))
            {
                bool isChkChallan = _dgCommon.get_InformationDataTable(string.Format("select chln_name from Dg_Tax_challan where chln_empSerial={0} and chln_taxYear={1} and chln_name='{2}'", obj.empSerial, obj.taxYear, obj.challanNo), _payCon).Rows.Count > 0;
                if (!isChkChallan)
                {
                    if (obj.file != null)
                    {
                        var extension = Path.GetExtension(obj.file.FileName);
                        if (extension == ".jpeg" || extension == ".jpg" || extension == ".pdf")
                        {
                            string filepath = $"{_webHost.WebRootPath}\\TaxChallanFile\\" + obj.compid + "\\";
                            if (!Directory.Exists(filepath))
                            {
                                Directory.CreateDirectory(filepath);
                            }
                            string fileFullPath = string.Concat(filepath, obj.empSerial.ToString(), "_", obj.challanNo, extension);
                            using (var stream = new FileStream(fileFullPath, FileMode.Create))
                            {
                                await obj.file.CopyToAsync(stream);
                            }
                            obj.challanFilePath = string.Concat(obj.empSerial.ToString(), "_", obj.challanNo, extension);
                            bool isSave = await _dgCommon.saveChangesAsync("Dg_Pay_Tax_SaveTaxChallanInfo", _payCon, obj);
                            if (isSave)
                            {
                                result.IsSuccess = isSave;
                                result.Message = "Save Successfully !!";
                                var challanInfo = await GetEmpTaxChallanInfo(obj.compid, obj.empSerial, obj.taxYear);
                                result.dataTable = challanInfo.dataTable;
                                return result;
                            }
                            result.Message = "Save Fail !!";
                            return result;
                        }
                        result.Message = "File Extension Not Valid !!";
                        return result;
                    }
                    result.Message = "Please Upload File !!";
                    return result;
                }
                result.Message = "Challan Already Exists !!";
                return result;
            }
            result.Message = "Challan No Or Amount Empty !!";
            return result;
        }
        public async Task<ReturnObject> GetEmpTaxChallanInfo(int compid,int empSerial,int taxYear)
        {
            var result = new ReturnObject();
            var dtChallanInfo = await _dgCommon.get_InformationDataTableAsync(string.Format("Dg_Pay_Tax_GetEmpTaxChallanInfo {0},{1},{2}", compid, empSerial, taxYear), _payCon);
            if (dtChallanInfo.Rows.Count > 0)
            {
                result.IsSuccess = true;
                result.dataTable = dtChallanInfo;
                return result;
            }
            result.Message = "No Data Available !!";
            return result;
        }
        public async Task<ReturnObject> DeleteTaxChallan(int chlnid)
        {
            var result = new ReturnObject();
            if (chlnid > 0)
            {
                var dtChallan = await _dgCommon.get_InformationDataTableAsync("select chln_compid,chln_empSerial,chln_taxYear,chln_filePath from Dg_Tax_challan where chln_id=" + chlnid, _payCon);
                int compid = !string.IsNullOrEmpty(dtChallan.Rows[0]["chln_compid"].ToString()) ? int.Parse(dtChallan.Rows[0]["chln_compid"].ToString()) : 0;
                string fileName = !string.IsNullOrEmpty(dtChallan.Rows[0]["chln_filePath"].ToString()) ? dtChallan.Rows[0]["chln_filePath"].ToString() : string.Empty;
                int empSerial = !string.IsNullOrEmpty(dtChallan.Rows[0]["chln_empSerial"].ToString()) ? int.Parse(dtChallan.Rows[0]["chln_empSerial"].ToString()) : 0;
                int taxYear = !string.IsNullOrEmpty(dtChallan.Rows[0]["chln_taxYear"].ToString()) ? int.Parse(dtChallan.Rows[0]["chln_taxYear"].ToString()) : 0;
                if (!string.IsNullOrEmpty(fileName))
                {
                    string filepath = string.Concat($"{_webHost.WebRootPath}\\TaxChallanFile\\", compid, "\\", fileName);
                    if (File.Exists(filepath))
                    {
                        File.Delete(filepath);
                    }
                }
                bool isDel = await _dgCommon.saveChangesAsync("delete from Dg_Tax_challan where chln_id=" + chlnid, _payCon);
                if (isDel)
                {
                    result.IsSuccess = isDel;
                    result.Message = "Delete Successfully !!";
                    var challanInfo = await GetEmpTaxChallanInfo(compid, empSerial, taxYear);
                    result.dataTable = challanInfo.dataTable;
                    return result;
                }
                result.Message = "Delete Fail !!";
                return result;
            }
            result.Message = "No Row Found !!";
            return result;
        }
        public async Task<ReturnObject> UpdateTaxChallan(EmployeeTaxChallan obj)
        {
            var result = new ReturnObject();
            if (obj.challan_id > 0)
            {
                if (!string.IsNullOrEmpty(obj.challanNo) && !string.IsNullOrEmpty(obj.amt.ToString()))
                {
                    if (obj.file != null)
                    {
                        var extension = Path.GetExtension(obj.file.FileName);
                        if (extension == ".jpeg" || extension == ".jpg" || extension == ".pdf")
                        {
                            var dtChallan = await _dgCommon.get_InformationDataTableAsync("select chln_name,chln_filePath from Dg_Tax_challan where chln_id=" + obj.challan_id, _payCon);
                            string challanNo = !string.IsNullOrEmpty(dtChallan.Rows[0]["chln_name"].ToString()) ? dtChallan.Rows[0]["chln_name"].ToString() : string.Empty;
                            string challanPath = !string.IsNullOrEmpty(dtChallan.Rows[0]["chln_filePath"].ToString()) ? dtChallan.Rows[0]["chln_filePath"].ToString() : string.Empty;
                            string filepath = $"{_webHost.WebRootPath}\\TaxChallanFile\\" + obj.compid + "\\";
                            string oldfileFullPath = string.Concat(filepath, challanPath);
                            if (File.Exists(oldfileFullPath))
                            {
                                File.Delete(oldfileFullPath);
                            }
                            string fileFullPath = string.Concat(filepath, obj.empSerial.ToString(), "_", obj.challanNo, extension);
                            using (var stream = new FileStream(fileFullPath, FileMode.Create))
                            {
                                await obj.file.CopyToAsync(stream);
                            }
                            obj.challanFilePath = string.Concat(obj.empSerial.ToString(), "_", obj.challanNo, extension);
                            bool isUpdate = await _dgCommon.saveChangesAsync("Dg_Pay_Tax_UpdateTaxChallanInfo", _payCon, obj);
                            if (isUpdate)
                            {
                                result.IsSuccess = isUpdate;
                                result.Message = "Update Successfully !!";
                                var challanInfo = await GetEmpTaxChallanInfo(obj.compid, obj.empSerial, obj.taxYear);
                                result.dataTable = challanInfo.dataTable;
                                return result;
                            }
                            result.Message = "Update Fail !!";
                            return result;
                        }
                        result.Message = "File Extension Not Valid !!";
                        return result;
                    }
                    result.Message = "Upload File Not Found !!";
                    return result;
                }
                result.Message = "Challan No Or Amount Empty !!";
                return result;
            }
            result.Message = "No Row Found !!";
            return result;
        }
        public async Task<EmployeeTaxReportRender> GetEmployeeTaxChallanInfoReport(EmployeeTaxReport obj)
        {
            var result = new EmployeeTaxReportRender();
            if (string.IsNullOrEmpty(obj.compid.ToString()))
            {
                result.message = "Company Is Required !!";
                return result;
            }
            var dtRepData = await _dgCommon.get_InformationDataTableAsync(string.Format("Dg_Pay_Tax_GetEmployeeTaxChallanInfoRPT {0},{1},{2}", obj.compid, obj.emp_serial, obj.taxYear), _payCon);
            string dataset = "challanInfo";
            string path = $"{_webHost.WebRootPath}\\Tax_Report\\Dg_EmpTaxChallanInfoReport.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser", obj.userName)
            };
            result.reportByte = _dgCommon.GenerateReport(dtRepData, dataset, path, obj.reportType, reportParameters);
            return result;
        }

        public async Task<ReturnObject> GetAllBankBranch(int bankid)
        {
            var result = new ReturnObject();
            var dtBnkBranch = await _dgCommon.get_InformationDataTableAsync(string.Format("Dg_Pay_GetAllBankBranch {0}", bankid), _payCon);
            if (dtBnkBranch.Rows.Count > 0)
            {
                result.IsSuccess = true;
                result.dataTable = dtBnkBranch;
                return result;
            }
            result.Message = "No Data Available !!";
            return result;
        }
        public async Task<ReturnObject> AddOrEditBankBranch(BankBranchPayload obj)
        {
            var result = new ReturnObject();
            var isExists = GetFirstOrDefault(x => x.bnk_id == obj.bnk_id && x.bnc_name == obj.b_name && x.bnc_id != obj.bnc_id);
            if (isExists == null)
            {
                var bncDbID = GetFirstOrDefault(x => x.bnk_id == obj.bnk_id && x.bnc_name == obj.b_name);
                obj.bnc_id = obj.bnc_id == 0 && bncDbID != null ? bncDbID.bnc_id : obj.bnc_id;
                if (obj.bnc_id == 0)
                {
                    if (await AddAsync(BankBranchPayload.PayloadToBankBranchDb_Obj(obj)))
                    {
                        result.IsSuccess = true;
                        result.Message = "Save Successfully !!";
                        var bankBrnch = await GetAllBankBranch(obj.bnk_id);
                        result.dataTable = bankBrnch.dataTable;
                        return result;
                    }
                    result.Message = "Save Fail !!";
                    return result;
                }
                if (await UpdateAsync(BankBranchPayload.PayloadToBankBranchDb_Obj(obj, GetFirstOrDefault(x => x.bnc_id == obj.bnc_id))))
                {
                    result.IsSuccess = true;
                    result.Message = "Update Successfully !!";
                    var bankBrnch = await GetAllBankBranch(obj.bnk_id);
                    result.dataTable = bankBrnch.dataTable;
                    return result;
                }
                result.Message = "Update Fail !!";
                return result;
            }
            result.Message = "Bank Branch Already Exists !!";
            return result;
        }
    }
}
