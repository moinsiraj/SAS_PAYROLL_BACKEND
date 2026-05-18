using BLL.Interfaces.Repository.ManualAttendances;
using BOL.Models;
using DAL.Data;
using EF.Core.Repository.Repository;

namespace DAL.Implementation.Repository.ManualAttendances
{
    public class ManualAttendancesRepository : CommonRepository<Attendance_DbModel>,IManualAttendancesRepository
    {
        public ManualAttendancesRepository(dg_hrpayrollContext context) : base(context)
        {
            
        }
    }
}
