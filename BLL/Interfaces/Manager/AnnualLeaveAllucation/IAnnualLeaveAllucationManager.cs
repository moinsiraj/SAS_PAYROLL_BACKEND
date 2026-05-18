using BOL.Models;
using EF.Core.Repository.Interface.Manager;
using System.Data;

namespace BLL.Interfaces.Manager.AnnualLeaveAllucation
{
    public interface IAnnualLeaveAllucationManager : ICommonManager<AnnualLeaveAllucation_DbModel>
    {
        Task<DataSet> GetAnnualLeave_process(int year, int casual, int medical, int annul, int comID);
        Task<ReturnObject> GetAnnualLeaveEmployee(int copmid, int? deptid = null, int? secid = null);
        Task<List<AnnualLeaveResponse>> AnnualLeave_process_save(AnnualLeave obj);
    }
}
