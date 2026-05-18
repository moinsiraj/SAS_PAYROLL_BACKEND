using BLL.Interfaces.Manager.Tiffinbillrules;
using BLL.Utility;
using BOL.Models;
using DAL.Data;
using DAL.Implementation.Repository.Tiffinbillrules;
using EF.Core.Repository.Manager;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Implementation.Manager.Tiffinbillrules
{
    public class TiffinbillrulesManager : CommonManager<Tiffinbillrule_DbModel>,ITiffinbillrulesManager
    {
        private readonly Dg_Common _dgCommon;
        private readonly SqlConnection _connection;
        public TiffinbillrulesManager(dg_hrpayrollContext context, Dg_Common dgCommon) :base(new TiffinbillrulesRepository(context))
        {
            _dgCommon = dgCommon;
            _connection = new SqlConnection(Getway.Dg_Payroll);
        }

        public List<ReturnObject> SaveEmployeeTiffinBillProcess(TiffinBillProcessPayload obj)
        {
            var result = new List<ReturnObject>();
            var single_date = new List<TiffinBillProcessSingle_date>();
            if (obj.employeeSl.Length > 0)
            {                
                int procMonth = Convert.ToDateTime(obj.formDate).Month;
                int procYear = Convert.ToDateTime(obj.formDate).Year;
                var dtSalConf = _dgCommon.get_InformationDataTable("select TOP 1 ss_emp_serial from dg_pay_salarysheet where ss_compid="+obj.companyId + " and month(ss_date)="+ procMonth + " and year(ss_date)="+ procYear + " and ss_confirmed=1", _connection);
                bool isSalConf = dtSalConf.Rows.Count > 0 ? true : false;
                if (!isSalConf)
                {
                    if (Convert.ToDateTime(obj.formDate) > Convert.ToDateTime(obj.toDate))
                    {
                        result.Add(new ReturnObject { IsSuccess = false, Message = "ToDate Smaller Than FormDate !!" });
                        //result.Message = "ToDate Smaller Than FormDate !!";
                        return result;
                    }
                    if (Convert.ToDateTime(obj.formDate).Month != Convert.ToDateTime(obj.toDate).Month)
                    {
                        result.Add(new ReturnObject { IsSuccess = false, Message = "FormDate And ToDate Are Not Same Month !!" });
                        //result.Message = "FormDate And ToDate Are Not Same Month !!";
                        return result;
                    }
                    var objDateList = GetDateRange(obj.formDate, obj.toDate);
                    obj.employeeSl.ToList().ForEach(emp =>
                    {
                        var dbDateList = new List<string>();                       
                        var dtIsRange = _dgCommon.get_InformationDataTable("select pl_empserial,pl_Startdate,pl_Enddate from dg_employeelist_tiffin_bill_process where pl_empserial=" + emp + " and pl_salary_month=" + procMonth + " and pl_salary_year=" + procYear, _connection);
                        var dtEmpNo = _dgCommon.get_InformationDataTable("select emp_no from dg_pay_Employee where emp_serial=" + emp, _connection);
                        int empNo = int.Parse(dtEmpNo.Rows[0]["emp_no"].ToString());
                        if (dtIsRange.Rows.Count > 0)
                        {
                            dtIsRange.AsEnumerable().ToList().ForEach(row =>
                            {
                                string dbFormDate = Convert.ToDateTime(row["pl_Startdate"]).ToString("MM/dd/yyyy");
                                string dbToDate = Convert.ToDateTime(row["pl_Enddate"]).ToString("MM/dd/yyyy");
                                dbDateList.AddRange(GetDateRange(dbFormDate, dbToDate));
                            });
                        }
                        bool isAnyDateEmp = dbDateList.Intersect(objDateList).Any();
                        if (!isAnyDateEmp)
                        {
                            bool isSave = _dgCommon.saveChanges("dg_pay_EmployeeTiffinBillProcess " + obj.companyId + ",'" + obj.formDate + "','" + obj.toDate + "'," + emp + ",'" + obj.userName + "'", _connection);
                            if (isSave)
                            {                               
                                for (var sDate = Convert.ToDateTime(obj.formDate); sDate <= Convert.ToDateTime(obj.toDate); sDate = sDate.AddDays(1))
                                {
                                    single_date.Add(new TiffinBillProcessSingle_date
                                    {
                                        pl_empserial = emp,
                                        pl_compid = obj.companyId,
                                        pl_salary_month = Convert.ToDateTime(obj.formDate).Month,
                                        pl_salary_year = Convert.ToDateTime(obj.formDate).Year,
                                        pl_Startdate = obj.formDate,
                                        pl_Enddate = obj.toDate,
                                        pl_date = sDate.ToString(),
                                        pl_user = obj.userName
                                    });
                                }
                                //var dtSingle_date = _dgCommon.ListToDataTable<TiffinBillProcessSingle_date>(single_date);
                                //if (dtSingle_date.Rows.Count > 0)
                                //{
                                //    _dgCommon.saveChangesByType("dg_employeelist_tiffin_bill_single_date_insert", _connection, new SqlParameter("@typeTable", dtSingle_date));
                                //}
                                result.Add(new ReturnObject { IsSuccess = true,Message="Employee("+ empNo + ") Save Successfully !!" });
                            }
                            else
                            {
                                result.Add(new ReturnObject { IsSuccess = false, Message = "Employee(" + empNo + ") Save Fail !!" });
                            }
                        }
                        else
                        {
                            result.Add(new ReturnObject { IsSuccess = false, Message = "Employee(" + empNo + ") Date Range Any Date Already Exists !!" });
                        }
                    });
                    var dtSingle_date = _dgCommon.ListToDataTable<TiffinBillProcessSingle_date>(single_date);
                    if (dtSingle_date.Rows.Count > 0)
                    {
                        _dgCommon.saveChangesByType("dg_employeelist_tiffin_bill_single_date_insert", _connection, new SqlParameter("@typeTable", dtSingle_date));
                    }
                    //result.IsSuccess = true;
                    //result.Message = "Process Successfully !!";
                    return result;
                }
                result.Add(new ReturnObject { IsSuccess = false, Message = "Salary Already Confirm This Month(" + Convert.ToDateTime(obj.formDate).ToString("MMMM-yyyy") + ") !!" });
                //result.Message = "Salary Already Confirm This Month(" + Convert.ToDateTime(obj.formDate).ToString("MMMM-yyyy") + ") !!";
                return result;
            }
            result.Add(new ReturnObject { IsSuccess = false, Message = "You Can Not Check Any Employee !!" });
            //result.Message = "You Can Not Check Any Employee !!";
            return result;
        }
        public async Task<ReturnObject> GetEmployeeTiffinNightBillProcess(int companyID,string monthYear)
        {
            var result = new ReturnObject();
            var dt = await _dgCommon.get_InformationDataTableAsync("dg_pay_emp_tiffinNight_bill_process "+ companyID + ",'"+ monthYear + "'", _connection);
            if(dt.Rows.Count > 0)
            {
                result.IsSuccess = true;
                result.Message = "Data Loaded !!";
                result.dataTable = dt;
                return result;
            }
            result.Message = "No Data Available !!";
            return result;
        }
        public ReturnObject DeleteEmployeeTiffinBillProcess(TiffinBillProcessDelPayload obj)
        {
            var result = new ReturnObject();
            if (obj.sl_No.Length > 0)
            {
                int procMonth = Convert.ToDateTime(obj.monthYear).Month;
                int procYear = Convert.ToDateTime(obj.monthYear).Year;
                var dtSalConf = _dgCommon.get_InformationDataTable("select TOP 1 ss_emp_serial from dg_pay_salarysheet where ss_compid=" + obj.companyId + " and month(ss_date)=" + procMonth + " and year(ss_date)=" + procYear + " and ss_confirmed=1", _connection);
                bool isSalConf = dtSalConf.Rows.Count > 0 ? true : false;
                if (!isSalConf)
                {
                    obj.sl_No.ToList().ForEach(emp =>
                    {
                        _dgCommon.saveChanges("dg_pay_emp_tiffinNight_bill_process_delete " + obj.companyId + "," + emp + ",'" + obj.monthYear + "'", _connection);
                    });
                    result.IsSuccess = true;
                    result.Message = "Delete Successfully !!";
                    return result;
                }
                result.Message = "Salary Already Confirm This Month(" + Convert.ToDateTime(obj.monthYear).ToString("MMMM-yyyy") + ") !!";
                return result;
            }
            result.Message = "You Can Not Check Any Employee !!";
            return result;
        }

        private List<string> GetDateRange(string fromDate, string toDate)
        {
            var sFormDate = Convert.ToDateTime(fromDate);
            var sToDate = Convert.ToDateTime(toDate);
            int totalDays = (sToDate - sFormDate).Days + 1;
            var dateList = new List<string>();
            for (int i = 0; i < totalDays; i++)
            {
                dateList.Add(sFormDate.AddDays(i).ToString("MM/dd/yyyy"));
            }
            return dateList;
        }
    }
}
