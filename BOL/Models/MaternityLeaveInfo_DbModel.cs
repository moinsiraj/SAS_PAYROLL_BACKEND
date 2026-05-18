using System.ComponentModel.DataAnnotations;

namespace BOL.Models
{
    public class MaternityLeaveInfo_DbModel
    {
        [Key]
        public int serialNo { get; set; }
        public int? com_code { get; set; } = null;
        public int? groupID { get; set; } = null;
        public int? emp_serial { get; set; } = null;
        public DateTime? date_of_first_notification { get; set; } = null;
        public DateTime? expected_date_delivery { get; set; } = null;
        public DateTime? lev_start_date { get; set; } = null;
        public DateTime? lev_end_date { get; set; } = null;
        public string leave_type { get; set; } = null;
        public string enteredby { get; set; } = null;
        public string updatedby { get; set; } = null;
        public DateTime? addtime { get; set; } = null;
        public DateTime? updatetime { get; set; } = null;
    }
    public class DgPayMaternityLeaveInfo
    {
        public int SerialNo { get; set; }
        public int? ComCode { get; set; } = null;
        public int? GroupId { get; set; } = null;
        public int? EmpSerial { get; set; } = null;
        public DateTime? DateOfFirstNotification { get; set; } = null;
        public DateTime? ExpectedDateDelivery { get; set; } = null;
        public DateTime? LevStartDate { get; set; } = null;
        public DateTime? LevEndDate { get; set; } = null;
        public string LeaveType { get; set; } = null;
        public string Enteredby { get; set; } = null;
        public string Updatedby { get; set; } = null;
        public DateTime? Addtime { get; set; } = null;
        public DateTime? Updatetime { get; set; } = null;

        public static MaternityLeaveInfo_DbModel CustomToDbModel(DgPayMaternityLeaveInfo obj)
        {
            try
            {
                var DbModel = new MaternityLeaveInfo_DbModel
                {
                    serialNo = obj.SerialNo,
                    com_code = obj.ComCode,
                    groupID = obj.GroupId,
                    emp_serial = obj.EmpSerial,
                    date_of_first_notification = obj.DateOfFirstNotification,
                    expected_date_delivery = obj.ExpectedDateDelivery,
                    lev_start_date = obj.LevStartDate,
                    lev_end_date = obj.LevEndDate,
                    leave_type = obj.LeaveType,
                    enteredby = obj.Enteredby,
                    updatedby = obj.Updatedby,
                    addtime = obj.Addtime,
                    updatetime = obj.Updatetime
                };
                return DbModel;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return null;
        }
        public static DgPayMaternityLeaveInfo DbToCustomModel(MaternityLeaveInfo_DbModel obj)
        {
            try
            {
                var customModel = new DgPayMaternityLeaveInfo
                {
                    SerialNo = obj.serialNo,
                    ComCode = obj.com_code,
                    GroupId = obj.groupID,
                    EmpSerial = obj.emp_serial,
                    DateOfFirstNotification = obj.date_of_first_notification,
                    ExpectedDateDelivery = obj.expected_date_delivery,
                    LevStartDate = obj.lev_start_date,
                    LevEndDate = obj.lev_end_date,
                    LeaveType = obj.leave_type,
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

    public class MaternityPaymentModification
    {
        public int companyID { get; set; }
        public int[] employeeSL { get; set; }
        public string formDate { get; set; }
        public string toDate { get; set; }
        public string clickTo { get; set; }
    }
}