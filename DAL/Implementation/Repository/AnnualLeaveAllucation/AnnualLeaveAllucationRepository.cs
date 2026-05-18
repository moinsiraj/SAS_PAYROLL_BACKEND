using BLL.Interfaces.Repository.AnnualLeaveAllucation;
using BOL.Models;
using DAL.Data;
using EF.Core.Repository.Repository;

namespace DAL.Implementation.Repository.AnnualLeaveAllucation
{
    public class AnnualLeaveAllucationRepository : CommonRepository<AnnualLeaveAllucation_DbModel>, IAnnualLeaveAllucationRepository
    {
        public AnnualLeaveAllucationRepository(dg_hrpayrollContext context) : base(context)
        {
        }
    }
}
