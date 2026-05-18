using BOL.Models;
using EF.Core.Repository.Interface.Repository;

namespace BLL.Interfaces.Repository.Users
{
    public interface IUsersRepository : ICommonRepository<User_DbModel>
    {
    }
}
