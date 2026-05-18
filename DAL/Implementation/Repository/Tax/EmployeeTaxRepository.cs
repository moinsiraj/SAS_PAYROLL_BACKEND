using BLL.Interfaces.Repository.Tax;
using BOL.Models;
using DAL.Data;
using EF.Core.Repository.Repository;

namespace DAL.Implementation.Repository.Tax
{
    public class EmployeeTaxRepository : CommonRepository<BankBranch>, IEmployeeTaxRepository
    {
        public EmployeeTaxRepository(dg_hrpayrollContext context) : base(context)
        {
        }
    }
}
