using BLL.Interfaces.Repository.AttbonusRule;
using BOL.Models;
using DAL.Data;
using EF.Core.Repository.Repository;

namespace DAL.Implementation.Repository.AttbonusRule
{
    public class AttbonusRuleRepository : CommonRepository<AttbonusRule_DbModel>, IAttbonusRuleRepository
    {
        public AttbonusRuleRepository(dg_hrpayrollContext context) : base(context)
        {
        }
    }
}
