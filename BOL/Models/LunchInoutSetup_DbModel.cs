using System.ComponentModel.DataAnnotations;

namespace BOL.Models
{
    public class LunchInoutSetup_DbModel
    {
        [Key]
        public int l_serial { get; set; }
        public int? groupID { get; set; } = null;
        public int? com_id { get; set; } = null;
        public string com_Name { get; set; } = null;
        public decimal? lunch_inTime { get; set; } = null;
        public decimal? lunch_outTime { get; set; } = null;
        public string enteredby { get; set; } = null;
        public string updatedby { get; set; } = null;
        public DateTime? addtime { get; set; } = null;
        public DateTime? updatetime { get; set; } = null;
    }
    public class DgLunchInoutSetup
    {
        public int LSerial { get; set; }
        public int? GroupId { get; set; } = null;
        public int? ComId { get; set; } = null;
        public string ComName { get; set; } = null;
        public decimal? LunchInTime { get; set; } = null;
        public decimal? LunchOutTime { get; set; } = null;
        public string Enteredby { get; set; } = null;
        public string Updatedby { get; set; } = null;
        public DateTime? Addtime { get; set; } = null;
        public DateTime? Updatetime { get; set; } = null;

        public static LunchInoutSetup_DbModel CustomToDbModel(DgLunchInoutSetup obj)
        {
            try
            {
                var dbModel = new LunchInoutSetup_DbModel
                {
                    l_serial = obj.LSerial,
                    groupID = obj.GroupId,
                    com_id = obj.ComId,
                    com_Name = obj.ComName,
                    lunch_inTime = obj.LunchInTime,
                    lunch_outTime = obj.LunchOutTime,
                    enteredby = obj.Enteredby,
                    updatedby = obj.Updatedby,
                    addtime = obj.Addtime,
                    updatetime = obj.Updatetime
                };
                return dbModel;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return null;
        }
        public static DgLunchInoutSetup DbToCustomModel(LunchInoutSetup_DbModel obj)
        {
            try
            {
                var customModel = new DgLunchInoutSetup
                {
                    LSerial = obj.l_serial,
                    GroupId = obj.groupID,
                    ComId = obj.com_id,
                    ComName = obj.com_Name,
                    LunchInTime = obj.lunch_inTime,
                    LunchOutTime = obj.lunch_outTime,
                    Enteredby = obj.enteredby,
                    Updatedby = obj.updatedby,
                    Addtime = obj.addtime,
                    Updatetime = obj.updatetime
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
    public class EmployeeLunchOutPayload
    {
        public int companyID { get; set; }
        public List<LunchOutEmpDetails> employeeInfo { get; set; }
        public string[] lunchOutDate { get; set; }
        public string loMonthYear { get; set; }
        public string userName { get; set; }
    }
    public class LunchOutEmpDetails
    {
        public int employeeNo { get; set; }
        public int employeeSl { get; set; }
    }
    public class EmpLunchOutList
    {
        public int companyID { get; set; }
        public int employeeNo { get; set; }
        public int employeeSerial { get; set; }       
        public string lunchOutDate { get; set; }
        public string status { get; set; }
        public string userName { get; set; }
    }
    public class LunchOutEmployeeInfo
    {
        public int companyID { get; set; }
        public int departmentID { get; set; }
        public int sectionID { get; set; }
        public string[] dateRange { get; set; }
    }
}