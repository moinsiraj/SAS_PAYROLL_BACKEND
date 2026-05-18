using System.ComponentModel.DataAnnotations;

namespace BOL.Models
{
    public class ButtonPermission_DbModel
    {
        [Key]
        public int b_sl { get; set; }
        public int b_titlecode { get; set; }
        public string b_title { get; set; }
        public int? Emp_serial { get; set; } = null;
        public string b_user { get; set; }
        public int b_rights { get; set; }
        public DateTime b_udate { get; set; }
        public string b_uadmin { get; set; }
    }

    public class DgButtonPermission
    {
        public int BSl { get; set; }
        public int BTitlecode { get; set; }
        public string BTitle { get; set; }
        public int? EmpSerial { get; set; } = null;
        public string BUser { get; set; }
        public int BRights { get; set; }
        public DateTime BUdate { get; set; }
        public string BUadmin { get; set; }

        public static ButtonPermission_DbModel CustonToDbModel(DgButtonPermission obj)
        {
            try
            {
                var dbModel = new ButtonPermission_DbModel
                {
                    b_sl = obj.BSl,
                    b_titlecode = obj.BTitlecode,
                    b_title = obj.BTitle,
                    Emp_serial = obj.EmpSerial,
                    b_user = obj.BUser,
                    b_rights = obj.BRights,
                    b_udate = obj.BUdate,
                    b_uadmin = obj.BUadmin
                };
                return dbModel;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return null;
        }
        public static DgButtonPermission DbToCustomModel(ButtonPermission_DbModel obj)
        {
            try
            {
                var customModel = new DgButtonPermission
                {
                    BSl = obj.b_sl,
                    BTitlecode = obj.b_titlecode,
                    BTitle = obj.b_title,
                    EmpSerial = obj.Emp_serial,
                    BUser = obj.b_user,
                    BRights = obj.b_rights,
                    BUdate = obj.b_udate,
                    BUadmin = obj.b_uadmin
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
