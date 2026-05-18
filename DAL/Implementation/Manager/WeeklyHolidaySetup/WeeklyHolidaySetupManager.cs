using BLL.Interfaces.Manager.WeeklyHolidaySetup;
using BLL.Utility;
using BOL.Models;
using System.Data.SqlClient;

namespace DAL.Implementation.Manager.WeeklyHolidaySetup
{
    public class WeeklyHolidaySetupManager : IWeeklyHolidaySetupManager
    {
        private readonly Dg_Common _dgCommon;
        private readonly SqlConnection _connection;
        public WeeklyHolidaySetupManager(Dg_Common dgCommon)
        {
            _dgCommon = dgCommon;
            _connection = new SqlConnection(Getway.Dg_Payroll);
        }

        public List<ReturnObject> SaveWeeklyHolidaySetup(WeeklyHolidayPayload obj)
        {
            var result = new List<ReturnObject>();
            try
            {                              
                if (obj.employeeBasic.Count > 0)
                {
                    int WH_Month = Convert.ToDateTime(obj.monthYear).Month;
                    int WH_Year = Convert.ToDateTime(obj.monthYear).Year;
                    var dtIsSalConf = _dgCommon.get_InformationDataTable("SELECT ss_emp_serial FROM dg_pay_salarysheet WHERE ss_compid=" + obj.employeeBasic[0].companyID + " AND MONTH(ss_date)=" + WH_Month + " AND YEAR(ss_date)=" + WH_Year + " AND ss_confirmed=1", _connection);
                    bool isSalConf = dtIsSalConf.Rows.Count > 0 ? true : false;
                    if (!isSalConf)
                    {
                        foreach (var employee in obj.employeeBasic)
                        {                          
                            if (obj.whType == "Temporary")
                            {
                                _dgCommon.saveChanges("DELETE dg_weekly_holiday_setup WHERE wh_emp_serial=" + employee.employeeSL + " AND wh_Type='Temporary' AND wh_month=" + WH_Month + " AND wh_year=" + WH_Year, _connection);
                                if (obj.holiDate.Length > 0)
                                {
                                    foreach (var hDate in obj.holiDate)
                                    {
                                        var dtLeave = _dgCommon.get_InformationDataTable("SELECT at_status_code FROM dg_pay_attendance WHERE at_date='"+ hDate + "' AND at_emp_serial="+ employee.employeeSL + " AND (RTRIM(at_status_code)='ML' or RTRIM(at_status_code)='AL' or RTRIM(at_status_code)='CL' or RTRIM(at_status_code)='SL' or RTRIM(at_status_code)='LWP' or RTRIM(at_status_code)='M/L' or RTRIM(at_status_code)='M/C' or RTRIM(at_status_code)='ML' or RTRIM(at_status_code)='COM')", _connection);
                                        bool isLeave = dtLeave.Rows.Count > 0 ? true : false;
                                        if (!isLeave)
                                        {
                                            var dtSH = _dgCommon.get_InformationDataTable("SELECT sh_description FROM dg_pay_specialholidays_empWise WHERE sh_compid="+ employee.companyID + " AND sh_emp_serial="+ employee.employeeSL + " AND sh_date ='"+ hDate + "'", _connection);
                                            bool isSh = dtSH.Rows.Count > 0 ? true : false;
                                            if (!isSh)
                                            {
                                                var dtCov = _dgCommon.get_InformationDataTable("SELECT cd_empSerial FROM dg_pay_attcovering_days_empWise WHERE cd_compid="+ employee.companyID + " AND cd_empSerial="+ employee.employeeSL + " AND cd_covDate='" + hDate + "'", _connection);
                                                bool isCov = dtCov.Rows.Count > 0 ? true : false;
                                                if (!isCov)
                                                {
                                                    bool isSave = _dgCommon.saveChanges("Dg_Pay_SaveHolidaySetup " + employee.companyID + "," + employee.employeeNO + "," + employee.employeeSL + ",'" + hDate + "','" + obj.monthYear + "','" + obj.whType + "','" + obj.fixedDayName + "','" + obj.userName + "'", _connection);
                                                    if (isSave)
                                                    {
                                                        result.Add(new ReturnObject
                                                        {
                                                            IsSuccess = true,
                                                            Message = "Employee(" + employee.employeeNO + ") Date(" + Convert.ToDateTime(hDate).ToString("dd/MMM/yyyy") + ") Holiday Setup Successfully !!",
                                                        });
                                                    }
                                                    else
                                                    {
                                                        result.Add(new ReturnObject
                                                        {
                                                            IsSuccess = false,
                                                            Message = "Employee(" + employee.employeeNO + ") Date(" + Convert.ToDateTime(hDate).ToString("dd/MMM/yyyy") + ") Holiday Setup Failed !!",
                                                        });
                                                    }
                                                }
                                                else
                                                {
                                                    result.Add(new ReturnObject
                                                    {
                                                        IsSuccess = false,
                                                        Message = "Employee(" + employee.employeeNO + ") Date(" + Convert.ToDateTime(hDate).ToString("dd/MMM/yyyy") + ") Already Covering Day !!",
                                                    });
                                                }
                                            }
                                            else
                                            {
                                                result.Add(new ReturnObject
                                                {
                                                    IsSuccess = false,
                                                    Message = "Employee(" + employee.employeeNO + ") Date(" + Convert.ToDateTime(hDate).ToString("dd/MMM/yyyy") + ") Already Special Holiday !!",
                                                });
                                            }
                                        }
                                        else
                                        {
                                            result.Add(new ReturnObject
                                            {
                                                IsSuccess = false,
                                                Message = "Employee(" + employee.employeeNO + ") Date(" + Convert.ToDateTime(hDate).ToString("dd/MMM/yyyy") + ") Already Leave !!",
                                            });
                                        }
                                    }
                                }
                                else
                                {

                                    result.Add(new ReturnObject
                                    {
                                        IsSuccess = false,
                                        Message = "You Can Not Enter Any Weekly Holiday Date !!",
                                    });
                                }
                            }
                            else
                            {
                                _dgCommon.saveChanges("DELETE dg_weekly_holiday_setup WHERE wh_emp_serial=" + employee.employeeSL + " AND wh_Type='Permanent' AND wh_month=" + WH_Month + " AND wh_year=" + WH_Year, _connection);
                                bool isSave = _dgCommon.saveChanges("Dg_Pay_SaveHolidaySetup " + employee.companyID + "," + employee.employeeNO + "," + employee.employeeSL + ",'','" + obj.monthYear + "','" + obj.whType + "','" + obj.fixedDayName + "','" + obj.userName + "'", _connection);
                                if (isSave)
                                {
                                    result.Add(new ReturnObject
                                    {
                                        IsSuccess = true,
                                        Message = "Employee(" + employee.employeeNO + ") " + obj.fixedDayName + " Holiday Setup Successfully !!",
                                    });
                                }
                                else
                                {
                                    result.Add(new ReturnObject
                                    {
                                        IsSuccess = false,
                                        Message = "Employee(" + employee.employeeNO + ") " + obj.fixedDayName + " Holiday Setup Failed !!",
                                    });
                                }
                            }
                        }
                    }
                    else
                    {
                        result.Add(new ReturnObject { IsSuccess=false,Message= "Salary Already Confirm This Month("+Convert.ToDateTime(obj.monthYear).ToString("MMMM-yyyy") +")" });
                    }
                }
                else
                {
                    result.Add(new ReturnObject
                    {
                        IsSuccess = false,
                        Message = "You Can Not Check Any Employee !!",
                    });
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                result.Add(new ReturnObject
                {
                    IsSuccess = false,
                    Message = "Something Went Wrong !!",
                });
            }
            return result;
        }
        public async Task<ReturnObject> GetWeeklyHoliday(string monthYear, int empSerial)
        {
            var result = new ReturnObject();
            try
            {
                var data = await _dgCommon.get_InformationDataTableAsync("Dg_Pay_GetEmployeeWeeklyHoliday '"+ monthYear + "',"+ empSerial, _connection);
                if(data.Rows.Count > 0)
                {
                    result.IsSuccess = true;
                    result.Message = "Data Loaded !!";
                    result.dataTable = data;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = "Data Not Found !!";
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                result.IsSuccess = false;
                result.Message = "Something Went Wrong !!";
            }
            return result;
        }
    }
}
