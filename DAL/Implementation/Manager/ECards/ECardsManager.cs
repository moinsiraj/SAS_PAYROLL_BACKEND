using BLL.Interfaces.Manager.ECards;
using BLL.Utility;
using BOL.Models;
using DAL.Data;
using DAL.Implementation.Repository.ECards;
using EF.Core.Repository.Manager;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Implementation.Manager.ECards
{
    public class ECardsManager : CommonManager<ECard_DbModel>, IECardsManager
    {
        private readonly Dg_Common _dgCommon;
        private readonly SqlConnection _sqlConnection;
        public ECardsManager(dg_hrpayrollContext context, Dg_Common dgCommon) : base(new ECardsRepository(context))
        {
            _dgCommon = dgCommon;
            _sqlConnection = new SqlConnection(Getway.Dg_Payroll);
        }

        public async Task<DataSet> GeECard(int comID, int emp_id, DateTime monthdate)
        {
            var data = await _dgCommon.get_InformationDtasetAsync("Ecard_Worker_C "+ comID + ","+ emp_id + ",'"+ monthdate + "'", _sqlConnection);
            return data;
        }
        public async Task<DataSet> GeECarsum(string CompId, int Empid, DateTime EDate)
        {
            var data = await _dgCommon.get_InformationDtasetAsync("E_card_Sum '"+ CompId + "',"+ Empid + ",'"+ EDate + "'", _sqlConnection);
            return data;
        }
    }
}
