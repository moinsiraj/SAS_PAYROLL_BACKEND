using BLL.Interfaces.Repository.LoanCategories;
using BOL.Models;
using DAL.Data;
using EF.Core.Repository.Repository;

namespace DAL.Implementation.Repository.LoanCategories
{
    public class LoanCategoriesRepository : CommonRepository<LoanCategory_DbModel>,ILoanCategoriesRepository
    {
        public LoanCategoriesRepository(dg_hrpayrollContext context) : base(context)
        {           
        }
    }
}
