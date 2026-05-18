using BOL.Models;
using EF.Core.Repository.Interface.Manager;
using System.Data;

namespace BLL.Interfaces.Manager.LeaveInfor
{
    public interface ILeaveInforManager : ICommonManager<LeaveInfor_DbModel>
    {
        Task<DataSet> GetCompanyName(int CompID, int EmpNO, int year);
        Task<DataSet> GetLeaveBalanceInfo(int EmpSerial, int year);
        Task<ReturnObject> UpdateLeaveInfo(int empSl, decimal anualBal);
    }
}
