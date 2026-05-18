using BOL.Models;
using EF.Core.Repository.Interface.Manager;

namespace BLL.Interfaces.Manager.Designations
{
    public interface IDesignationsManager : ICommonManager<Designation_DbModel>
    {
        Task<ReturnObject> GetDesignationCatList();
        Task<ReturnObject> AddOrEditDesignation(DesignationPayload obj);
        Task<ReturnObject<DesignationJoinList>> GetAllDesignation();
        Task<ReturnObject> GetDesigWiseBgtList(int companyId);
        Task<ReturnObject> AddOrEditDesignationBgt(DesignationBgtPayload obj);
        Task<ReturnObject> GetDesignationWiseBudgetAll(int companyID);
        Task<ReturnObject> GetSelectedDesignationWiseBudget(int bgtId);
        Task<ReturnObject> AddOrEditDesignationWiseBudget(DesignationBgt_Payload obj);
    }
}
