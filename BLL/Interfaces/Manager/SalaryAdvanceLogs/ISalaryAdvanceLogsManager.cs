using BOL.Models;
using EF.Core.Repository.Interface.Manager;
using System.Data;

namespace BLL.Interfaces.Manager.SalaryAdvanceLogs
{
    public interface ISalaryAdvanceLogsManager : ICommonManager<SalaryAdvanceLog_DbModel>
    {
        Task<DataSet> AdvanceProcess(int SAMonth, int SAYear, int sp_groupid, int sp_compid, int days);
        Task<DataSet> AdvanceProcess(int CompId, int month, int year);
        Task<DataSet> AdvanceProcessSum(int CompId, int month, int year);

        //New Action
        Task<ReturnObject> SaveAdvanceProcess(DgPaySalaryAdvanceLog obj);
        Task<ReturnObject> SaveAdvanceConfirmed(DgPaySalaryAdvanceLog obj);
        Task<ReturnObject> Salary_Advance_PaymentDate(PaySalaryAdvancePayment obj);
    }
}
