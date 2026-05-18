using BLL.Interfaces.Manager.Activedate;
using BOL.Models;
using DAL.Data;
using DAL.Implementation.Repository.Activedate;
using EF.Core.Repository.Manager;

namespace DAL.Implementation.Manager.Activedate
{
    public class ActiveDateManager : CommonManager<Activedate_DbModel>, IActiveDateManager
    {
        public ActiveDateManager(dg_hrpayrollContext context) : base(new ActiveDateRepository(context))
        {
        }
    }
}
