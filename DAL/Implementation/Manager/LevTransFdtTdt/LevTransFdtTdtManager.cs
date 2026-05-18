using BLL.Interfaces.Manager.LevTransFdtTdt;
using BOL.Models;
using DAL.Data;
using DAL.Implementation.Repository.LevTransFdtTdt;
using EF.Core.Repository.Manager;

namespace DAL.Implementation.Manager.LevTransFdtTdt
{
    public class LevTransFdtTdtManager : CommonManager<LevTransFdateTdate_DbModel>,ILevTransFdtTdtManager
    {
        public LevTransFdtTdtManager(dg_hrpayrollContext context) : base(new LevTransFdtTdtRepository(context))
        {            
        }
    }
}
