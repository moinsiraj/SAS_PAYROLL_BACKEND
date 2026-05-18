using BLL.Interfaces.Manager.AnnualLeaveAllucation;
using BLL.Utility;
using BOL.Models;
using DAL.Data;
using DAL.Implementation.Repository.AnnualLeaveAllucation;
using EF.Core.Repository.Manager;
using SharpCompress;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Implementation.Manager.AnnualLeaveAllucation
{
    public class AnnualLeaveAllucationManager : CommonManager<AnnualLeaveAllucation_DbModel>, IAnnualLeaveAllucationManager
    {
        private readonly Dg_Common _dgCommon;
        private readonly SqlConnection _connection;
        public AnnualLeaveAllucationManager(dg_hrpayrollContext context, Dg_Common dgCommon) : base(new AnnualLeaveAllucationRepository(context))
        {
            _dgCommon = dgCommon;
            _connection = new SqlConnection(Getway.Dg_Payroll);
        }

        public async Task<DataSet> GetAnnualLeave_process(int year, int casual, int medical, int annul, int comID)
        {
            var data = await _dgCommon.get_InformationDtasetAsync("Dg_Pay_Annual_Leave_generate " + year + "," + casual + "," + medical + "," + annul + "," + comID + "", _connection);
            return data;
        }
        public async Task<ReturnObject> GetAnnualLeaveEmployee(int copmid, int? deptid = null, int? secid = null)
        {
            var result = new ReturnObject();
            var dtAnLev = await _dgCommon.get_InformationDataTableAsync(string.Format("dg_pay_GetAnnual_leavegenarate {0},{1},{2}", copmid, deptid, secid), _connection);
            if (dtAnLev.Rows.Count > 0)
            {
                result.IsSuccess = true;
                result.dataTable = dtAnLev;
                return result;
            }
            result.Message = "No Data Available !!";
            return result;
        }
        public async Task<List<AnnualLeaveResponse>> AnnualLeave_process_save(AnnualLeave obj)
        {
            var result = new List<AnnualLeaveResponse>();
            if (obj.compid == 0 || string.IsNullOrEmpty(obj.compid.ToString()))
            {
                result.Add(new AnnualLeaveResponse { IsSuccess = false, Message = "Please Select Company !!" });
                return result;
            }
            if (string.IsNullOrEmpty(obj.genDate))
            {
                result.Add(new AnnualLeaveResponse { IsSuccess = false, Message = "Please Enter Leave To Generate Date !!" });
                return result;
            }
            if (obj.genType == 1)
            {
                var dtAnLev = await GetAnnualLeaveEmployee(obj.compid,0,0);
                if (dtAnLev.IsSuccess)
                {
                    var levEmployee = (DataTable)dtAnLev.dataTable;
                    levEmployee.AsEnumerable().ToList().ForEach(row =>
                    {
                        int emp_no = int.Parse(row["emp_no"].ToString());
                        bool isSave = _dgCommon.saveChanges(string.Format("Auto_Annual_leavegenarate {0},{1},'{2}','{3}'", obj.compid, emp_no, obj.genDate, obj.userName), _connection);
                        if (isSave)
                        {
                            result.Add(new AnnualLeaveResponse { IsSuccess = true, Message = "Employee No(" + emp_no + ") Leave Generate Done !!" });
                        }
                        else
                        {
                            result.Add(new AnnualLeaveResponse { IsSuccess = false, Message = "Employee No(" + emp_no + ") Leave Generate Fail !!" });
                        }
                    });
                    return result;
                }
                result.Add(new AnnualLeaveResponse { IsSuccess = false, Message = "Employee Not Found !!" });
                return result;
            }
            if (obj.employeeInfos.Count > 0)
            {
                obj.employeeInfos.ToList().ForEach(row =>
                {
                    int emp_no = row.empNo;
                    bool isSave = _dgCommon.saveChanges(string.Format("Auto_Annual_leavegenarate {0},{1},'{2}','{3}'", obj.compid, emp_no, obj.genDate, obj.userName), _connection);
                    if (isSave)
                    {
                        result.Add(new AnnualLeaveResponse { IsSuccess = true, Message = "Employee No(" + emp_no + ") Leave Generate Done !!" });
                    }
                    else
                    {
                        result.Add(new AnnualLeaveResponse { IsSuccess = false, Message = "Employee No(" + emp_no + ") Leave Generate Fail !!" });
                    }
                });
                return result;
            }
            result.Add(new AnnualLeaveResponse { IsSuccess = false, Message = "You Can't Select Any Employee !!" });
            return result;
        }
    }
}
