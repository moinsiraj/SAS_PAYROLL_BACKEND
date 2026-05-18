using System.ComponentModel.DataAnnotations;

namespace BOL.Models
{
    public class SalaryAdvanceLog_DbModel
    {
        [Key]
        public int serial { get; set; }
        public int sapl_compid { get; set; }
        public int sapl_month { get; set; }
        public int sapl_year { get; set; }
        public bool sapl_with_ot { get; set; }
        public bool sapl_With_TiffinBill { get; set; }
        public bool sapl_With_nightBill { get; set; }
        public int sapl_days { get; set; }
        public string sapl_user { get; set; }
        public DateTime sapl_udate { get; set; }
    }
    public class DgPaySalaryAdvanceLog
    {
        public int Serial { get; set; }
        public int SaplCompid { get; set; }
        public string SaplDate { get; set; }
        public int SaplMonth { get; set; }
        public int SaplYear { get; set; }
        public bool SaplWithOt { get; set; }
        public bool SaplWithTiffinBill { get; set; }
        public bool SaplWithNightBill { get; set; }
        public int SaplDays { get; set; }
        public string SaplUser { get; set; }
        public DateTime SaplUdate { get; set; }

        public static SalaryAdvanceLog_DbModel CustomToDbModel(DgPaySalaryAdvanceLog obj)
        {
            try
            {
                var dbModel = new SalaryAdvanceLog_DbModel
                {
                    serial = obj.Serial,
                    sapl_compid = obj.SaplCompid,
                    sapl_month = Convert.ToDateTime(obj.SaplDate).Month,
                    sapl_year = Convert.ToDateTime(obj.SaplDate).Year,
                    sapl_with_ot = obj.SaplWithOt,
                    sapl_With_TiffinBill = obj.SaplWithTiffinBill,
                    sapl_With_nightBill = obj.SaplWithNightBill,
                    sapl_days = obj.SaplDays,
                    sapl_user = obj.SaplUser,
                    sapl_udate = DateTime.Now,
                };
                return dbModel;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return null;
        }
        public static DgPaySalaryAdvanceLog DbToCustomModel(SalaryAdvanceLog_DbModel obj)
        {
            try
            {
                var customModel = new DgPaySalaryAdvanceLog
                {
                    Serial = obj.serial,
                    SaplCompid = obj.sapl_compid,
                    SaplMonth = obj.sapl_month,
                    SaplYear = obj.sapl_year,
                    SaplWithOt = obj.sapl_with_ot,
                    SaplWithTiffinBill = obj.sapl_With_TiffinBill,
                    SaplWithNightBill = obj.sapl_With_nightBill,
                    SaplDays = obj.sapl_days,
                    SaplUser = obj.sapl_user,
                    SaplUdate = obj.sapl_udate
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
    public class PaySalaryAdvancePayment
    {
        public int compid { get; set; }
        public string pDate { get; set; }
        public string salMonth { get; set; }
    }
}