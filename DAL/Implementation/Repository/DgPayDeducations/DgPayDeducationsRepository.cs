using BLL.Interfaces.Repository.DgPayDeducations;
using BOL.Models;
using DAL.Data;
using EF.Core.Repository.Repository;

namespace DAL.Implementation.Repository.DgPayDeducations
{
    public class DgPayDeducationsRepository : CommonRepository<DgPayDeducation_DbModel>, IDgPayDeducationsRepository
    {
        public DgPayDeducationsRepository(dg_hrpayrollContext context) : base(context)
        {           
        }
    }
}
