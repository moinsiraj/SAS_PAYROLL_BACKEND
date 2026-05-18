using BLL.Interfaces.Repository.Departments;
using BOL.Models;
using DAL.Data;
using EF.Core.Repository.Repository;

namespace DAL.Implementation.Repository.Departments
{
    //public class DepartmentsRepository : CommonRepository<Department_DbModel>,IDepartmentsRepository
    //{
    //    public DepartmentsRepository(dg_hrpayrollContext context) : base(context)
    //    {           
    //    }
    //}


    //From SpecFo
    public class DepartmentsRepository : CommonRepository<Department_DbModel>, IDepartmentsRepository
    {
        public DepartmentsRepository(dg_SpecFoContext context) : base(context)
        {
        }
    }
}
