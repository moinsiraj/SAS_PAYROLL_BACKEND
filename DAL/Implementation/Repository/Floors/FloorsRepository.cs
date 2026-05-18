using BLL.Interfaces.Repository.Floors;
using BOL.Models;
using DAL.Data;
using EF.Core.Repository.Repository;

namespace DAL.Implementation.Repository.Floors
{
    public class FloorsRepository : CommonRepository<Floor_DbModel>,IFloorsRepository
    {
        public FloorsRepository(dg_SpecFoContext context) : base(context)
        {           
        }
    }
}
