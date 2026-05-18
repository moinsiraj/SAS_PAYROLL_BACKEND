using BOL.Models;
using EF.Core.Repository.Interface.Manager;

namespace BLL.Interfaces.Manager.Allowancesde
{
    public interface IAllowancesdeManager : ICommonManager<Allowancesde_DbModel>
    {
        Task<ReturnObject> AddOrEditAllowance(AllowancesdepPayload obj);
        Task<ReturnObject<Allowancesde_DbModel>> GetAllAllowances();
    }
}
