using BLL.Interfaces.Repository.Grades;
using BOL.Models;
using DAL.Data;
using EF.Core.Repository.Repository;

namespace DAL.Implementation.Repository.Grades
{
    public class GradesRepository : CommonRepository<Grade_DbModel>,IGradesRepository
    {
        public GradesRepository(dg_hrpayrollContext context) : base(context)
        {            
        }
    }
}
