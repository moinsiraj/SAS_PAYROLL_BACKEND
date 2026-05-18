using BOL.Models;
using EF.Core.Repository.Interface.Repository;

namespace BLL.Interfaces.Repository.Login
{
    public interface ILoginRepository : ICommonRepository<User_DbModel>
    {
    }
}
