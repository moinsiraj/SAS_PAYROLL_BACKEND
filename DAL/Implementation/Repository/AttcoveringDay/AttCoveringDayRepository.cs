using BLL.Interfaces.Repository.AttcoveringDay;
using BOL.Models;
using DAL.Data;
using EF.Core.Repository.Repository;
using Microsoft.EntityFrameworkCore;

namespace DAL.Implementation.Repository.AttcoveringDay
{
    public class AttCoveringDayRepository : CommonRepository<AttcoveringDay_DbModel>, IAttCoveringDayRepository
    {
        public AttCoveringDayRepository(dg_hrpayrollContext context) : base(context)
        {
        }
    }
}
