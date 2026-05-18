using BOL.Models;
using EF.Core.Repository.Interface.Manager;

namespace BLL.Interfaces.Manager.Divisions
{
    public interface IDivisionsManager : ICommonManager<Division_DbModel>
    {
        Task<ReturnObject> AddOrEditDivision(DivisionPayload obj);
        Task<ReturnObject<Division_DbModel>> GetAllDivisions();
        Task<ReturnObject<Division_DbModel>> GetDivisionById(int id);
    }
}
