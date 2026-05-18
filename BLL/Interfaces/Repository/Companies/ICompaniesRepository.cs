using BOL.Models;
using EF.Core.Repository.Interface.Repository;

namespace BLL.Interfaces.Repository.Companies
{
    public interface ICompaniesRepository : ICommonRepository<Company_DbModel>
    {
    }
}
