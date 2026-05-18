using BLL.Interfaces.Repository.Activedate;
using BOL.Models;
using DAL.Data;
using EF.Core.Repository.Repository;

namespace DAL.Implementation.Repository.Activedate
{
    public class ActiveDateRepository : CommonRepository<Activedate_DbModel>, IActiveDateRepository
    {
        public ActiveDateRepository(dg_hrpayrollContext context) : base(context)
        {

        }
    }
}
