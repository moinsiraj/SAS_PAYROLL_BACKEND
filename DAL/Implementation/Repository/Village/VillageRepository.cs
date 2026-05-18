using BLL.Interfaces.Repository.Village;
using BOL.Models;
using DAL.Data;
using EF.Core.Repository.Repository;

namespace DAL.Implementation.Repository.Village
{
    public class VillageRepository : CommonRepository<Village_DbModel>,IVillageRepository
    {
        public VillageRepository(dg_hrpayrollContext context):base(context)
        {            
        }
    }
}
