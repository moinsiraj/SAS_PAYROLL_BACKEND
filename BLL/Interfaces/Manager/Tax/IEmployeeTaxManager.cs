using BOL.Models;
using EF.Core.Repository.Interface.Repository;

namespace BLL.Interfaces.Manager.Tax
{
    public interface IEmployeeTaxManager : ICommonRepository<BankBranch>
    {
        Task<ReturnObject> GetCompanyForTax(string userName);
        Task<ReturnObject> GetTaxYearInfo();
        Task<ReturnObject> GetEmployeeInfoForTax(int compid, string empPrefix);
        Task<ReturnObject> GetExistingTaxInfo(int taxYear, int empSL);
        Task<ReturnObject> SaveEmployeeTaxInfo(EmployeeTaxModel obj);
        Task<EmployeeTaxReportRender> GetEmployeeTaxInfoReport(EmployeeTaxReport obj);

        Task<ReturnObject> GetEmployeeInfoFromTax(int compid, int taxYear, string empPrefix);
        Task<ReturnObject> GetBankInfoForTax();
        Task<ReturnObject> GetBankInfoBranchForTax(int bankID);
        Task<ReturnObject> SaveTaxChallan(EmployeeTaxChallan obj);
        Task<ReturnObject> GetEmpTaxChallanInfo(int compid, int empSerial, int taxYear);
        Task<ReturnObject> DeleteTaxChallan(int chlnid);
        Task<ReturnObject> UpdateTaxChallan(EmployeeTaxChallan obj);
        Task<EmployeeTaxReportRender> GetEmployeeTaxChallanInfoReport(EmployeeTaxReport obj);
        Task<ReturnObject> GetAllBankBranch(int bankid);
        Task<ReturnObject> AddOrEditBankBranch(BankBranchPayload obj);
    }
}
