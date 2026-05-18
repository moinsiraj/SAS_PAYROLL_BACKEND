using System.ComponentModel.DataAnnotations;

namespace BOL.Models
{
    public class Allowancesde_DbModel
    {
        [Key]
        public int al_code { get; set; }
        public int? al_groupid { get; set; } = null;
        public string al_description { get; set; } = null;
        public string al_type { get; set; } = null;
        public string al_user { get; set; } = null;
        public DateTime? al_udate { get; set; } = null;
        public string al_updateBy { get; set; } = null;
        public DateTime? al_updatedt { get; set; } = null;
    }
    public class AllowancesdepPayload
    {
        [Required(ErrorMessage = "Allowance ID Is Required !!")]
        public int allowanceID { get; set; }
        [Required(ErrorMessage = "Allowance Name Is Required !!")]
        public string allowanceName { get; set; }
        [Required(ErrorMessage = "Allowance Type Is Required !!")]
        public string allowanceType { get; set; }
        [Required(ErrorMessage = "Create User Name Is Required !!")]
        public string userName { get; set; }

        public static Allowancesde_DbModel PayloadToAllowanceDb_Obj(AllowancesdepPayload obj, Allowancesde_DbModel uObj=null)
        {
            var dbModel = new Allowancesde_DbModel
            {
                al_code = obj.allowanceID,
                al_groupid = 11,
                al_description = obj.allowanceName,
                al_type = obj.allowanceType,
                al_user = uObj == null ? obj.userName : uObj.al_user,
                al_udate = uObj == null ? DateTime.Now : uObj.al_udate,
                al_updateBy = uObj == null ? null : obj.userName,
                al_updatedt = uObj == null ? null : DateTime.Now,
            };
            return dbModel;
        }
    }
    public class DgPayAllowancesde
    {
        public int AlCode { get; set; }
        public string AlDescription { get; set; } = null;
        public string AlType { get; set; } = null;
        public string AlUser { get; set; } = null;
        public DateTime? AlUdate { get; set; } = null;
        public int? AlGroupid { get; set; } = null;

        public static Allowancesde_DbModel CustonToDbModel(DgPayAllowancesde obj)
        {
            try
            {
                var dbModel = new Allowancesde_DbModel
                {
                    al_code = obj.AlCode,
                    al_description = obj.AlDescription,
                    al_type = obj.AlType,
                    al_user = obj.AlUser,
                    al_udate = obj.AlUdate,
                    al_groupid = obj.AlGroupid
                };
                return dbModel;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return null;
        }
        public static DgPayAllowancesde DbToCustomModel(Allowancesde_DbModel obj)
        {
            try
            {
                var customModel = new DgPayAllowancesde
                {
                    AlCode = obj.al_code,
                    AlDescription = obj.al_description,
                    AlType = obj.al_type,
                    AlUser = obj.al_user,
                    AlUdate = obj.al_udate,
                    AlGroupid = obj.al_groupid
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