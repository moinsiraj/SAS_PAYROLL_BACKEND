using System.ComponentModel.DataAnnotations;

namespace BOL.Models
{
    public class LoanCategory_DbModel
    {
        [Key]
        public int lc_id { get; set; }
        public string lc_category { get; set; } = null;
        public string lc_user { get; set; } = null;
        public DateTime? lc_udate { get; set; } = null;
    }
    public class DgPayLoancategory
    {
        public int LcId { get; set; }
        public string LcCategory { get; set; } = null;
        public string LcUser { get; set; } = null;
        public DateTime? LcUdate { get; set; } = null;

        public static LoanCategory_DbModel CustonToDbModel(DgPayLoancategory obj)
        {
            try
            {
                var dbModel = new LoanCategory_DbModel
                {
                    lc_id = obj.LcId,
                    lc_category = obj.LcCategory,
                    lc_user = obj.LcUser,
                    lc_udate = obj.LcUdate
                };
                return dbModel;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return null;
        }
        public static DgPayLoancategory DbToCustomModel(LoanCategory_DbModel obj)
        {
            try
            {
                var customModel = new DgPayLoancategory
                {
                    LcId = obj.lc_id,
                    LcCategory = obj.lc_category,
                    LcUser = obj.lc_user,
                    LcUdate = obj.lc_udate
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