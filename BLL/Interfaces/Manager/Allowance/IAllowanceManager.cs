using BOL.Models;
using EF.Core.Repository.Interface.Manager;
using System.Data;

namespace BLL.Interfaces.Manager.Allowance
{
    public interface IAllowanceManager : ICommonManager<Allowance_DbModel>
    {
        Task<DataSet> GetAllowanceType(string AllowanceType);
        Task<DataSet> GetEmpWiseAllowance(decimal al_emp_serial, string alDate);
        Task<DataSet> GetAllowanceDateWise(int CompID, string AllowanceType, DateTime StartDate, DateTime EndDate);
        Task<ReturnObject> SaveEmpAllowance(DgPayAllowancePostPayload obj);
    }
}
