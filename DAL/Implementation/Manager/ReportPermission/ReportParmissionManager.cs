using BLL.Interfaces.Manager.ReportPermission;
using BLL.Utility;
using BOL.Models;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Implementation.Manager.ReportPermission
{
    public class ReportParmissionManager : IReportPermissionManager
    {
        private readonly Dg_Common _dgCommon;
        private readonly SqlConnection _connection;
        public ReportParmissionManager(Dg_Common dgCommon)
        {
            _dgCommon = dgCommon;
            _connection = new SqlConnection(Getway.Dg_Payroll);
        }

        public async Task<DataTable> GetUserWise_reportlist(string userName, string reportType)
        {
            var data = await _dgCommon.get_InformationDataTableAsync("user_wise_reportlist '" + userName + "','" + reportType + "'", _connection);
            return data;
        }
        private async Task<DataTable> GetReportCatagory()
        {
            var data = await _dgCommon.get_InformationDataTableAsync("select distinct report_type from Dg_Pay_Report_permission", _connection);
            return data;
        }
        private async Task<DataTable> GetTotalReportListByUser(string userName,string typeRep)
        {
            var data = await _dgCommon.get_InformationDataTableAsync("select * from Dg_Pay_Report_permission where report_user='"+ userName + "' and report_type='"+ typeRep + "'", _connection);
            return data;
        }
        public async Task<List<TreeListReport>> GetReportListByCatagory(string userName)
        {
            List<TreeListReport> listObjTree = new List<TreeListReport>();
            List<Dg_ReportPermission> listObj = new List<Dg_ReportPermission>();
            var rptListType = await this.GetReportCatagory();
            for (int i = 0; i < rptListType.Rows.Count; i++)
            {               
                var childData = await this.GetTotalReportListByUser(userName, rptListType.Rows[i]["report_type"].ToString());
                foreach (DataRow row in childData.Rows)
                {
                    listObj.Add(new Dg_ReportPermission
                    {
                        report_id = Convert.ToInt32(row["report_id"]),
                        report_type = row["report_type"].ToString(),
                        report_name = row["report_name"].ToString(),
                        report_url = row["report_url"].ToString(),
                        report_user = row["report_user"].ToString(),
                        report_permission = Convert.ToBoolean(row["report_permission"])
                    });
                }
                listObjTree.Add(new TreeListReport
                {
                    id = i+1,
                    title = rptListType.Rows[i]["report_type"].ToString(),
                    reports = listObj.Where(x => x.report_type == rptListType.Rows[i]["report_type"].ToString()).ToList(),
                });
            }
            return listObjTree;
        }
        public async Task<bool> UpdateUserWiseReportPermission(ReportPermissionUpdate obj)
        {
            bool flag = false;
            try
            {
                var arr1 = obj.reportID;
                var arr2 = await this.GetPrimaryKeysofTable(obj.userName);
                var notInArray1 = arr2.Except(arr1);
                var getVal = notInArray1.ToArray();
                for (int j = 0; j < arr1.Length; j++)
                {
                    int repID = arr1[j];
                    SqlCommand cmd = new SqlCommand("update Dg_Pay_Report_permission set report_permission=1 where report_user='" + obj.userName + "' and report_id=" + repID, _connection);
                    cmd.CommandType = CommandType.Text;
                    await _connection.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    await _connection.CloseAsync();
                }
                for (int i = 0; i < getVal.Length; i++)
                {
                    int repID = getVal[i];
                    SqlCommand cmd1 = new SqlCommand("update Dg_Pay_Report_permission set report_permission=0 where report_user='" + obj.userName + "' and report_id=" + repID, _connection);
                    cmd1.CommandType = CommandType.Text;
                    await _connection.OpenAsync();
                    await cmd1.ExecuteNonQueryAsync();
                    await _connection.CloseAsync();
                }
                flag = true;
            }
            catch (Exception ex)
            {
                ex.ToString();
                flag = false;
            }
            finally
            {
                await _connection.CloseAsync();
            }
            return flag;
        }

        private async Task<int[]> GetPrimaryKeysofTable(string usrName)
        {
            var tblData = await _dgCommon.get_InformationDataTableAsync("select report_id from Dg_Pay_Report_permission where report_user='" + usrName + "'", _connection);
            int[] result = new int[tblData.Rows.Count];
            int i = 0;
            for (int j = 0; j < tblData.Rows.Count; j++)
            {
                result[i++] = Convert.ToInt32(tblData.Rows[j]["report_id"].ToString());
            }
            return result;
        }


        //NEW ACTION
        public async Task<List<TreeListReport>> GetTotalReportList()
        {
            List<TreeListReport> listObjTree = new List<TreeListReport>();
            List<Dg_ReportPermission> listObj = new List<Dg_ReportPermission>();
            var rptListType = await _dgCommon.get_InformationDataTableAsync("select distinct rep_cat from dg_pay_totalReportList", _connection);
            for (int i = 0; i < rptListType.Rows.Count; i++)
            {
                string reportCat = rptListType.Rows[i]["rep_cat"].ToString();
                var childData = await _dgCommon.get_InformationDataTableAsync("select * from dg_pay_totalReportList where rep_cat='" + reportCat + "'", _connection);
                foreach (DataRow row in childData.Rows)
                {
                    listObj.Add(new Dg_ReportPermission
                    {
                        report_id = Convert.ToInt32(row["rep_id"]),
                        rep_show_Sl = Convert.ToInt32(row["rep_show_Sl"]),
                        report_type = row["rep_cat"].ToString(),
                        report_name = row["rep_name"].ToString(),
                        report_url = row["rep_url"].ToString(),
                        report_for = row["rep_for"].ToString(),
                        report_IsExcel = Convert.ToBoolean(row["rep_IsExcel"])
                    });
                }
                listObjTree.Add(new TreeListReport
                {
                    id = i + 1,
                    title = rptListType.Rows[i]["rep_cat"].ToString(),
                    reports = listObj.Where(x => x.report_type == rptListType.Rows[i]["rep_cat"].ToString()).ToList(),
                });
            }
            return listObjTree;
        }
        public async Task<DataTable> GetReportUserDropdown(int compid)
        {
            var data = await _dgCommon.get_InformationDataTableAsync("select ca_serial,ca_accessuser from dg_pay_companyaccess where permission=1 and ca_compid=" + compid, _connection);
            return data;
        }
        public async Task<DataTable> GetPermissionReportByUser(int compid,string userName)
        {
            var data = await _dgCommon.get_InformationDataTableAsync("select report_type,report_name,report_url,report_IsExcel from Dg_Pay_Report_permission where report_compid=" + compid + " and report_user='"+ userName + "'", _connection);
            return data;
        }
        public async Task<DataTable> GetReportPermissionGroup(int compid)
        {
            var data = await _dgCommon.get_InformationDataTableAsync("select rep_Ugroup,rep_groupName,rep_groupCompid from dg_pay_reportGroup where rep_groupCompid="+ compid, _connection);
            return data;
        }
        public async Task<string> Save_Pay_ReportPermission(ReportPermissionPayload obj)
        {
            string message = string.Empty;
            try
            {
                if (obj.userPermissionStatus == "U")
                {
                    await _dgCommon.saveChangesAsync("delete Dg_Pay_Report_permission where report_user='" + obj.report_user + "' and report_compid=" + obj.report_compid + "", _connection);
                    foreach (var item in obj.permissionForm)
                    {
                        await _dgCommon.saveChangesAsync("Dg_Pay_ReportPermission_Save '" + item.report_type + "'," + item.report_serial + ",'" + item.report_name + "','" + item.report_url + "','" + obj.report_user + "','" + obj.userPermissionStatus + "'," + obj.report_compid + "," + obj.report_permission_code + ",'" + obj.permission_by + "','0','"+item.report_IsExcel + "'", _connection);
                    }
                    message = "User Wise Save Successfully !!";
                }
                else
                {
                    await _dgCommon.saveChangesAsync("UPDATE SpecFo.dbo.Smt_Users SET report_permission_code=" + obj.report_permission_code+" WHERE FullName='"+obj.report_user+"'", _connection);
                    message = "Group Wise Save Successfully !!";
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                message = "Something Was Wrong !!";
            }
            return message;
        }
        public async Task<DataTable> GetUserAndGroupWise_reportlist(int compid,string userName, string reportType)
        {
            var data = await _dgCommon.get_InformationDataTableAsync("user_wise_reportlist "+ compid + ",'" + userName + "','" + reportType + "'", _connection);
            return data;
        }
        public async Task<string> Save_Pay_ReportGroup(ReportPermissionGroupPayload obj)
        {
            string message = string.Empty;
            try
            {
                if (obj.permissionForm.Count >0)
                {
                    var data = await _dgCommon.get_InformationDataTableAsync("Dg_Pay_ReportPermissionGroup_Save " + obj.rep_Ugroup + ",'" + obj.rep_groupName + "'," + obj.rep_groupCompid + ",'" + obj.rep_groupCreate + "'", _connection);
                    string res = !string.IsNullOrEmpty(data.Rows[0][0].ToString())? data.Rows[0][0].ToString():string.Empty;
                    if (res != "Already Exists")
                    {
                        foreach (var item in obj.permissionForm)
                        {
                            await _dgCommon.saveChangesAsync("Dg_Pay_ReportPermission_Save '" + item.report_type + "',"+item.report_serial+",'" + item.report_name + "','" + item.report_url + "','Group','G'," + obj.rep_groupCompid + ",0,'" + obj.rep_groupCreate + "','" + obj.rep_groupName + "'", _connection);
                        }
                        message = "Submitted Successfully !!";
                    }
                    else
                    {
                        message = res;
                    }
                }
                else
                {
                    message = "You Can Not Check Any Report !!";
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                message = "Something Was Wrong !!";
            }
            return message;
        }
        public async Task<DataTable> GetPermissionReportByGroupID(int compid, int rep_groupId)
        {
            var data = await _dgCommon.get_InformationDataTableAsync("select report_type,report_name,report_url from Dg_Pay_Report_permission where permission=1 and report_compid=" + compid + " and report_permission_code="+ rep_groupId, _connection);
            return data;
        }
    }
}