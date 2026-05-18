using BLL.Interfaces.Repository.Loans;
using BOL.Models;
using DAL.Data;
using EF.Core.Repository.Repository;

namespace DAL.Implementation.Repository.Loans
{
    public class LoansRepository : CommonRepository<Loan_DbModel>,ILoansRepository
    {
        public LoansRepository(dg_hrpayrollContext context) : base(context)
        {           
        }
    }
}
