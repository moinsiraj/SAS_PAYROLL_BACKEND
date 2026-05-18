using BOL.Models;
using EF.Core.Repository.Interface.Manager;

namespace BLL.Interfaces.Manager.Login
{
    public interface ILoginManager : ICommonManager<User_DbModel>
    {
        Task<User_DbModel> GetMasterUserTableInfo(string userID, string password);
        Task<bool> UpdateUserPassBatch();
        Task<string> SaveUserPasswordChange(int compID,string loginID,string confPassword, string newPassword);
        Task<ReturnObject> SendPasswordResetCode(UserPasswordResetPayload obj);
        Task<ReturnObject> CheckResetCodeValidity(string userId, string vCode);
        Task<object> CheckUserEmailSet(UserPasswordResetPayload obj);
        Task<ReturnObject> SetupUserEmailId(UserEmailSetPayload obj);
        Task<List<object>> GetPermitedMenuList2(List<MenuList2> obj);
        Task<List<object>> GetPermitedMenuList(List<MenuList> obj);
        Task<List<object>> SetBtnPermission(List<ButtonList> obj);
    }
}
