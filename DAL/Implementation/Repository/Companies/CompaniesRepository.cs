using BLL.Interfaces.Repository.Companies;
using BOL.Models;
using DAL.Data;
using EF.Core.Repository.Repository;

namespace DAL.Implementation.Repository.Companies
{
    public class CompaniesRepository : CommonRepository<Company_DbModel>,ICompaniesRepository
    {
        public CompaniesRepository(dg_hrpayrollContext context) : base(context)
        {
        }
    }
}
