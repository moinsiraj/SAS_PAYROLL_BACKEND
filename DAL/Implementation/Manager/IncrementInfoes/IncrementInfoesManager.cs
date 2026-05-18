using BLL.Interfaces.Manager.IncrementInfoes;
using BLL.Utility;
using BOL.Models;
using DAL.Data;
using DAL.Implementation.Repository.IncrementInfoes;
using EF.Core.Repository.Manager;
using System;
using System.Data;
using System.Data.SqlClient;
using static BOL.Models.DgEmpIncrementInfo;

namespace DAL.Implementation.Manager.IncrementInfoes
{
    public class IncrementInfoesManager : CommonManager<IncrementInfo_DbModel>, IIncrementInfoesManager
    {
        private readonly Dg_Common _dgCommon;
        private readonly SqlConnection _connection;
        public IncrementInfoesManager(dg_hrpayrollContext context, Dg_Common dgCommon) : base(new IncrementInfoesRepository(context))
        {
            _dgCommon = dgCommon;
            _connection = new SqlConnection(Getway.Dg_Payroll);
        }

        public async Task<DataSet> Increment_list(int Compid)
        {
            var data = await _dgCommon.get_InformationDtasetAsync("Dg_increment_list "+ Compid, _connection);
            return data;
        }
        public async Task<DataSet> Increment_Batch_list(int com_code, string inc_type, int dependon,
            decimal inc_gross, decimal inc_basic, decimal inc_grossPrct, decimal inc_BasicPrct,
            DateTime date, string uid, DateTime cutofdate)
        {
            var data = await _dgCommon.get_InformationDtasetAsync("Increment_Batch "+ com_code + ",'"+ inc_type + "',"+ dependon + ","+ inc_gross + ","+ inc_basic + ","+ inc_grossPrct + ","+ inc_BasicPrct + ",'"+ date + "','"+ uid + "','"+ cutofdate + "'", _connection);
            return data;
        }

        // New Action 11/30/2023
        public async Task<ReturnObject> SaveEmployeeIncrementInfo(EmployeeIncrementPayload obj)
        {
            var result = new ReturnObject();
            try
            {
                var dtIsExistsSalConf = _dgCommon.get_InformationDataTable("SELECT ss_emp_serial FROM dg_pay_salarysheet WHERE ss_compid="+obj.companyID+" and MONTH(ss_date)=MONTH('" + obj.cutOfDate + "') AND YEAR(ss_date)=YEAR('"+ obj.cutOfDate + "') AND ss_confirmed=1", _connection);
                bool isConfSal = dtIsExistsSalConf.Rows.Count > 0 ? true : false;
                if (!isConfSal)
                {
                    bool isSave = await _dgCommon.saveChangesAsync("Dg_Pay_SaveEmployeeIncrementInfo", _connection,obj);
                    if (isSave)
                    {
                        result.IsSuccess = true;
                        result.Message = "Save Successfully !!";
                    }
                    else
                    {
                        result.Message = "Save Fail !!";
                    }
                }
                else
                {
                    result.Message = "Salary Already Confirm This Month(" + Convert.ToDateTime(obj.cutOfDate).ToString("MMMM-yyyy") + ") !!";
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                result.Message = "Something went wrong !!";
            }
            return result;
        }
        public async Task<DataTable> GetEmpIncrementInfoList(int compid)
        {
            var data = await _dgCommon.get_InformationDataTableAsync("Dg_Pay_EmpIncrementInfoList "+ compid, _connection);
            return data;
        }
        public async Task<string> SaveEmpIncrementApprove(List<EmpIncrementApprovePayload> obj)
        {
            string message=string.Empty;
            try
            {
                if (obj.Count > 0)
                {
                    foreach (var item in obj)
                    {
                        await _dgCommon.saveChangesAsync("Dg_Pay_EmpIncrementApproveSave "+ item.incID + ",'"+ item.userName + "'", _connection);
                    }
                    message = "Approved Successfully !!";
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                message = "Something went wrong !!";
            }
            return message;
        }
        public async Task<string> DeleteEmpIncrement(List<EmpIncrementApprovePayload> obj)
        {
            string message=string.Empty;
            try
            {
                if (obj.Count > 0)
                {
                    foreach (var item in obj)
                    {
                        await _dgCommon.saveChangesAsync("DELETE dg_emp_increment_info WHERE inc_id="+ item.incID, _connection);
                    }
                    message = "Delete Successfully !!";
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                message = "Something went wrong !!";
            }
            return message;
        }
    }
}
