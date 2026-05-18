using System.ComponentModel.DataAnnotations;

namespace BOL.Models
{
    public class Leavetransaction_DbModel
    {
        [Key]
        public int ltr_serial { get; set; }
        public int? ltr_groupid { get; set; } = null;
        public int? ltr_compid { get; set; } = null;
        public DateTime ltr_date { get; set; }
        public int ltr_emp_serial { get; set; }
        public int? ltr_casual { get; set; } = null;
        public int? ltr_medical { get; set; } = null;
        public int? ltr_anual { get; set; } = null;
        public int? ltrNp { get; set; } = null;
        public int? ltr_Csl { get; set; } = null;
        public string ltr_user { get; set; } = null;
        public DateTime? ltr_udate { get; set; } = null;
    }
    public class DgPayLeavetransaction
    {
        public int LtrSerial { get; set; }
        public int? LtrGroupid { get; set; } = null;
        public int? LtrCompid { get; set; } = null;
        public DateTime LtrDate { get; set; }
        public int LtrEmpSerial { get; set; }
        public int? LtrCasual { get; set; } = null;
        public int? LtrMedical { get; set; } = null;
        public int? LtrAnual { get; set; } = null;
        public int? ltrNp { get; set; } = null;
        public int? ltrCsl { get; set; } = null;
        public string LtrUser { get; set; } = null;
        public DateTime? LtrUdate { get; set; } = null;

        public static Leavetransaction_DbModel CustomToDbModel(DgPayLeavetransaction obj)
        {
            try
            {
                var dbModel = new Leavetransaction_DbModel
                {
                    ltr_serial = obj.LtrSerial,
                    ltr_groupid = obj.LtrGroupid,
                    ltr_compid = obj.LtrCompid,
                    ltr_date = obj.LtrDate,
                    ltr_emp_serial = obj.LtrEmpSerial,
                    ltr_casual = obj.LtrCasual,
                    ltr_medical = obj.LtrMedical,
                    ltr_anual = obj.LtrAnual,
                    ltrNp = obj.ltrNp,
                    ltr_Csl = obj.ltrCsl,
                    ltr_user = obj.LtrUser,
                    ltr_udate = obj.LtrUdate
                };
                return dbModel;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return null;
        }
        public static DgPayLeavetransaction DbToCustomModel(Leavetransaction_DbModel obj)
        {
            try
            {
                var customModel = new DgPayLeavetransaction
                {
                    LtrSerial = obj.ltr_serial,
                    LtrGroupid = obj.ltr_groupid,
                    LtrCompid = obj.ltr_compid,
                    LtrDate = obj.ltr_date,
                    LtrEmpSerial = obj.ltr_emp_serial,
                    LtrCasual = obj.ltr_casual,
                    LtrMedical = obj.ltr_medical,
                    LtrAnual = obj.ltr_anual,
                    ltrNp = obj.ltrNp,
                    ltrCsl = obj.ltr_Csl,
                    LtrUser = obj.ltr_user,
                    LtrUdate = obj.ltr_udate
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
    public class LevTransFdateTdate_DbModel
    {
        [Key]
        public int SL { get; set; }
        public int? lev_compid { get; set; } = null;
        public int? lev_emp_serial { get; set; } = null;
        public DateTime lev_from_date { get; set; }
        public DateTime? lev_to_date { get; set; } = null;
        public string lev_type { get; set; } = null;
        public int? leave_days { get; set; } = null;
        public string lev_reasion { get; set; } = null;
        public string lev_leavetime_location { get; set; } = null;
        public string lev_user { get; set; } = null;
        public DateTime? lev_add_date { get; set; } = null;
        public bool lev_isOnline { get; set; } = false;
    }
    public class LeaveTransactionPayload
    {
        public int companyId { get; set; }
        public int empSerial { get; set; }
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }
        public string levType { get; set; }
        public int leaveDays { get; set; }
        public string levReasion { get; set; }
        public string levLocation { get; set; }
        public int casual { get; set; }
        public int medical { get; set; }
        public int anual { get; set; }
        public int notPayable { get; set; }
        public int comlev { get; set; }
        public string user { get; set; }
        public int isOnline { get; set; } = 0;
    }
    public class LeaveTransaction_BulkPayload
    {
        public int companyId { get; set; }
        public int[] empSerial { get; set; }
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }
        public string levType { get; set; }
        public int leaveDays { get; set; }
        public string levReasion { get; set; }
        public string levLocation { get; set; }
        public int casual { get; set; }
        public int medical { get; set; }
        public int anual { get; set; }
        public int notPayable { get; set; }
        public int comlev { get; set; }
        public string user { get; set; }
        public int isOnline { get; set; }
        public static LeaveTransactionPayload Finalobj(LeaveTransaction_BulkPayload obj, int empserial)
        {
            var result = new LeaveTransactionPayload
            {
                companyId = obj.companyId,
                empSerial = empserial,
                fromDate = obj.fromDate,
                toDate = obj.toDate,
                levType = obj.levType,
                leaveDays = obj.leaveDays,
                levReasion = obj.levReasion,
                levLocation = obj.levLocation,
                casual = obj.casual,
                medical = obj.medical,
                anual = obj.anual,
                notPayable = obj.notPayable,
                comlev = obj.comlev,
                user = obj.user,
                isOnline = obj.isOnline,
            };
            return result;
        }
    }
    public class LeaveTransactionOnlinePayload
    {
        public int levOn_compid { get; set; }
        public int levOn_emp_serial { get; set; }
        public string levOn_from_date { get; set; }
        public string levOn_to_date { get; set; }
        public string levOn_type { get; set; }
        public int leaveOn_days { get; set; }
        public string levOn_reasion { get; set; }
        public string levOn_leavetime_location { get; set; }
        public int transEmpSerial { get; set; }
        public int recomEmpSerial { get; set; }
        public int dptEmpSerial { get; set; }
        public string userName { get; set; }
    }
    public class LeaveOnlineApp
    {
        public int[] leaveID { get; set;}
        public int empSerial { get; set; }
        public string userName { get; set; }
    }
}
