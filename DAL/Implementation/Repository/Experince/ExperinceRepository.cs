using BLL.Interfaces.Repository.Experince;
using BOL.Models;
using DAL.Data;
using EF.Core.Repository.Repository;

namespace DAL.Implementation.Repository.Experince
{
    public class ExperinceRepository : CommonRepository<EmpExperince>,IExperinceRepository
    {
        public ExperinceRepository(dg_hrpayrollContext context) : base(context)
        {            
        }
    }
}
