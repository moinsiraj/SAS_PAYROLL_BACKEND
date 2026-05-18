using System.ComponentModel.DataAnnotations;

namespace BOL.Models
{
    public class CompanyAccess_DbModel
    {
        [Key]
        public int ca_serial { get; set; }
        public int? ca_compid { get; set; } = null;
        public string ca_compName { get; set; } = null;
        public int? permission { get; set; } = null;
        public string ca_accessuser { get; set; } = null;
        public string ca_user { get; set; } = null;
        public DateTime? ca_udate { get; set; } = null;
    }
    public class DgPayCompanyAccessPayload
    {
        public int caSerial { get; set; }
        public int companyID { get; set; }
        public int permission { get; set; }
        public string accessUser { get; set; }
        public string userName { get; set; }

        public static CompanyAccess_DbModel CompanyAccessDB(DgPayCompanyAccessPayload obj)
        {
            var result = new CompanyAccess_DbModel
            {
                ca_serial = obj.caSerial,
                ca_compid = obj.companyID,
                permission = obj.permission,
                ca_accessuser = obj.accessUser,
                ca_user = obj.userName,
                ca_udate = DateTime.Now,
            };
            return result;
        }
    }
    public class DgPayCompanyaccess
    {
        public int CaSerial { get; set; }
        public int? CaCompid { get; set; } = null;
        public string CaCompName { get; set; } = null;
        public int? Permission { get; set; } = null;
        public string CaAccessuser { get; set; } = null;
        public string CaUser { get; set; } = null;
        public DateTime? CaUdate { get; set; } = null;

        public static CompanyAccess_DbModel CustonToDbModel(DgPayCompanyaccess obj)
        {
            try
            {
                var dbModel = new CompanyAccess_DbModel
                {
                    ca_serial = obj.CaSerial,
                    ca_compid = obj.CaCompid,
                    ca_compName = obj.CaCompName,
                    permission = obj.Permission,
                    ca_accessuser = obj.CaAccessuser,
                    ca_user = obj.CaUser,
                    ca_udate = obj.CaUdate
                };
                return dbModel;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return null;
        }
        public static DgPayCompanyaccess DbToCustomModel(CompanyAccess_DbModel obj)
        {
            try
            {
                var customModel = new DgPayCompanyaccess
                {
                    CaSerial = obj.ca_serial,
                    CaCompid = obj.ca_compid,
                    CaCompName = obj.ca_compName,
                    Permission = obj.permission,
                    CaAccessuser = obj.ca_accessuser,
                    CaUser = obj.ca_user,
                    CaUdate = obj.ca_udate
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