using BLL.Interfaces.Repository.BankInfos;
using BOL.Models;
using DAL.Data;
using EF.Core.Repository.Repository;

namespace DAL.Implementation.Repository.BankInfos
{
    public class BankInfosRepository : CommonRepository<BankInfos_DbModel>, IBankInfosRepository
    {
        public BankInfosRepository(dg_hrpayrollContext context) : base(context)
        {            
        }
    }
}
