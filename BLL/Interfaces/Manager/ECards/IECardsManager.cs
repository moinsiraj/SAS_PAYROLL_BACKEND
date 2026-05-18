using BOL.Models;
using EF.Core.Repository.Interface.Manager;
using System.Data;

namespace BLL.Interfaces.Manager.ECards
{
    public interface IECardsManager : ICommonManager<ECard_DbModel>
    {
        Task<DataSet> GeECard(int comID, int emp_id, DateTime monthdate);
        Task<DataSet> GeECarsum(string CompId, int Empid, DateTime EDate);
    }
}
