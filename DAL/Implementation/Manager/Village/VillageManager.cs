using BLL.Interfaces.Manager.Village;
using BOL.Models;
using DAL.Data;
using DAL.Implementation.Repository.Village;
using EF.Core.Repository.Manager;

namespace DAL.Implementation.Manager.Village
{
    public class VillageManager : CommonManager<Village_DbModel>,IVillageManager
    {
        public VillageManager(dg_hrpayrollContext context):base(new VillageRepository(context))
        {
        }
    }
}
