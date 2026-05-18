using System.ComponentModel.DataAnnotations;

namespace BOL.Models
{
    public class Attendance_DbModel
    {
        public int? at_groupid { get; set; } = null;
        public int? at_compid { get; set; } = null;
        [Key]
        public int at_emp_serial { get; set; }
        public string at_proxid { get; set; } = null;
        public DateTime at_date { get; set; }
        public decimal? at_intime { get; set; } = null;
        public DateTime? at_outdate { get; set; } = null;
        public decimal? at_outtime { get; set; } = null;
        public decimal? at_late { get; set; } = null;
        public decimal? at_shift { get; set; } = null;
        public string at_shift_name { get; set; } = null;
        public decimal? at_work_hrs { get; set; } = null;
        public decimal? at_work_min { get; set; } = null;
        public decimal? at_ot_hrs { get; set; } = null;
        public decimal? at_ot_min { get; set; } = null;
        public decimal? at_exot_hrs { get; set; } = null;
        public decimal? at_exot_min { get; set; } = null;
        public string at_status { get; set; } = null;
        public string at_status_code { get; set; } = null;
        public string at_user { get; set; } = null;
        public DateTime? at_udate { get; set; } = null;
        public bool? at_manual { get; set; } = null;
        public bool? at_coverday { get; set; } = null;
        public string at_holiday { get; set; } = null;
        public int? At_Extra { get; set; } = null;
        public decimal? at_intime_previous { get; set; } = null;
        public DateTime? at_outdate_previous { get; set; } = null;
        public decimal? at_outtime_previous { get; set; } = null;
    }
    public class DgPayAttendance
    {
        public int? AtGroupid { get; set; } = null;
        public int? AtCompid { get; set; } = null;
        public int AtEmpSerial { get; set; }
        public string AtProxid { get; set; } = null;
        public DateTime AtDate { get; set; }
        public decimal? AtIntime { get; set; } = null;
        public DateTime? AtOutdate { get; set; } = null;
        public decimal? AtOuttime { get; set; } = null;
        public decimal? AtLate { get; set; } = null;
        public decimal? AtShift { get; set; } = null;
        public string AtShiftName { get; set; } = null;
        public decimal? AtWorkHrs { get; set; } = null;
        public decimal? AtWorkMin { get; set; } = null;
        public decimal? AtOtHrs { get; set; } = null;
        public decimal? AtOtMin { get; set; } = null;
        public decimal? AtExotHrs { get; set; } = null;
        public decimal? AtExotMin { get; set; } = null;
        public string AtStatus { get; set; } = null;
        public string AtStatusCode { get; set; } = null;
        public string AtUser { get; set; } = null;
        public DateTime? AtUdate { get; set; } = null;
        public bool? AtManual { get; set; } = null;
        public bool? AtCoverday { get; set; } = null;
        public string AtHoliday { get; set; } = null;
        public int? AtExtra { get; set; } = null;
        public decimal? AtIntimePrevious { get; set; } = null;
        public DateTime? at_outdate_previous { get; set; } = null;
        public decimal? at_outtime_previous { get; set; } = null;

