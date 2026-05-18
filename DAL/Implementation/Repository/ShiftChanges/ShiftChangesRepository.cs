using BLL.Interfaces.Repository.ShiftChanges;
using BOL.Models;
using DAL.Data;
using EF.Core.Repository.Repository;

namespace DAL.Implementation.Repository.ShiftChanges
{
    public class ShiftChangesRepository : CommonRepository<ShiftChange_DbModel>,IShiftChangesRepository
    {
        public ShiftChangesRepository(dg_hrpayrollContext context):base(context)
        {           
        }
    }
}
