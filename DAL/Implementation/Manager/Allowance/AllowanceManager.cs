using BLL.Interfaces;
using BLL.Interfaces.Manager.Allowance;
using BLL.Utility;
using BOL.Models;
using DAL.Data;
using DAL.Implementation.Repository.Allowance;
using EF.Core.Repository.Manager;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using System.Data;
using System.Data.SqlClient;
using System.Net;

namespace DAL.Implementation.Manager.Allowance
{
    public class AllowanceManager : CommonManager<Allowance_DbModel>, IAllowanceManager
    {
        private readonly Dg_Common _dgCommon;
        private readonly SqlConnection _connection;
        public AllowanceManager(dg_hrpayrollContext context, Dg_Common dgCommon) : base(new AllowanceRepository(context))
        {
            _dgCommon = dgCommon;
            _connection = new SqlConnection(Getway.Dg_Payroll);
        }

        public async Task<DataSet> GetAllowanceType(string AllowanceType)
        {
            var data = await _dgCommon.get_InformationDtasetAsync("D_Allowance '" + AllowanceType + "'", _connection);
            return data;
        }
        public async Task<DataSet> GetEmpWiseAllowance(decimal al_emp_serial,string alDate)
        {
            var data = await _dgCommon.get_InformationDtasetAsync("D_Allowance_empwise '" + al_emp_serial + "','"+ alDate + "'", _connection);
            return data;
        }
        public async Task<DataSet> GetAllowanceDateWise(int CompID, string AllowanceType, DateTime StartDate, DateTime EndDate)
        {
            var data = await _dgCommon.get_InformationDtasetAsync("D_Allowance_Search " + CompID + ",'" + AllowanceType + "','" + StartDate + "','" + EndDate + "'", _connection);
            return data;
        }
        private DataTable GetAllowanceSerialID(int emp_serial, int alID, string alDate)
        {
            var data = _dgCommon.get_InformationDataTable("select al_serial from dg_pay_allowances where al_emp_serial=" + emp_serial + " and al_id=" + alID + " and Month(al_date)=Month('" + alDate + "') and Year(al_date)=Year('" + alDate + "')", _connection);
            return data;
        }
        private DataTable GetEmployeeSalaryConf(string alDate, int compid)
        {
            var data = _dgCommon.get_InformationDataTable("SELECT ss_emp_serial FROM dg_pay_salarysheet WHERE ss_compid=" + compid + " AND MONTH(ss_date)=MONTH('" + alDate + "') AND YEAR(ss_date)=YEAR('" + alDate + "') AND ss_confirmed=1", _connection);
            return data;
        }
        public async Task<ReturnObject> SaveEmpAllowance(DgPayAllowancePostPayload obj)
        {
            var result = new ReturnObject();
            try
            {
                var dtSalaryConf = this.GetEmployeeSalaryConf(obj.AlDate, obj.AlCompid);
                bool isExistsSalConf = dtSalaryConf.Rows.Count > 0 ? true : false;
                if (!isExistsSalConf)
                {
                    if (obj.AllowanceChild.Count > 0)
                    {
                        foreach (var item in obj.AllowanceChild)
                        {
                            if (item.AlValue != 0)
                            {
                                string oAlDate = Convert.ToDateTime(obj.AlDate).ToString("yyyy-MM-dd");
                                var serialData = this.GetAllowanceSerialID(obj.AlEmpSerial, item.al_id, oAlDate);
                                int alserial = serialData.Rows.Count > 0 ? Convert.ToInt32(serialData.Rows[0]["al_serial"]) : 0;
                                var dbObj = new Allowance_DbModel
                                {
                                    al_serial = alserial,
                                    al_emp_serial = obj.AlEmpSerial,
                                    al_groupid = obj.AlGroupid,
                                    al_compid = obj.AlCompid,
                                    al_date = Convert.ToDateTime(oAlDate),
                                    al_id = item.al_id,
                                    al_value = item.AlValue,
                                    al_user = obj.AlUser,
                                    al_udate = DateTime.Now
                                };
                                if (alserial == 0)
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
                    result.Message = "Salary Already Confirmed This Month("+Convert.ToDateTime(obj.AlDate).ToString("MMMM-yyyy")+") !!";
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
