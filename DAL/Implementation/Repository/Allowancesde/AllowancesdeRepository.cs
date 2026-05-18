using BLL.Interfaces.Repository.Allowancesde;
using BOL.Models;
using DAL.Data;
using EF.Core.Repository.Repository;

namespace DAL.Implementation.Repository.Allowancesde
{
    public class AllowancesdeRepository : CommonRepository<Allowancesde_DbModel>, IAllowancesdeRepository
    {
        public AllowancesdeRepository(dg_hrpayrollContext context) : base(context)
        {
        }
    }
}
