using System.ComponentModel.DataAnnotations;

namespace BOL.Models
{
    public class Allowance_DbModel
    {
        [Key]
        public int al_serial { get; set; }
        public decimal al_emp_serial { get; set; }
        public int? al_groupid { get; set; } = null;
        public int? al_compid { get; set; } = null;
        public DateTime? al_date { get; set; } = null;
        public int? al_id { get; set; } = null;
        public decimal? al_value { get; set; } = null;
        public string al_user { get; set; } = null;
        public DateTime? al_udate { get; set; } = null;
    }
    public class DgPayAllowancePostPayload
    {
        public int AlEmpSerial { get; set; }
        public int AlGroupid { get; set; }
        public int AlCompid { get; set; }
        public string AlDate { get; set; }
        public List<DgPayAllowancePostPayloadChild> AllowanceChild { get; set; }
        public string AlUser { get; set; }
        //public DateTime AlUdate { get; set; }
    }
    public class DgPayAllowancePostPayloadChild
    {
        public int al_id { get; set; }
        public decimal AlValue { get; set; }
    }
    public class DgPayAllowance
    {
        public int AlSerial { get; set; }
        public decimal AlEmpSerial { get; set; }
        public int? AlGroupid { get; set; } = null;
        public int? AlCompid { get; set; } = null;
        public DateTime? AlDate { get; set; } = null;
        public int? AlId { get; set; } = null;
        public decimal? AlValue { get; set; } = null;
        public string AlUser { get; set; } = null;
        public DateTime? AlUdate { get; set; } = null;

        public static Allowance_DbModel CustonToDbModel(DgPayAllowance obj)
        {
            try
            {
                var dbModel = new Allowance_DbModel
                {
                    al_serial = obj.AlSerial,
                    al_emp_serial = obj.AlEmpSerial,
                    al_groupid = obj.AlGroupid,
                    al_compid = obj.AlCompid,
                    al_date = obj.AlDate,
                    al_id = obj.AlId,
                    al_value = obj.AlValue,
                    al_user = obj.AlUser,
                    al_udate = obj.AlUdate
                };
                return dbModel;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return null;
        }
        public static DgPayAllowance DbToCustomModel(Allowance_DbModel obj)
        {
            try
            {
                var customModel = new DgPayAllowance
                {
                    AlSerial = obj.al_serial,
                    AlEmpSerial = obj.al_emp_serial,
                    AlGroupid = obj.al_groupid,
                    AlCompid = obj.al_compid,
                    AlDate = obj.al_date,
                    AlId = obj.al_id,
                    AlValue = obj.al_value,
                    AlUser = obj.al_user,
                    AlUdate = obj.al_udate
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