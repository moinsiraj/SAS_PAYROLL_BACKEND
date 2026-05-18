using BLL.Interfaces.Manager.App;
using BLL.Utility;
using BOL.Models;
using DAL.Data;
using DAL.Implementation.Repository.App;
using EF.Core.Repository.Manager;
using System.Data.SqlClient;

namespace DAL.Implementation.Manager.App
{
    public class AppManager : CommonManager<Object>, IAppManager
    {
        private readonly Dg_Common _dgCommon;
        private readonly SqlConnection _sqlConnection;
        public AppManager(dg_hrpayrollContext context, Dg_Common dgCommon) : base(new AppRepository(context))
        {
            _dgCommon = dgCommon;
            _sqlConnection = new SqlConnection(Getway.Dg_Payroll);
        }

        public async Task<ReturnObject> GetAppDeshboard(int compid)
        {
            var result = new ReturnObject();
            var data = await _dgCommon.get_InformationDataTableAsync(string.Format("App_Deshboard_employee {0}", compid), _sqlConnection);
            if (data.Rows.Count > 0)
            {
                result.IsSuccess = true;
                result.dataTable = _dgCommon.GetSingleListObject<AppModel>(data);
                return result;
            }
            result.Message = "No Data Available !!";
            return result;
        }
        public async Task<ReturnObject> GetAppHourlyManpower(string compid, string fromdate, string todate)
        {
            var result = new ReturnObject();
            var data = await _dgCommon.get_InformationDataTableAsync(string.Format("dg_pay_App_hour_wise_manpower_d2d '{0}','{1}','{2}'", compid, fromdate, todate), _sqlConnection);
            if (data.Rows.Count > 0)
            {
                result.IsSuccess = true;
                result.dataTable = data;
                return result;
            }
            result.Message = "No Data Available !!";
            return result;
        }
        public async Task<ReturnObject> GetAppHourlyManpower_sectionwise(int compid, string fromdate, string todate)
        {
            var result = new ReturnObject();
            var data = await _dgCommon.get_InformationDataTableAsync(string.Format("dg_pay_app_hourly_sectionwise_manpower_d2d '{0}','{1}','{2}'", compid, fromdate, todate), _sqlConnection);
            if (data.Rows.Count > 0)
            {
                result.IsSuccess = true;
                result.dataTable = data;
                return result;
            }
            result.Message = "No Data Available !!";
            return result;
        }
        public async Task<ReturnObject> GetAppHour_sectionwise_lyManpower_list(int compid, string fromdate, string todate, int section, int eight, int eight_pointfive, int nine, int nine_pointfive, int ten, int ten_pointfive, int eliven, int eliven_pointfive, int twelve, int AfterTwelve)
        {
            var result = new ReturnObject();
            var data = await _dgCommon.get_InformationDataTableAsync(string.Format("dg_pay_app_hourly_sectionwise_manpower_list_d2d {0},'{1}','{2}',{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13}", compid, fromdate, todate, section, eight, eight_pointfive, nine, nine_pointfive, ten, ten_pointfive, eliven, eliven_pointfive, twelve, AfterTwelve), _sqlConnection);
            if (data.Rows.Count > 0)
            {
                result.IsSuccess = true;
                result.dataTable = data;
                return result;
            }
            result.Message = "No Data Available !!";
            return result;
        }
    }
}
