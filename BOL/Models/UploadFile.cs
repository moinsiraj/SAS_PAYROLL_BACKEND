using Microsoft.AspNetCore.Http;
using System.Data;

namespace BOL.Models
{
    public class UploadFile
    {
        public int CompanyID { get; set; }
        public int txt_formatID { get; set; }
        public string DateFormat { get; set; }
        public IFormFile file { get; set; }
        public string userName { get; set; }
    }
    public class UploadFileEmpWise
    {
        public int CompanyID { get; set; }
        public int txt_formatID { get; set; }
        public string EmpSerial { get; set; }
        public string DateFormat { get; set; }
        public IFormFile file { get; set; }
        public string userName { get; set; }
    }
    public class MyEntity
    {
        public string emp_ProxID { get; set; }
        public string emp_Slno { get; set; }
        public string date { get; set; }
        public string time { get; set; }
    }
    public class TextUploadMessage : ReturnObject
    {
        public int empNo { get; set; }

    }
    public class AttendanceModel
    {
        public int at_compid { get; set; } = 0;
        public int at_emp_serial { get; set; } = 0;
        public string at_date { get; set; } = null;
        public decimal at_intime { get; set; } = new decimal(0);
        public decimal at_outtime { get; set; } = new decimal(0);
        public string at_outdate { get; set; } = null;
        public decimal at_late { get; set; } = new decimal(0);
        public string at_status { get; set; } = null;
        public string at_status_code { get; set; } = null;
        public bool? at_manual_in { get; set; } = null;
        public string at_manual_in_by { get; set; } = null;
        public string at_manual_in_date { get; set; } = null;
        public bool? at_manual_out { get; set; } = null;
        public string at_manual_out_by { get; set; } = null;
        public string at_manual_out_date { get; set; } = null;
        public decimal? at_text_intime { get; set; } = null;
        public string at_text_out_date { get; set; } = null;
        public decimal? at_text_outtime { get; set; } = null;

        public static DataTable AddNewDataColumn()
        {
            var dt = new DataTable();
            var columns = new[]
            {
                new DataColumn("at_compid", typeof(string)),
                new DataColumn("at_emp_serial", typeof(string)),
                new DataColumn("at_date", typeof(string)),
                new DataColumn("at_intime", typeof(decimal)),
                new DataColumn("at_outtime", typeof(decimal)),
                new DataColumn("at_outdate",typeof(string)),
                new DataColumn("at_late",typeof(decimal)),
                new DataColumn("at_status",typeof(string)),
                new DataColumn("at_status_code",typeof(string)),
                new DataColumn("at_manual_in",typeof(bool)),
                new DataColumn("at_manual_in_by",typeof(string)),
                new DataColumn("at_manual_in_date",typeof(string)),
                new DataColumn("at_manual_out",typeof(bool)),
                new DataColumn("at_manual_out_by",typeof(string)),
                new DataColumn("at_manual_out_date",typeof(string)),
                new DataColumn("at_attendance_uploadby",typeof(string)),
                new DataColumn("at_ot_process",typeof(bool)),
                new DataColumn("at_text_out_date", typeof(string)),
                new DataColumn("at_text_intime", typeof(decimal)),
                new DataColumn("at_text_outtime", typeof(decimal)),
            };
            dt.Columns.AddRange(columns);
            return dt;
        }
        public static void SetAttDataColumnVal(DataTable attnTable, AttendanceModel output,string userName)
        {
            DataRow row = attnTable.NewRow();
            row["at_compid"] = output.at_compid;
            row["at_emp_serial"] = output.at_emp_serial;
            row["at_date"] = Convert.ToDateTime(output.at_date).ToString("yyyy/MM/dd");
            row["at_intime"] = output.at_intime;
            row["at_outtime"] = output.at_outtime;
            row["at_outdate"] = Convert.ToDateTime(output.at_outdate).ToString("yyyy/MM/dd");
            row["at_late"] = output.at_late;
            row["at_status"] = output.at_status;
            row["at_status_code"] = output.at_status_code;
            row["at_manual_in"] = string.IsNullOrEmpty(output.at_manual_in.ToString()) ? DBNull.Value : output.at_manual_in;
            row["at_manual_in_by"] = string.IsNullOrEmpty(output.at_manual_in_by) ? DBNull.Value : output.at_manual_in_by;
            row["at_manual_in_date"] = string.IsNullOrEmpty(output.at_manual_in_date) ? DBNull.Value : output.at_manual_in_date;
            row["at_manual_out"] = string.IsNullOrEmpty(output.at_manual_out.ToString()) ? DBNull.Value : output.at_manual_out;
            row["at_manual_out_by"] = string.IsNullOrEmpty(output.at_manual_out_by) ? DBNull.Value : output.at_manual_out_by;
            row["at_manual_out_date"] = string.IsNullOrEmpty(output.at_manual_out_date) ? DBNull.Value : output.at_manual_out_date;
            row["at_attendance_uploadby"] = userName;
            row["at_ot_process"] = false;
            row["at_text_out_date"] = string.IsNullOrEmpty(output.at_text_out_date) ? DBNull.Value : output.at_text_out_date;
            row["at_text_intime"] = string.IsNullOrEmpty(output.at_text_intime.ToString()) ? DBNull.Value : output.at_text_intime;
            row["at_text_outtime"] = string.IsNullOrEmpty(output.at_text_outtime.ToString()) ? DBNull.Value : output.at_text_outtime;
            attnTable.Rows.Add(row);
        }
    }
    public class AttendanceOtProcess
    {
        public int compid { get; set; }
        public string procs_date { get; set; }
        public int processType { get; set; }
        public string userName { get; set; }
        public List<AttendanceOtProcessChild> empInfo { get; set; }

