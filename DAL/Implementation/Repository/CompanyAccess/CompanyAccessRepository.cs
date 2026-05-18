using BLL.Interfaces.Repository.CompanyAccess;
using BOL.Models;
using DAL.Data;
using EF.Core.Repository.Repository;

namespace DAL.Implementation.Repository.CompanyAccess
{
    public class CompanyAccessRepository : CommonRepository<CompanyAccess_DbModel>,ICompanyAccessRepository
    {
        public CompanyAccessRepository(dg_hrpayrollContext context) : base(context)
        {
        }
    }
}
