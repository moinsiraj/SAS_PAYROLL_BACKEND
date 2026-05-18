using BLL.Interfaces.Manager.PostOffice;
using BOL.Models;
using DAL.Data;
using DAL.Implementation.Repository.PostOffice;
using EF.Core.Repository.Manager;

namespace DAL.Implementation.Manager.PostOffice
{
    public class PostOfficeManager : CommonManager<PostOffice_DbModel>,IPostOfficeManager
    {
        public PostOfficeManager(dg_hrpayrollContext context) : base(new PostOfficeRepository(context))
        {            
        }
    }
}
