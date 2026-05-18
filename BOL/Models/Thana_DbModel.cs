using System.ComponentModel.DataAnnotations;

namespace BOL.Models
{
    public class Thana_DbModel
    {
        [Key]
        public int th_id { get; set; }
        public int? th_groupid { get; set; } = null;
        public int? th_division { get; set; } = null;
        public string th_divname { get; set; } = null;
        public int? th_district { get; set; } = null;
        public string th_disname { get; set; } = null;
        public string th_name { get; set; } = null;
        public string th_nameBangla { get; set; } = null;
        public string th_user { get; set; } = null;
        public DateTime? th_udate { get; set; } = null;
        public string th_updateBy { get; set; } = null;
        public DateTime? th_upDate { get; set; } = null;
    }
    public class ThanaPayload
    {
        [Required(ErrorMessage = "Thana ID Is Required !!")]
        public int thanaID { get; set; }
        [Required(ErrorMessage = "Division Id Is Required !!")]
        public int divisionID { get; set; }
        [Required(ErrorMessage = "Division Id Is Required !!")]
        public int districtID { get; set; }
        [Required(ErrorMessage = "Thana Name Is Required !!")]
        public string thanaName { get; set; }
        [Required(ErrorMessage = "Thana Name Bangla Is Required !!")]
        public string thanaNameBangla { get; set; }
        [Required(ErrorMessage = "Create User Name Is Required !!")]
        public string userName { get; set; }

        public static Thana_DbModel PayloadToThanaDb_Obj(ThanaPayload obj, Thana_DbModel uObj=null)
        {
            var dbModel = new Thana_DbModel()
            {
                th_id = obj.thanaID,
                th_groupid = 11,
                th_division = obj.divisionID,
                th_district = obj.districtID,
                th_name = obj.thanaName,
                th_nameBangla = obj.thanaNameBangla,
                th_user = uObj == null ? obj.userName : uObj.th_user,
                th_udate = uObj == null ? DateTime.Now : uObj.th_udate,
                th_updateBy = uObj == null ? null : obj.userName,
                th_upDate = uObj == null ? null : DateTime.Now,
            };
            return dbModel;
        }
    }
    public class DgPayThana
    {
        public int ThId { get; set; }
        public int? ThDivision { get; set; } = null;
        public string ThDivname { get; set; } = null;
        public int? ThDistrict { get; set; } = null;
        public string ThDisname { get; set; } = null;
        public string ThName { get; set; } = null;
        public string ThUser { get; set; } = null;
        public DateTime? ThUdate { get; set; } = null;
        public int? ThGroupid { get; set; } = null;
        public string ThNameBangla { get; set; } = null;

        public static Thana_DbModel CustomToDbModel(DgPayThana obj)
        {
            try
            {
                var dbModel = new Thana_DbModel
                {
                    th_id = obj.ThId,
                    th_division = obj.ThDivision,
                    th_divname = obj.ThDivname,
                    th_district = obj.ThDistrict,
                    th_disname = obj.ThDisname,
                    th_name = obj.ThName,
                    th_user = obj.ThUser,
                    th_udate = obj.ThUdate,
                    th_groupid = obj.ThGroupid,
                    th_nameBangla = obj.ThNameBangla
                };
                return dbModel;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return null;
        }
        public static DgPayThana DbToCustomModel(Thana_DbModel obj)
        {
            try
            {
                var customModel = new DgPayThana
                {
                    ThId = obj.th_id,
                    ThDivision = obj.th_division,
                    ThDivname = obj.th_divname,
                    ThDistrict = obj.th_district,
                    ThDisname = obj.th_disname,
                    ThName = obj.th_name,
                    ThUser = obj.th_user,
                    ThUdate = obj.th_udate,
                    ThGroupid = obj.th_groupid,
                    ThNameBangla = obj.th_nameBangla
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