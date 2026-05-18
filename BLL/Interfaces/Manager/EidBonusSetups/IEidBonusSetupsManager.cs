using BOL.Models;
using EF.Core.Repository.Interface.Manager;

namespace BLL.Interfaces.Manager.EidBonusSetups
{
    public interface IEidBonusSetupsManager : ICommonManager<EidBonusSetup_DbModel>
    {
        Task<ReturnObject> EidBonusProcess(EidBonusProcess obj);
        Task<ReturnObject> EidBounsConfirm(EidBonusProcess obj);
    }
}
