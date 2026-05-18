using BLL.Interfaces.Repository.Leavetransactions;
using BOL.Models;
using DAL.Data;
using EF.Core.Repository.Repository;

namespace DAL.Implementation.Repository.Leavetransactions
{
    public class LeavetransactionsRepository : CommonRepository<Leavetransaction_DbModel>,ILeavetransactionsRepository
    {
        public LeavetransactionsRepository(dg_hrpayrollContext context) : base(context)
        {            
        }
    }
}
