using BLL.Interfaces.Repository.Tiffinbillrules;
using BOL.Models;
using DAL.Data;
using EF.Core.Repository.Repository;

namespace DAL.Implementation.Repository.Tiffinbillrules
{
    public class TiffinbillrulesRepository : CommonRepository<Tiffinbillrule_DbModel>,ITiffinbillrulesRepository
    {
        public TiffinbillrulesRepository(dg_hrpayrollContext context):base(context)
        {           
        }
    }
}
