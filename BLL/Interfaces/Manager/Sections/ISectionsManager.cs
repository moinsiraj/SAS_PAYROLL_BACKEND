using BOL.Models;
using EF.Core.Repository.Interface.Manager;
using System.Data;

namespace BLL.Interfaces.Manager.Sections
{
    public interface ISectionsManager : ICommonManager<Section_DbModel>
    {
        Task<List<DgPaySection>> GetSectionByUserPermission(string userName);
        //DgPaySection DbToCustomModel(Section_DbModel obj);
        Task<ReturnObject> AddOrEditSection(SectionPayload obj);
        Task<ReturnObject<SectionList>> GetAllSectionByCompany(int compnayID);
    }
}