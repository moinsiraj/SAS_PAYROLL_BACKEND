using BLL.Interfaces.Manager.Leavetransactions;
using BLL.Utility;
using BOL.Models;
using DAL.Data;
using DAL.Implementation.Repository.Leavetransactions;
using EF.Core.Repository.Manager;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Implementation.Manager.Leavetransactions
{
    public class LeavetransactionsManager : CommonManager<Leavetransaction_DbModel>,ILeavetransactionsManager
    {
        private readonly dg_hrpayrollContext _context;
        private readonly Dg_Common _dgCommon;
        private readonly SqlConnection _sqlConnection;
        public LeavetransactionsManager(dg_hrpayrollContext context, Dg_Common dgCommon) : base(new LeavetransactionsRepository(context))
        {
            _context = context;
            _dgCommon = dgCommon;
            _sqlConnection = new SqlConnection(Getway.Dg_Payroll);
        }

        public async Task<DataSet> Getleave_info_comdatewise(int CompID, DateTime Sdate, DateTime Edate)
        {
            var data = await _dgCommon.get_InformationDtasetAsync("D_leave_info_comdatewise "+ CompID + ",'"+ Sdate + "','"+ Edate + "'", _sqlConnection);
            return data;
        }
        public async Task<DataTable> GetEmployeeNo(int compID,int levYear)
        {
            var data = await _dgCommon.get_InformationDataTableAsync("select distinct emp_serial,emp_no from dg_pay_leaveInfor inner join dg_pay_Employee on lev_emp_serial=emp_serial where lev_compid=" + compID + " and lev_year=" + levYear + " and oi_active=1", _sqlConnection);
            return data;
        }

        public async Task<ReturnObject> SaveLeaveLeavetransaction(LeaveTransactionPayload obj)
        {
            var result = new ReturnObject();
            try
            {
                var isAttn = _dgCommon.get_InformationDataTable("select top 1 at_emp_serial from dg_pay_attendance where at_date between '" + obj.fromDate + "' and '" + obj.toDate + "' and at_emp_serial=" + obj.empSerial + " and (at_intime>0 or at_outtime>0)", _sqlConnection);
                if (isAttn.Rows.Count > 0)
                {
                    result.Message = "Employee In/Out Time Already Exists This Date Range !!";
                    return result;
                }
                var queryArr = new string[] { "Dg_Pay_SaveLeaveFromDateToDate", "Dg_Pay_SaveLeaveTransaction" };
                bool isSave = await _dgCommon.saveChangesAsync(queryArr, _sqlConnection, obj);
                if (isSave)
                {
                    result.IsSuccess = true;
                    result.Message = "Save Successfully !!";
                }
                else
                {
                    result.Message = "Save Fail";
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                result.Message = "Something Went Wrong !!";
            }
            return result;
        }
        public async Task<ReturnObject> SaveLeaveLeavetransaction_Bulk(LeaveTransaction_BulkPayload obj)
        {
            var result = new ReturnObject();
            int loopCount = 0;
            try
            {
                if (obj.empSerial.Length > 0)
                {
                    foreach (var item in obj.empSerial)
                    {
                        var payload = LeaveTransaction_BulkPayload.Finalobj(obj, item);
                        bool isSave = await _dgCommon.saveChangesAsync("Dg_Pay_SaveLeaveTransaction_Bulk", _sqlConnection, payload);
                        if (isSave)
                        {
                            loopCount++;
                        }
                    }

                    if (loopCount > 0)
                    {
                        result.IsSuccess = true;
                        result.Message = "Save Successfully !!";
                        return result;
                    }
                    result.Message = "Save Fail";
                    return result;
                }
                result.Message = "Select Employee First !!";
                return result;
            }
            catch (Exception ex)
            {
                ex.ToString();
                result.Message = "Something Went Wrong !!";
            }
            return result;
        }
        public async Task<bool> deleteLevTrans(int id, string userName)
        {
            bool delete = false;
            await _sqlConnection.OpenAsync();
            try
            {
                delete = await _dgCommon.saveChangesAsync($"Dg_Pay_DeleteLeavHistory_FromDateToDate {id},'{userName}'", _sqlConnection);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            finally
            {
                await _sqlConnection.CloseAsync();
            }
            return delete;
        }


        //Online Leave Transaction
        public async Task<ReturnObject> GetRecommenderAndDPTPerson(string prefixText)
        {
            var result = new ReturnObject();
            var dt = await _dgCommon.get_InformationDataTableAsync(string.Format("Dg_Pay_Sp_OnlineLeaveRecommenderPerson '{0}'", prefixText), _sqlConnection);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    int compid = int.Parse(row["compid"].ToString());
                    int empno = int.Parse(row["emp_no"].ToString());
                    string imagePath = string.Format("http://103.125.255.105/payroll_service/EmployeeImage/{0}/{1}.jpg", compid, empno);
                    using (HttpClient client = new HttpClient())
                    {
                        var response = await client.GetAsync(imagePath);
                        row["empImage"] = response.IsSuccessStatusCode ? imagePath : string.Empty;
                    }
                }
                result.IsSuccess = true;
                result.dataTable = dt;
                return result;
            }
            result.Message = "No Data Available !!";
            return result;
        }
        public async Task<ReturnObject> GetEmployeeSlByCompEmp_online(int compid, int empno)
        {
            var result = new ReturnObject();
            var dt = await _dgCommon.get_InformationDataTableAsync(string.Format("select emp_serial from dg_pay_Employee where compid={0} and emp_no={1}", compid, empno), _sqlConnection);
            if (dt.Rows.Count > 0)
            {
                result.IsSuccess = true;
                result.dataTable = new { empSerial = int.Parse(dt.Rows[0]["emp_serial"].ToString()) };
                return result;
            }
            result.Message = "No Data Available !!";
            return result;
        }
        public async Task<ReturnObject> GetEmployeeLeaveOnlineBalance(int empSerial, int year)
        {
            var result = new ReturnObject();
            var dt = await _dgCommon.get_InformationDataTableAsync(string.Format("dg_employee_leaveBalance_information_online {0},{1}", empSerial, year), _sqlConnection);
            if (dt.Rows.Count > 0)
            {
                result.IsSuccess = true;
                result.dataTable = dt;
                return result;
            }
            result.Message = "No Data Available !!";
            return result;
        }
        public async Task<ReturnObject> SaveEmployeeLeave_online(LeaveTransactionOnlinePayload obj)
        {
            var result = new ReturnObject();
            if (!isEmpFdateTdateMain(obj.levOn_compid,obj.levOn_emp_serial,obj.levOn_from_date,obj.levOn_to_date) && !isEmpFdateTdateOnline(obj.levOn_compid, obj.levOn_emp_serial, obj.levOn_from_date, obj.levOn_to_date))
            {
                bool isSave = await _dgCommon.saveChangesAsync("Dg_Pay_Sp_SaveEmployeeLeaveOnline", _sqlConnection, obj);
                if (isSave)
                {
                    result.IsSuccess = isSave;
                    result.Message = "Employee Leave Save Successfully !!";
                    var leaveInfo = await GetEmployeeLeaveAddInfo_online(obj.levOn_compid,obj.userName);
                    result.dataTable = leaveInfo.dataTable;
                    return result;
                }
                result.Message = "Employee Leave Save Fail !!";
                return result;
            }
            result.Message = "Leave Already Exists !!";
            return result;
        }
        public async Task<ReturnObject> GetEmployeeLeaveAddInfo_online(int compid, string userName)
        {
            var result = new ReturnObject();
            var dt = await _dgCommon.get_InformationDataTableAsync(string.Format("Dg_Pay_Sp_GetEmployeeLeaveOnlineAddInfo {0},'{1}'", compid, userName), _sqlConnection);
            if (dt.Rows.Count > 0)
            {
                result.IsSuccess = true;
                result.dataTable = dt;
                return result;
            }
            result.Message = "No Data Available !!";
            return result;
        }
        public async Task<ReturnObject> DeleteEmployeeLeave_online(int compid, int leaveID, string userName)
        {
            var result = new ReturnObject();
            if (leaveID > 0)
            {
                bool isDelete = await _dgCommon.saveChangesAsync(string.Format("delete from dg_pay_leave_info_forfromdatetodate_online where levOn_SL={0} and levOn_hr_emp=0", leaveID), _sqlConnection);
                if (isDelete)
                {
                    result.IsSuccess = isDelete;
                    result.Message = "Employee Leave Delete Successfully !!";
                    var leaveInfo = await GetEmployeeLeaveAddInfo_online(compid, userName);
                    result.dataTable = leaveInfo.dataTable;
                    return result;
                }
                result.Message = "Employee Leave Delete Fail !!";
                return result;
            }
            result.Message = "Delete ID Not Found !!";
            return result;
        }

        // Approval
        public async Task<ReturnObject> GetLeaveTransferPersonList(int transEmpSerial)
        {
            var result = new ReturnObject();
            var dt = await _dgCommon.get_InformationDataTableAsync(string.Format("Dg_Pay_Sp_GetEmployeeLeaveOnline_transEmpList {0}", transEmpSerial), _sqlConnection);
            if (dt.Rows.Count > 0)
            {
                result.IsSuccess = true;
                result.dataTable = dt;
                return result;
            }
            result.Message = "No Data Available !!";
            return result;
        }
        public async Task<ReturnObject> UpdateLeaveTransferPerson(LeaveOnlineApp obj)
        {
            var result = new ReturnObject();
            bool isUpdate = false;
            if (obj.leaveID.Length > 0)
            {
                for (int i = 0; i < obj.leaveID.Length; i++)
                {
                    isUpdate = await _dgCommon.saveChangesAsync(string.Format("update dg_pay_leave_info_forfromdatetodate_online set levOn_trans_emp=1,levOn_trans_empName='{0}',levOn_trans_empDt=getdate() where levOn_SL={1}", obj.userName, obj.leaveID[i]), _sqlConnection);
                }               
                if (isUpdate)
                {
                    result.IsSuccess = isUpdate;
                    result.Message = "Update Successfully !!";
                    var data = await GetLeaveTransferPersonList(obj.empSerial);
                    result.dataTable = data.dataTable;
                    return result;
                }
                result.Message = "Update Fail !!";
                return result;
            }
            result.Message = "Leave ID Not Found !!";
            return result;
        }
        public async Task<ReturnObject> GetLeaveRecommenderPersonList(int transEmpSerial)
        {
            var result = new ReturnObject();
            var dt = await _dgCommon.get_InformationDataTableAsync(string.Format("Dg_Pay_Sp_GetEmployeeLeaveOnline_recommenderEmpList {0}", transEmpSerial), _sqlConnection);
            if (dt.Rows.Count > 0)
            {
                result.IsSuccess = true;
                result.dataTable = dt;
                return result;
            }
            result.Message = "No Data Available !!";
            return result;
        }
        public async Task<ReturnObject> UpdateLeaveRecommenderPerson(LeaveOnlineApp obj)
        {
            var result = new ReturnObject();
            bool isUpdate = false;
            if (obj.leaveID.Length > 0)
            {
                for (int i = 0; i < obj.leaveID.Length; i++)
                {
                    isUpdate = await _dgCommon.saveChangesAsync(string.Format("update dg_pay_leave_info_forfromdatetodate_online set levOn_recom_emp=1,levOn_recom_empName='{0}',levOn_recom_empDt=getdate() where levOn_SL={1}", obj.userName, obj.leaveID[i]), _sqlConnection);
                }                
                if (isUpdate)
                {
                    result.IsSuccess = isUpdate;
                    result.Message = "Update Successfully !!";
                    var data = await GetLeaveRecommenderPersonList(obj.empSerial);
                    result.dataTable = data.dataTable;
                    return result;
                }
                result.Message = "Update Fail !!";
                return result;
            }
            result.Message = "Leave ID Not Found !!";
            return result;
        }
        public async Task<ReturnObject> GetLeaveDeptHeadPersonList(int transEmpSerial)
        {
            var result = new ReturnObject();
            var dt = await _dgCommon.get_InformationDataTableAsync(string.Format("Dg_Pay_Sp_GetEmployeeLeaveOnline_deptHeadEmpList {0}", transEmpSerial), _sqlConnection);
            if (dt.Rows.Count > 0)
            {
                result.IsSuccess = true;
                result.dataTable = dt;
                return result;
            }
            result.Message = "No Data Available !!";
            return result;
        }
        public async Task<ReturnObject> UpdateLeaveDeptHeadPerson(LeaveOnlineApp obj)
        {
            var result = new ReturnObject();
            bool isUpdate = false;
            if (obj.leaveID.Length > 0)
            {
                for (int i = 0; i < obj.leaveID.Length; i++)
                {
                    isUpdate = await _dgCommon.saveChangesAsync(string.Format("update dg_pay_leave_info_forfromdatetodate_online set levOn_dept_emp=1,levOn_dept_empName='{0}',levOn_dept_empDt=getdate() where levOn_SL={1}", obj.userName, obj.leaveID[i]), _sqlConnection);
                }                
                if (isUpdate)
                {
                    result.IsSuccess = isUpdate;
                    result.Message = "Update Successfully !!";
                    var data = await GetLeaveDeptHeadPersonList(obj.empSerial);
                    result.dataTable = data.dataTable;
                    return result;
                }
                result.Message = "Update Fail !!";
                return result;
            }
            result.Message = "Leave ID Not Found !!";
            return result;
        }
        public async Task<ReturnObject> GetLeaveHrList(string userName)
        {
            var result = new ReturnObject();
            var dt = await _dgCommon.get_InformationDataTableAsync(string.Format("Dg_Pay_Sp_GetEmployeeLeaveOnline_hrList '{0}'", userName), _sqlConnection);
            if (dt.Rows.Count > 0)
            {
                result.IsSuccess = true;
                result.dataTable = dt;
                return result;
            }
            result.Message = "No Data Available !!";
            return result;
        }
        public async Task<ReturnObject> UpdateLeaveHrPerson(LeaveOnlineApp obj)
        {
            var result = new ReturnObject();
            bool isUpdate = false;
            if (obj.leaveID.Length > 0)
            {
                for (int i = 0; i < obj.leaveID.Length; i++)
                {
                    isUpdate = await _dgCommon.saveChangesAsync(string.Format("update dg_pay_leave_info_forfromdatetodate_online set levOn_hr_emp=1,levOn_hr_empName='{0}',levOn_hr_empSerial={1},levOn_hr_empDt=getdate() where levOn_SL={2}", obj.userName, obj.empSerial, obj.leaveID[i]), _sqlConnection);
                }               
                if (isUpdate)
                {
                    result.IsSuccess = isUpdate;
                    result.Message = "Update Successfully !!";
                    var data = await GetLeaveHrList(obj.userName);
                    result.dataTable = data.dataTable;
                    return result;
                }
                result.Message = "Update Fail !!";
                return result;
            }
            result.Message = "Leave ID Not Found !!";
            return result;
        }
        public async Task<ReturnObject> GetLeaveAppList(string userName)
        {
            var result = new ReturnObject();
            var dt = await _dgCommon.get_InformationDataTableAsync(string.Format("Dg_Pay_Sp_GetEmployeeLeaveOnline_appList '{0}'", userName), _sqlConnection);
            if (dt.Rows.Count > 0)
            {
                result.IsSuccess = true;
                result.dataTable = dt;
                return result;
            }
            result.Message = "No Data Available !!";
            return result;
        }
        public async Task<ReturnObject> UpdateLeaveAppPerson(LeaveOnlineApp obj)
        {
            var result = new ReturnObject();
            bool isUpdate = false;
            if (obj.leaveID.Length > 0)
            {
                for (int i = 0; i < obj.leaveID.Length; i++)
                {
                    isUpdate = await _dgCommon.saveChangesAsync(string.Format("Dg_Pay_Sp_UpdateLeaveApp_online {0},{1},'{2}'", obj.leaveID[i], obj.empSerial, obj.userName), _sqlConnection);
                }                
                if (isUpdate)
                {
                    result.IsSuccess = isUpdate;
                    result.Message = "Update Successfully !!";
                    var data = await GetLeaveAppList(obj.userName);
                    result.dataTable = data.dataTable;
                    return result;
                }
                result.Message = "Update Fail !!";
                return result;
            }
            result.Message = "Leave ID Not Found !!";
            return result;
        }
        public async Task<ReturnObject> GetLeaveAppListAll(string userName)
        {
            var result = new ReturnObject();
            var dt = await _dgCommon.get_InformationDataTableAsync(string.Format("Dg_Pay_Sp_GetEmployeeLeaveOnline_appListALL '{0}'", userName), _sqlConnection);
            if (dt.Rows.Count > 0)
            {
                result.IsSuccess = true;
                result.dataTable = dt;
                return result;
            }
            result.Message = "No Data Available !!";
            return result;
        }

        private bool isEmpFdateTdateMain(int compid, int empSerial, string fDate, string tDate)
        {
            var dt = _dgCommon.get_InformationDataTable(string.Format("select lev_emp_serial from dg_pay_leave_info_forfromdatetodate where lev_compid={0} and lev_emp_serial={1} and lev_from_date='{2}' and lev_to_date='{3}'", compid, empSerial, fDate, tDate), _sqlConnection);
            bool flag = dt.Rows.Count > 0;
            return flag;
        }
        private bool isEmpFdateTdateOnline(int compid, int empSerial, string fDate, string tDate)
        {
            var dt = _dgCommon.get_InformationDataTable(string.Format("select levOn_emp_serial from dg_pay_leave_info_forfromdatetodate_online where levOn_compid={0} and levOn_emp_serial={1} and levOn_from_date='{2}' and levOn_to_date='{3}'", compid, empSerial, fDate, tDate), _sqlConnection);
            bool flag = dt.Rows.Count > 0;
            return flag;
        }
    }
}
