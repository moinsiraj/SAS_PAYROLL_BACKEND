using BLL.Interfaces.Repository.Allowance;
using BOL.Models;
using DAL.Data;
using EF.Core.Repository.Repository;

namespace DAL.Implementation.Repository.Allowance
{
    public class AllowanceRepository : CommonRepository<Allowance_DbModel>, IAllowanceRepository
    {
        public AllowanceRepository(dg_hrpayrollContext context) : base(context)
        {

        }
    }
}
