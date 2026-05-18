using BOL.Models;
using EF.Core.Repository.Interface.Repository;
using System.Data;
using static BOL.Models.DgEmpIncrementInfo;

namespace BLL.Interfaces.Manager.IncrementInfoes
{
    public interface IIncrementInfoesManager : ICommonRepository<IncrementInfo_DbModel>
    {
        Task<DataSet> Increment_list(int Compid);
        Task<DataSet> Increment_Batch_list(int com_code, string inc_type, int dependon,
            decimal inc_gross, decimal inc_basic, decimal inc_grossPrct, decimal inc_BasicPrct,
            DateTime date, string uid, DateTime cutofdate);
        Task<ReturnObject> SaveEmployeeIncrementInfo(EmployeeIncrementPayload obj);
        Task<DataTable> GetEmpIncrementInfoList(int compid);
        Task<string> SaveEmpIncrementApprove(List<EmpIncrementApprovePayload> obj);
        Task<string> DeleteEmpIncrement(List<EmpIncrementApprovePayload> obj);
    }
}
