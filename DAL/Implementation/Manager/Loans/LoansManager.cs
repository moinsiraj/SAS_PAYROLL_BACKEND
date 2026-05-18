using BLL.Interfaces.Manager.Loans;
using BOL.Models;
using DAL.Data;
using DAL.Implementation.Repository.Loans;
using EF.Core.Repository.Manager;

namespace DAL.Implementation.Manager.Loans
{
    public class LoansManager : CommonManager<Loan_DbModel>,ILoansManager
    {
        public LoansManager(dg_hrpayrollContext context) : base(new LoansRepository(context))
        {           
        }        
    }
}
