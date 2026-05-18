using BOL.Models;
using EF.Core.Repository.Interface.Manager;
using System.Data;

namespace BLL.Interfaces.Manager.Departments
{
    public interface IDepartmentsManager : ICommonManager<Department_DbModel>
    {
        Task<List<DgPayDepartment>> GetDepartmentByUserName(string userName);
        Task<ReturnObject> AddOrEditDepartment(DepartmentPayload obj);
        Task<ReturnObject<Department_DbModel>> GetAllDepartmentByCompany(int compnayID);
    }
}