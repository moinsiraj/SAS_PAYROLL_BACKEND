using BOL.Models;
using EF.Core.Repository.Interface.Manager;
using Microsoft.AspNetCore.Http;

namespace BLL.Interfaces.Manager.Experince
{
    public interface IExperinceManager : ICommonManager<EmpExperince>
    {
        Task<EmpExperinceView> GetExpcSingel(int EmpNo);
        Task<bool> UpdateExpcInfo(EmpExperince obj);        
    }
}
