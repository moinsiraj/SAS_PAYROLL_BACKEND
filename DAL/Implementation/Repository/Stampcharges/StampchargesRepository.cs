using BLL.Interfaces.Repository.Stampcharges;
using BOL.Models;
using DAL.Data;
using EF.Core.Repository.Repository;

namespace DAL.Implementation.Repository.Stampcharges
{
    public class StampchargesRepository : CommonRepository<Stampcharge_DbModel>,IStampchargesRepository
    {
        public StampchargesRepository(dg_hrpayrollContext context):base(context)
        {           
        }
    }
}
