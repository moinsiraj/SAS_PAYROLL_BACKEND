using System.ComponentModel.DataAnnotations;

namespace BOL.Models
{
    public class Designation_DbModel
    {
        [Key]
        public int dec_id { get; set; }
        public int? dec_groupid { get; set; } = null;
        public string dec_name { get; set; } = null;
        public string dec_name_bangla { get; set; } = null;
        public decimal? hd_allownAmt { get; set; } = null;
        public int? dec_cat_id { get; set; } = 0;
        public string dec_user { get; set; } = null;
        public DateTime? dec_udate { get; set; } = null;
        public string dec_updateBy { get; set; } = null;
        public DateTime? dec_update { get; set; } = null;
    }
    public class DesignationCat_DbModel
    {
        [Key]
        public int desig_cat_id { get; set; }
        public string desig_cat_name { get; set; } = null;
    }
    public class DesignationJoinList : DesignationCat_DbModel
    {
        public int dec_id { get; set; }
        public int? dec_groupid { get; set; } = null;
        public string dec_name { get; set; } = null;
        public string dec_name_bangla { get; set; } = null;
        public decimal? hd_allownAmt { get; set; } = null;
        public string dec_user { get; set; } = null;
        public DateTime? dec_udate { get; set; } = null;
        public string dec_updateBy { get; set; } = null;
        public DateTime? dec_update { get; set; } = null;
    }
    public class DesignationPayload
    {
        [Required(ErrorMessage = "Designation ID Is Required !!")]
        public int designationID { get; set; }
        [Required(ErrorMessage = "Designation Name Is Required !!")]
        public string designationName { get; set; }
        [Required(ErrorMessage = "Designation Name Bangla Is Required !!")]
        public string designationNameBN { get; set; }
        [Required(ErrorMessage = "Create User Name Is Required !!")]
        public int descCatid { get; set; }
        public string userName { get; set; }

        public static Designation_DbModel PayloadToDesignationDb_Obj(DesignationPayload obj, Designation_DbModel uObj=null)
        {
            var dbModel = new Designation_DbModel
            {
                dec_id = obj.designationID,
                dec_name = obj.designationName,
                dec_name_bangla = obj.designationNameBN,
                dec_cat_id = obj.descCatid,
                hd_allownAmt = 0,
                dec_user = uObj == null ? obj.userName : uObj.dec_user,
                dec_udate = uObj == null ? DateTime.Now : uObj.dec_udate,
                dec_updateBy = uObj == null ? null : obj.userName,
                dec_update = uObj == null ? null : DateTime.Now,
            };
            return dbModel;
        }
    }
    public class DgPayDesignation
    {
        public int DecId { get; set; }
        public string DecName { get; set; } = null;
        public string DecNameBangla { get; set; } = null;
        public string DecUser { get; set; } = null;
        public DateTime? DecUdate { get; set; } = null;
        public int? DecGroupid { get; set; } = null;
        public decimal? HdAllownAmt { get; set; } = null;

        public static Designation_DbModel CustonToDbModel(DgPayDesignation obj)
        {
            try
            {
                var dbModel = new Designation_DbModel
                {
                    dec_id = obj.DecId,
                    dec_name = obj.DecName,
                    dec_name_bangla = obj.DecNameBangla,
                    dec_user = obj.DecUser,
                    dec_udate = obj.DecUdate,
                    dec_groupid = obj.DecGroupid,
                    hd_allownAmt = obj.HdAllownAmt
                };
                return dbModel;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return null;
        }
        public static DgPayDesignation DbToCustomModel(Designation_DbModel obj)
        {
            try
            {
                var customModel = new DgPayDesignation
                {
                    DecId = obj.dec_id,
                    DecName = obj.dec_name,
                    DecNameBangla = obj.dec_name_bangla,
                    DecUser = obj.dec_user,
                    DecUdate = obj.dec_udate,
                    DecGroupid = obj.dec_groupid,
                    HdAllownAmt = obj.hd_allownAmt
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
    public class DesignationBgtPayload
    {
        public int companyId { get; set; }
        public string companyName { get; set; }
        public string userName { get; set; }
        public List<DesignationBgtInfo> designationBgtInfos { get; set; }
    }
    public class DesignationBgtInfo
    {
        public int designationId { get; set;}
        public string designationName { get; set; }
        public int bgtQty { get; set;}
    }
    public class DesignationBgt_Payload
    {
        public int bgt_id { get; set; }
        public int companyId { get; set;}
        public int departmentId { get; set; }
        public int sectionId { get; set; }
        public int buildingId { get; set; }
        public int floorId { get; set; }
        public int lineId { get; set; }
        public int designationId { get; set; }
        public int manpower { get; set;}
        public decimal bgtAmount { get; set; }
        public string userName { get; set; }
    }
}