        public static Attendance_DbModel CustomToDbModel(DgPayAttendance obj)
        {
            try
            {
                var dbModel = new Attendance_DbModel
                {
                    at_groupid = obj.AtGroupid,
                    at_compid = obj.AtCompid,
                    at_emp_serial = obj.AtEmpSerial,
                    at_proxid = obj.AtProxid,
                    at_date = obj.AtDate,
                    at_intime = obj.AtIntime,
                    at_outdate = obj.AtOutdate,
                    at_outtime = obj.AtOuttime,
                    at_late = obj.AtLate,
                    at_shift = obj.AtShift,
                    at_shift_name = obj.AtShiftName,
                    at_work_hrs = obj.AtWorkHrs,
                    at_work_min = obj.AtWorkMin,
                    at_ot_hrs = obj.AtOtHrs,
                    at_ot_min = obj.AtOtMin,
                    at_exot_hrs = obj.AtExotHrs,
                    at_exot_min = obj.AtExotMin,
                    at_status = obj.AtStatus,
                    at_status_code = obj.AtStatusCode,
                    at_user = obj.AtUser,
                    at_udate = obj.AtUdate,
                    at_manual = obj.AtManual,
                    at_coverday = obj.AtCoverday,
                    at_holiday = obj.AtHoliday,
                    At_Extra = obj.AtExtra,
                    at_intime_previous = obj.AtIntimePrevious,
                    at_outdate_previous = obj.at_outdate_previous,
                    at_outtime_previous = obj.at_outtime_previous
                };
                return dbModel;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return null;
        }
        public static DgPayAttendance DbToCustomModel(Attendance_DbModel obj)
        {
            try
            {
                var customModel = new DgPayAttendance
                {
                    AtGroupid = obj.at_groupid,
                    AtCompid = obj.at_compid,
                    AtEmpSerial = obj.at_emp_serial,
                    AtProxid = obj.at_proxid,
                    AtDate = obj.at_date,
                    AtIntime = obj.at_intime,
                    AtOutdate = obj.at_outdate,
                    AtOuttime = obj.at_outtime,
                    AtLate = obj.at_late,
                    AtShift = obj.at_shift,
                    AtShiftName = obj.at_shift_name,
                    AtWorkHrs = obj.at_work_hrs,
                    AtWorkMin = obj.at_work_min,
                    AtOtHrs = obj.at_ot_hrs,
                    AtOtMin = obj.at_ot_min,
                    AtExotHrs = obj.at_exot_hrs,
                    AtExotMin = obj.at_exot_min,
                    AtStatus = obj.at_status,
                    AtStatusCode = obj.at_status_code,
                    AtUser = obj.at_user,
                    AtUdate = obj.at_udate,
                    AtManual = obj.at_manual,
                    AtCoverday = obj.at_coverday,
                    AtHoliday = obj.at_holiday,
                    AtExtra = obj.At_Extra,
                    AtIntimePrevious = obj.at_intime_previous,
                    at_outdate_previous = obj.at_outdate_previous,
                    at_outtime_previous = obj.at_outtime_previous
                };
                return customModel;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return null;
        }
    }
    public class ManualAttSeletedList
    {
        public int compid { get; set; }
        public string comp_name { get; set; }
        public int emp_serial { get; set; }
        public int emp_no { get; set; }
        public int emp_proxid { get; set; }
        public string pi_fullname { get; set; }
        public int sh_code { get; set; }
        public string sh_name { get; set; }
        public string sh_status { get; set; }
        public int oi_department { get; set; }
        public string oi_departmente_name { get; set; }
        public int oi_section { get; set; }
        public string oi_section_name { get; set; }
        public int oi_designation { get; set; }
        public string oi_designation_name { get; set; }
        public int oi_bulding { get; set; }
        public string oi_bulding_name { get; set; }
        public string oi_joineddate { get; set; }
        public string at_date { get; set; }
        public decimal? at_intime { get; set; }
        public string at_outdate { get; set; }
        public decimal? at_outtime { get; set; }
        public string at_status { get; set; }
        public decimal? sh_InTime { get; set; }
        public decimal? sh_OutTime { get; set; }
    }
    public class ManualAttPara
    {
        public int emp_serial { get; set; }
        public string fromDate { get; set; }
        public string todate { get; set; }
        public bool weeklyholiday { get; set; }
        public bool leave { get; set; }
    }
    public class ManualAttSavePara
    {
        public int at_emp_serial { get; set; }
        public string at_date { get; set; }
        public decimal? at_intime { get; set; }
        public string at_outdate { get; set; }
        public decimal? at_outtime { get; set; }
        public string User { get; set; }
    }
    public class AttendanceAbsentPaylod
    {
        public int emp_serial { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
        public string userName { get; set; }
    }
    public class GetEmployeeListPayload
    {
        public int? compid { get; set; } = null;
        public int?[] emp_no { get; set; } = null;
    }
    public class GetListAttendanceRowDel
    {
        public int? emp_serial { get; set; } = null;
        public int? compid { get; set; } = null;
        public int? emp_no { get; set; } = null;
        public string emp_proxid { get; set; } = null;
        public string pi_fullname { get; set; } = null;
        public string oi_departmente_name { get; set; } = null;
        public string oi_section_name { get; set; } = null;
        public string oi_bulding_name { get; set; } = null;
        public string oi_floor_name { get; set; } = null;
        public string oi_line_name { get; set; } = null;
        public string oi_shift_name { get; set; } = null;
        public string oi_joineddate { get; set; } = null;
    }
    public class GetEmpAttendanceListPayload
    {
        public int Compid { get; set; }
        public int Department { get; set; }
        public int section { get; set; }
        public int Building { get; set; }
        public int Floor { get; set; }
        public int Line { get; set; }
        public int shift { get; set; }
        public string From_date { get; set; }
        public string To_date { get; set; }
        public decimal Hour { get; set; }
        public bool weeklyholiday { get; set; }
        public bool leave { get; set; }
    }
    public class EmpAttendanceChangeSavePayload
    {
        public int[] at_emp_serial { get; set; }
        public decimal Hour { get; set;}
        public string form_date { get; set; }
        public string to_date { get; set; }
        public string User { get; set; }

        public static EmpAttendanceChangeSave MainPayload(int Empserial,EmpAttendanceChangeSavePayload obj)
        {
            var result = new EmpAttendanceChangeSave
            {
                at_emp_serial = Empserial,
                From_date = obj.form_date,
                To_Date = obj.to_date,
                Hour = obj.Hour,
                User = obj.User
            };
            return result;
        }
    }
    public class EmpAttendanceChangeSave
    {
        public int at_emp_serial { get; set; }
        public string From_date { get; set; }
        public string To_Date { get; set; }
        public decimal Hour { get; set; }
        public string User { get; set; }
    }
}