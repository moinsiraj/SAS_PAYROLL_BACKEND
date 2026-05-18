using BLL.Interfaces.Manager.AttcoveringDay;
using BLL.Utility;
using BOL.Models;
using DAL.Data;
using DAL.Implementation.Repository.AttcoveringDay;
using EF.Core.Repository.Manager;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Implementation.Manager.AttcoveringDay
{
    public class AttCoveringDayManager : CommonManager<AttcoveringDay_DbModel>, IAttCoveringDayManager
    {
        private readonly Dg_Common _dgCommon;
        private readonly SqlConnection _payCon;
        public AttCoveringDayManager(dg_hrpayrollContext context, Dg_Common dgCommon) : base(new AttCoveringDayRepository(context))
        {
            _dgCommon = dgCommon;
            _payCon = new SqlConnection(Getway.Dg_Payroll);
        }

        public async Task<DataTable> GetEmployeeListForCoverday(int Compid, int Department, int section, int Building, int Floor, int Line, int Shift, int Grade, int salcat, string formDate, string toDate)
        {
            var dateLs = new List<string>();
            for (var Date = Convert.ToDateTime(formDate); Date <= Convert.ToDateTime(toDate); Date = Date.AddDays(1))
            {
                dateLs.Add("''" + Date + "''");
            }
            string dateRng = "in(" + string.Join(",", dateLs) + ")";
            var data = await _dgCommon.get_InformationDataTableAsync("Emp_filtering_list_for_SH_and_Cov " + Compid + "," + Department + "," + section + "," + Building + "," + Floor + "," + Line + "," + Shift + "," + Grade + "," + salcat + ",'" + dateRng + "'", _payCon);
            return data;
        }
        public List<ReturnObject> SaveEmpWiseCoveringDay(EmpWiseCoveringdayPayload obj)
        {
            var result = new List<ReturnObject>();
            try
            {
                int formDateMonth = Convert.ToDateTime(obj.formDate).Month;
                int formDateYear = Convert.ToDateTime(obj.formDate).Year;
                int toDateMonth = Convert.ToDateTime(obj.toDate).Month;
                int toDateYear = Convert.ToDateTime(obj.toDate).Year;
                int currentMonth = DateTime.Now.Month;
                int currentYear = DateTime.Now.Year;
                int prvMonth = DateTime.Now.AddDays(-10).Month;
                int prvYear = DateTime.Now.AddDays(-10).Year;
                if (((formDateMonth == prvMonth && formDateYear == prvYear) || (formDateMonth == currentMonth && formDateYear == currentYear)) && ((toDateMonth == prvMonth && toDateYear == prvYear) || (toDateMonth == currentMonth && toDateYear == currentYear)))
                {
                    if (obj.empInfos.Count > 0)
                    {
                        //var dtConfSal = _dgCommon.get_InformationDataTable("select ss_emp_serial from dg_pay_salarysheet where ss_compid=" + obj.companyID + " and (month(ss_date)=" + formDateMonth + " or month(ss_date)=" + toDateMonth + " or month(ss_date)=" + prvMonth + ") and (year(ss_date)=" + formDateYear + " or year(ss_date)=" + toDateYear + " or year(ss_date)=" + prvYear + ") and ss_confirmed=1", _payCon);
                        var dtConfSal = _dgCommon.get_InformationDataTable("select ss_emp_serial from dg_pay_salarysheet where ss_compid=" + obj.companyID + " and (month(ss_date)=" + formDateMonth + " or month(ss_date)=" + toDateMonth + ") and (year(ss_date)=" + formDateYear + " or year(ss_date)=" + toDateYear + ") and ss_confirmed=1", _payCon);
                        bool isConfSal = dtConfSal.Rows.Count > 0 ? true : false;
                        if (!isConfSal)
                        {
                            var Coveringday = new List<AttcoveringDayPayloadList>();
                            var EcardSum = new List<AttcoveringDayEcardSum>();
                            obj.empInfos.ToList().ForEach(empInfo =>
                            {
                                for (var sDate = Convert.ToDateTime(obj.formDate); sDate <= Convert.ToDateTime(obj.toDate); sDate = sDate.AddDays(1))
                                {
                                    string sDateN =  sDate.ToString("MM-dd-yyyy");
                                    var dtExistsCov = _dgCommon.get_InformationDataTable("select cd_empSerial from dg_pay_attcovering_days_empWise where cd_empSerial=" + empInfo.emp_serial + " and cd_covDate='" + sDateN + "'", _payCon);
                                    bool isExistsCov = dtExistsCov.Rows.Count > 0 ? true : false;
                                    if (!isExistsCov)
                                    {
                                        var dtExistsAttnSH = _dgCommon.get_InformationDataTable("select at_emp_serial from dg_pay_attendance where at_emp_serial=" + empInfo.emp_serial + " and at_date='" + sDateN + "' and at_holiday='WH'", _payCon);
                                        bool isExtstsAttnSH = dtExistsAttnSH.Rows.Count > 0 ? true : false;
                                        if (isExtstsAttnSH)
                                        {
                                            Coveringday.Add(new AttcoveringDayPayloadList
                                            {
                                                comp_id = obj.companyID,
                                                emp_serial_id = empInfo.emp_serial,
                                                emp_no = empInfo.emp_no,
                                                shiftDate = sDateN,
                                                fDate = obj.formDate,
                                                tDate = obj.toDate,
                                                acHoliday = obj.acHoliday,
                                                isGov = false,
                                                user = obj.userName
                                            });
                                            result.Add(new ReturnObject { IsSuccess = true, Message = "Employee(" + empInfo.emp_no + ") Date(" + Convert.ToDateTime(sDate).ToString("dd/MMM/yyyy") + ") Save Successfully !!" });
                                        }
                                        else
                                        {
                                            result.Add(new ReturnObject { Message = "Employee(" + empInfo.emp_no + ") Is Not Weekly Holiday This Date(" + Convert.ToDateTime(sDate).ToString("dd/MMM/yyyy") + ") !!" });
                                        }
                                    }
                                    else
                                    {
                                        result.Add(new ReturnObject { Message = "Employee(" + empInfo.emp_no + ") Covering Day Already Exists This Date(" + Convert.ToDateTime(sDate).ToString("dd/MMM/yyyy") + ") !!" });
                                    }
                                }
                            });
                            var dtCoveringday = _dgCommon.ListToDataTable<AttcoveringDayPayloadList>(Coveringday);
                            if (dtCoveringday.Rows.Count > 0)
                            {
                                var attUpdate = _dgCommon.get_InformationDataTableByType("Dg_Pay_GetAttendance_list_ForCoveringDay", _payCon, new SqlParameter("@covTable", dtCoveringday));
                                if (attUpdate.Rows.Count > 0)
                                {
                                    attUpdate.AsEnumerable().ToList().ForEach(x =>
                                    {
                                        string at_status_code = x.Field<string>("at_status_code").ToString().Trim();
                                        int employeeId = Convert.ToInt32(x.Field<int>("empNo"));
                                        string at_date = Convert.ToDateTime(x.Field<DateTime>("at_date")).ToString("dd/MMM/yyyy");
                                        if (at_status_code == "WH")
                                        {
                                            x["at_status"] = "Absent";
                                            x["at_status_code"] = "AB";
                                            x["at_holiday"] = "";
                                            x["at_coverday"] = true;
                                            //result.Add(new ReturnObject { IsSuccess = true, Message = "Employee(" + employeeId + ") Date(" + Convert.ToDateTime(at_date).ToString("dd/MMM/yyyy") + ") Save Successfully !!" });
                                        }
                                        else
                                        {
                                            x["at_holiday"] = "";
                                            x["at_coverday"] = true;
                                            //result.Add(new ReturnObject { IsSuccess = true, Message = "Employee(" + employeeId + ") Date(" + Convert.ToDateTime(at_date).ToString("dd/MMM/yyyy") + ") Save Successfully !!" });
                                        }
                                    });
                                    _dgCommon.saveChangesByType("Dg_Pay_UpdateAtten_afterChangeCov", _payCon, new SqlParameter("@attUpdateTable", attUpdate));
                                    var distinctdtCoveringday = dtCoveringday.AsEnumerable().GroupBy(row => Convert.ToInt32(row.Field<string>("emp_serial_id"))).Select(group => group.First()).CopyToDataTable();
                                    distinctdtCoveringday.AsEnumerable().ToList().ForEach(r =>
                                    {
                                        int compID = Convert.ToInt32(r.Field<string>("comp_id"));
                                        int empNo = Convert.ToInt32(r.Field<string>("emp_no"));
                                        int empSerial = Convert.ToInt32(r.Field<string>("emp_serial_id"));
                                        string shiftDate = r.Field<string>("shiftDate").ToString();
                                        var dtTotalP = _dgCommon.get_InformationDataTable("select isnull(count(dg_pay_Attendance.at_date),0) as ttlP from dg_pay_Attendance where dg_pay_Attendance.at_emp_serial=" + empSerial + " and (RTRIM(dg_pay_Attendance.at_status_code) ='' or RTRIM(dg_pay_Attendance.at_status_code) ='LA ') and Month(dg_pay_Attendance.at_date)=Month('" + shiftDate + "') and Year(dg_pay_Attendance.at_date)=Year('" + shiftDate + "') and RTRIM(dg_pay_Attendance.at_holiday)=''", _payCon);
                                        var dtTotalAB = _dgCommon.get_InformationDataTable("select isnull(count(dg_pay_Attendance.at_date),0) as ttlAB from dg_pay_Attendance  where dg_pay_Attendance.at_emp_serial=" + empSerial + " and RTRIM(dg_pay_Attendance.at_status_code) ='AB' and Month(dg_pay_Attendance.at_date)=Month('" + shiftDate + "') and Year(dg_pay_Attendance.at_date)=Year('" + shiftDate + "')", _payCon);
                                        var dtTotalL = _dgCommon.get_InformationDataTable("select isnull(Count(dg_pay_Attendance.at_date),0) as ttlL from dg_pay_Attendance  where dg_pay_Attendance.at_emp_serial=" + empSerial + " and (RTRIM(dg_pay_Attendance.at_status_code) ='LA') and Month(dg_pay_Attendance.at_date)=Month('" + shiftDate + "') and Year(dg_pay_Attendance.at_date)=Year('" + shiftDate + "') and RTRIM(dg_pay_Attendance.at_holiday)<>'WH'", _payCon);
                                        var dtTotalWH = _dgCommon.get_InformationDataTable("select isnull(count(dg_pay_Attendance.at_date),0) as ttlWH from dg_pay_Attendance  where dg_pay_Attendance.at_emp_serial=" + empSerial + " and RTRIM(dg_pay_Attendance.at_holiday)='WH'  and Month(dg_pay_Attendance.at_date)=Month('" + shiftDate + "') and Year(dg_pay_Attendance.at_date)=Year('" + shiftDate + "')", _payCon);
                                        var dtTotalH = _dgCommon.get_InformationDataTable("select isnull(count(dg_pay_Attendance.at_date),0) as ttlH from dg_pay_Attendance  where dg_pay_Attendance.at_emp_serial=" + empSerial + " and (RTRIM(dg_pay_Attendance.at_status_code)='SH' OR RTRIM(dg_pay_Attendance.at_holiday)='SH') and Month(dg_pay_Attendance.at_date)=Month('" + shiftDate + "') and Year(dg_pay_Attendance.at_date)=Year('" + shiftDate + "')", _payCon);                                      
                                        EcardSum.Add(new AttcoveringDayEcardSum
                                        {
                                            comp_id = compID,
                                            emp_id = empNo,
                                            Present = dtTotalP.Rows.Count > 0 ? Convert.ToInt32(dtTotalP.Rows[0]["ttlP"]) : 0,
                                            Absent = dtTotalAB.Rows.Count > 0 ? Convert.ToInt32(dtTotalAB.Rows[0]["ttlAB"]) : 0,
                                            Late = dtTotalL.Rows.Count > 0 ? Convert.ToInt32(dtTotalL.Rows[0]["ttlL"]) : 0,
                                            WeekHoliday = dtTotalWH.Rows.Count > 0 ? Convert.ToInt32(dtTotalWH.Rows[0]["ttlWH"]) : 0,
                                            SpecialHoliday = dtTotalH.Rows.Count > 0 ? Convert.ToInt32(dtTotalH.Rows[0]["ttlH"]) : 0,
                                            shDate = shiftDate
                                        });
                                    });
                                    var dtEcardSum = _dgCommon.ListToDataTable<AttcoveringDayEcardSum>(EcardSum);
                                    _dgCommon.saveChangesByType("Dg_Pay_EcardSum_For_SH", _payCon, new SqlParameter("@ecardSumTable", dtEcardSum));
                                }
                                _dgCommon.saveChangesByType("Dg_Pay_Saveattcovering_days_empWise", _payCon, new SqlParameter("@typeTable", dtCoveringday));
                            }
                        }
                        else
                        {
                            result.Add(new ReturnObject { Message = "Salary Already Confirmed This Month(" + Convert.ToDateTime(obj.formDate).ToString("MMMM-yyyy") + ") !!" });
                        }
                    }
                    else
                    {
                        result.Add(new ReturnObject { Message = "You Can Not Check Any Employee From List !!" });
                    }
                }
                else
                {
                    result.Add(new ReturnObject { Message = "Previous Month Date Range Is Not Valid Date Range !!" });
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                result.Add(new ReturnObject { Message = "Something Went Wrong !!" });
            }
            return result;
        }
        public async Task<ReturnObject> GetCreatedCoverdingDays_List(int compid, string formDate, string toDate)
        {
            var result = new ReturnObject();
            try
            {
                var data = await _dgCommon.get_InformationDataTableAsync("Dg_Pay_CreatedCoveringdays_List " + compid + ",'" + formDate + "','" + toDate + "'", _payCon);
                if (data.Rows.Count > 0)
                {
                    result.IsSuccess = true;
                    result.Message = "Data Loaded !!";
                    result.dataTable = data;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = "Data Not Loaded !!";
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return result;
        }
        public List<ReturnObject> DeleteEmpWiseSpecialholiday(DeleteCoveringdayPayload obj)
        {
            var result = new List<ReturnObject>();
            try
            {
                if (obj.delChilds.Count > 0)
                {
                    var dtConfSal = _dgCommon.get_InformationDataTable("select ss_emp_serial from dg_pay_salarysheet where ss_compid=" + obj.compID + " and (month(ss_date)=" + obj.formDate + " or month(ss_date)=" + obj.toDate + ") and (year(ss_date)=" + obj.formDate + " or year(ss_date)=" + obj.toDate + ") and ss_confirmed=1", _payCon);
                    bool isConfSal = dtConfSal.Rows.Count > 0 ? true : false;
                    if (!isConfSal)
                    {
                        var Coveringday = new List<AttcoveringDayPayloadList>();
                        var EcardSum = new List<AttcoveringDayEcardSum>();
                        obj.delChilds.ToList().ForEach(empInfo =>
                        {
                            for (var sDate = Convert.ToDateTime(obj.formDate); sDate <= Convert.ToDateTime(obj.toDate); sDate = sDate.AddDays(1))
                            {
                                string sDateN = sDate.ToString("MM-dd-yyyy");
                                Coveringday.Add(new AttcoveringDayPayloadList
                                {
                                    comp_id = obj.compID,
                                    emp_serial_id = empInfo.empSerial,
                                    emp_no = empInfo.empNumber,
                                    shiftDate = sDateN,
                                    fDate = obj.formDate,
                                    tDate = obj.toDate,
                                    acHoliday = string.Empty,
                                    isGov = false,
                                    user = string.Empty
                                });
                                result.Add(new ReturnObject { IsSuccess = true, Message = "Employee(" + empInfo.empNumber + ") Date(" + Convert.ToDateTime(sDate).ToString("dd/MMM/yyyy") + ") Delete Successfully !!" });
                            }
                        });
                        var dtCoveringday = _dgCommon.ListToDataTable<AttcoveringDayPayloadList>(Coveringday);
                        if (dtCoveringday.Rows.Count > 0)
                        {
                            var attUpdate = _dgCommon.get_InformationDataTableByType("Dg_Pay_GetAttendance_list_ForCoveringDay", _payCon, new SqlParameter("@covTable", dtCoveringday));
                            if (attUpdate.Rows.Count > 0)
                            {
                                attUpdate.AsEnumerable().ToList().ForEach(x =>
                                {
                                    string at_status_code = x.Field<string>("at_status_code").ToString().Trim();
                                    string at_actu_holiday = x.Field<string>("at_actu_holiday").ToString().Trim();
                                    string at_dateName = Convert.ToDateTime(x.Field<DateTime>("at_date")).ToString("dddd");
                                    int employeeId = Convert.ToInt32(x.Field<int>("empNo"));
                                    string at_date = Convert.ToDateTime(x.Field<DateTime>("at_date")).ToString("dd/MMM/yyyy");
                                    if (at_status_code == "AB" && at_actu_holiday == "WH")
                                    {
                                        x["at_status"] = at_dateName;
                                        x["at_status_code"] = "WH";
                                        x["at_holiday"] = "WH";
                                        x["at_coverday"] = false;
                                        //result.Add(new ReturnObject { IsSuccess = true, Message = "Employee(" + employeeId + ") Date(" + Convert.ToDateTime(at_date).ToString("dd/MMM/yyyy") + ") Delete Successfully !!" });
                                    }
                                    else if ((at_status_code == "" || at_status_code == "LA") && at_actu_holiday == "WH")
                                    {
                                        x["at_holiday"] = "WH";
                                        x["at_coverday"] = false;
                                        //result.Add(new ReturnObject { IsSuccess = true, Message = "Employee(" + employeeId + ") Date(" + Convert.ToDateTime(at_date).ToString("dd/MMM/yyyy") + ") Delete Successfully !!" });
                                    }
                                });
                                _dgCommon.saveChangesByType("Dg_Pay_UpdateAtten_afterChangeCov", _payCon, new SqlParameter("@attUpdateTable", attUpdate));
                                var distinctdtCoveringday = dtCoveringday.AsEnumerable().GroupBy(row => Convert.ToInt32(row.Field<string>("emp_serial_id"))).Select(group => group.First()).CopyToDataTable();
                                distinctdtCoveringday.AsEnumerable().ToList().ForEach(r =>
                                {
                                    int compID = Convert.ToInt32(r.Field<string>("comp_id"));
                                    int empNo = Convert.ToInt32(r.Field<string>("emp_no"));
                                    int empSerial = Convert.ToInt32(r.Field<string>("emp_serial_id"));
                                    string shiftDate = r.Field<string>("shiftDate").ToString();
                                    var dtTotalP = _dgCommon.get_InformationDataTable("select isnull(count(dg_pay_Attendance.at_date),0) as ttlP from dg_pay_Attendance where dg_pay_Attendance.at_emp_serial=" + empSerial + " and (RTRIM(dg_pay_Attendance.at_status_code) ='' or RTRIM(dg_pay_Attendance.at_status_code) ='LA ') and Month(dg_pay_Attendance.at_date)=Month('" + shiftDate + "') and Year(dg_pay_Attendance.at_date)=Year('" + shiftDate + "') and RTRIM(dg_pay_Attendance.at_holiday)=''", _payCon);
                                    var dtTotalAB = _dgCommon.get_InformationDataTable("select isnull(count(dg_pay_Attendance.at_date),0) as ttlAB from dg_pay_Attendance  where dg_pay_Attendance.at_emp_serial=" + empSerial + " and RTRIM(dg_pay_Attendance.at_status_code) ='AB' and Month(dg_pay_Attendance.at_date)=Month('" + shiftDate + "') and Year(dg_pay_Attendance.at_date)=Year('" + shiftDate + "')", _payCon);
                                    var dtTotalL = _dgCommon.get_InformationDataTable("select isnull(Count(dg_pay_Attendance.at_date),0) as ttlL from dg_pay_Attendance  where dg_pay_Attendance.at_emp_serial=" + empSerial + " and (RTRIM(dg_pay_Attendance.at_status_code) ='LA') and Month(dg_pay_Attendance.at_date)=Month('" + shiftDate + "') and Year(dg_pay_Attendance.at_date)=Year('" + shiftDate + "') and RTRIM(dg_pay_Attendance.at_holiday)<>'WH'", _payCon);
                                    var dtTotalWH = _dgCommon.get_InformationDataTable("select isnull(count(dg_pay_Attendance.at_date),0) as ttlWH from dg_pay_Attendance  where dg_pay_Attendance.at_emp_serial=" + empSerial + " and RTRIM(dg_pay_Attendance.at_holiday)='WH'  and Month(dg_pay_Attendance.at_date)=Month('" + shiftDate + "') and Year(dg_pay_Attendance.at_date)=Year('" + shiftDate + "')", _payCon);
                                    var dtTotalH = _dgCommon.get_InformationDataTable("select isnull(count(dg_pay_Attendance.at_date),0) as ttlH from dg_pay_Attendance  where dg_pay_Attendance.at_emp_serial=" + empSerial + " and (RTRIM(dg_pay_Attendance.at_status_code)='SH' OR RTRIM(dg_pay_Attendance.at_holiday)='SH') and Month(dg_pay_Attendance.at_date)=Month('" + shiftDate + "') and Year(dg_pay_Attendance.at_date)=Year('" + shiftDate + "')", _payCon);

                                    EcardSum.Add(new AttcoveringDayEcardSum
                                    {
                                        comp_id = compID,
                                        emp_id = empNo,
                                        Present = dtTotalP.Rows.Count > 0 ? Convert.ToInt32(dtTotalP.Rows[0]["ttlP"]) : 0,
                                        Absent = dtTotalAB.Rows.Count > 0 ? Convert.ToInt32(dtTotalAB.Rows[0]["ttlAB"]) : 0,
                                        Late = dtTotalL.Rows.Count > 0 ? Convert.ToInt32(dtTotalL.Rows[0]["ttlL"]) : 0,
                                        WeekHoliday = dtTotalWH.Rows.Count > 0 ? Convert.ToInt32(dtTotalWH.Rows[0]["ttlWH"]) : 0,
                                        SpecialHoliday = dtTotalH.Rows.Count > 0 ? Convert.ToInt32(dtTotalH.Rows[0]["ttlH"]) : 0,
                                        shDate = shiftDate
                                    });
                                });
                                var dtEcardSum = _dgCommon.ListToDataTable<AttcoveringDayEcardSum>(EcardSum);
                                _dgCommon.saveChangesByType("Dg_Pay_EcardSum_For_SH", _payCon, new SqlParameter("@ecardSumTable", dtEcardSum));
                            }
                            _dgCommon.saveChangesByType("Dg_Pay_DeleteCoveringDay", _payCon, new SqlParameter("@typeTable", dtCoveringday));
                        }
                    }
                    else
                    {
                        result.Add(new ReturnObject { Message = "Salary Already Confirmed This Month(" + Convert.ToDateTime(obj.formDate).ToString("MMMM-yyyy") + ") !!" });
                    }
                }
                else
                {
                    result.Add(new ReturnObject { Message = "You Can Not Check Any Employee From List !!" });
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                result.Add(new ReturnObject { Message = "Something Went Wrong !!" });
            }
            return result;
        }
    }
}