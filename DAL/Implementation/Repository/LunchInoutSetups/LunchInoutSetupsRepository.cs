using BLL.Interfaces.Repository.LunchInoutSetups;
using BOL.Models;
using DAL.Data;
using EF.Core.Repository.Repository;

namespace DAL.Implementation.Repository.LunchInoutSetups
{
    public class LunchInoutSetupsRepository : CommonRepository<LunchInoutSetup_DbModel>,ILunchInoutSetupsRepository
    {
        public LunchInoutSetupsRepository(dg_hrpayrollContext context) : base(context)
        {            
        }
    }
}
