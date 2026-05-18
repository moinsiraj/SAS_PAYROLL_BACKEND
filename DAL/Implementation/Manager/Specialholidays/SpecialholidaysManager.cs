using BLL.Interfaces.Manager.Specialholidays;
using BLL.Utility;
using BOL.Models;
using DAL.Data;
using DAL.Implementation.Repository.Specialholidays;
using EF.Core.Repository.Manager;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Implementation.Manager.Specialholidays
{
    public class SpecialholidaysManager : CommonManager<Specialholiday_DbModel>,ISpecialholidaysManager
    {
        private readonly Dg_Common _dgCommon;
        private readonly SqlConnection _payCon;
        public SpecialholidaysManager(dg_hrpayrollContext context, Dg_Common dgCommon) :base(new SpecialholidaysRepository(context))
        {
            _dgCommon = dgCommon;
            _payCon = new SqlConnection(Getway.Dg_Payroll);
        }

        public List<ReturnObject> SaveEmpWiseSpecialholiday(EmpWiseSpecialholidayPayload obj)
        {
            var result = new List<ReturnObject>();
            try
            {
                // Prev Code
                /*int formDateMonth = Convert.ToDateTime(obj.formDate).Month;
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
                        var dtConfSal = _dgCommon.get_InformationDataTable("select ss_emp_serial from dg_pay_salarysheet where ss_compid=" + obj.companyID + " and (month(ss_date)=" + formDateMonth + " or month(ss_date)=" + toDateMonth + " or month(ss_date)=" + prvMonth + ") and (year(ss_date)=" + formDateYear + " or year(ss_date)=" + toDateYear + " or year(ss_date)=" + prvYear + ") and ss_confirmed=1", _payCon);
                        bool isConfSal = dtConfSal.Rows.Count > 0 ? true : false;
                        if (!isConfSal)
                        {
                            var Specialholiday = new List<SpecialholidayPayloadList>();
                            var EcardSum = new List<SpecialholidayEcardSum>();
                            obj.empInfos.ToList().ForEach(empInfo =>
                            {
                                for (var sDate = Convert.ToDateTime(obj.formDate); sDate <= Convert.ToDateTime(obj.toDate); sDate = sDate.AddDays(1))
                                {
                                    var isExistsData = _dgCommon.get_InformationDataTable("select sh_emp_serial from dg_pay_specialholidays_empWise where sh_emp_serial=" + empInfo.emp_serial + " and sh_date='" + sDate + "'", _payCon);
                                    bool isExists = isExistsData.Rows.Count > 0 ? true : false;
                                    if (!isExists)
                                    {
                                        var dtExistsLeave = _dgCommon.get_InformationDataTable("select at_emp_serial from dg_pay_attendance where at_emp_serial=" + empInfo.emp_serial + " and at_date='" + sDate + "' and (rtrim(at_status_code)='AL' OR rtrim(at_status_code)='CL' OR rtrim(at_status_code)='ML' OR rtrim(at_status_code)='M/L' OR rtrim(at_status_code)='SL' OR rtrim(at_status_code)='LWP' OR rtrim(at_status_code)='COM' OR rtrim(at_status_code)='M/C')", _payCon);
                                        bool isExistsLeave = dtExistsLeave.Rows.Count > 0 ? true : false;
                                        if (!isExistsLeave)
                                        {
                                            var dtExistsWH = _dgCommon.get_InformationDataTable("select at_emp_serial from dg_pay_attendance where at_emp_serial=" + empInfo.emp_serial + " and at_date='" + sDate + "' and rtrim(at_holiday)='WH'", _payCon);
                                            bool isExistsWH = dtExistsWH.Rows.Count > 0 ? true : false;
                                            if (!isExistsWH)
                                            {
                                                Specialholiday.Add(new SpecialholidayPayloadList
                                                {
                                                    comp_id = obj.companyID,
                                                    emp_serial_id = empInfo.emp_serial,
                                                    emp_no = empInfo.emp_no,
                                                    shiftDate = sDate.ToString(),
                                                    fDate = obj.formDate,
                                                    tDate = obj.toDate,
                                                    description = obj.description,
                                                    isGov = obj.isGov,
                                                    user = obj.userName
                                                });
                                                result.Add(new ReturnObject { IsSuccess = true, Message = "Employee(" + empInfo.emp_no + ") Date(" + Convert.ToDateTime(sDate).ToString("dd/MMM/yyyy") + ") Save Successfully !!" });
                                            }
                                            else
                                            {
                                                result.Add(new ReturnObject { IsSuccess = false, Message = "Employee(" + empInfo.emp_no + ") Weekend Day This Date(" + Convert.ToDateTime(sDate).ToString("dd/MMM/yyyy") + ") !!" });
                                            }
                                        }
                                        else
                                        {
                                            result.Add(new ReturnObject { IsSuccess = false, Message = "Employee(" + empInfo.emp_no + ") Leave This Date(" + Convert.ToDateTime(sDate).ToString("dd/MMM/yyyy") + ") !!" });
                                        }
                                    }
                                    else
                                    {
                                        result.Add(new ReturnObject { IsSuccess = false, Message = "Employee(" + empInfo.emp_no + ") Holiday Already Exists This Date(" + Convert.ToDateTime(sDate).ToString("dd/MMM/yyyy") + ") !!" });
                                    }
                                }
                            });
                            var dtSpecialholiday = _dgCommon.ListToDataTable<SpecialholidayPayloadList>(Specialholiday);
                            if (dtSpecialholiday.Rows.Count > 0)
                            {
                                var attUpdate = _dgCommon.get_InformationDataTableByType("Dg_Pay_GetAttendance_list_byTempTable", _payCon, new SqlParameter("@shTable", dtSpecialholiday));
                                if (attUpdate.Rows.Count > 0)
                                {
                                    attUpdate.AsEnumerable().ToList().ForEach(x =>
                                    {
                                        string at_status_code = x.Field<string>("at_status_code").ToString().Trim();
                                        string at_holiday = x.Field<string>("at_holiday").ToString().Trim();
                                        int employeeId = Convert.ToInt32(x.Field<int>("empNo"));
                                        string at_date = Convert.ToDateTime(x.Field<DateTime>("at_date")).ToString("dd/MMM/yyyy");
                                        if (at_status_code == "" || at_status_code == "LA")
                                        {
                                            x["at_holiday"] = "SH";
                                            x["at_goverment_holiday"] = obj.isGov;
                                            x["at_holiday_status"] = obj.description;
                                            //result.Add(new ReturnObject { IsSuccess = true, Message = "Employee(" + employeeId + ") Date(" + Convert.ToDateTime(at_date).ToString("dd/MMM/yyyy") + ") Save Successfully !!" });
                                        }
                                        //else if (at_status_code == "AB" || at_status_code == "WH")
                                        else if (at_status_code == "AB")
                                        {
                                            x["at_status"] = obj.description;
                                            x["at_status_code"] = "SH";
                                            x["at_holiday"] = "SH";
                                            x["at_goverment_holiday"] = obj.isGov;
                                            x["at_holiday_status"] = obj.description;
                                            //result.Add(new ReturnObject { IsSuccess = true, Message = "Employee(" + employeeId + ") Date(" + Convert.ToDateTime(at_date).ToString("dd/MMM/yyyy") + ") Save Successfully !!" });
                                        }
                                    });
                                    _dgCommon.saveChangesByType("Dg_Pay_UpdateAtten_afterChangeSH", _payCon, new SqlParameter("@attUpdateTable", attUpdate));
                                    var distinctDtSpecialholiday = dtSpecialholiday.AsEnumerable().GroupBy(row => Convert.ToInt32(row.Field<string>("emp_serial_id"))).Select(group => group.First()).CopyToDataTable();
                                    distinctDtSpecialholiday.AsEnumerable().ToList().ForEach(r =>
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

                                        EcardSum.Add(new SpecialholidayEcardSum
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
                                    var dtEcardSum = _dgCommon.ListToDataTable<SpecialholidayEcardSum>(EcardSum);
                                    _dgCommon.saveChangesByType("Dg_Pay_EcardSum_For_SH", _payCon, new SqlParameter("@ecardSumTable", dtEcardSum));
                                }
                                _dgCommon.saveChangesByType("Dg_Pay_SaveSpecialholidays_empWise", _payCon, new SqlParameter("@typeTable", dtSpecialholiday));
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
                }*/

                //New Code 11/30/2024
                if (obj.empInfos.Count > 0)
                {
                    var Specialholiday = new List<SpecialholidayPayloadList>();
                    var EcardSum = new List<SpecialholidayEcardSum>();
                    obj.empInfos.ToList().ForEach(empInfo =>
                    {
                        for (var sDate = Convert.ToDateTime(obj.formDate); sDate <= Convert.ToDateTime(obj.toDate); sDate = sDate.AddDays(1))
                        {
                            var isExistsData = _dgCommon.get_InformationDataTable("select sh_emp_serial from dg_pay_specialholidays_empWise where sh_emp_serial=" + empInfo.emp_serial + " and sh_date='" + sDate + "'", _payCon);
                            bool isExists = isExistsData.Rows.Count > 0 ? true : false;
                            if (!isExists)
                            {
                                var dtExistsLeave = _dgCommon.get_InformationDataTable("select at_emp_serial from dg_pay_attendance where at_emp_serial=" + empInfo.emp_serial + " and at_date='" + sDate + "' and (rtrim(at_status_code)='AL' OR rtrim(at_status_code)='CL' OR rtrim(at_status_code)='ML' OR rtrim(at_status_code)='M/L' OR rtrim(at_status_code)='SL' OR rtrim(at_status_code)='LWP' OR rtrim(at_status_code)='COM' OR rtrim(at_status_code)='M/C')", _payCon);
                                bool isExistsLeave = dtExistsLeave.Rows.Count > 0 ? true : false;
                                if (!isExistsLeave)
                                {
                                    Specialholiday.Add(new SpecialholidayPayloadList
                                    {
                                        comp_id = obj.companyID,
                                        emp_serial_id = empInfo.emp_serial,
                                        emp_no = empInfo.emp_no,
                                        shiftDate = sDate.ToString(),
                                        fDate = obj.formDate,
                                        tDate = obj.toDate,
                                        description = obj.description,
                                        isGov = obj.isGov,
                                        user = obj.userName
                                    });
                                    result.Add(new ReturnObject { IsSuccess = true, Message = "Employee(" + empInfo.emp_no + ") Date(" + Convert.ToDateTime(sDate).ToString("dd/MMM/yyyy") + ") Save Successfully !!" });
                                    
                                    /*var dtExistsWH = _dgCommon.get_InformationDataTable("select at_emp_serial from dg_pay_attendance where at_emp_serial=" + empInfo.emp_serial + " and at_date='" + sDate + "' and rtrim(at_holiday)='WH'", _payCon);
                                    bool isExistsWH = dtExistsWH.Rows.Count > 0 ? true : false;
                                    if (!isExistsWH)
                                    {
                                        Specialholiday.Add(new SpecialholidayPayloadList
                                        {
                                            comp_id = obj.companyID,
                                            emp_serial_id = empInfo.emp_serial,
                                            emp_no = empInfo.emp_no,
                                            shiftDate = sDate.ToString(),
                                            fDate = obj.formDate,
                                            tDate = obj.toDate,
                                            description = obj.description,
                                            isGov = obj.isGov,
                                            user = obj.userName
                                        });
                                        result.Add(new ReturnObject { IsSuccess = true, Message = "Employee(" + empInfo.emp_no + ") Date(" + Convert.ToDateTime(sDate).ToString("dd/MMM/yyyy") + ") Save Successfully !!" });
                                    }
                                    else
                                    {
                                        result.Add(new ReturnObject { IsSuccess = false, Message = "Employee(" + empInfo.emp_no + ") Weekend Day This Date(" + Convert.ToDateTime(sDate).ToString("dd/MMM/yyyy") + ") !!" });
                                    }*/
                                }
                                else
                                {
                                    result.Add(new ReturnObject { IsSuccess = false, Message = "Employee(" + empInfo.emp_no + ") Leave This Date(" + Convert.ToDateTime(sDate).ToString("dd/MMM/yyyy") + ") !!" });
                                }
                            }
                            else
                            {
                                result.Add(new ReturnObject { IsSuccess = false, Message = "Employee(" + empInfo.emp_no + ") Holiday Already Exists This Date(" + Convert.ToDateTime(sDate).ToString("dd/MMM/yyyy") + ") !!" });
                            }
                        }
                    });
                    var dtSpecialholiday = _dgCommon.ListToDataTable<SpecialholidayPayloadList>(Specialholiday);
                    if (dtSpecialholiday.Rows.Count > 0)
                    {
                        var attUpdate = _dgCommon.get_InformationDataTableByType("Dg_Pay_GetAttendance_list_byTempTable", _payCon, new SqlParameter("@shTable", dtSpecialholiday));
                        if (attUpdate.Rows.Count > 0)
                        {
                            attUpdate.AsEnumerable().ToList().ForEach(x =>
                            {
                                string at_status_code = x.Field<string>("at_status_code").ToString().Trim();
                                string at_holiday = x.Field<string>("at_holiday").ToString().Trim();
                                int employeeId = Convert.ToInt32(x.Field<int>("empNo"));
                                string at_date = Convert.ToDateTime(x.Field<DateTime>("at_date")).ToString("dd/MMM/yyyy");
                                if (at_status_code == "" || at_status_code == "LA")
                                {
                                    x["at_holiday"] = "SH";
                                    x["at_goverment_holiday"] = obj.isGov;
                                    x["at_holiday_status"] = obj.description;
                                    //result.Add(new ReturnObject { IsSuccess = true, Message = "Employee(" + employeeId + ") Date(" + Convert.ToDateTime(at_date).ToString("dd/MMM/yyyy") + ") Save Successfully !!" });
                                }
                                //else if (at_status_code == "AB" || at_status_code == "WH")
                                else if (at_status_code == "AB" || at_status_code == "WH")
                                {
                                    x["at_status"] = obj.description;
                                    x["at_status_code"] = "SH";
                                    x["at_holiday"] = "SH";
                                    x["at_goverment_holiday"] = obj.isGov;
                                    x["at_holiday_status"] = obj.description;
                                    //result.Add(new ReturnObject { IsSuccess = true, Message = "Employee(" + employeeId + ") Date(" + Convert.ToDateTime(at_date).ToString("dd/MMM/yyyy") + ") Save Successfully !!" });
                                }
                            });
                            _dgCommon.saveChangesByType("Dg_Pay_UpdateAtten_afterChangeSH", _payCon, new SqlParameter("@attUpdateTable", attUpdate));
                            var distinctDtSpecialholiday = dtSpecialholiday.AsEnumerable().GroupBy(row => Convert.ToInt32(row.Field<string>("emp_serial_id"))).Select(group => group.First()).CopyToDataTable();
                            distinctDtSpecialholiday.AsEnumerable().ToList().ForEach(r =>
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

                                EcardSum.Add(new SpecialholidayEcardSum
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
                            var dtEcardSum = _dgCommon.ListToDataTable<SpecialholidayEcardSum>(EcardSum);
                            _dgCommon.saveChangesByType("Dg_Pay_EcardSum_For_SH", _payCon, new SqlParameter("@ecardSumTable", dtEcardSum));
                        }
                        _dgCommon.saveChangesByType("Dg_Pay_SaveSpecialholidays_empWise", _payCon, new SqlParameter("@typeTable", dtSpecialholiday));
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
                result.Add(new ReturnObject { IsSuccess = false, Message = "Something Went Wrong !!" });
            }
            return result;
        }
        public async Task<ReturnObject> GetCreatedSpecialholidays_List(int compid,string formDate,string toDate)
        {
            var result = new ReturnObject();
            try
            {
                var data = await _dgCommon.get_InformationDataTableAsync("Dg_Pay_CreatedSpecialholidays_List "+ compid + ",'"+ formDate + "','"+ toDate + "'", _payCon);
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
        public List<ReturnObject> DeleteEmpWiseSpecialholiday(DeleteSpecialholidayPayload obj)
        {
            var result = new List<ReturnObject>();
            try
            {
                if (obj.delChilds.Count > 0)
                {
                    var Specialholiday = new List<SpecialholidayPayloadList>();
                    var EcardSum = new List<SpecialholidayEcardSum>();
                    var dtEcardSum = new DataTable();
                    var dtSpecialholiday = new DataTable();
                    var dtConfSal = _dgCommon.get_InformationDataTable("select ss_emp_serial from dg_pay_salarysheet where ss_compid=" + obj.compID + " and (month(ss_date)=" + obj.formDate + " or month(ss_date)=" + obj.toDate + ") and (year(ss_date)=" + obj.formDate + " or year(ss_date)=" + obj.toDate + ") and ss_confirmed=1", _payCon);
                    bool isConfSal = dtConfSal.Rows.Count > 0 ? true : false;
                    if (!isConfSal)
                    {
                        obj.delChilds.ToList().ForEach(delChild =>
                        {
                            for (var sDate = Convert.ToDateTime(obj.formDate); sDate <= Convert.ToDateTime(obj.toDate); sDate = sDate.AddDays(1))
                            {
                                Specialholiday.Add(new SpecialholidayPayloadList
                                {
                                    comp_id = obj.compID,
                                    emp_serial_id = delChild.empSerial,
                                    emp_no = delChild.empNumber,
                                    shiftDate = sDate.ToString("MM-dd-yyyy"),
                                    fDate = obj.formDate,
                                    tDate = obj.toDate,
                                    description = string.Empty,
                                    isGov = false,
                                    user = string.Empty
                                });
                                result.Add(new ReturnObject { IsSuccess = true, Message = "Employee(" + delChild.empNumber + ") Date(" + sDate + ") Delete Successfully !!" });
                            }
                        });
                        dtSpecialholiday = _dgCommon.ListToDataTable<SpecialholidayPayloadList>(Specialholiday);
                        if (dtSpecialholiday.Rows.Count > 0)
                        {
                            var attUpdate = _dgCommon.get_InformationDataTableByType("Dg_Pay_GetAttendance_list_byTempTable", _payCon, new SqlParameter("@shTable", dtSpecialholiday));
                            if (attUpdate.Rows.Count > 0)
                            {
                                attUpdate.AsEnumerable().ToList().ForEach(x =>
                                {
                                    string at_status = x.Field<string>("at_status_code").ToString().Trim();
                                    string at_actu_holiday = x.Field<string>("at_actu_holiday").ToString().Trim();
                                    string NameDay = x.Field<string>("NameDay").ToString().Trim();
                                    int employeeId = Convert.ToInt32(x.Field<int>("empNo"));
                                    string at_date = Convert.ToDateTime(x.Field<DateTime>("at_date")).ToString("dd/MMM/yyyy");
                                    if (at_status == "SH" && at_actu_holiday == "")
                                    {
                                        x["at_status"] = "Absent";
                                        x["at_status_code"] = "AB";
                                        x["at_holiday"] = "";
                                        x["at_goverment_holiday"] = DBNull.Value;
                                        x["at_holiday_status"] = null;
                                        //result.Add(new ReturnObject { IsSuccess = true, Message = "Employee(" + employeeId + ") Date(" + at_date + ") Delete Successfully !!" });
                                    }
                                    else if (at_status == "SH" && at_actu_holiday == "WH")
                                    {
                                        x["at_status"] = NameDay;
                                        x["at_status_code"] = "WH";
                                        x["at_holiday"] = "WH";
                                        x["at_goverment_holiday"] = DBNull.Value;
                                        x["at_holiday_status"] = null;
                                        //result.Add(new ReturnObject { IsSuccess = true, Message = "Employee(" + employeeId + ") Date(" + at_date + ") Delete Successfully !!" });
                                    }
                                    else if ((at_status == "" || at_status == "LA") && at_actu_holiday == "")
                                    {
                                        x["at_holiday"] = "";
                                        x["at_goverment_holiday"] = DBNull.Value;
                                        x["at_holiday_status"] = null;
                                        //result.Add(new ReturnObject { IsSuccess = true, Message = "Employee(" + employeeId + ") Date(" + at_date + ") Delete Successfully !!" });
                                    }
                                    else if ((at_status == "" || at_status == "LA") && at_actu_holiday == "WH")
                                    {
                                        x["at_holiday"] = "WH";
                                        x["at_goverment_holiday"] = DBNull.Value;
                                        x["at_holiday_status"] = null;
                                        //result.Add(new ReturnObject { IsSuccess = true, Message = "Employee(" + employeeId + ") Date(" + at_date + ") Delete Successfully !!" });
                                    }
                                });
                                _dgCommon.saveChangesByType("Dg_Pay_UpdateAtten_afterChangeSH", _payCon, new SqlParameter("@attUpdateTable", attUpdate));
                                var distinctDtSpecialholiday = dtSpecialholiday.AsEnumerable().GroupBy(row => Convert.ToInt32(row.Field<string>("emp_serial_id"))).Select(group => group.First()).CopyToDataTable();
                                distinctDtSpecialholiday.AsEnumerable().ToList().ForEach(r =>
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
                                    EcardSum.Add(new SpecialholidayEcardSum
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
                                dtEcardSum = _dgCommon.ListToDataTable<SpecialholidayEcardSum>(EcardSum);
                                _dgCommon.saveChangesByType("Dg_Pay_EcardSum_For_SH", _payCon, new SqlParameter("@ecardSumTable", dtEcardSum));
                            }
                            _dgCommon.saveChangesByType("Dg_Pay_DeleteSpecialHoliday", _payCon, new SqlParameter("@typeTable", dtSpecialholiday));
                        }
                    }
                    else
                    {
                        result.Add(new ReturnObject { Message = "Salary Already Confirmed This Month(" + Convert.ToDateTime(obj.formDate).ToString("MMMM-yyyy") + ") !!" });
                    }
                }
                else
                {
                    result.Add(new ReturnObject
                    { Message = "You Can Not Check Any Employee !!" });
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                result = null;
                result.Add(new ReturnObject
                { Message = "Delete Fail !!" });
            }
            return result;
        }
        public async Task<DataTable> GetEmployeeListForSpecialholiday(int Compid, int Department,int section, int Building, int Floor, int Line,int Shift, int Grade, int salcat, string formDate, string toDate)
        {
            var dateLs = new List<string>();
            for (var Date = Convert.ToDateTime(formDate); Date <= Convert.ToDateTime(toDate); Date = Date.AddDays(1))
            {
                dateLs.Add("''" + Date + "''");
            }
            string dateRng = "in(" + string.Join(",", dateLs) + ")";
            var data = await _dgCommon.get_InformationDataTableAsync("Emp_filtering_list_for_SH_and_Cov " + Compid + ","+ Department + ","+ section + ","+ Building + ","+ Floor + ","+ Line + ","+ Shift + ","+ Grade + ","+ salcat + ",'"+ dateRng + "'", _payCon);
            return data;
        }
        public async Task<ReturnObject> SpecialholidaysProcess(SpecialholidayProcessPayload obj)
        {
            var result = new ReturnObject();
            try
            {
                var dtCheckAttProc = await _dgCommon.get_InformationDataTableAsync(string.Format("select procs_compid from dg_pay_Process_startOrStop where procs_compid={0} and procs_type in('att_text_upload','att_text_upload_Single') group by procs_compid", obj.companyID), _payCon);
                if (dtCheckAttProc.Rows.Count > 0)
                {
                    result.Message = "Attendance Upload In Progress,Please Wait... !!";
                    return result;
                }
                var dtCheckSalProc = await _dgCommon.get_InformationDataTableAsync(string.Format("select procs_compid from dg_pay_Process_startOrStop where procs_compid={0} and procs_type in('emp_sal_process_bulk','emp_sal_process_Single') group by procs_compid", obj.companyID), _payCon);
                if (dtCheckSalProc.Rows.Count > 0)
                {
                    result.Message = "Salary Process In Progress,Please Wait... !!";
                    return result;
                }
                await _dgCommon.saveChangesAsync(string.Format("dg_pay_processStartOrStop_insert {0},'{1}',{2},'{3}','{4}'", obj.companyID, "att_Hdot_process", 1, DateTime.Now.ToString("MM-dd-yyyy"), "SystemUser"), _payCon);
                var isSave = await _dgCommon.saveChangesAsync("Dg_Pay_SH_WH_OT_Process " + obj.companyID + ",'" + obj.processMonthYear + "'", _payCon);
                if (isSave)
                {
                    result.IsSuccess = true;
                    result.Message = "Process Successfully !!";
                }
                else
                {
                    result.Message = "Not Found New Process Data !!";
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            finally
            {
                await _dgCommon.saveChangesAsync(string.Format("delete from dg_pay_Process_startOrStop where procs_compid={0} and procs_date='{1}' and procs_type='att_Hdot_process'", obj.companyID, DateTime.Now.ToString("MM-dd-yyyy")), _payCon);
            }
            return result;
        }
        public ReturnObject SaveHolidayProcess(HolidayProcessPayload obj)
        {
            var result = new ReturnObject();
            if (obj.employeeSl.Length > 0)
            {
                //_dgCommon.saveChanges("DELETE dg_employeelist_holiday_bill_process WHERE pl_compid=" + obj.companyId, _payCon);
                if (Convert.ToDateTime(obj.formDate) > Convert.ToDateTime(obj.toDate))
                {
                    result.Message = "ToDate Smaller Than FormDate !!";
                    return result;
                }
                if (Convert.ToDateTime(obj.formDate).Month != Convert.ToDateTime(obj.toDate).Month)
                {
                    result.Message = "FormDate And ToDate Are Not Same Month !!";
                    return result;
                }
                int procMonth = Convert.ToDateTime(obj.formDate).Month;
                int procYear = Convert.ToDateTime(obj.formDate).Year;
                var dtSalConf = _dgCommon.get_InformationDataTable("select TOP 1 ss_emp_serial from dg_pay_salarysheet where ss_compid=" + obj.companyId + " and month(ss_date)=" + procMonth + " and year(ss_date)=" + procYear + " and ss_confirmed=1", _payCon);
                bool isSalConf = dtSalConf.Rows.Count > 0 ? true : false;
                if (!isSalConf)
                {
                    obj.employeeSl.ToList().ForEach(emp =>
                    {
                        _dgCommon.saveChanges("dg_pay_EmployeeHolidayProcess " + obj.companyId + ",'" + obj.formDate + "','" + obj.toDate + "'," + emp + ",'" + obj.userName + "'", _payCon);
                    });
                    result.IsSuccess = true;
                    result.Message = "Save Successfully !!";
                    return result;
                }
                result.Message = "Salary Already Confirm This Month(" + Convert.ToDateTime(obj.formDate).ToString("MMMM-yyyy") + ") !!";
                return result;                
            }
            result.Message = "You Can Not Check Any Employee !!";
            return result;
        }
        public async Task<ReturnObject> GetEmployeeHolidayBillProcess(int companyID, string monthYear)
        {
            var result = new ReturnObject();
            var dt = await _dgCommon.get_InformationDataTableAsync("dg_pay_emp_Holiday_bill_process " + companyID + ",'" + monthYear + "'", _payCon);
            if (dt.Rows.Count > 0)
            {
                result.IsSuccess = true;
                result.Message = "Data Loaded !!";
                result.dataTable = dt;
                return result;
            }
            result.Message = "No Data Available !!";
            return result;
        }
        public ReturnObject DeleteEmployeeHolidayBillProcess(HolidayBillProcessDelPayload obj)
        {
            var result = new ReturnObject();
            if (obj.employeeSl.Length > 0)
            {
                int procMonth = Convert.ToDateTime(obj.monthYear).Month;
                int procYear = Convert.ToDateTime(obj.monthYear).Year;
                var dtSalConf = _dgCommon.get_InformationDataTable("select TOP 1 ss_emp_serial from dg_pay_salarysheet where ss_compid=" + obj.companyId + " and month(ss_date)=" + procMonth + " and year(ss_date)=" + procYear + " and ss_confirmed=1", _payCon);
                bool isSalConf = dtSalConf.Rows.Count > 0 ? true : false;
                if (!isSalConf)
                {
                    obj.employeeSl.ToList().ForEach(emp =>
                    {
                        _dgCommon.saveChanges("dg_pay_emp_Holiday_bill_process_delete " + obj.companyId + "," + emp + ",'" + obj.monthYear + "'", _payCon);
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

        public DataTable test_save()
        {
            var data = _dgCommon.get_InformationDataTable("select emp_serial,emp_no from dg_pay_Employee where compid=40 and oi_active=1", _payCon);
            DataTable dt = new DataTable();
            DataColumn[] newCol = new DataColumn[]
            {
                new DataColumn("emp_serialForhad", typeof(int))
            };
            dt.Columns.AddRange(newCol);
            for (int i = 0; i < data.Rows.Count; i++)
            {
                dt.Rows.Add(data.Rows[i]["emp_serial"]);
            }
            //var dt2 = _dgCommon.get_InformationDataTable("forhad_tst "+ dt + "", _payCon);
            _payCon.Open();
            SqlCommand cmd = new SqlCommand("forhad_tst "+ dt, _payCon);
            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.AddWithValue("@tbl", dt);
            //tvparam.SqlDbType = SqlDbType.Structured;
            //tvparam.TypeName = "dbo.IDList";
            DataTable dt2 = new DataTable();
            dt2.Load(cmd.ExecuteReader());
            _payCon.Close();
            return dt2;
        }
    }
}