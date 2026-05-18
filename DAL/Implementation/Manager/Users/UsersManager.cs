using BLL.Interfaces.Manager.Users;
using BOL.Models;
using DAL.Data;
using DAL.Implementation.Repository.Users;
using EF.Core.Repository.Manager;

namespace DAL.Implementation.Manager.Users
{
    public class UsersManager : CommonManager<User_DbModel>,IUsersManager
    {
        public UsersManager(dg_hrpayrollContext context):base(new UsersRepository(context))
        {           
        }        
    }
}
