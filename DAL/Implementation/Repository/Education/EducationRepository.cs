using BLL.Interfaces.Repository.Education;
using BOL.Models;
using DAL.Data;
using EF.Core.Repository.Repository;

namespace DAL.Implementation.Repository.Education
{
    public class EducationRepository : CommonRepository<EmpEducation>, IEducationRepository
    {
        public EducationRepository(dg_hrpayrollContext context) : base(context)
        {            
        }
    }
}
