using BLL.Interfaces.Manager.Menurights;
using BLL.Utility;
using BOL.Models;
using DAL.Data;
using DAL.Implementation.Repository.Menurights;
using EF.Core.Repository.Manager;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Implementation.Manager.Menurights
{
    public class MenurightsManager : CommonManager<Menuright_DbModel>,IMenurightsManager
    {
        private readonly Dg_Common _dgCommon;
        private readonly SqlConnection _connection;
        public MenurightsManager(dg_hrpayrollContext context, Dg_Common dgCommon) : base(new MenurightsRepository(context))
        {
            _dgCommon = dgCommon;
            _connection = new SqlConnection(Getway.Dg_Payroll);
        }

        public async Task<DataSet> GetCompanywiseuserlist(int compid)
        {
            var data = await _dgCommon.get_InformationDtasetAsync("UserList_comp_wise "+ compid, _connection);
            return data;
        }
        public async Task<DataSet> Getuserwisemenulist(string m_user)
        {
            var data = await _dgCommon.get_InformationDtasetAsync("user_wise_menulist '"+ m_user + "'", _connection);
            return data;
        }
    }
}
