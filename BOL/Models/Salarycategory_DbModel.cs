using System.ComponentModel.DataAnnotations;

namespace BOL.Models
{
    public class Salarycategory_DbModel
    {
        [Key]
        public int cat_id { get; set; }
        public int? ct_groupid { get; set; } = null;
        public int? cat_compid { get; set; } = null;
        public string cat_compname { get; set; } = null;
        public string cat_name { get; set; } = null;      
        public int? cat_ot { get; set; } = null;
        public string cat_createBy { get; set; } = null;
        public DateTime? cat_createdt { get; set; } = null;
        public string cat_updateBy { get; set; } = null;
        public DateTime? cat_updatedt { get; set; } = null;

    }
    public class SalarycategoryPayload
    {
        [Required(ErrorMessage = "Create Salary Category ID Is Required !!")]
        public int salCatID { get; set; }
        [Required(ErrorMessage = "Create Company ID Is Required !!")]
        public int companyID { get; set; }
        [Required(ErrorMessage = "Create Company Name Is Required !!")]
        public string companyName { get; set; }
        [Required(ErrorMessage = "Create Salary Category Name Is Required !!")]
        public string salCatName { get; set; }
        [Required(ErrorMessage = "Create Salary Category OT Status Is Required !!")]
        public int salOTstatus { get; set; }
        [Required(ErrorMessage = "Create User Name Is Required !!")]
        public string userName { get; set; }

        public static Salarycategory_DbModel PayloadToSalaryCategoryDb_Obj(SalarycategoryPayload obj, Salarycategory_DbModel uObj=null)
        {
            var dbModel = new Salarycategory_DbModel
            {
                cat_id = obj.salCatID,
                ct_groupid = 11,
                cat_compid = obj.companyID,
                cat_compname = obj.companyName,
                cat_name = obj.salCatName,
                cat_ot = obj.salOTstatus,
                cat_createBy = uObj == null ? obj.userName : uObj.cat_createBy,
                cat_createdt = uObj == null ? DateTime.Now : uObj.cat_createdt,
                cat_updateBy = uObj == null ? null : obj.userName,
                cat_updatedt = uObj == null ? null : DateTime.Now,
            };
            return dbModel;
        }
    }
    public class DgPaySalarycategory
    {
        public int CatId { get; set; }
        public string CatName { get; set; } = null;
        public int? CatCompid { get; set; } = null;
        public string CatCompname { get; set; } = null;
        public int? CatOt { get; set; } = null;
        public int? CtGroupid { get; set; } = null;

        public static Salarycategory_DbModel CustomToDbModel(DgPaySalarycategory obj)
        {
            try
            {
                var dbModel = new Salarycategory_DbModel
                {
                    cat_id = obj.CatId,
                    cat_name = obj.CatName,
                    cat_compid = obj.CatCompid,
                    cat_compname = obj.CatCompname,
                    cat_ot = obj.CatOt,
                    ct_groupid = obj.CtGroupid
                };
                return dbModel;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return null;
        }
        public static DgPaySalarycategory DbToCustomModel(Salarycategory_DbModel obj)
        {
            try
            {
                var customModel = new DgPaySalarycategory
                {
                    CatId = obj.cat_id,
                    CatName = obj.cat_name,
                    CatCompid = obj.cat_compid,
                    CatCompname = obj.cat_compname,
                    CatOt = obj.cat_ot,
                    CtGroupid = obj.ct_groupid
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