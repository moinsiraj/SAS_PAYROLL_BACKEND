using BLL.Interfaces.Repository.BuildingUnit;
using BOL.Models;
using DAL.Data;
using EF.Core.Repository.Repository;

namespace DAL.Implementation.Repository.BuildingUnit
{
    public class BuildingUnitRepository : CommonRepository<BuildingUnit_DbModel>,IBuildingUnitRepository
    {
        public BuildingUnitRepository(dg_SpecFoContext context) : base(context)
        {
        }
    }
}
