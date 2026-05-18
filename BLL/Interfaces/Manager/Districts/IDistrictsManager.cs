using BOL.Models;
using EF.Core.Repository.Interface.Manager;

namespace BLL.Interfaces.Manager.Districts
{
    public interface IDistrictsManager : ICommonManager<District_DbModel>
    {
        Task<ReturnObject> AddOrEditDistrict(DistrictPayload obj);
        Task<ReturnObject<District_DbModel>> GetAllDistrict();
    }
}
