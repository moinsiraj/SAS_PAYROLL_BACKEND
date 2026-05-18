using BOL.Models;
using EF.Core.Repository.Interface.Repository;

namespace BLL.Interfaces.Repository.Loans
{
    public interface ILoansRepository : ICommonRepository<Loan_DbModel>
    {
    }
}
