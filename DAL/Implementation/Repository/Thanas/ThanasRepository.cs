using BLL.Interfaces.Repository.Thanas;
using BOL.Models;
using DAL.Data;
using EF.Core.Repository.Repository;

namespace DAL.Implementation.Repository.Thanas
{
    public class ThanasRepository : CommonRepository<Thana_DbModel>,IThanasRepository
    {
        public ThanasRepository(dg_hrpayrollContext context):base(context)
        {            
        }
    }
}
