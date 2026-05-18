using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Runtime.CompilerServices;

namespace BOL.Models
{
    public class Specialholiday_DbModel
    {
        [Key]
        public int serial { get; set; }
        public DateTime? sh_date { get; set; } = null;
        public string sh_description { get; set; } = null;
        public int? sh_compid { get; set; } = null;
        public string sh_compName { get; set; } = null;
        public int? sh_groupid { get; set; } = null;
        public string sh_user { get; set; } = null;
        public DateTime? sh_udate { get; set; } = null;
    }
    public class EmpWiseSpecialholidayPayload
    {
        public int companyID { get; set; }
        public List<EmployeeInfoSpecialholiday> empInfos { get; set; }
        public string formDate { get; set; }
        public string toDate { get; set; }
        public string description { get; set; }
        public bool isGov { get; set; }
        public string userName { get; set; }
    }
    public class EmployeeInfoSpecialholiday
    {
        public int emp_serial { get; set; }
        public int emp_no { get; set; }
    }
    public class DgPaySpecialholiday
    {
        public int Serial { get; set; }
        public DateTime? ShDate { get; set; } = null;
        public string ShDescription { get; set; } = null;
        public int? ShCompid { get; set; } = null;
        public string ShCompName { get; set; } = null;
        public int? ShGroupid { get; set; } = null;
        public string ShUser { get; set; } = null;
        public DateTime? ShUdate { get; set; } = null;

        public static Specialholiday_DbModel CustomToDbModel(DgPaySpecialholiday obj)
        {
            try
            {
                var dbModel = new Specialholiday_DbModel
                {
                    serial = obj.Serial,
                    sh_date = obj.ShDate,
                    sh_description = obj.ShDescription,
                    sh_compid = obj.ShCompid,
                    sh_compName = obj.ShCompName,
                    sh_groupid = obj.ShGroupid,
                    sh_user = obj.ShUser,
                    sh_udate = obj.ShUdate
                };
                return dbModel;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return null;
        }
        public static DgPaySpecialholiday DbToCustomModel(Specialholiday_DbModel obj)
        {
            try
            {
                var customModel = new DgPaySpecialholiday
                {
                    Serial = obj.serial,
                    ShDate = obj.sh_date,
                    ShDescription = obj.sh_description,
                    ShCompid = obj.sh_compid,
                    ShCompName = obj.sh_compName,
                    ShGroupid = obj.sh_groupid,
                    ShUser = obj.sh_user,
                    ShUdate = obj.sh_udate
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
    public class DeleteSpecialholidayPayload
    {
        public int compID { get; set; }
        public string formDate { get; set; }
        public string toDate { get; set; }
        public List<DeleteSpecialholidayChild> delChilds { get; set; }
    }
    public class DeleteSpecialholidayChild
    {
        public int empSerial { get; set; }
        public int empNumber { get; set; }
    }
    public class SpecialholidayPayloadList
    {
        public int comp_id { get; set; }
        public int emp_serial_id { get; set; }
        public int emp_no { get; set; }
        public string shiftDate { get; set; }
        public string fDate { get; set; }
        public string tDate { get; set; }
        public string description { get; set; }
        public bool isGov { get; set; }
        public string user { get; set; }
    }
    public class SpecialholidayEcardSum
    {
        public int comp_id { get; set; }
        public int emp_id { get; set; }
        public int Present { get; set; }
        public int Absent { get; set; }
        public int Late { get; set; }
        public int WeekHoliday { get; set; }
        public int SpecialHoliday { get; set; }
        public string shDate { get; set; }
    }
    public class SpecialholidayProcessPayload
    {
        public int companyID { get; set; }
        public string processMonthYear { get; set; }
    }
    public class HolidayProcessPayload
    {
        public int companyId { get; set; }
        public string formDate { get; set; }
        public string toDate { get; set; }
        public int[] employeeSl { get; set; }
        public string userName { get; set; }
    }
    public class HolidayBillProcessDelPayload
    {
        public int companyId { get; set; }
        public string monthYear { get; set; }
        public int[] employeeSl { get; set; }
    }
}