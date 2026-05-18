using BOL.Models;
using EF.Core.Repository.Interface.Manager;

namespace BLL.Interfaces.Manager.LunchInoutSetups
{
    public interface ILunchInoutSetupsManager : ICommonManager<LunchInoutSetup_DbModel>
    {
        List<ReturnObject> SaveEmployeeLunchOutHistory(EmployeeLunchOutPayload obj);
        Task<ReturnObject> GetEmployeeInfoForLunchOut(LunchOutEmployeeInfo obj);
        List<ReturnObject> DeleteEmployeeLunchOut(EmployeeLunchOutPayload obj);
    }
}
