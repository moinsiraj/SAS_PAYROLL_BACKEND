using BLL.Interfaces.Manager.LeaveInfor;
using BLL.Utility;
using BOL.Models;
using DAL.Data;
using DAL.Implementation.Repository.LeaveInfor;
using EF.Core.Repository.Manager;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Implementation.Manager.LeaveInfor
{
    public class LeaveInforManager : CommonManager<LeaveInfor_DbModel>,ILeaveInforManager
    {
        private readonly Dg_Common _dgCommon;
        private readonly SqlConnection _sqlConnection;
        public LeaveInforManager(dg_hrpayrollContext context, Dg_Common dgCommon) : base(new LeaveInforRepository(context))
        {
            _dgCommon = dgCommon;
            _sqlConnection = new SqlConnection(Getway.Dg_Payroll);
        }

        public async Task<DataSet> GetCompanyName(int CompID, int EmpNO, int year)
        {
            var data = await _dgCommon.get_InformationDtasetAsync("D_leave_info "+ CompID + ","+ EmpNO + ","+ year + "", _sqlConnection);
            return data;
        }
        public async Task<DataSet> GetLeaveBalanceInfo(int EmpSerial, int year)
        {
            var data = await _dgCommon.get_InformationDtasetAsync("dg_employee_leave_information_yearwise "+ EmpSerial + ","+ year + "", _sqlConnection);
            return data;
        }
        public async Task<ReturnObject> UpdateLeaveInfo(int empSl,decimal anualBal)
        {
            var result = new ReturnObject();
            bool isUpdate = await _dgCommon.saveChangesAsync("Dg_Pay_Anual_leave_opening_balance " + empSl + "," + anualBal, _sqlConnection);
            if (isUpdate)
            {
                result.IsSuccess = true;
                result.Message = "Save Successfully !!";
                return result;
            }
            result.Message = "Save Fail !!";
            return result;
        }
    }
}
