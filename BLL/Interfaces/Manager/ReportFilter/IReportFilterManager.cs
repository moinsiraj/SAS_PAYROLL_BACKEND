using BOL.Models;
using System.Data;

namespace BLL.Interfaces.Manager.ReportFilter
{
    public interface IReportFilterManager
    {
        Task<ReturnObject> GetFilterEmpData(EmployeeCheckPaylod obj);
        Task<bool> Save_EmpReportParameter(ReportParameterModel obj);
        Task<bool> Save_LevReportParameter(ReportParameterModel obj);
        Task<bool> Save_AttReportParameter(ReportPrintPayload obj);
        Task<bool> Save_SalReportParameter(ReportParameterModel obj);

        //New Action 11/23/2023
        Task<DataTable> GetDepartmentForRepFiltter(int companyID);
        Task<DataTable> GetSectionsForRepFiltter(DepartmenIdtArr obj);
        Task<DataTable> GetBuldingForRepFiltter(int companyID);
        Task<DataTable> GetFloorForRepFiltter(floorIdPayload obj);
        Task<DataTable> GetLineForRepFiltter(lineIdPayload obj);
        Task<DataTable> GetShiftForRepFiltter(ShiftIdPayload obj);
        Task<DataTable> GetGradeForRepFiltter(GradeIdPayload obj);
        Task<DataTable> GetSalCatForRepFiltter(SalCatIdPayload obj);


        Task<DataTable> GetRepFiltterSection(DepartmenIdtArr obj);
        Task<DataTable> GetRepFiltterLine(floorIdArr obj);
        Task<ReturnObject> GetFilterCheckEmpNo(Rep_EmpIdNoCheckPaylod obj);
        Task<bool> Print_SalReportData(ReportPrintPayload obj);
        Task<DataTable> GetEmployeeInactiveReason();
    }
}