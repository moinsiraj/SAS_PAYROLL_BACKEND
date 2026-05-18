using BOL.Models;
using EF.Core.Repository.Interface.Manager;

namespace BLL.Interfaces.Manager.BuildingUnit
{
    public interface IBuildingUnitManager : ICommonManager<BuildingUnit_DbModel>
    {
        Task<List<DgPayBuildingUnit>> GetBuildingUnitsByUser(string userName);
        Task<ReturnObject> AddOrEditBuildingUnit(BuildingUnitPayload obj);
        Task<ReturnObject<BuildingUnit_DbModel>> GetAllBuildingUnitByCompany(int compnayID);
    }
}
