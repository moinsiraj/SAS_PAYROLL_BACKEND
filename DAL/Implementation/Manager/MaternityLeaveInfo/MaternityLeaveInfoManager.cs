using BLL.Interfaces.Manager.MaternityLeaveInfo;
using BLL.Utility;
using BOL.Models;
using DAL.Data;
using DAL.Implementation.Repository.MaternityLeaveInfo;
using EF.Core.Repository.Manager;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Implementation.Manager.MaternityLeaveInfo
{
    public class MaternityLeaveInfoManager : CommonManager<MaternityLeaveInfo_DbModel>,IMaternityLeaveInfoManager
    {
        private readonly Dg_Common _dgCommon;
        private readonly SqlConnection _connection;
        public MaternityLeaveInfoManager(dg_hrpayrollContext context, Dg_Common dgCommon) : base(new MaternityLeaveInfoRepository(context))
        {
            _dgCommon = dgCommon;
            _connection = new SqlConnection(Getway.Dg_Payroll);
        }

        public async Task<DataSet> GetMaternityLeaveInfo(int CompID, DateTime Startdate, DateTime Enddate)
        {
            var data = await _dgCommon.get_InformationDtasetAsync("D_Maternityleave_SdateToEdate "+ CompID + ",'"+ Startdate + "','"+ Enddate + "'", _connection);
            return data;
        }
        public async Task<ReturnObject> SaveMaternityLeaveInfo(DgPayMaternityLeaveInfo obj)
        {
            var result = new ReturnObject();
            try
            {
                var dtIsexists = _dgCommon.get_InformationDataTable("SELECT dg_pay_Employee.emp_no FROM dg_Pay_Maternity_leave_info inner join dg_pay_Employee on dg_Pay_Maternity_leave_info.emp_serial=dg_pay_Employee.emp_serial WHERE dg_Pay_Maternity_leave_info.emp_serial="+obj.EmpSerial+" AND YEAR(lev_start_date)=YEAR('"+ Convert.ToDateTime(obj.LevStartDate).Year + "')", _connection);
                bool isExists = dtIsexists.Rows.Count > 0 ? true : false;
                if (!isExists)
                {
                    bool isSave = await _dgCommon.saveChangesAsync("Dg_Pay_SaveMaternityLeaveInfo", _connection, obj);
                    if (isSave)
                    {
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
                    result.Message = "Employee("+ dtIsexists.Rows[0]["emp_no"] + ") Maternity Leave Already Exists This Year(" + Convert.ToDateTime(obj.LevStartDate).Year + ") !!";
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
        public async Task<ReturnObject> GetMaternityLeaveEmpByCompany(int companyID,string formDate,string toDate)
        {
            var result = new ReturnObject();
            try
            {
                var dtMaternity = await _dgCommon.get_InformationDataTableAsync("dg_pay_FemaleEmp_for_maternityLeave "+ companyID + ",'"+ formDate + "','"+ toDate + "'", _connection);
                if (dtMaternity.Rows.Count > 0)
                {
                    result.IsSuccess = true;
                    result.Message = "Data Loaded !!";
                    result.dataTable = dtMaternity;
                }
                else
                {
                    result.Message = "Data Not Found !!";
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return result;
        }
        public ReturnObject UpdateArrowRight(MaternityPaymentModification obj)
        {
            var result = new ReturnObject();
            try
            {
                var empSlArr = new List<int>();
                foreach (var item in obj.employeeSL)
                {                    
                    empSlArr.Add(item);
                }
                string empSlIds = "emp_serial in(" + string.Join(",", empSlArr) + ")";
                bool isUpdate = _dgCommon.saveChanges("dg_pay_UpdateFemaleEmp_for_maternityPayment '" + empSlIds + "','" + obj.clickTo + "'", _connection);
                if (isUpdate == true)
                {
                    result.IsSuccess = true;
                    result.Message = "Maternity Not Payment Add !!";
                    result.dataTable = _dgCommon.get_InformationDataTable("dg_pay_FemaleEmp_for_maternityLeave " + obj.companyID + ",'" + obj.formDate + "','" + obj.toDate + "'", _connection);
                }
                else
                {
                    result.Message = "Maternity Not Payment Not Add !!";
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                result.Message = "Something Went Wrong !!";
            }
            return result;
        }
        public ReturnObject UpdateArrowLeft(MaternityPaymentModification obj)
        {
            var result = new ReturnObject();
            try
            {
                var empSlArr = new List<int>();
                foreach (var item in obj.employeeSL)
                {
                    empSlArr.Add(item);
                }
                string empSlIds = "emp_serial in(" + string.Join(",", empSlArr) + ")";
                bool isUpdate = _dgCommon.saveChanges("dg_pay_UpdateFemaleEmp_for_maternityPayment '" + empSlIds + "','" + obj.clickTo + "'", _connection);
                if (isUpdate == true)
                {
                    result.IsSuccess = true;
                    result.Message = "Maternity Not Payment Remove !!";
                    result.dataTable = _dgCommon.get_InformationDataTable("dg_pay_FemaleEmp_for_maternityLeave " + obj.companyID + ",'" + obj.formDate + "','" + obj.toDate + "'", _connection);
                }
                else
                {
                    result.Message = "Maternity Not Payment Can Not Remove !!";
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                result.Message = "Something Went Wrong !!";
            }
            return result;
        }
    }
}
