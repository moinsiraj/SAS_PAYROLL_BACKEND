using BOL.Models;
using EF.Core.Repository.Interface.Manager;
using System.Data;

namespace BLL.Interfaces.Manager.Leavetransactions
{
    public interface ILeavetransactionsManager : ICommonManager<Leavetransaction_DbModel>
    {
        Task<DataSet> Getleave_info_comdatewise(int CompID, DateTime Sdate, DateTime Edate);
        Task<DataTable> GetEmployeeNo(int compID, int levYear);
        Task<ReturnObject> SaveLeaveLeavetransaction(LeaveTransactionPayload obj);
        Task<ReturnObject> SaveLeaveLeavetransaction_Bulk(LeaveTransaction_BulkPayload obj);
        Task<bool> deleteLevTrans(int id, string userName);

        //Online Leave Transaction
        Task<ReturnObject> GetRecommenderAndDPTPerson(string prefixText);
        Task<ReturnObject> GetEmployeeSlByCompEmp_online(int compid, int empno);
        Task<ReturnObject> GetEmployeeLeaveOnlineBalance(int empSerial, int year);
        Task<ReturnObject> SaveEmployeeLeave_online(LeaveTransactionOnlinePayload obj);
        Task<ReturnObject> GetEmployeeLeaveAddInfo_online(int compid, string userName);
        Task<ReturnObject> DeleteEmployeeLeave_online(int compid, int leaveID, string userName);

        // Approval
        Task<ReturnObject> GetLeaveTransferPersonList(int transEmpSerial);
        Task<ReturnObject> UpdateLeaveTransferPerson(LeaveOnlineApp obj);
        Task<ReturnObject> GetLeaveRecommenderPersonList(int transEmpSerial);
        Task<ReturnObject> UpdateLeaveRecommenderPerson(LeaveOnlineApp obj);
        Task<ReturnObject> GetLeaveDeptHeadPersonList(int transEmpSerial);
        Task<ReturnObject> UpdateLeaveDeptHeadPerson(LeaveOnlineApp obj);
        Task<ReturnObject> GetLeaveHrList(string userName);
        Task<ReturnObject> UpdateLeaveHrPerson(LeaveOnlineApp obj);
        Task<ReturnObject> GetLeaveAppList(string userName);
        Task<ReturnObject> UpdateLeaveAppPerson(LeaveOnlineApp obj);
        Task<ReturnObject> GetLeaveAppListAll(string userName);
    }
}
