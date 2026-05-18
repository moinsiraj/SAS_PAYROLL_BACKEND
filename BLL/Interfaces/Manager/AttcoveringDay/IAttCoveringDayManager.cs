using BOL.Models;
using EF.Core.Repository.Interface.Manager;
using System.Data;

namespace BLL.Interfaces.Manager.AttcoveringDay
{
    public interface IAttCoveringDayManager : ICommonManager<AttcoveringDay_DbModel>
    {
        Task<DataTable> GetEmployeeListForCoverday(int Compid, int Department, int section, int Building, int Floor, int Line, int Shift, int Grade, int salcat, string formDate, string toDate);
        List<ReturnObject> SaveEmpWiseCoveringDay(EmpWiseCoveringdayPayload obj);
        Task<ReturnObject> GetCreatedCoverdingDays_List(int compid, string formDate, string toDate);
        List<ReturnObject> DeleteEmpWiseSpecialholiday(DeleteCoveringdayPayload obj);
    }
}