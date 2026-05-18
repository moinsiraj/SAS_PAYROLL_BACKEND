using System.ComponentModel.DataAnnotations;

namespace BOL.Models
{
    public class Division_DbModel
    {
        [Key]
        public int div_id { get; set; }
        public string div_name { get; set; } = null;
        public string div_name_bangla { get; set; } = null;
        public int? div_groupid { get; set; } = null;
        public string div_user { get; set; } = null;
        public DateTime? div_udate { get; set; } = null;
        public string div_updateBy { get; set; } = null;
        public DateTime? div_update { get; set; } = null;
    }
    public class DivisionPayload
    {
        [Required(ErrorMessage = "Division ID Is Required !!")]
        public int divID { get; set; }
        [Required(ErrorMessage = "Division Name Is Required !!")]
        public string divName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Division Name Bangla Is Required !!")]
        public string divNameBangla { get; set; } = string.Empty;
        [Required(ErrorMessage = "Create User Name Is Required !!")]
        public string userName { get; set; } = string.Empty;
        public static Division_DbModel PayloadToDivisionDb_Obj(DivisionPayload obj, Division_DbModel uObj=null)
        {
            var dbObject = new Division_DbModel
            {
                div_id = obj.divID,
                div_name = obj.divName,
                div_name_bangla = obj.divNameBangla,
                div_groupid = 11,
                div_user = uObj == null ? obj.userName : uObj.div_user,
                div_udate = uObj == null ? DateTime.Now : uObj.div_udate,
                div_updateBy = uObj == null ? null : obj.userName,
                div_update = uObj == null ? null : DateTime.Now,
            };
            return dbObject;
        }
    }
    public class DgPayDivision
    {
        public int DivId { get; set; }
        public string DivName { get; set; } = null;
        public string DivNameBangla { get; set; } = null;
        public int? DivGroupid { get; set; } = null;
        public string DivUser { get; set; } = null;
        public DateTime? DivUdate { get; set; } = null;
        public string DivUpdateBy { get; set; } = null;
        public DateTime? DivUpdate { get; set; } = null;

        public static Division_DbModel CustonToDbModel(DgPayDivision obj)
        {
            try
            {
                var dbModel = new Division_DbModel
                {
                    div_id = obj.DivId,
                    div_name = obj.DivName,
                    div_name_bangla = obj.DivNameBangla,
                    div_groupid = obj.DivGroupid,
                    div_user = obj.DivUser,
                    div_udate = obj.DivUdate,
                    div_update = obj.DivUpdate,
                    div_updateBy = obj.DivUpdateBy,
                };
                return dbModel;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return null;
        }
        public static DgPayDivision DbToCustomModel(Division_DbModel obj)
        {
            try
            {
                var customModel = new DgPayDivision
                {
                    DivId = obj.div_id,
                    DivName = obj.div_name,
                    DivNameBangla = obj.div_name_bangla,
                    DivGroupid = obj.div_groupid,
                    DivUser = obj.div_user,
                    DivUdate = obj.div_udate,
                    DivUpdate = obj.div_update,
                    DivUpdateBy = obj.div_updateBy,
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