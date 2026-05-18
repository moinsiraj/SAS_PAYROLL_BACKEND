using BLL.Interfaces.Repository.Districts;
using BOL.Models;
using DAL.Data;
using EF.Core.Repository.Repository;

namespace DAL.Implementation.Repository.Districts
{
    public class DistrictsRepository : CommonRepository<District_DbModel>,IDistrictsRepository
    {
        public DistrictsRepository(dg_hrpayrollContext context) : base(context)
        {
        }
    }
}
