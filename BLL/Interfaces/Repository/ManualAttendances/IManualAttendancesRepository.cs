using BOL.Models;
using EF.Core.Repository.Interface.Repository;

namespace BLL.Interfaces.Repository.ManualAttendances
{
    public interface IManualAttendancesRepository : ICommonRepository<Attendance_DbModel>
    {
    }
}
