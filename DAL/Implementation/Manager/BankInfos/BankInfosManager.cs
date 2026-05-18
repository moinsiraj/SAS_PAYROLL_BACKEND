using BLL.Interfaces.Manager.BankInfos;
using BOL.Models;
using DAL.Data;
using DAL.Implementation.Repository.BankInfos;
using EF.Core.Repository.Manager;

namespace DAL.Implementation.Manager.BankInfos
{
    public class BankInfosManager : CommonManager<BankInfos_DbModel>, IBankInfosManager
    {
        public BankInfosManager(dg_hrpayrollContext context) : base( new BankInfosRepository(context))
        {            
        }
    }
}
