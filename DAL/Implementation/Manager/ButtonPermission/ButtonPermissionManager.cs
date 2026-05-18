using BLL.Interfaces.Manager.ButtonPermission;
using BLL.Utility;
using BOL.Models;
using DAL.Data;
using DAL.Implementation.Repository.ButtonPermission;
using EF.Core.Repository.Manager;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Implementation.Manager.ButtonPermission
{
    public class ButtonPermissionManager : CommonManager<ButtonPermission_DbModel>,IButtonPermissionManager
    {
        private readonly Dg_Common _dgCommon;
        private readonly SqlConnection _sqlConnection;
        public ButtonPermissionManager(dg_hrpayrollContext context, Dg_Common dgCommon) : base( new ButtonPermissionRepository(context))
        {
            _dgCommon = dgCommon;
            _sqlConnection = new SqlConnection(Getway.Dg_Payroll);
        }

        public async Task<DataSet> GetUserWiseButtonList(string b_user)
        {
            var data = await _dgCommon.get_InformationDtasetAsync("user_wise_buttonlist '"+ b_user + "'", _sqlConnection);
            return data;
        }
    }
}
