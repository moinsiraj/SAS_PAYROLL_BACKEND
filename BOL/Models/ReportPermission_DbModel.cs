using System.ComponentModel.DataAnnotations;

namespace BOL.Models
{
    public class ReportPermission_DbModel
    {
        [Key]
        public int report_id { get; set; }
        public string report_type { get; set; } = null;
        public string report_name { get; set; } = null;
        public string report_url { get; set; } = null;       
        public string report_permission_status { get; set; } = null;
        public bool? report_permission { get; set; } = null;
        public bool? hideToInputField { get; set; } = null;
        public string permission_by { get; set; } = null;
        public string permission_date { get; set; } = null;
    }
    public class ReportArrayPayload
    {
        public string report_type { get; set; }
        public int report_serial { get; set; }
        public string report_name { get; set; }
        public string report_url { get; set; }
        public bool report_IsExcel { get; set; }
    }
    public class ReportPermissionPayload
    {
        public int report_compid { get; set; }
        public string report_user { get; set; }
        public string userPermissionStatus { get; set; }
        public int report_permission_code { get; set; }
        public string permission_by { get;set; }
        public List<ReportArrayPayload> permissionForm { get; set; }
        public bool report_IsExcel { get; set; }
    }
    public class ReportPermissionGroupPayload
    {
        public int rep_Ugroup { get; set; }
        public string rep_groupName { get; set; }
        public int rep_groupCompid { get; set; }
        public string rep_groupCreate { get; set; }
        public List<ReportArrayPayload> permissionForm { get; set; }
    }
    public class Dg_ReportPermission
    {
        public int report_id { get; set; }
        public int rep_show_Sl { get; set; }
        public string report_type { get; set; }
        public string report_name { get; set; }
        public string report_url { get; set; }
        public string report_user { get; set; }
        public string report_for { get; set; }
        public bool report_permission { get; set; }
        public bool report_IsExcel { get; set; }
    }
    public class TreeListReport
    {
        public int id { get; set; }
        public string title { get; set; }
        public List<Dg_ReportPermission> reports { get; set; }
    }
    public class ReportPermissionUpdate
    {
        public string userName { get; set; }
        public int[] reportID { get; set; }
    }
}