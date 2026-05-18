using BOL.Models;
using EF.Core.Repository.Interface.Manager;
using System.Data;

namespace BLL.Interfaces.Manager.Specialholidays
{
    public interface ISpecialholidaysManager : ICommonManager<Specialholiday_DbModel>
    {
        List<ReturnObject> SaveEmpWiseSpecialholiday(EmpWiseSpecialholidayPayload obj);
        Task<ReturnObject> GetCreatedSpecialholidays_List(int compid, string formDate, string toDate);
        List<ReturnObject> DeleteEmpWiseSpecialholiday(DeleteSpecialholidayPayload obj);
        Task<DataTable> GetEmployeeListForSpecialholiday(int Compid, int Department,
            int section, int Building, int Floor, int Line,
            int Shift, int Grade, int salcat,string formDate,string toDate);
        Task<ReturnObject> SpecialholidaysProcess(SpecialholidayProcessPayload obj);
        ReturnObject SaveHolidayProcess(HolidayProcessPayload obj);
        Task<ReturnObject> GetEmployeeHolidayBillProcess(int companyID, string monthYear);
        ReturnObject DeleteEmployeeHolidayBillProcess(HolidayBillProcessDelPayload obj);
        DataTable test_save();
    }
}
