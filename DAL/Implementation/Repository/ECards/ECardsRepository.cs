using BLL.Interfaces.Repository.ECards;
using BOL.Models;
using DAL.Data;
using EF.Core.Repository.Repository;

namespace DAL.Implementation.Repository.ECards
{
    public class ECardsRepository : CommonRepository<ECard_DbModel>, IECardsRepository
    {
        public ECardsRepository(dg_hrpayrollContext context) : base(context)
        {
            
        }
    }
}
