using BOL.Models;
using EF.Core.Repository.Interface.Manager;

namespace BLL.Interfaces.Manager.Floors
{
    public interface IFloorsManager : ICommonManager<Floor_DbModel>
    {
        Task<List<DgPayFloor>> GetFloorByUserName(string userName);
        Task<ReturnObject> AddOrEditFloor(FloorPayload obj);
        Task<ReturnObject<Floor_DbModel>> GetAllFloorByCompany(int compnayID);
    }
}
