using BLL.Interfaces.Repository.App;
using DAL.Data;
using EF.Core.Repository.Repository;

namespace DAL.Implementation.Repository.App
{
    public class AppRepository : CommonRepository<Object>, IAppRepository
    {
        public AppRepository(dg_hrpayrollContext context) : base(context)
        {
        }
    }
}
