using BLL.Interfaces.Repository.LeaveInfor;
using BOL.Models;
using DAL.Data;
using EF.Core.Repository.Repository;

namespace DAL.Implementation.Repository.LeaveInfor
{
    public class LeaveInforRepository : CommonRepository<LeaveInfor_DbModel>,ILeaveInforRepository
    {
        public LeaveInforRepository(dg_hrpayrollContext context) : base(context)
        {           
        }
    }
}