        public static DataTable AddNewDataColumn()
        {
            var dt = new DataTable();
            var columns = new[]
            {
                new DataColumn("at_emp_serial", typeof(int)),
                new DataColumn("at_emp_no", typeof(int)),
                new DataColumn("at_compid", typeof(int)),
                new DataColumn("at_date", typeof(string)),
                new DataColumn("at_holiday", typeof(string)),
                new DataColumn("at_work_hrs", typeof(decimal)),
                new DataColumn("at_work_min",typeof(decimal)),
                new DataColumn("at_ot_hrs",typeof(decimal)),
                new DataColumn("at_ot_min",typeof(decimal)),
                new DataColumn("at_exot_hrs",typeof(decimal)),
                new DataColumn("at_exot_min",typeof(decimal)),
                new DataColumn("at_exot_hrs_2hour_max",typeof(decimal)),
                new DataColumn("at_exot_min_2hour_less",typeof(decimal)),
                new DataColumn("at_ot_ex_ot_hour_min",typeof(decimal)),
                new DataColumn("at_ot_ex_ot_hour_min_with_wh_ot",typeof(decimal)),
                new DataColumn("at_holiday_ot_for_oneday",typeof(decimal)),
                new DataColumn("at_ot_process_by",typeof(string))
            };
            dt.Columns.AddRange(columns);
            return dt;
        }
        public static void SetAttDataColumnVal(DataTable getResult,DataTable setResult,string userName)
        {
            DataRow row = getResult.NewRow();
            row["at_emp_serial"] = int.Parse(setResult.Rows[0]["at_emp_serial"].ToString());
            row["at_emp_no"] = int.Parse(setResult.Rows[0]["at_emp_no"].ToString());
            row["at_compid"] = int.Parse(setResult.Rows[0]["at_compid"].ToString());
            row["at_date"] = Convert.ToDateTime(setResult.Rows[0]["at_date"]).ToString("yyyy-MM-dd");
            row["at_holiday"] = setResult.Rows[0]["at_holiday"].ToString();
            row["at_work_hrs"] = decimal.Parse(setResult.Rows[0]["at_work_hrs"].ToString());
            row["at_work_min"] = decimal.Parse(setResult.Rows[0]["at_work_min"].ToString());
            row["at_ot_hrs"] = decimal.Parse(setResult.Rows[0]["at_ot_hrs"].ToString());
            row["at_ot_min"] = decimal.Parse(setResult.Rows[0]["at_ot_min"].ToString());
            row["at_exot_hrs"] = decimal.Parse(setResult.Rows[0]["at_exot_hrs"].ToString());
            row["at_exot_min"] = decimal.Parse(setResult.Rows[0]["at_exot_min"].ToString());
            row["at_exot_hrs_2hour_max"] = decimal.Parse(setResult.Rows[0]["at_exot_hrs_2hour_max"].ToString());
            row["at_exot_min_2hour_less"] = decimal.Parse(setResult.Rows[0]["at_exot_min_2hour_less"].ToString());
            row["at_ot_ex_ot_hour_min"] = decimal.Parse(setResult.Rows[0]["at_ot_ex_ot_hour_min"].ToString());
            row["at_ot_ex_ot_hour_min_with_wh_ot"] = decimal.Parse(setResult.Rows[0]["at_ot_ex_ot_hour_min_with_wh_ot"].ToString());
            row["at_holiday_ot_for_oneday"] = decimal.Parse(setResult.Rows[0]["at_holiday_ot_for_oneday"].ToString());
            row["at_ot_process_by"] = userName;
            getResult.Rows.Add(row);
        }
        public static DataTable AddNewDataColumnESum()
        {
            var dt = new DataTable();
            var columns = new[]
            {
                new DataColumn("comid", typeof(int)),
                new DataColumn("emp_id", typeof(int)),                
                new DataColumn("TTwHR", typeof(decimal)),
                new DataColumn("ttOT", typeof(decimal)),
                new DataColumn("ttOT_inc_previous_2hour", typeof(decimal)),
                new DataColumn("ttOT_inc_After_2hour", typeof(decimal)),
                new DataColumn("ttOT_inc_previous_4hour", typeof(decimal)),
                new DataColumn("ttOT_inc_After_4hour", typeof(decimal)),
                new DataColumn("ttEXOT", typeof(decimal)),
                new DataColumn("tt_ot_exot", typeof(decimal)),
                new DataColumn("tt_ot_inc_previous_bayer_4hour", typeof(decimal)),
                new DataColumn("tt_ot_inc_After_bayer_4hour", typeof(decimal)),
                new DataColumn("tt_ot_exot_bayer_4hour", typeof(decimal)),
                new DataColumn("Tt_HDOT", typeof(decimal)),
                new DataColumn("tt_ot_exot_hd_ot", typeof(decimal)),
                new DataColumn("month", typeof(int)),
                new DataColumn("year", typeof(int)),
                new DataColumn("TTHwHR", typeof(decimal))
            };
            dt.Columns.AddRange(columns);
            return dt;
        }
        public static void SetAttEsumColumnVal(DataTable getResult, DataTable setResult)
        {
            DataRow row = getResult.NewRow();
            row["comid"] = int.Parse(setResult.Rows[0]["comid"].ToString());
            row["emp_id"] = int.Parse(setResult.Rows[0]["emp_id"].ToString());
            row["TTwHR"] = decimal.Parse(setResult.Rows[0]["TTwHR"].ToString());
            row["ttOT"] = decimal.Parse(setResult.Rows[0]["ttOT"].ToString());
            row["ttOT_inc_previous_2hour"] = decimal.Parse(setResult.Rows[0]["ttOT_inc_previous_2hour"].ToString());
            row["ttOT_inc_After_2hour"] = decimal.Parse(setResult.Rows[0]["ttOT_inc_After_2hour"].ToString());
            row["ttOT_inc_previous_4hour"] = decimal.Parse(setResult.Rows[0]["ttOT_inc_previous_4hour"].ToString());
            row["ttOT_inc_After_4hour"] = string.IsNullOrEmpty(setResult.Rows[0]["ttOT_inc_After_4hour"].ToString()) ? DBNull.Value : decimal.Parse(setResult.Rows[0]["ttOT_inc_After_4hour"].ToString());
            row["ttEXOT"] = decimal.Parse(setResult.Rows[0]["ttEXOT"].ToString());
            row["tt_ot_exot"] = decimal.Parse(setResult.Rows[0]["tt_ot_exot"].ToString());
            row["tt_ot_inc_previous_bayer_4hour"] = string.IsNullOrEmpty(setResult.Rows[0]["tt_ot_inc_previous_bayer_4hour"].ToString()) ? DBNull.Value : decimal.Parse(setResult.Rows[0]["tt_ot_inc_previous_bayer_4hour"].ToString());
            row["tt_ot_inc_After_bayer_4hour"] = string.IsNullOrEmpty(setResult.Rows[0]["tt_ot_inc_After_bayer_4hour"].ToString()) ? DBNull.Value : decimal.Parse(setResult.Rows[0]["tt_ot_inc_After_bayer_4hour"].ToString());
            row["tt_ot_exot_bayer_4hour"] = decimal.Parse(setResult.Rows[0]["tt_ot_exot_bayer_4hour"].ToString());
            row["Tt_HDOT"] = decimal.Parse(setResult.Rows[0]["Tt_HDOT"].ToString());
            row["tt_ot_exot_hd_ot"] = string.IsNullOrEmpty(setResult.Rows[0]["tt_ot_exot_hd_ot"].ToString()) ? DBNull.Value : decimal.Parse(setResult.Rows[0]["tt_ot_exot_hd_ot"].ToString());
            row["month"] = int.Parse(setResult.Rows[0]["month"].ToString());
            row["year"] = int.Parse(setResult.Rows[0]["year"].ToString());
            row["TTHwHR"] = string.IsNullOrEmpty(setResult.Rows[0]["TTHwHR"].ToString()) ? DBNull.Value : decimal.Parse(setResult.Rows[0]["TTHwHR"].ToString());
            getResult.Rows.Add(row);
        }
    }
    public class AttendanceOtProcessChild
    {
        public int empserial { get; set; }
        public int empNo { get; set; }
    }
    public class TextUploadResponseEmp
    {
        public int emp_no { get; set; }
        public string sDate { get; set; }

