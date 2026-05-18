using BOL.Models;
using EF.Core.Repository.Interface.Repository;

namespace BLL.Interfaces.Repository.Employees
{
    public interface IEmployeesRepository : ICommonRepository<Employee_DbModel>
    {
    }
}
