using BLL.Interfaces.Manager.Companies;
using BOL.Models;
using DAL.Data;
using DAL.Implementation.Repository.Companies;
using EF.Core.Repository.Manager;

namespace DAL.Implementation.Manager.Companies
{
    public class CompaniesManager : CommonManager<Company_DbModel>,ICompaniesManager
    {
        public CompaniesManager(dg_hrpayrollContext context) : base(new CompaniesRepository(context))
        {
        }
    }
}
