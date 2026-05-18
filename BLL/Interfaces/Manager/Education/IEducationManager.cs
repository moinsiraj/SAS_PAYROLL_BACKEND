using BOL.Models;
using EF.Core.Repository.Interface.Manager;

namespace BLL.Interfaces.Manager.Education
{
    public interface IEducationManager : ICommonManager<EmpEducation>
    {
        Task<EmpEducation> GetEduSingel(int EmpNo);
        Task<bool> UpdateEduInfo(EmpEducation obj);
    }
}
