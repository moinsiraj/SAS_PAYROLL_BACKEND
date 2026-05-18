using System.ComponentModel.DataAnnotations;

namespace BOL.Models
{
    public class AttbonusRule_DbModel
    {
        [Key]
        public int atbr_serial { get; set; }
        public int? ComID { get; set; } = null;
        public string ComName { get; set; } = null;
        public int? atbr_absentdays { get; set; } = null;
        public int? atbr_late { get; set; } = null;
        public int? atbr_leave { get; set; } = null;
        public int? atbr_groupid { get; set; } = null;
        public string updatedby { get; set; } = null;
        public DateTime? updatedtime { get; set; } = null;
    }
    public class DgPayAttbonusRule
    {
        public int AtbrSerial { get; set; }
        public int? ComId { get; set; } = null;
        public string ComName { get; set; } = null;
        public int? AtbrAbsentdays { get; set; } = null;
        public int? AtbrLate { get; set; } = null;
        public int? AtbrLeave { get; set; } = null;
        public int? AtbrGroupid { get; set; } = null;
        public string Updatedby { get; set; } = null;
        public DateTime? Updatedtime { get; set; } = null;

        public static AttbonusRule_DbModel CustonToDbModel(DgPayAttbonusRule obj)
        {
            try
            {
                var dbModel = new AttbonusRule_DbModel
                {
                    atbr_serial = obj.AtbrSerial,
                    ComID = obj.ComId,
                    ComName = obj.ComName,
                    atbr_absentdays = obj.AtbrAbsentdays,
                    atbr_late = obj.AtbrLate,
                    atbr_leave = obj.AtbrLeave,
                    atbr_groupid = obj.AtbrGroupid,
                    updatedby = obj.Updatedby,
                    updatedtime = obj.Updatedtime
                };
                return dbModel;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return null;
        }
        public static DgPayAttbonusRule DbToCustomModel(AttbonusRule_DbModel obj)
        {
            try
            {
                var customModel = new DgPayAttbonusRule
                {
                    AtbrSerial = obj.atbr_serial,
                    ComId = obj.ComID,
                    ComName = obj.ComName,
                    AtbrAbsentdays = obj.atbr_absentdays,
                    AtbrLate = obj.atbr_late,
                    AtbrLeave = obj.atbr_leave,
                    AtbrGroupid = obj.atbr_groupid,
                    Updatedby = obj.updatedby,
                    Updatedtime = obj.updatedtime
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
}
