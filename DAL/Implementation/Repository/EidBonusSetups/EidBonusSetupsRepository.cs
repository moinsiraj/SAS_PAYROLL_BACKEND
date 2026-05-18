using BLL.Interfaces.Repository.EidBonusSetups;
using BOL.Models;
using DAL.Data;
using EF.Core.Repository.Repository;

namespace DAL.Implementation.Repository.EidBonusSetups
{
    public class EidBonusSetupsRepository : CommonRepository<EidBonusSetup_DbModel>, IEidBonusSetupsRepository
    {
        public EidBonusSetupsRepository(dg_hrpayrollContext context) : base(context)
        {           
        }
    }
}
