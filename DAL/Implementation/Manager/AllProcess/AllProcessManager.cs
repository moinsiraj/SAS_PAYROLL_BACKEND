using BLL.Interfaces.Manager.AllProcess;
using BLL.Utility;
using BOL.Models;
using BOL.Models.Hubs;
using DAL.Data;
using DAL.Implementation.Repository.AllProcess;
using EF.Core.Repository.Manager;
using Microsoft.AspNetCore.SignalR;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Implementation.Manager.AllProcess
{
    public class AllProcessManager : CommonManager<AllProcess_DbModel>, IAllProcessManager
    {
        private readonly Dg_Common _dgCommon;
        private readonly IHubContext<CustomHubs> _hubContext;
        private readonly SqlConnection _sqlConnection;
        public AllProcessManager(dg_hrpayrollContext context, Dg_Common dgCommon, IHubContext<CustomHubs> hubContext) : base(new AllProcessRepository(context))
        {
            _dgCommon = dgCommon;
            _hubContext = hubContext;
            _sqlConnection = new SqlConnection(Getway.Dg_Payroll);
        }

        public async Task<DataSet> GetDeshboard(int compid)
        {
            var data = await _dgCommon.get_InformationDtasetAsync("Deshboard_employee " + compid, _sqlConnection);
            return data;
        }
        public async Task<DataSet> GetAttendance_Process(DateTime SDate, int CompID)
        {
            var data = await _dgCommon.get_InformationDtasetAsync("Attandance_Process '" + SDate + "'," + CompID, _sqlConnection);
            return data;
        }
        public async Task<DataSet> GetSalary_Process(int groupid, int compid, DateTime pDate)
        {
            var data = await _dgCommon.get_InformationDtasetAsync("Dg_Pay_Sal_SalaryProcess_Full " + groupid + "," + compid + ",'" + pDate + "'", _sqlConnection);
            return data;
        }
        public ReturnObject SalaryProcessSave(int groupid, int compid, string pDate)
        {
            var result = new ReturnObject();
            try
            {
                var dtIsExists = _dgCommon.get_InformationDataTable("SELECT ss_compid FROM dg_pay_salarysheet WHERE ss_confirmed=1 AND ss_compid="+ compid + " AND MONTH(ss_date)=MONTH('"+ Convert.ToDateTime(pDate) + "') AND YEAR(ss_date)=YEAR('"+ Convert.ToDateTime(pDate) + "')", _sqlConnection);
                bool isExists = dtIsExists.Rows.Count > 0 ? true : false;
                if (!isExists)
                {
                    bool isProcess = _dgCommon.saveChanges("Dg_Pay_Sal_SalaryProcess_Full " + groupid + "," + compid + ",'" + pDate + "'", _sqlConnection);
                    if (isProcess)
                    {
                        result.IsSuccess = true;
                        result.Message = "Salary Process Successfull This Month(" + Convert.ToDateTime(pDate).ToString("MMMM-yyyy") + ") !!";
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = "Salary Process Failed This Month(" + Convert.ToDateTime(pDate).ToString("MMMM-yyyy") + ") !!";
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = "Salary Already Confirm This Month("+Convert.ToDateTime(pDate).ToString("MMMM-yyyy") +") !!";
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
                result.IsSuccess = false;
                result.Message = "Something Went Wrong !!";
            }
            return result;
        }
        public async Task<ReturnObject> SalaryProcessSave_New(int groupid, int compid, string pDate,string userName)
        {
            var result = new ReturnObject();
            var checkProcess = await GetIsAllProcessContinue(compid, pDate, "emp_sal_process_bulk", userName);
            if (checkProcess.dataTable != null)
            {
                result.Message = "Someone Is Processing !!";
                return result;
            }
            var dtCheckAttProc = await _dgCommon.get_InformationDataTableAsync(string.Format("select procs_compid from dg_pay_Process_startOrStop where procs_compid={0} and procs_type in('att_text_upload','att_text_upload_Single','att_menual_upload') group by procs_compid", compid), _sqlConnection);
            if (dtCheckAttProc.Rows.Count > 0)
            {
                result.Message = "Attendance Upload In Progress,Please Wait... !!";
                return result;
            }
            var dtCheckOtProc = await _dgCommon.get_InformationDataTableAsync(string.Format("select procs_compid from dg_pay_Process_startOrStop where procs_compid={0} and procs_type in('att_ot_process','att_Hdot_process') group by procs_compid", compid), _sqlConnection);
            if (dtCheckOtProc.Rows.Count > 0)
            {
                result.Message = "OT Process In Progress,Please Wait... !!";
                return result;
            }

            var dtIsExists = _dgCommon.get_InformationDataTable("SELECT ss_compid FROM dg_pay_salarysheet WHERE ss_confirmed=1 AND ss_compid=" + compid + " AND MONTH(ss_date)=MONTH('" + Convert.ToDateTime(pDate) + "') AND YEAR(ss_date)=YEAR('" + Convert.ToDateTime(pDate) + "')", _sqlConnection);
            bool isExists = dtIsExists.Rows.Count > 0;
            if (!isExists)
            {
                var dtProcsEmp = await _dgCommon.get_InformationDataTableAsync(string.Format("select at_emp_serial from dg_pay_attendance where at_compid={0} and month(at_date)=month('{1}') and year(at_date)=year('{2}') group by at_emp_serial,at_compid order by at_emp_serial", compid, Convert.ToDateTime(pDate), Convert.ToDateTime(pDate)), _sqlConnection);
                if (dtProcsEmp.Rows.Count > 0)
                {
                    try
                    {
                        bool isProcess = false;
                        await _dgCommon.saveChangesAsync(string.Format("dg_pay_processStartOrStop_insert {0},'{1}',{2},'{3}','{4}'", compid, "emp_sal_process_bulk", 1, pDate, userName), _sqlConnection);
                        var tasks = new List<Task>();
                        var semaphore = new SemaphoreSlim(20);
                        for (int i = 0; i < dtProcsEmp.Rows.Count; i++)
                        {
                            var dtProcessC = await _dgCommon.get_InfoDataTableUseConStringAsync(string.Format("select procs_status from dg_pay_Process_startOrStop where procs_compid={0} and procs_date='{1}' and procs_type='emp_sal_process_bulk'", compid, pDate), Getway.Dg_Payroll);
                            isProcess = (dtProcessC.Rows.Count > 0 && !string.IsNullOrEmpty(dtProcessC.Rows[0]["procs_status"].ToString())) ? bool.Parse(dtProcessC.Rows[0]["procs_status"].ToString()) : false;
                            if (isProcess)
                            {
                                await semaphore.WaitAsync();
                                int emp_serial = !string.IsNullOrEmpty(dtProcsEmp.Rows[i]["at_emp_serial"].ToString()) ? int.Parse(dtProcsEmp.Rows[i]["at_emp_serial"].ToString()) : 0;
                                if (emp_serial > 0)
                                {
                                    var task = Task.Run(async () =>
                                    {
                                        try
                                        {
                                            await _dgCommon.saveChangesUseConStringAsync(string.Format("dg_pay_Sal_SalaryProcess {0},{1},{2},'{3}'", emp_serial, groupid, compid, pDate), Getway.Dg_Payroll);
                                        }
                                        finally
                                        {
                                            semaphore.Release();
                                        }
                                    });
                                    tasks.Add(task);
                                }
                                var res = Math.Abs(((i + 1) * 100) / dtProcsEmp.Rows.Count);
                                if (res <= 99)
                                    await _hubContext.Clients.Group(string.Concat("bSalProc_", compid, "_", userName)).SendAsync("SalBulk_ReceiveProgress", res);
                            }
                            else
                            {
                                break;
                            }
                        }
                        await Task.WhenAll(tasks);
                        await _dgCommon.saveChangesAsync(string.Format("dg_pay_Sal_SalaryProcess_summaryUP {0},'{1}'", compid, pDate), _sqlConnection);
                        if (isProcess)
                        {
                            result.IsSuccess = true;
                            result.Message = string.Format("Salary Process Successfull This Month({0}) !!", Convert.ToDateTime(pDate).ToString("MMMM-yyyy"));
                        }
                        else
                        {
                            result.Message = "Close Anther Tab Use Your UserID !!";
                        }
                    }
                    catch (Exception ex)
                    {
                        ex.ToString();
                        result.Message = "Something Went Wrong !!";
                    }
                    finally
                    {
                        await _dgCommon.saveChangesAsync(string.Format("delete from dg_pay_Process_startOrStop where procs_compid={0} and procs_date='{1}' and procs_type='emp_sal_process_bulk'", compid, pDate), _sqlConnection);
                        await _hubContext.Clients.Group(string.Concat("bSalProc_", compid, "_", userName)).SendAsync("SalBulk_ReceiveProgress", 100);
                    }
                    return result;
                }
                result.Message = string.Format("Process Employee Not Found This Month({0}) !!", Convert.ToDateTime(pDate).ToString("MMMM-yyyy"));
                return result;
            }
            result.Message = string.Format("Salary Already Confirm This Month({0}) !!", Convert.ToDateTime(pDate).ToString("MMMM-yyyy"));
            return result;
        }
        public async Task<ReturnObject> Salary_Process_Single(SingleSalaryProcess obj)
        {
            var result = new ReturnObject();           
            try
            {
                var dtCheckAttProc = await _dgCommon.get_InformationDataTableAsync(string.Format("select procs_compid from dg_pay_Process_startOrStop where procs_compid={0} and procs_type in('att_text_upload','att_text_upload_Single','att_menual_upload') group by procs_compid", obj.compid), _sqlConnection);
                if (dtCheckAttProc.Rows.Count > 0)
                {
                    result.Message = "Attendance Upload In Progress,Please Wait... !!";
                    return result;
                }
                var dtCheckOtProc = await _dgCommon.get_InformationDataTableAsync(string.Format("select procs_compid from dg_pay_Process_startOrStop where procs_compid={0} and procs_type in('att_ot_process','att_Hdot_process') group by procs_compid", obj.compid), _sqlConnection);
                if (dtCheckOtProc.Rows.Count > 0)
                {
                    result.Message = "OT Process In Progress,Please Wait... !!";
                    return result;
                }
                await _dgCommon.saveChangesAsync(string.Format("dg_pay_processStartOrStop_insert {0},'{1}',{2},'{3}','{4}'", obj.compid, "emp_sal_process_Single", 1, obj.pDate, "SystemUser"), _sqlConnection);
                foreach (var item in obj.emp_serial)
                {
                    var dtIsExists = _dgCommon.get_InformationDataTable("SELECT ss_compid FROM dg_pay_salarysheet WHERE ss_confirmed=1 AND ss_compid=" + obj.compid + " AND MONTH(ss_date)=MONTH('" + Convert.ToDateTime(obj.pDate) + "') AND YEAR(ss_date)=YEAR('" + Convert.ToDateTime(obj.pDate) + "')", _sqlConnection);
                    bool isExists = dtIsExists.Rows.Count > 0 ? true : false;
                    if (!isExists)
                    {
                        bool isProcess = _dgCommon.saveChanges("dg_Pay_Sal_SalaryProcess " + item + "," + obj.groupid + "," + obj.compid + ",'" + obj.pDate + "'", _sqlConnection);
                        if (isProcess)
                        {
                            result.IsSuccess = true;
                            result.Message = "Process Successfully !!";
                        }
                        else
                        {
                            result.IsSuccess = false;
                            result.Message = "Process Failed !!";
                        }
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = "Salary Already Confirm This Month(" + Convert.ToDateTime(obj.pDate).ToString("MMMM-yyyy") + ") !!";
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                result.Message = "Something Went Wrong !!";
            }
            finally
            {
                await _dgCommon.saveChangesAsync(string.Format("delete from dg_pay_Process_startOrStop where procs_compid={0} and procs_date='{1}' and procs_type='emp_sal_process_Single'", obj.compid, obj.pDate), _sqlConnection);
            }
            return result;
        }
        public async Task<DataSet> GetSalary_Confarmations(int com_id, int month, int year,string userName)
        {
            var data = await _dgCommon.get_InformationDtasetAsync("dg_Pay_Salary_Confirmation_process " + com_id + "," + month + "," + year + ",'" + userName + "'", _sqlConnection);
            return data;
        }
        public async Task<DataSet> GetCreate_User(string name, string EmailId, string Password, int Designation, DateTime Getdate, int CompId, int Emp_ID, string Active_status, int Emp_serial, int Compliance)
        {
            var data = await _dgCommon.get_InformationDtasetAsync("D_Create_User '" + name + "','" + EmailId + "','" + Password + "'," + Designation + ",'" + Getdate + "'," + CompId + "," + Emp_ID + ",'" + Active_status + "'," + Emp_serial + "," + Compliance + "", _sqlConnection);
            return data;
        }
        public async Task<DataTable> GetSearchUserlist()
        {
            var data = await _dgCommon.get_InformationDataTableAsync("UserList", _sqlConnection);
            return data;
        }
        public async Task<string> SalaryPaymentDate(int compid, string pDate,string salMonth)
        {
            string message = string.Empty;
            var data = await _dgCommon.get_InformationDataTableAsync("select ss_emp_serial from dg_pay_salarysheet where ss_compid=" + compid + " and Month(ss_date)=Month('"+ salMonth + "') and Year(ss_date)=year('"+ salMonth + "') and ss_confirmed=1", _sqlConnection);
            if(data.Rows.Count > 0)
            {
                message = "Salaries already confirmed this month("+Convert.ToDateTime(salMonth).ToString("MMMM-yyyy") +") ";
            }
            else
            {
                await _dgCommon.saveChangesAsync("dg_Pay_Salary_payment_date_process " + compid + ",'" + pDate + "','"+ salMonth + "'", _sqlConnection);
                message = "Update Successfully !!";
            }
            return message;
        }

        public async Task<ReturnObject> PostAllProcessClose(int compid, string procsDt, string processTP,string userName)
        {
            var result = new ReturnObject();
            if (compid > 0 && !string.IsNullOrEmpty(procsDt))
            {
                bool isUpdate = await _dgCommon.saveChangesAsync(string.Format("update dg_pay_Process_startOrStop set procs_status=0,procs_stopBy='{0}' where procs_compid={1} and procs_date='{2}' and procs_type='{3}'", userName,compid, procsDt, processTP), _sqlConnection);
                if (isUpdate)
                {
                    result.IsSuccess = isUpdate;
                    return result;
                }
                return result;
            }
            return result;
        }
        public async Task<ReturnObject> GetIsAllProcessContinue(int compid, string procsDt, string processTP,string userName)
        {
            var result = new ReturnObject();
            if (_dgCommon.get_InformationDataTable(string.Format("select procs_compid from dg_pay_Process_startOrStop where procs_compid={0} and procs_date='{1}' and procs_type='{2}' and procs_startBy='{3}'", compid, procsDt, processTP, userName),_sqlConnection).Rows.Count > 0)
                return result;

            var dtIsProcess = await _dgCommon.get_InformationDataTableAsync(string.Format("select procs_compid,procs_type,procs_status from dg_pay_Process_startOrStop where procs_compid={0} and procs_date='{1}' and procs_type='{2}'", compid, procsDt, processTP), _sqlConnection);
            if (dtIsProcess.Rows.Count > 0)
            {
                result.IsSuccess = true;
                result.dataTable = dtIsProcess;
                return result;
            }
            return result;
        }       
    }
}