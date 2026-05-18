using BOL.Models;
using EF.Core.Repository.Interface.Manager;

namespace BLL.Interfaces.Manager.Tiffinbillrules
{
    public interface ITiffinbillrulesManager : ICommonManager<Tiffinbillrule_DbModel>
    {
        List<ReturnObject> SaveEmployeeTiffinBillProcess(TiffinBillProcessPayload obj);
        Task<ReturnObject> GetEmployeeTiffinNightBillProcess(int companyID, string monthYear);
        ReturnObject DeleteEmployeeTiffinBillProcess(TiffinBillProcessDelPayload obj);
    }
}
