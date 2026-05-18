using BLL.Interfaces.Repository.Divisions;
using BOL.Models;
using DAL.Data;
using EF.Core.Repository.Repository;

namespace DAL.Implementation.Repository.Divisions
{
    public class DivisionsRepository : CommonRepository<Division_DbModel>,IDivisionsRepository
    {
        public DivisionsRepository(dg_hrpayrollContext context) : base(context)
        {
        }
    }
}
