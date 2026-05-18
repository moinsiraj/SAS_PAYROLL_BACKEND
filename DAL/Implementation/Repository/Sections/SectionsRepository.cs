using BLL.Interfaces.Repository.Sections;
using BOL.Models;
using DAL.Data;
using EF.Core.Repository.Repository;

namespace DAL.Implementation.Repository.Sections
{
    public class SectionsRepository : CommonRepository<Section_DbModel>,ISectionsRepository
    {
        //public SectionsRepository(dg_hrpayrollContext context):base(context)
        //{           
        //}

        public SectionsRepository(dg_SpecFoContext context) : base(context)
        {
        }
    }
}
