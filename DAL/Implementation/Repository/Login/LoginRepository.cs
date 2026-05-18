using BLL.Interfaces.Repository.Login;
using BOL.Models;
using DAL.Data;
using EF.Core.Repository.Repository;

namespace DAL.Implementation.Repository.Login
{
    public class LoginRepository : CommonRepository<User_DbModel>,ILoginRepository
    {
        public LoginRepository(dg_hrpayrollContext context) : base(context)
        {           
        }
    }
}
