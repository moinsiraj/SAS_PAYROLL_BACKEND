using BOL.Models;
using EF.Core.Repository.Interface.Manager;

namespace BLL.Interfaces.Manager.Deduction
{
    public interface IDeductionManager : ICommonManager<Deduction_DbModel>
    {
        Task<ReturnObject> AddOrEditDeduction(DeductionPayload obj);
        Task<ReturnObject<Deduction_DbModel>> GetAllDeduction();
    }
}
