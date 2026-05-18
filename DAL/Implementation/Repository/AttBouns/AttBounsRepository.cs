using BLL.Interfaces.Repository.AttBouns;
using BOL.Models;
using DAL.Data;
using EF.Core.Repository.Repository;

namespace DAL.Implementation.Repository.AttBouns
{
    public class AttBounsRepository : CommonRepository<AttBonus_DbModel>, IAttBounsRepository
    {
        public AttBounsRepository(dg_hrpayrollContext context) : base(context)
        {
        }
    }
}
