using BOL.Models;
using EF.Core.Repository.Interface.Manager;

namespace BLL.Interfaces.Manager.Thanas
{
    public interface IThanasManager : ICommonManager<Thana_DbModel>
    {
        Task<ReturnObject> AddOrEditThana(ThanaPayload obj);
        Task<ReturnObject<Thana_DbModel>> GetAllThana();
    }
}
