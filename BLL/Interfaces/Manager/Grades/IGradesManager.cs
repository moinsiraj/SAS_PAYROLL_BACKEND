using BOL.Models;
using EF.Core.Repository.Interface.Manager;

namespace BLL.Interfaces.Manager.Grades
{
    public interface IGradesManager : ICommonManager<Grade_DbModel>
    {
        Task<ReturnObject> AddOrEditGrade(GradePayload obj);
        Task<ReturnObject<Grade_DbModel>> GetAllGrade();
    }
}
