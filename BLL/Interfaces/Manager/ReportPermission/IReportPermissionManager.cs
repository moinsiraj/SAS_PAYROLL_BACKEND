using System.Data;
using BOL.Models;

namespace BLL.Interfaces.Manager.ReportPermission
{
    public interface IReportPermissionManager
    {
        Task<DataTable> GetUserWise_reportlist(string userName, string reportType);
        Task<List<TreeListReport>> GetReportListByCatagory(string userName);
        Task<bool> UpdateUserWiseReportPermission(ReportPermissionUpdate obj);

        //New Action
        Task<List<TreeListReport>> GetTotalReportList();
        Task<DataTable> GetReportUserDropdown(int compid);
        Task<DataTable> GetPermissionReportByUser(int compid, string userName);
        Task<DataTable> GetReportPermissionGroup(int compid);
        Task<string> Save_Pay_ReportPermission(ReportPermissionPayload obj);
        Task<DataTable> GetUserAndGroupWise_reportlist(int compid, string userName, string reportType);
        Task<string> Save_Pay_ReportGroup(ReportPermissionGroupPayload obj);
        Task<DataTable> GetPermissionReportByGroupID(int compid, int rep_groupId);
    }
}