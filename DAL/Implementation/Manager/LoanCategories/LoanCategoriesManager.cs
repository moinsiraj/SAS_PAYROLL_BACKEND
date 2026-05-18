using BLL.Interfaces.Manager.LoanCategories;
using BOL.Models;
using DAL.Data;
using DAL.Implementation.Repository.LoanCategories;
using EF.Core.Repository.Manager;

namespace DAL.Implementation.Manager.LoanCategories
{
    public class LoanCategoriesManager :CommonManager<LoanCategory_DbModel>,ILoanCategoriesManager
    {
        public LoanCategoriesManager(dg_hrpayrollContext context) : base(new LoanCategoriesRepository(context))
        {           
        }        
    }
}
