using BLL.Interfaces.Repository.Designations;
using BOL.Models;
using DAL.Data;
using EF.Core.Repository.Repository;

namespace DAL.Implementation.Repository.Designations
{
    public class DesignationsRepository : CommonRepository<Designation_DbModel>,IDesignationsRepository
    {
        public DesignationsRepository(dg_hrpayrollContext context) : base(context)
        {
        }
    }
}
