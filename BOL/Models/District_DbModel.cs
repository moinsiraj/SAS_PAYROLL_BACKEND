using System.ComponentModel.DataAnnotations;

namespace BOL.Models
{
    public class District_DbModel
    {
        [Key]
        public int di_id { get; set; }
        public int? di_groupid { get; set; } = null;
        public int? di_divid { get; set; } = null;
        public string di_divname { get; set; } = null;
        public string di_name { get; set; } = null;
        public string di_nameBangla { get; set; } = null;
        public string di_user { get; set; } = null;
        public DateTime? di_udate { get; set; } = null;
        public string di_updateBy { get; set; } = null;
        public DateTime? di_updateDate { get; set; } = null;
    }
    public class DgPayDistrict
    {
        public int DiId { get; set; }
        public int? DiDivid { get; set; } = null;
        public string DiDivname { get; set; } = null;
        public string DiName { get; set; } = null;
        public int? DiGroupid { get; set; } = null;
        public string DiUser { get; set; } = null;
        public DateTime? DiUdate { get; set; } = null;
        public string DiNameBangla { get; set; } = null;

        public static District_DbModel CustonToDbModel(DgPayDistrict obj)
        {
            try
            {
                var dbModel = new District_DbModel
                {
                    di_id = obj.DiId,
                    di_divid = obj.DiDivid,
                    di_divname = obj.DiDivname,
                    di_name = obj.DiName,
                    di_groupid = obj.DiGroupid,
                    di_user = obj.DiUser,
                    di_udate = obj.DiUdate,
                    di_nameBangla = obj.DiNameBangla
                };
                return dbModel;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return null;
        }
        public static DgPayDistrict DbToCustomModel(District_DbModel obj)
        {
            try
            {
                var customModel = new DgPayDistrict
                {
                    DiId = obj.di_id,
                    DiDivid = obj.di_divid,
                    DiDivname = obj.di_divname,
                    DiName = obj.di_name,
                    DiGroupid = obj.di_groupid,
                    DiUser = obj.di_user,
                    DiUdate = obj.di_udate,
                    DiNameBangla = obj.di_nameBangla
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

    //New 1/30/2024 CreateBy Forhad
    public class DistrictPayload
    {
        [Required(ErrorMessage = "District Name Is Required !!")]
        public int dis_id { get; set; }
        [Required(ErrorMessage = "Division Name Is Required !!")]
        public int div_id { get; set; }
        [Required(ErrorMessage = "District Name Is Required !!")]
        public string dis_name { get; set;}
        [Required(ErrorMessage = "District Name Bangla Is Required !!")]
        public string dis_nameBN { get; set;}
        [Required(ErrorMessage = "Create User Name Is Required !!")]
        public string userName { get; set; }

        public static District_DbModel PayloadToDistrictDb_Obj(DistrictPayload obj, District_DbModel uObj=null)
        {
            var dbModel = new District_DbModel
            {
                di_id = obj.dis_id,
                di_groupid = 11,
                di_divid = obj.div_id,
                di_name = obj.dis_name,
                di_nameBangla = obj.dis_nameBN,
                di_user = uObj == null ? obj.userName : uObj.di_user,
                di_udate = uObj == null ? DateTime.Now : uObj.di_udate,
                di_updateBy = uObj == null ? null : obj.userName,
                di_updateDate = uObj == null ? null : DateTime.Now,
            };
            return dbModel;
        }
    }

}