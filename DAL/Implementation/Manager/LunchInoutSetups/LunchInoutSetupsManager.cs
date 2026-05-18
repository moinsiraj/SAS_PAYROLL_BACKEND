using BLL.Interfaces.Manager.LunchInoutSetups;
using BLL.Utility;
using BOL.Models;
using DAL.Data;
using DAL.Implementation.Repository.LunchInoutSetups;
using EF.Core.Repository.Manager;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Implementation.Manager.LunchInoutSetups
{
    public class LunchInoutSetupsManager : CommonManager<LunchInoutSetup_DbModel>,ILunchInoutSetupsManager
    {
        private readonly Dg_Common _dgCommon;
        private readonly SqlConnection _payCon;
        public LunchInoutSetupsManager(dg_hrpayrollContext context, Dg_Common dgCommon) : base(new LunchInoutSetupsRepository(context))
        {
            _dgCommon = dgCommon;
            _payCon = new SqlConnection(Getway.Dg_Payroll);
        }

        public List<ReturnObject> SaveEmployeeLunchOutHistory(EmployeeLunchOutPayload obj)
        {
            var result = new List<ReturnObject>();
            var lunchOutList = new List<EmpLunchOutList>();
            if (obj.employeeInfo.Count > 0)
            {
                obj.employeeInfo.ToList().ForEach(emp =>
                {
                    obj.lunchOutDate.ToList().ForEach(Date =>
                    {
                        int empMonth = Convert.ToDateTime(Date).Month;
                        int empYear = Convert.ToDateTime(Date).Year;
                        var dtConfSal = _dgCommon.get_InformationDataTable("select ss_emp_serial from dg_pay_salarysheet where ss_emp_serial=" + emp.employeeSl + " and month(ss_date)=" + empMonth + " and year(ss_date)=" + empYear + " and ss_confirmed=1", _payCon);
                        bool isConfSal = dtConfSal.Rows.Count > 0 ? true : false;
                        if (!isConfSal)
                        {
                            var dtattnData = _dgCommon.get_InformationDataTable("select at_emp_serial from dg_pay_attendance where at_compid=" + obj.companyID + " and at_emp_serial=" + emp.employeeSl + " and at_date='" + Date + "'", _payCon);
                            bool isAttnData = dtattnData.Rows.Count > 0 ? true : false;
                            if (isAttnData)
                            {
                                var dtLo = _dgCommon.get_InformationDataTable("select lo_empNo from dg_pay_lunchoutHistory where lo_compid=" + obj.companyID + " and lo_empSerial=" + emp.employeeSl + " and lo_date='" + Date + "'", _payCon);
                                bool isLo = dtLo.Rows.Count > 0 ? true : false;
                                if (!isLo)
                                {
                                    var dtAttn = _dgCommon.get_InformationDataTable("select at_emp_serial from dg_pay_attendance where at_emp_serial=" + emp.employeeSl + " and at_date='" + Date + "' and (at_holiday<>'' or (rtrim(at_status_code)='AB' OR rtrim(at_status_code)='AL' OR rtrim(at_status_code)='CL' OR rtrim(at_status_code)='ML' OR rtrim(at_status_code)='M/L' OR rtrim(at_status_code)='SL' OR rtrim(at_status_code)='LWP' OR rtrim(at_status_code)='COM' OR rtrim(at_status_code)='M/C'))", _payCon);
                                    bool isExistsLvWhSh = dtAttn.Rows.Count > 0 ? true : false;
                                    if (!isExistsLvWhSh)
                                    {
                                        lunchOutList.Add(new EmpLunchOutList
                                        {
                                            companyID = obj.companyID,
                                            employeeNo = emp.employeeNo,
                                            employeeSerial = emp.employeeSl,
                                            lunchOutDate = Date,
                                            status = "LO",
                                            userName = obj.userName
                                        });
                                    }
                                    else
                                    {
                                        result.Add(new ReturnObject
                                        {
                                            Message = "Employee(" + emp.employeeNo + ") May Be Absent,Leave Or Weekly/Special Holiday This Date(" + Convert.ToDateTime(Date).ToString("dd/MMM/yyyy") + ") !!"
                                        });
                                    }
                                }
                                else
                                {
                                    result.Add(new ReturnObject
                                    {
                                        Message = "Employee(" + emp.employeeNo + ") Already Set Lunch Out This Date(" + Convert.ToDateTime(Date).ToString("dd/MMM/yyyy") + ") !!"
                                    });
                                }
                            }
                            else
                            {
                                result.Add(new ReturnObject
                                {
                                    Message = "Employee(" + emp.employeeNo + ") Attendance Not Exists This Date(" + Convert.ToDateTime(Date).ToString("dd/MMM/yyyy") + ") !!"
                                });
                            }
                        }
                        else
                        {
                            result.Add(new ReturnObject
                            {
                                Message = "Employee(" + emp.employeeNo + ") Salary Already Confirmed This Month(" + Convert.ToDateTime(Date).ToString("MMMM-yyyy") + ") !!"
                            });
                        }
                    });
                });


                var dtLunchOut = _dgCommon.ListToDataTable<EmpLunchOutList>(lunchOutList);
                if (dtLunchOut.Rows.Count > 0)
                {
                    var attUpdate = _dgCommon.get_InformationDataTableByType("Dg_Pay_GetAttendanceByLunchOutType", _payCon, new SqlParameter("@lunchOutType", dtLunchOut));
                    if (attUpdate.Rows.Count > 0)
                    {
                        attUpdate.AsEnumerable().ToList().ForEach(att =>
                        {
                            string at_status_code = att.Field<string>("at_status_code").ToString().Trim();
                            string at_holiday = att.Field<string>("at_holiday").ToString().Trim();
                            int employeeNo = Convert.ToInt32(att.Field<int>("lo_EmpID"));
                            string attDate = Convert.ToDateTime(att.Field<DateTime>("at_date")).ToString("dd/MMM/yyyy");
                            if (at_status_code == "" || at_status_code == "LA")
                            {
                                att["at_lo_status"] = "LO";
                                result.Add(new ReturnObject
                                {
                                    IsSuccess = true,
                                    Message = "Employee(" + employeeNo + ") Date(" + attDate + ") Save Successfully !!"
                                });
                            }
                        });
                        _dgCommon.saveChangesByType("Dg_Pay_UpdateAtten_AfterInsertLunchOut", _payCon, new SqlParameter("@attUpdateTable", attUpdate));
                    }
                    _dgCommon.saveChangesByType("Dg_Pay_SaveEmpLunchOut", _payCon, new SqlParameter("@lunchOutType", dtLunchOut));
                }
            }
            else
            {
                result.Add(new ReturnObject
                {
                    Message = "You Can Not Check Any Employee From List !!"
                });
            }
            return result;
        }
        public async Task<ReturnObject> GetEmployeeInfoForLunchOut(LunchOutEmployeeInfo obj)
        {
            var result = new ReturnObject();
            var dateLs = new List<string>();
            obj.dateRange.ToList().ForEach(date =>
            {
                dateLs.Add("''"+ date + "''");
            });
            string dateRng = "in(" + string.Join(",", dateLs) + ")";
            var dtEmpInfo = await _dgCommon.get_InformationDataTableAsync("Dg_Pay_EmployeeInfoForLunchOut " + obj.companyID + "," + obj.departmentID + "," + obj.sectionID + ",'" + dateRng + "'", _payCon);
            if (dtEmpInfo.Rows.Count > 0)
            {
                result.IsSuccess = true;
                result.Message = "Data Loaded !!";
                result.dataTable = dtEmpInfo;
                return result;
            }
            result.Message = "No Data Available !!";
            return result;
        }
        public List<ReturnObject> DeleteEmployeeLunchOut(EmployeeLunchOutPayload obj)
        {
            var result = new List<ReturnObject>();
            var lunchOutList = new List<EmpLunchOutList>();
            obj.employeeInfo.ToList().ForEach(emp =>
            {
                obj.lunchOutDate.ToList().ForEach(Date =>
                {
                    int empMonth = Convert.ToDateTime(Date).Month;
                    int empYear = Convert.ToDateTime(Date).Year;
                    var dtConfSal = _dgCommon.get_InformationDataTable("select ss_emp_serial from dg_pay_salarysheet where ss_emp_serial=" + emp.employeeSl + " and month(ss_date)=" + empMonth + " and year(ss_date)=" + empYear + " and ss_confirmed=1", _payCon);
                    bool isConfSal = dtConfSal.Rows.Count > 0 ? true : false;
                    if (!isConfSal)
                    {
                        var dtLo = _dgCommon.get_InformationDataTable("select lo_empNo from dg_pay_lunchoutHistory where lo_compid=" + obj.companyID + " and lo_empSerial=" + emp.employeeSl + " and lo_date='" + Date + "'", _payCon);
                        bool isLo = dtLo.Rows.Count > 0 ? true : false;
                        if (isLo)
                        {
                            lunchOutList.Add(new EmpLunchOutList
                            {
                                companyID = obj.companyID,
                                employeeNo = emp.employeeNo,
                                employeeSerial = emp.employeeSl,
                                lunchOutDate = Date,
                                status = "LO",
                                userName = obj.userName
                            });
                        }
                        else
                        {
                            result.Add(new ReturnObject
                            {
                                Message = "Employee(" + emp.employeeNo + ") Lunch Out Not Found This Date(" + Convert.ToDateTime(Date).ToString("dd/MMM/yyyy") + ") !!"
                            });
                        }
                    }
                    else
                    {
                        result.Add(new ReturnObject
                        {
                            Message = "Employee(" + emp.employeeNo + ") Salary Already Confirmed This Month(" + Convert.ToDateTime(Date).ToString("MMMM-yyyy") + ") !!"
                        });
                    }
                });
            });
            var dtLunchOut = _dgCommon.ListToDataTable<EmpLunchOutList>(lunchOutList);
            if (dtLunchOut.Rows.Count > 0)
            {
                var attUpdate = _dgCommon.get_InformationDataTableByType("Dg_Pay_GetAttendanceByLunchOutType", _payCon, new SqlParameter("@lunchOutType", dtLunchOut));
                if (attUpdate.Rows.Count > 0)
                {
                    attUpdate.AsEnumerable().ToList().ForEach(att =>
                    {
                        string at_lo_status = att.Field<string>("at_lo_status").ToString().Trim();
                        int employeeNo = Convert.ToInt32(att.Field<int>("lo_EmpID"));
                        string attDate = Convert.ToDateTime(att.Field<DateTime>("at_date")).ToString("dd/MMM/yyyy");
                        if (at_lo_status == "LO")
                        {
                            att["at_lo_status"] = null;
                            result.Add(new ReturnObject
                            {
                                IsSuccess = true,
                                Message = "Employee(" + employeeNo + ") Date("+ attDate + ") Delete Successfully !!"
                            });
                        }
                    });
                    _dgCommon.saveChangesByType("Dg_Pay_UpdateAtten_AfterInsertLunchOut", _payCon, new SqlParameter("@attUpdateTable", attUpdate));
                }
                _dgCommon.saveChangesByType("Dg_Pay_DeleteEmployeeLunchOut", _payCon, new SqlParameter("@lunchOutType", dtLunchOut));
            }
            return result;
        }
    }
}