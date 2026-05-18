using BLL.Interfaces.Manager.Stampcharges;
using BOL.Models;
using DAL.Data;
using DAL.Implementation.Repository.Stampcharges;
using EF.Core.Repository.Manager;

namespace DAL.Implementation.Manager.Stampcharges
{
    public class StampchargesManager : CommonManager<Stampcharge_DbModel>,IStampchargesManager
    {
        public StampchargesManager(dg_hrpayrollContext context):base(new StampchargesRepository(context))
        {            
        }        
    }
}
