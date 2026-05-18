using BOL.Models;
using EF.Core.Repository.Interface.Manager;
using System.Data;

namespace BLL.Interfaces.Manager.ButtonPermission
{
    public interface IButtonPermissionManager : ICommonManager<ButtonPermission_DbModel>
    {
        Task<DataSet> GetUserWiseButtonList(string b_user);
    }
}
