using BLL.Interfaces.Manager.EidBonusSetups;
using BLL.Utility;
using BOL.Models;
using DAL.Data;
using DAL.Implementation.Repository.EidBonusSetups;
using EF.Core.Repository.Manager;
using System.Data.SqlClient;

namespace DAL.Implementation.Manager.EidBonusSetups
{
    public class EidBonusSetupsManager : CommonManager<EidBonusSetup_DbModel>, IEidBonusSetupsManager
    {
        private readonly Dg_Common _dgCommon;
        private readonly SqlConnection _payCon;
        public EidBonusSetupsManager(dg_hrpayrollContext context, Dg_Common dgCommon) : base(new EidBonusSetupsRepository(context))
        {
            _dgCommon = dgCommon;
            _payCon = new SqlConnection(Getway.Dg_Payroll);
        }

        public async Task<ReturnObject> EidBonusProcess(EidBonusProcess obj)
        {
            var result = new ReturnObject();
            bool isProc = await _dgCommon.saveChangesAsync("dg_pay_Eid_Bonus_process", _payCon, obj);
            if (isProc)
            {
                result.IsSuccess = true;
                result.Message = "Eid Bonus Process Successfull !!";
            }
            else
            {
                result.Message = "Eid Bonus Already Confirmed Process Fail !!";
            }
            return result;
        }
        public async Task<ReturnObject> EidBounsConfirm(EidBonusProcess obj)
        {
            var result = new ReturnObject();
            bool isConf = await _dgCommon.saveChangesAsync("update dg_pay_eidbonus_payment set conformation=1,updatedby='" + obj.enteredby + "',updatetime=getdate() where com_code=" + obj.CompId + " and [month]=month('" + obj.proc_month + "') and [year]=year('" + obj.proc_month + "')", _payCon);
            if (isConf)
            {
                result.IsSuccess = true;
                result.Message = "Eid Bonus Confirm Successfull !!";
            }
            else
            {
                result.Message = "Eid Bonus Confirm Fail !!";
            }
            return result;
        }
    }
}