        public static DataTable AddNewEcardDataColumn()
        {
            var dt = new DataTable();
            var columns = new[]
            {
                new DataColumn("comp_id", typeof(int)),
                new DataColumn("emp_id", typeof(int)),
                new DataColumn("Present", typeof(int)),
                new DataColumn("Absent", typeof(int)),
                new DataColumn("Late", typeof(int)),
                new DataColumn("WeekHoliday",typeof(decimal)),
                new DataColumn("SpecialHoliday",typeof(decimal)),
                new DataColumn("shDate",typeof(string))
            };
            dt.Columns.AddRange(columns);
            return dt;
        }
        public static void SetEcardDataColumnVal(DataTable acuTable,DataTable output,string sDate)
        {
            DataRow row = acuTable.NewRow();
            row["comp_id"] = int.Parse(output.Rows[0]["comid"].ToString());
            row["emp_id"] = int.Parse(output.Rows[0]["emp_id"].ToString());
            row["Present"] = !string.IsNullOrEmpty(output.Rows[0]["ttalpresent"].ToString()) ? int.Parse(output.Rows[0]["ttalpresent"].ToString()) : DBNull.Value;
            row["Absent"] = !string.IsNullOrEmpty(output.Rows[0]["ttabsent"].ToString()) ? int.Parse(output.Rows[0]["ttabsent"].ToString()) : DBNull.Value;
            row["Late"] = !string.IsNullOrEmpty(output.Rows[0]["total_late"].ToString()) ? int.Parse(output.Rows[0]["total_late"].ToString()) : DBNull.Value;
            row["WeekHoliday"] = !string.IsNullOrEmpty(output.Rows[0]["total_WH"].ToString()) ? decimal.Parse(output.Rows[0]["total_WH"].ToString()) : DBNull.Value;
            row["SpecialHoliday"] = !string.IsNullOrEmpty(output.Rows[0]["totalHoliday"].ToString()) ? decimal.Parse(output.Rows[0]["totalHoliday"].ToString()) : DBNull.Value;
            row["shDate"] = sDate;
            acuTable.Rows.Add(row);
        }
    }
}
