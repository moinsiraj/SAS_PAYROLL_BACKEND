using BLL.Interfaces.Manager.AttBouns;
using BOL.Models;
using DAL.Data;
using DAL.Implementation.Repository.AttBouns;
using EF.Core.Repository.Manager;

namespace DAL.Implementation.Manager.AttBouns
{
    public class AttBounsManager : CommonManager<AttBonus_DbModel>, IAttBounsManager
    {
        public AttBounsManager(dg_hrpayrollContext context) : base(new AttBounsRepository(context))
        {
        }
    }
}
