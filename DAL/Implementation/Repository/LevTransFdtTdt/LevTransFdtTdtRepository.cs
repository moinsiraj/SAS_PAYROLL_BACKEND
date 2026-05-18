using BLL.Interfaces.Repository.LevTransFdtTdt;
using BOL.Models;
using DAL.Data;
using EF.Core.Repository.Repository;

namespace DAL.Implementation.Repository.LevTransFdtTdt
{
    public class LevTransFdtTdtRepository : CommonRepository<LevTransFdateTdate_DbModel>,ILevTransFdtTdtRepository
    {
        public LevTransFdtTdtRepository(dg_hrpayrollContext context) : base(context)
        {
        }
    }
}
