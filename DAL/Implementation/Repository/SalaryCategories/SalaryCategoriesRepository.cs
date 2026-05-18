using BLL.Interfaces.Repository.SalaryCategories;
using BOL.Models;
using DAL.Data;
using EF.Core.Repository.Repository;

namespace DAL.Implementation.Repository.SalaryCategories
{
    public class SalaryCategoriesRepository : CommonRepository<Salarycategory_DbModel>,ISalaryCategoriesRepository
    {
        public SalaryCategoriesRepository(dg_hrpayrollContext context):base(context)
        {            
        }
    }
}
