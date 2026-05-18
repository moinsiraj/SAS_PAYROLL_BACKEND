using BLL.Interfaces.Repository.Menurights;
using BOL.Models;
using DAL.Data;
using EF.Core.Repository.Repository;

namespace DAL.Implementation.Repository.Menurights
{
    public class MenurightsRepository : CommonRepository<Menuright_DbModel>,IMenurightsRepository
    {
        public MenurightsRepository(dg_hrpayrollContext context) : base(context)
        {
            
        }
    }
}
