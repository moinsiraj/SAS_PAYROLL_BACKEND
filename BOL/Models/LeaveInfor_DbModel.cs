using System.ComponentModel.DataAnnotations;

namespace BOL.Models
{
    public class LeaveInfor_DbModel
    {
        [Key]
        public int lev_serial { get; set; }
        public int? lev_groupid { get; set; } = null;
        public int? lev_compid { get; set; } = null;
        public int lev_year { get; set; }
        public decimal lev_emp_serial { get; set; }
        public decimal? lev_casual { get; set; } = null;
        public decimal? lev_medical { get; set; } = null;
        public decimal? lev_anual { get; set; } = null;
        public int? lev_lwp { get; set; } = null;
        public int? lev_total_leavedays { get; set; } = null;
        public decimal? lev_casual_bal { get; set; } = null;
        public decimal? lev_medical_bal { get; set; } = null;
        public decimal? lev_anual_bal { get; set; } = null;
        public int? lev_lwp_bal { get; set; } = null;
        public string lev_reason { get; set; } = null;
        public string lev_address { get; set; } = null;
        public bool? lev_approved_status { get; set; } = null;
        public string lev_user { get; set; } = null;
        public DateTime? lev_udate { get; set; } = null;
    }
    public class DgPayLeaveInfor
    {
        public int LevSerial { get; set; }
        public int? LevGroupid { get; set; }
        public int? LevCompid { get; set; }
        public int LevYear { get; set; }
        public decimal LevEmpSerial { get; set; }
        public decimal? LevCasual { get; set; }
        public decimal? LevMedical { get; set; }
        public decimal? LevAnual { get; set; }
        public int? LevLwp { get; set; } = null;
        public int? LevTotalLeavedays { get; set; } = null;
        public decimal? LevCasualBal { get; set; }
        public decimal? LevMedicalBal { get; set; }
        public decimal? LevAnualBal { get; set; }
        public int? LevLwpBal { get; set; } = null;
        public string LevReason { get; set; } = null;
        public string LevAddress { get; set; } = null;
        public bool? LevApprovedStatus { get; set; } = null;
        public string LevUser { get; set; }
        public DateTime? LevUdate { get; set; }

        public static LeaveInfor_DbModel CustomToDbModel(DgPayLeaveInfor obj)
        {
            try
            {
                var DbModel = new LeaveInfor_DbModel
                {
                    lev_serial = obj.LevSerial,
                    lev_groupid = obj.LevGroupid,
                    lev_compid = obj.LevCompid,
                    lev_year = obj.LevYear,
                    lev_emp_serial = obj.LevEmpSerial,
                    lev_casual = obj.LevCasual,
                    lev_medical = obj.LevMedical,
                    lev_anual = obj.LevAnual,
                    lev_lwp = obj.LevLwp,
                    lev_total_leavedays = obj.LevTotalLeavedays,
                    lev_casual_bal = obj.LevCasualBal,
                    lev_medical_bal = obj.LevMedicalBal,
                    lev_anual_bal = obj.LevAnualBal,
                    lev_lwp_bal = obj.LevLwpBal,
                    lev_reason = obj.LevReason,
                    lev_address = obj.LevAddress,
                    lev_approved_status = obj.LevApprovedStatus,
                    lev_user = obj.LevUser,
                    lev_udate = obj.LevUdate
                };
                return DbModel;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return null;
        }
        public static DgPayLeaveInfor DbToCustomModel(LeaveInfor_DbModel obj)
        {
            try
            {
                var custonModel = new DgPayLeaveInfor
                {
                    LevSerial = obj.lev_serial,
                    LevGroupid = obj.lev_groupid,
                    LevCompid = obj.lev_compid,
                    LevYear = obj.lev_year,
                    LevEmpSerial = obj.lev_emp_serial,
                    LevCasual = obj.lev_casual,
                    LevMedical = obj.lev_medical,
                    LevAnual = obj.lev_anual,
                    LevLwp = obj.lev_lwp,
                    LevTotalLeavedays = obj.lev_total_leavedays,
                    LevCasualBal = obj.lev_casual_bal,
                    LevMedicalBal = obj.lev_medical_bal,
                    LevAnualBal = obj.lev_anual_bal,
                    LevLwpBal = obj.lev_lwp_bal,
                    LevReason = obj.lev_reason,
                    LevAddress = obj.lev_address,
                    LevApprovedStatus = obj.lev_approved_status,
                    LevUser = obj.lev_user,
                    LevUdate = obj.lev_udate
                };
                return custonModel;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return null;
        }
    }
}