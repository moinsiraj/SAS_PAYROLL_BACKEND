using BLL.Interfaces.Manager.AttbonusRule;
using BOL.Models;
using DAL.Data;
using DAL.Implementation.Repository.AttbonusRule;
using EF.Core.Repository.Manager;

namespace DAL.Implementation.Manager.AttbonusRule
{
    public class AttbonusRuleManager : CommonManager<AttbonusRule_DbModel>, IAttbonusRuleManager
    {
        public AttbonusRuleManager(dg_hrpayrollContext context) : base(new AttbonusRuleRepository(context))
        {
        }
    }
}
