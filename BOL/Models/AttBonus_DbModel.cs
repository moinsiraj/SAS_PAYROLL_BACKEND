using System.ComponentModel.DataAnnotations;

namespace BOL.Models
{
    public class AttBonus_DbModel
    {
        [Key]
        public int attb_serial { get; set; }
        public int? attb_designation { get; set; } = null;
        public string attb_designationName { get; set; } = null;
        public decimal? attb_value { get; set; } = null;
        public int? attb_compid { get; set; } = null;
        public string attb_compidName { get; set; } = null;
        public int? attb_groupid { get; set; } = null;
        public string attb_user { get; set; } = null;
        public DateTime? attb_udate { get; set; } = null;
    }
    public class DgPayAttbonu
    {
        public int AttbSerial { get; set; }
        public int? AttbDesignation { get; set; } = null;
        public string AttbDesignationName { get; set; } = null;
        public decimal? AttbValue { get; set; } = null;
        public int? AttbCompid { get; set; } = null;
        public string AttbCompidName { get; set; } = null;
        public int? AttbGroupid { get; set; } = null;
        public string AttbUser { get; set; } = null;
        public DateTime? AttbUdate { get; set; } = null;

        public static AttBonus_DbModel CustonToDbModel(DgPayAttbonu obj)
        {
            try
            {
                var dbModel = new AttBonus_DbModel
                {
                    attb_serial = obj.AttbSerial,
                    attb_designation = obj.AttbDesignation,
                    attb_designationName = obj.AttbDesignationName,
                    attb_value = obj.AttbValue,
                    attb_compid = obj.AttbCompid,
                    attb_compidName = obj.AttbCompidName,
                    attb_groupid = obj.AttbGroupid,
                    attb_user = obj.AttbUser,
                    attb_udate = obj.AttbUdate
                };
                return dbModel;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return null;
        }
        public static DgPayAttbonu DbToCustomModel(AttBonus_DbModel obj)
        {
            try
            {
                var customModel = new DgPayAttbonu
                {
                    AttbSerial = obj.attb_serial,
                    AttbDesignation = obj.attb_designation,
                    AttbDesignationName = obj.attb_designationName,
                    AttbValue = obj.attb_value,
                    AttbCompid = obj.attb_compid,
                    AttbCompidName = obj.attb_compidName,
                    AttbGroupid = obj.attb_groupid,
                    AttbUser = obj.attb_user,
                    AttbUdate = obj.attb_udate
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
