using BOL.Models;
using System.Data;

namespace BLL.Interfaces.Manager.EmployeeTransfer
{
    public interface IEmployeeTransferManager
    {
        Task<ReturnObject> SaveEmployeeTransfer(EmployeeTransferModel obj);
        Task<DataTable> GetEmployeeTransferList(int companyID,int employeeID);
        Task<DataTable> GetTransferToCompany(int fCompid);
        Task<ReturnObject> ApproveEmployeeTransfer(int transID, string userName);
        Task<ReturnObject> DeleteEmployeeTransfer(int transID);
    }
}