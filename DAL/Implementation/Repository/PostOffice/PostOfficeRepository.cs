using BLL.Interfaces.Repository.PostOffice;
using BOL.Models;
using DAL.Data;
using EF.Core.Repository.Repository;

namespace DAL.Implementation.Repository.PostOffice
{
    public class PostOfficeRepository : CommonRepository<PostOffice_DbModel>,IPostOfficeRepository
    {
        public PostOfficeRepository(dg_hrpayrollContext context) : base(context)
        {            
        }
    }
}
