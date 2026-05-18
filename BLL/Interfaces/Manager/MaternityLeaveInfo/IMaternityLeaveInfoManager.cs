using BOL.Models;
using EF.Core.Repository.Interface.Manager;
using System.Data;

namespace BLL.Interfaces.Manager.MaternityLeaveInfo
{
    public interface IMaternityLeaveInfoManager : ICommonManager<MaternityLeaveInfo_DbModel>
    {
        Task<DataSet> GetMaternityLeaveInfo(int CompID, DateTime Startdate, DateTime Enddate);
        Task<ReturnObject> SaveMaternityLeaveInfo(DgPayMaternityLeaveInfo obj);
        Task<ReturnObject> GetMaternityLeaveEmpByCompany(int companyID, string formDate, string toDate);
        ReturnObject UpdateArrowRight(MaternityPaymentModification obj);
        ReturnObject UpdateArrowLeft(MaternityPaymentModification obj);
    }
}
