using System.ComponentModel.DataAnnotations;

namespace BOL.Models
{
    public class Menuright_DbModel
    {
        [Key]
        public int m_sl { get; set; }
        public int m_titlecode { get; set; }
        public string m_title { get; set; }
        public int? Emp_serial { get; set; } = null;
        public string m_user { get; set; }
        public int m_rights { get; set; }
        public DateTime m_udate { get; set; }
        public string m_uadmin { get; set; }
    }
    public class DgMenuright
    {
        public int MSl { get; set; }
        public int MTitlecode { get; set; }
        public string MTitle { get; set; }
        public int? EmpSerial { get; set; } = null;
        public string MUser { get; set; }
        public int MRights { get; set; }
        public DateTime MUdate { get; set; }
        public string MUadmin { get; set; }

        public static Menuright_DbModel CustomToDbModel(DgMenuright obj)
        {
            try
            {
                var dbModel = new Menuright_DbModel
                {
                    m_sl = obj.MSl,
                    m_titlecode = obj.MTitlecode,
                    m_title = obj.MTitle,
                    Emp_serial = obj.EmpSerial,
                    m_user = obj.MUser,
                    m_rights = obj.MRights,
                    m_udate = obj.MUdate,
                    m_uadmin = obj.MUadmin
                };
                return dbModel;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return null;
        }
        public static DgMenuright DbToCustomModel(Menuright_DbModel obj)
        {
            try
            {
                var customModel = new DgMenuright
                {
                    MSl = obj.m_sl,
                    MTitlecode = obj.m_titlecode,
                    MTitle = obj.m_title,
                    EmpSerial = obj.Emp_serial,
                    MUser = obj.m_user,
                    MRights = obj.m_rights,
                    MUdate = obj.m_udate,
                    MUadmin = obj.m_uadmin
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
