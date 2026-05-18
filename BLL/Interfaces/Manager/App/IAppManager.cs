using BOL.Models;
using EF.Core.Repository.Interface.Manager;

namespace BLL.Interfaces.Manager.App
{
    public interface IAppManager : ICommonManager<Object>
    {
        Task<ReturnObject> GetAppDeshboard(int compid);
        Task<ReturnObject> GetAppHourlyManpower(string compid, string fromdate, string todate);
        Task<ReturnObject> GetAppHourlyManpower_sectionwise(int compid, string fromdate, string todate);
        Task<ReturnObject> GetAppHour_sectionwise_lyManpower_list(int compid, string fromdate, string todate, int section, int eight, int eight_pointfive, int nine, int nine_pointfive, int ten, int ten_pointfive, int eliven, int eliven_pointfive, int twelve, int AfterTwelve);
    }
}
