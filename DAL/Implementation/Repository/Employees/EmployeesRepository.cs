using BLL.Interfaces.Repository.Employees;
using BOL.Models;
using DAL.Data;
using EF.Core.Repository.Repository;

namespace DAL.Implementation.Repository.Employees
{
    public class EmployeesRepository : CommonRepository<Employee_DbModel>,IEmployeesRepository
    {
        public EmployeesRepository(dg_hrpayrollContext context) : base(context)
        {           
        }
    }
}
