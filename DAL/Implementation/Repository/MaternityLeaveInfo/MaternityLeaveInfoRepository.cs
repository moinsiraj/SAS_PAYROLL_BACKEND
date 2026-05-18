using BLL.Interfaces.Repository.MaternityLeaveInfo;
using BOL.Models;
using DAL.Data;
using EF.Core.Repository.Repository;

namespace DAL.Implementation.Repository.MaternityLeaveInfo
{
    public class MaternityLeaveInfoRepository : CommonRepository<MaternityLeaveInfo_DbModel>,IMaternityLeaveInfoRepository
    {
        public MaternityLeaveInfoRepository(dg_hrpayrollContext context) : base(context)
        {            
        }
    }
}
