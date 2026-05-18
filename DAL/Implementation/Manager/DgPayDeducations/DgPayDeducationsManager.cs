using BLL.Interfaces;
using BLL.Interfaces.Manager.DgPayDeducations;
using BLL.Utility;
using BOL.Models;
using DAL.Data;
using DAL.Implementation.Repository.DgPayDeducations;
using EF.Core.Repository.Manager;
using System.Data;
using System.Data.SqlClient;
using System.Net;

namespace DAL.Implementation.Manager.DgPayDeducations
{
    public class DgPayDeducationsManager : CommonManager<DgPayDeducation_DbModel>, IDgPayDeducationsManager
    {
        private readonly Dg_Common _dgCommon;
        private readonly SqlConnection _connection;
        public DgPayDeducationsManager(dg_hrpayrollContext context, Dg_Common dgCommon) : base(new DgPayDeducationsRepository(context))
        {
            _dgCommon = dgCommon;
            _connection = new SqlConnection(Getway.Dg_Payroll);
        }

        public async Task<DataSet> GetDeductionType(string DeductionType)
        {
            var data = await _dgCommon.get_InformationDtasetAsync("D_Deduction '"+ DeductionType + "'", _connection);
            return data;
        }
        public async Task<DataSet> GetEmpwiseDeduction(string al_emp_serial,string dDate)
        {
            var data = await _dgCommon.get_InformationDtasetAsync("D_Deduction_empwise '"+ al_emp_serial + "','"+ dDate + "'", _connection);
            return data;
        }
        public async Task<DataSet> GetDeductionDatewise(int CompID, string DeductionType, DateTime StartDate, DateTime EndDate)
        {
            var data = await _dgCommon.get_InformationDtasetAsync("D_Deduction_Search "+ CompID + ",'"+ DeductionType + "','"+ StartDate + "','"+ EndDate + "'", _connection);
            return data;
        }
        private DataTable GetDeductionSerialID(int emp_serial, int dID, string dDate)
        {
            var data = _dgCommon.get_InformationDataTable("select d_serial from dg_pay_deducations where d_emp_serial=" + emp_serial + " and d_id=" + dID + " and Month(d_date)=Month('" + dDate + "') and Year(d_date)=Year('" + dDate + "')", _connection);
            return data;
        }
        private DataTable GetEmployeeSalaryConf(string dDate,int compid)
        {
            var data = _dgCommon.get_InformationDataTable("SELECT ss_emp_serial FROM dg_pay_salarysheet WHERE ss_compid="+ compid + "  MONTH(ss_date)=MONTH('" + dDate + "') AND YEAR(ss_date)=YEAR('" + dDate + "') AND ss_confirmed=1", _connection);
            return data;
        }
        public async Task<ReturnObject> SaveEmpDeducation(DgPayDeducationPostPayload obj)
        {
            var result = new ReturnObject();
            try
            {
                var dtSalaryConf = this.GetEmployeeSalaryConf(obj.DDate,obj.DCompid);
                bool isExistsSalConf = dtSalaryConf.Rows.Count > 0 ? true : false;
                if (!isExistsSalConf)
                {
                    if (obj.DeducationChild.Count > 0)
                    {
                        foreach (var item in obj.DeducationChild)
                        {
                            if (item.DValue != 0)
                            {
                                string dDate = Convert.ToDateTime(obj.DDate).ToString("yyyy-MM-dd");
                                var serialData = this.GetDeductionSerialID(obj.DEmpSerial, item.DId, dDate);
                                int dserial = serialData.Rows.Count > 0 ? Convert.ToInt32(serialData.Rows[0]["d_serial"]) : 0;
                                var dbObj = new DgPayDeducation_DbModel
                                {
                                    d_serial = dserial,
                                    d_emp_serial = obj.DEmpSerial,
                                    d_groupid = obj.DGroupid,
                                    d_compid = obj.DCompid,
                                    d_date = Convert.ToDateTime(dDate),
                                    d_id = item.DId,
                                    d_value = item.DValue,
                                    d_user = obj.DUser,
                                    d_udate = DateTime.Now
                                };
                                if (dserial == 0)
                                {
                                    await AddAsync(dbObj);
                                }
                                else
                                {
                                    await UpdateAsync(dbObj);
                                }
                            }
                        }
                        result.IsSuccess = true;
                        result.Message = "Save Successfully !!";
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = "Data Save Failed !!";
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = "Salary Already Confirmed This Month(" + Convert.ToDateTime(obj.DDate).ToString("MMMM-yyyy") + ") !!";
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
    }
}