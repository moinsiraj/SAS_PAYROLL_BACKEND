using BLL.Interfaces.Repository.AllProcess;
using BOL.Models;
using DAL.Data;
using EF.Core.Repository.Repository;

namespace DAL.Implementation.Repository.AllProcess
{
    public class AllProcessRepository : CommonRepository<AllProcess_DbModel>, IAllProcessRepository
    {
        public AllProcessRepository(dg_hrpayrollContext context) : base(context)
        {

        }
    }
}
