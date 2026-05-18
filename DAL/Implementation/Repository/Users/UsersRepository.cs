using BLL.Interfaces.Repository.Users;
using BOL.Models;
using DAL.Data;
using EF.Core.Repository.Repository;

namespace DAL.Implementation.Repository.Users
{
    public class UsersRepository : CommonRepository<User_DbModel>,IUsersRepository
    {
        public UsersRepository(dg_hrpayrollContext context):base(context)
        {           
        }
    }
}
