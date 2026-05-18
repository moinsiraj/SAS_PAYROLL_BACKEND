using BLL.Interfaces.Repository.Deduction;
using BOL.Models;
using DAL.Data;
using EF.Core.Repository.Repository;

namespace DAL.Implementation.Repository.Deduction
{
    public class DeductionRepository : CommonRepository<Deduction_DbModel>,IDeductionRepository
    {
        public DeductionRepository(dg_hrpayrollContext context) : base(context)
        {
        }
    }
}
