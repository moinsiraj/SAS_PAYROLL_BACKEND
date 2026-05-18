using BOL.Models;
using EF.Core.Repository.Interface.Repository;

namespace BLL.Interfaces.Repository.LoanCategories
{
    public interface ILoanCategoriesRepository : ICommonRepository<LoanCategory_DbModel>
    {
    }
}
