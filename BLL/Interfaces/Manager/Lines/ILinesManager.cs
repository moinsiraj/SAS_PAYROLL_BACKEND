using BOL.Models;
using EF.Core.Repository.Interface.Manager;

namespace BLL.Interfaces.Manager.Lines
{
    public interface ILinesManager : ICommonManager<Line_DbModel>
    {
        Task<List<DgPayLine>> GetLineByUserName(string userName);
        Task<ReturnObject> AddOrEditLine(LinePayload obj);
        Task<ReturnObject<LineList>> GetAllLineByCompany(int compnayID);
        Task<ReturnObject> GetBlockByCompany(int compnayID, int floorID);
        Task<ReturnObject> AddOrEditLineBlock(LineBlockPayload obj);
        Task<ReturnObject> GetAllLineBlockByCompany(int compnayID);
        Task<ReturnObject> GetLineBgtList(int companyId);
        Task<ReturnObject> AddOrEditLineBgt(LineBgtPayload obj);
    }
}
