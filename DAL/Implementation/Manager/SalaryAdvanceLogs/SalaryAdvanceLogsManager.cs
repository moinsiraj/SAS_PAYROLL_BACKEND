using BLL.Interfaces.Manager.SalaryAdvanceLogs;
using BLL.Utility;
using BOL.Models;
using DAL.Data;
using DAL.Implementation.Repository.SalaryAdvanceLogs;
using EF.Core.Repository.Manager;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Implementation.Manager.SalaryAdvanceLogs
{
    public class SalaryAdvanceLogsManager : CommonManager<SalaryAdvanceLog_DbModel>,ISalaryAdvanceLogsManager
    {
        private readonly Dg_Common _dgCommon;
        private readonly SqlConnection _connection;
        public SalaryAdvanceLogsManager(dg_hrpayrollContext context, Dg_Common dgCommon) :base(new SalaryAdvanceLogsRepository(context))
        {
            _dgCommon = dgCommon;
            _connection = new SqlConnection(Getway.Dg_Payroll);
        }

        public async Task<DataSet> AdvanceProcess(int SAMonth, int SAYear, int sp_groupid, int sp_compid, int days)
        {
            var data = await _dgCommon.get_InformationDtasetAsync("dg_Pay_Sal_ProcessSalaryAdvance "+ SAMonth + ","+ SAYear + ","+ sp_groupid + ","+ sp_compid + ","+ days + "", _connection);
            return data;
        }
        public async Task<DataSet> AdvanceProcess(int CompId, int month, int year)
        {
            var data = await _dgCommon.get_InformationDtasetAsync("dg_SalaryAdvance_Search "+ CompId + ","+ month + ","+ year + "", _connection);
            return data;
        }
        public async Task<DataSet> AdvanceProcessSum(int CompId, int month, int year)
        {
            var data = await _dgCommon.get_InformationDtasetAsync("dg_SalaryAdvanceSum_Search "+ CompId + ","+ month + ","+ year + "", _connection);
            return data;
        }
        
        //New Action
        public async Task<ReturnObject> SaveAdvanceProcess(DgPaySalaryAdvanceLog obj)
        {
            var result = new ReturnObject();
            var isConf = _dgCommon.get_InformationDataTable("select top 1 ss_compid from dg_pay_salary_advance where ss_compid=" + obj.SaplCompid + " and month(ss_date)=month('" + obj.SaplDate + "') and year(ss_date)=year('" + obj.SaplDate + "') and ss_confirmed=1", _connection);
            bool confExists = isConf.Rows.Count > 0 ? true : false;
            if (!confExists)
            {
                var nData = DgPaySalaryAdvanceLog.CustomToDbModel(obj);
                bool isSave = await AddAsync(nData);
                if (isSave)
                {
                    result.IsSuccess = true;
                    result.Message = "Salary Advance Process Successfully !!";
                    await _dgCommon.saveChangesAsync("dg_Pay_Sal_ProcessSalaryAdvance " + obj.SaplCompid + ",'" + obj.SaplDate + "','" + obj.SaplWithOt + "','" + obj.SaplWithTiffinBill + "','" + obj.SaplWithNightBill + "'," + obj.SaplDays + "", _connection);
                    return result;
                }
                result.Message = "Salary Advance Process Fail !!";
                return result;
            }
            result.Message = "Salary Advance Already Confirmed !!";
            return result;
        }
        public async Task<ReturnObject> SaveAdvanceConfirmed(DgPaySalaryAdvanceLog obj)
        {
            var result = new ReturnObject();
            var isConf = _dgCommon.get_InformationDataTable("select top 1 ss_compid from dg_pay_salary_advance where ss_compid=" + obj.SaplCompid + " and month(ss_date)=month('" + obj.SaplDate + "') and year(ss_date)=year('" + obj.SaplDate + "') and ss_confirmed=1", _connection);
            bool confExists = isConf.Rows.Count > 0 ? true : false;
            if (!confExists)
            {
                bool isSave = await _dgCommon.saveChangesAsync("update dg_pay_salary_advance set ss_confirmed=1 where ss_compid=" + obj.SaplCompid + " and month(ss_date)=month('" + obj.SaplDate + "') and year(ss_date)=year('" + obj.SaplDate + "')", _connection);
                if (isSave)
                {
                    result.IsSuccess = true;
                    result.Message = "Salary Advance Confirmed !!";
                    return result;
                }
                result.Message = "Salary Advance Confirmed Fail !!";
                return result;
            }
            result.Message = "No Data Found Salary Advance Confirmation !!";
            return result;
        }
        public async Task<ReturnObject> Salary_Advance_PaymentDate(PaySalaryAdvancePayment obj)
        {
            var result = new ReturnObject();
            var data = await _dgCommon.get_InformationDataTableAsync("select ss_emp_serial from dg_pay_salary_advance where ss_compid=" + obj.compid + " and Month(ss_date)=Month('" + obj.salMonth + "') and Year(ss_date)=year('" + obj.salMonth + "') and ss_confirmed=1", _connection);
            bool isConfSal = data.Rows.Count > 0 ? true : false;
            if (!isConfSal)
            {
                bool isSave = await _dgCommon.saveChangesAsync("dg_Pay_Salary_Advance_payment_date_process " + obj.compid + ",'" + obj.pDate + "','" + obj.salMonth + "'", _connection);
                if (isSave)
                {
                    result.IsSuccess= true;
                    result.Message = "Update Successfully !!";
                    return result;
                }
                result.Message = "Update Fail !!";
                return result;
            }
            result.Message = "Salaries Advance already confirmed this month(" + Convert.ToDateTime(obj.salMonth).ToString("MMMM-yyyy") + ") !!";
            return result;
        }
    }
}
