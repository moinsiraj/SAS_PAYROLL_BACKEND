using BLL.Interfaces.Repository.Shifts;
using BOL.Models;
using DAL.Data;
using EF.Core.Repository.Repository;

namespace DAL.Implementation.Repository.Shifts
{
    public class ShiftsRepository : CommonRepository<Shift_DbModel>,IShiftsRepository
    {
        public ShiftsRepository(dg_hrpayrollContext context):base(context)
        {           
        }
    }
}
