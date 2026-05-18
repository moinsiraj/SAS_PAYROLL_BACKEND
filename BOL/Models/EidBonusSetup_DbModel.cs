using System.ComponentModel.DataAnnotations;

namespace BOL.Models
{
    public class EidBonusSetup_DbModel
    {
        [Key]
        public int eb_serial { get; set; }
        public int gid { get; set; }
        public int? com_code { get; set; } = null;
        public string com_name { get; set; } = null;
        public int? groupid { get; set; } = null;
        public string type { get; set; } = null;
        public string dependon { get; set; } = null;
        public decimal? days_365 { get; set; } = null;
        public decimal? days_270 { get; set; } = null;
        public decimal? days_180 { get; set; } = null;
        public decimal? days_90 { get; set; } = null;
        public decimal? bellowdays_90 { get; set; } = null;
        public string enteredby { get; set; } = null;
        public string updatedby { get; set; } = null;
        public DateTime? addtime { get; set; } = null;
        public DateTime? updatetime { get; set; } = null;
        public int? stampCharge { get; set; } = null;
        public int? salary_category { get; set; } = null;
        public string category_name { get; set; } = null;
        public string type_days_365 { get; set; } = null;
        public string type_days_270 { get; set; } = null;
        public string type_days_180 { get; set; } = null;
        public string type_days_90 { get; set; } = null;
        public string type_days_90Less { get; set; } = null;
    }
    public class DgPayEidBonusSetup
    {
        public int EbSerial { get; set; }
        public int Gid { get; set; }
        public int? ComCode { get; set; } = null;
        public string ComName { get; set; } = null;
        public int? Groupid { get; set; } = null;
        public string Type { get; set; } = null;
        public string Dependon { get; set; } = null;
        public decimal? Days365 { get; set; } = null;
        public decimal? Days270 { get; set; } = null;
        public decimal? Days180 { get; set; } = null;
        public decimal? Days90 { get; set; } = null;
        public decimal? Bellowdays90 { get; set; } = null;
        public string Enteredby { get; set; } = null;
        public string Updatedby { get; set; } = null;
        public DateTime? Addtime { get; set; } = null;
        public DateTime? Updatetime { get; set; } = null;
        public int? StampCharge { get; set; } = null;
        public int? SalaryCategory { get; set; } = null;
        public string CategoryName { get; set; } = null;
        public string TypeDays365 { get; set; } = null;
        public string TypeDays270 { get; set; } = null;
        public string TypeDays180 { get; set; } = null;
        public string TypeDays90 { get; set; } = null;
        public string TypeDays90less { get; set; } = null;

        public static EidBonusSetup_DbModel CustomToDbModel(DgPayEidBonusSetup obj)
        {
            try
            {
                var CustomDb = new EidBonusSetup_DbModel
                {
                    eb_serial = obj.EbSerial,
                    gid = obj.Gid,
                    com_code = obj.ComCode,
                    com_name = obj.ComName,
                    groupid = obj.Groupid,
                    type = obj.Type,
                    dependon = obj.Dependon,
                    days_365 = obj.Days365,
                    days_270 = obj.Days270,
                    days_180 = obj.Days180,
                    days_90 = obj.Days90,
                    bellowdays_90 = obj.Bellowdays90,
                    enteredby = obj.Enteredby,
                    updatedby = obj.Updatedby,
                    addtime = obj.Addtime,
                    updatetime = obj.Updatetime,
                    stampCharge = obj.StampCharge,
                    salary_category = obj.SalaryCategory,
                    category_name = obj.CategoryName,
                    type_days_365 = obj.TypeDays365,
                    type_days_270 = obj.TypeDays270,
                    type_days_180 = obj.TypeDays180,
                    type_days_90 = obj.TypeDays90,
                    type_days_90Less = obj.TypeDays90less
                };
                return CustomDb;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return null;
        }
        public static DgPayEidBonusSetup DbToCustomModel(EidBonusSetup_DbModel obj)
        {
            try
            {
                var DbCustom = new DgPayEidBonusSetup
                {
                    EbSerial = obj.eb_serial,
                    Gid = obj.gid,
                    ComCode = obj.com_code,
                    ComName = obj.com_name,
                    Groupid = obj.groupid,
                    Type = obj.type,
                    Dependon = obj.dependon,
                    Days365 = obj.days_365,
                    Days270 = obj.days_270,
                    Days180 = obj.days_180,
                    Days90 = obj.days_90,
                    Bellowdays90 = obj.bellowdays_90,
                    Enteredby = obj.enteredby,
                    Updatedby = obj.updatedby,
                    Addtime = obj.addtime,
                    Updatetime = obj.updatetime,
                    StampCharge = obj.stampCharge,
                    SalaryCategory = obj.salary_category,
                    CategoryName = obj.category_name,
                    TypeDays365 = obj.type_days_365,
                    TypeDays270 = obj.type_days_270,
                    TypeDays180 = obj.type_days_180,
                    TypeDays90 = obj.type_days_90,
                    TypeDays90less = obj.type_days_90Less
                };
                return DbCustom;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return null;
        }
    }
    public class EidBonusProcess
    {
        public int CompId { get; set; }
        public string Cut_of_date { get; set; }
        public string proc_month { get; set; }
        public string Eid_name { get; set; }
        public string enteredby { get; set; }
    }
}