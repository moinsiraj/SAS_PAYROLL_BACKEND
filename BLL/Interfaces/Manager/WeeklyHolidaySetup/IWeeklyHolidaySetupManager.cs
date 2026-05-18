using BOL.Models;

namespace BLL.Interfaces.Manager.WeeklyHolidaySetup
{
    public interface IWeeklyHolidaySetupManager
    {
        List<ReturnObject> SaveWeeklyHolidaySetup(WeeklyHolidayPayload obj);
        Task<ReturnObject> GetWeeklyHoliday(string monthYear,int empSerial);
    }
}
