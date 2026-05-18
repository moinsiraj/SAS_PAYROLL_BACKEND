using BLL.Interfaces.Repository.IncrementInfoes;
using BOL.Models;
using DAL.Data;
using EF.Core.Repository.Repository;

namespace DAL.Implementation.Repository.IncrementInfoes
{
    public class IncrementInfoesRepository : CommonRepository<IncrementInfo_DbModel>, IIncrementInfoesRepository
    {
        public IncrementInfoesRepository(dg_hrpayrollContext context) : base(context)
        {
            
        }
    }
}
