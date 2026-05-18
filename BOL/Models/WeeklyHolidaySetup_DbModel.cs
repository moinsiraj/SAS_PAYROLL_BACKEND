using System.ComponentModel.DataAnnotations;

namespace BOL.Models
{
    public class WeeklyHolidaySetup_DbModel
    {
        [Key]
        public int wh_sl { get; set; }
        public int wh_compid { get; set;}
        public int wh_emp_no { get; set;}
        public int wh_emp_serial { get; set;}
        public DateOnly wh_date { get; set; }
        public string wh_day_name { get; set;}
        public string wh_create_uid { get; set; }
        public DateTime wh_create_udate { get;set; }
    }
    public class WeeklyHolidayPayload
    {
        public List<EmployeeInfoList> employeeBasic {  get; set; }
        public string[] holiDate { get; set; }
        public string monthYear { get; set; }
        public string fixedDayName { get; set; }
        public string whType { get; set; }
        public string userName { get; set; }
    }
    public class EmployeeInfoList
    {
        public int companyID { get; set; }
        public int employeeNO { get; set; }
        public int employeeSL { get; set; }
    }
}