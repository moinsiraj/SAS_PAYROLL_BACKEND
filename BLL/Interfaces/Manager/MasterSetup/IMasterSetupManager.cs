using BOL.Models;

namespace BLL.Interfaces.Manager.MasterSetup
{
    public interface IMasterSetupManager
    {
        Task<List<MasterMenuTree>> GetMasterMenuList();
        Task<List<MasterButtonTree>> GetMasterButtonList(string userName);
        Task<ReturnObject> SaveUserWiseButton(ButtonPermissionSavePayload obj);
        Task<List<TreeListReport>> GetTotalReportList();
        Task<ReturnObject> GetReportUserDropdown(int compid);
        Task<ReturnObject> GetPermissionReportByUser(int compid, string userName);
        Task<ReturnObject> Save_Pay_ReportPermission(ReportPermissionPayload obj);
        Task<ReturnObject> AddOrEditMenuGroup(MenuGroupPayload obj);
        Task<ReturnObject> GetAllMenuGroup();
        Task<ReturnObject> GetMenuGroupSelect(int groupId);
        Task<ReturnObject> AddOrEditNewUser(NewUserCreate obj);
        Task<ReturnObject> GetNewUserAll();
        Task<ReturnObject> GetUserByCompany(int compid);
        Task<ReturnObject> GetMenuUserSelect(string userName);
    }
}
