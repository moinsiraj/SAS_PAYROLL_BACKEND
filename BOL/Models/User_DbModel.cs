using System.ComponentModel.DataAnnotations;

namespace BOL.Models
{
    public class User_DbModel
    {
        [Key]
        public int ID { get; set; }
        public string FullName { get; set; } = null;
        public string UserFullname { get; set; } = null;
        public string EmailId { get; set; } = null;
        public string Password { get; set; } = null;
        public string Designation { get; set; } = null;
        public DateTime? CreatedDate { get; set; } = null;
        public int? CompId { get;set; } = null;
        public string Emp_ID { get; set; } = null;
        public string Active_status { get; set; } = null;
        public int? Emp_serial { get; set; } = null;
        public int? Compliance { get; set; } = null;
    }
    public class TblUser
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null;
        public string UserFullName { get; set; } = null;
        public string EmailId { get; set; } = null;
        public string Password { get; set; } = null;
        public string Designation { get; set; } = null;
        public DateTime? CreatedDate { get; set; } = null;
        public int? CompId { get; set; } = null;
        public string EmpId { get; set; } = null;
        public string ActiveStatus { get; set; } = null;
        public int? EmpSerial { get; set; } = null;
        public int? Compliance { get; set; } = null;

        public static User_DbModel CustomToDbModel(TblUser obj)
        {
            try
            {
                var dbModel = new User_DbModel
                {
                    ID = obj.Id,
                    FullName = obj.FullName,
                    UserFullname = obj.UserFullName,
                    EmailId = obj.EmailId,
                    Password = obj.Password,
                    Designation = obj.Designation,
                    CreatedDate = obj.CreatedDate,
                    CompId = obj.CompId,
                    Emp_ID = obj.EmpId,
                    Active_status = obj.ActiveStatus,
                    Emp_serial = obj.EmpSerial,
                    Compliance = obj.Compliance
                };
                return dbModel;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return null;
        }
        public static TblUser DbToCustomModel(User_DbModel obj)
        {
            try
            {
                var customModel = new TblUser
                {
                    Id = obj.ID,
                    FullName = obj.FullName,
                    UserFullName = obj.UserFullname,
                    EmailId = obj.EmailId,
                    Password = obj.Password,
                    Designation = obj.Designation,
                    CreatedDate = obj.CreatedDate,
                    CompId = obj.CompId,
                    EmpId = obj.Emp_ID,
                    ActiveStatus = obj.Active_status,
                    EmpSerial = obj.Emp_serial,
                    Compliance = obj.Compliance
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
    public class UserModel
    {
        public int ID { get; set; }
        public int? CompID { get; set; } = null;
        public string compName { get; set; } = null;
        public string FullName { get; set; } = null;
        public string UserFullName { get; set; } = null;
        public string EmailId { get; set; } = null;
        public string Password { get; set; } = null;
        public string Designation { get; set; } = null;
        public string UserMessage { get; set; } = null;
        public string AccessToken { get; set; } = null;
        public DateTime? CreatedDate { get; set; } = null;
    }

    public class MenuList
    {
        public string menuText { get; set; }
        public string userName { get; set; }
    }
    public class MenuList2
    {
        public string mainMenuName { get; set; }
        public string menuText { get; set; }
        public string menuUrl { get; set; }
        public string separatorName { get; set; }
        public string userName { get; set; }
    }
    public class ButtonList
    {
        public string buttonName { get; set; }
        public string pageName { get; set; }
        public bool isShow { get; set; }
        public string userName { get; set; }
    }
    public class MasterMainButtonMenu
    {
        public string menuName { get; set; }
        public string url { get; set; }
        public string menuDesc { get; set; }
        public List<MasterButton> masterButtons { get; set; }
    }
    public class MasterButtonTree
    {
        public int id { get; set; }
        public string parentMenu { get; set; }
        public List<MasterMainButtonMenu> childMenu { get; set; }
    }
    public class MasterButton
    {
        public string buttonID { get; set; }
        public string buttonText { get; set; }
        public bool isPermission { get; set; }
    }
    public class ButtonPermissionSavePayload
    {
        public string userName { get; set; }
        public List<ButtonPermissionSave> saveChild { get; set; }
    }
    public class ButtonPermissionSave
    {
        public string parentMenu { get; set; }
        public string url { get; set; }
        public string buttonID { get; set; }
    }
    
    
    public class MasterMenuTree
    {
        public int id { get; set; }
        public string moduleName { get; set; }
        public List<MenuModuleChild> mainMenu { get; set; }
    }
    public class MenuModuleChild
    {
        public string mainMenuName { get; set; }
        public List<MainMenuChild> subMenu { get; set; }
    }
    public class MainMenuChild
    {
        public string subMenuName { get; set; }
        public string url { get; set; }
    }
    public class MenuGroupPayload
    {
        public int mGroupId { get; set; }
        public string mGroupName { get; set; }
        public string userName { get; set; }
        public List<MainMenuChild> mainMenus { get; set; }
    }
    public class NewUserCreate
    {
        public int userSerial { get; set; }
        public int nCompanyID { get; set; }
        public int nUserDept { get; set; }
        public int nSectionID { get; set; }
        public string cUserName { get; set; }
        public string cPassWord { get; set; }
        public string cUserFullname { get; set; }
        public string Permission_status { get; set; }
        public int nULevelID { get; set; }
        public int nUgroup { get; set; }
        public string Email { get; set; }
        public string Activity_status { get; set; }
        public List<MainMenuChild> mainMenus { get; set; }

        public static SinglelNewUserDbPayload GetUserDbPayload(NewUserCreate obj, string enctPass)
        {
            var result = new SinglelNewUserDbPayload
            {
                userSerial = obj.userSerial,
                nCompanyID = obj.nCompanyID,
                nUserDept = obj.nUserDept,
                nSectionID = obj.nSectionID,
                cUserName = obj.cUserName,
                cPassWord = obj.cPassWord,
                cPassWord_2 = enctPass,
                cUserFullname = obj.cUserFullname,
                Permission_status = obj.Permission_status,
                nULevelID = obj.nULevelID,
                nUgroup = obj.nUgroup == 0 ? 99 : obj.nUgroup,
                Email = obj.Email,
                Activity_status = obj.Activity_status
            };
            return result;
        }
    }
    public class SinglelNewUserDbPayload
    {
        public int userSerial { get; set; }
        public int nCompanyID { get; set; }
        public int nUserDept { get; set; }
        public int nSectionID { get; set; }
        public string cUserName { get; set; }
        public string cPassWord { get; set; }
        public string cPassWord_2 { get; set; }
        public string cUserFullname { get; set; }
        public string Permission_status { get; set; }
        public int nULevelID { get; set; }
        public int nUgroup { get; set; }
        public string Email { get; set; }
        public string Activity_status { get; set; }       
    }
    public class UserPasswordResetPayload
    {
        public string userId { get; set; }
    }
    public class UserEmailSetPayload
    {
        public string userId { get; set; }
        public string emailId { get; set; }
    }
}