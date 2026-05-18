using BOL.Models;
using EF.Core.Repository.Interface.Manager;
using System.Data;

namespace BLL.Interfaces.Manager.DgPayDeducations
{
    public interface IDgPayDeducationsManager : ICommonManager<DgPayDeducation_DbModel>
    {
        Task<DataSet> GetDeductionType(string DeductionType);
        Task<DataSet> GetEmpwiseDeduction(string al_emp_serial, string dDate);
        Task<DataSet> GetDeductionDatewise(int CompID, string DeductionType, DateTime StartDate, DateTime EndDate);
        Task<ReturnObject> SaveEmpDeducation(DgPayDeducationPostPayload obj);
    }
}
