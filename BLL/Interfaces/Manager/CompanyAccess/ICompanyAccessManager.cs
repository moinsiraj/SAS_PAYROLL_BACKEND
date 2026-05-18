using BOL.Models;
using EF.Core.Repository.Interface.Manager;
using System.Data;

namespace BLL.Interfaces.Manager.CompanyAccess
{
    public interface ICompanyAccessManager : ICommonManager<CompanyAccess_DbModel>
    {
        Task<DataSet> User_List();
        Task<DataSet> User_List_New();
        Task<DataSet> User_Listfrom_ACCESS(string user);

        //New Action 8/15/2024
        ReturnObject GetAllCompanyAccess();
        Task<ReturnObject> GetAccessByCompanyUser(int companyID, string userName);
        Task<ReturnObject<DgPayCompanyaccess>> AddOrEditCompanyAccess(DgPayCompanyAccessPayload obj);
    }
}
