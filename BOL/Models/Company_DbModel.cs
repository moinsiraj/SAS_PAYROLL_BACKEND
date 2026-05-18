using System.ComponentModel.DataAnnotations;

namespace BOL.Models
{
    public class Company_DbModel
    {
        [Key]
        public int com_id { get; set; }
        public string com_name { get; set; } = null;
        public string com_ad1 { get; set; } = null;
        public string com_ad2 { get; set; } = null;
        public string com_tel { get; set; } = null;
        public string com_email { get; set; } = null;
        public string com_web { get; set; } = null;
        public int? com_gid { get; set; } = null;
        public string com_user { get; set; } = null;
        public DateTime? com_udate { get; set; } = null;
        public string com_nameBangla { get; set; } = null;
        public string com_ad1Bangla { get; set; } = null;
        public string com_ad2Bangla { get; set; } = null;
    }
    public class DgPayCompany
    {
        public int ComId { get; set; }
        public string ComName { get; set; } = null;
        public string ComAd1 { get; set; } = null;
        public string ComAd2 { get; set; } = null;
        public string ComTel { get; set; } = null;
        public string ComEmail { get; set; } = null;
        public string ComWeb { get; set; } = null;
        public int? ComGid { get; set; } = null;
        public string ComUser { get; set; } = null;
        public DateTime? ComUdate { get; set; } = null;
        public string ComNameBangla { get; set; } = null;
        public string ComAd1Bangla { get; set; } = null;
        public string ComAd2Bangla { get; set; } = null;

        public static Company_DbModel CustonToDbModel(DgPayCompany obj)
        {
            try
            {
                var dbModel = new Company_DbModel
                {
                    com_id = obj.ComId,
                    com_name = obj.ComName,
                    com_ad1 = obj.ComAd1,
                    com_ad2 = obj.ComAd2,
                    com_tel = obj.ComTel,
                    com_email = obj.ComEmail,
                    com_web = obj.ComWeb,
                    com_gid = obj.ComGid,
                    com_user = obj.ComUser,
                    com_udate = obj.ComUdate,
                    com_nameBangla = obj.ComNameBangla,
                    com_ad1Bangla = obj.ComAd1Bangla,
                    com_ad2Bangla = obj.ComAd2Bangla
                };
                return dbModel;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return null;
        }
        public static DgPayCompany DbToCustomModel(Company_DbModel obj)
        {
            try
            {
                var customModel = new DgPayCompany
                {
                    ComId = obj.com_id,
                    ComName = obj.com_name,
                    ComAd1 = obj.com_ad1,
                    ComAd2 = obj.com_ad2,
                    ComTel = obj.com_tel,
                    ComEmail = obj.com_email,
                    ComWeb = obj.com_web,
                    ComGid = obj.com_gid,
                    ComUser = obj.com_user,
                    ComUdate = obj.com_udate,
                    ComNameBangla = obj.com_nameBangla,
                    ComAd1Bangla = obj.com_ad1Bangla,
                    ComAd2Bangla = obj.com_ad2Bangla
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