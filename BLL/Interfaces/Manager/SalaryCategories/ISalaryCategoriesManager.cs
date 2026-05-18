using BOL.Models;
using EF.Core.Repository.Interface.Manager;

namespace BLL.Interfaces.Manager.SalaryCategories
{
    public interface ISalaryCategoriesManager : ICommonManager<Salarycategory_DbModel>
    {
        Task<ReturnObject> AddOrEditSalaryCategory(SalarycategoryPayload obj);
        Task<ReturnObject<Salarycategory_DbModel>> GetAllSalaryCategoryByCompany(int companyID);
    }
}
