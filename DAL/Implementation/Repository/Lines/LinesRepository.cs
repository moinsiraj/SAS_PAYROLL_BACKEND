using BLL.Interfaces.Repository.Lines;
using BOL.Models;
using DAL.Data;
using EF.Core.Repository.Repository;

namespace DAL.Implementation.Repository.Lines
{
    public class LinesRepository : CommonRepository<Line_DbModel>,ILinesRepository
    {
        public LinesRepository(dg_SpecFoContext context) : base(context)
        {
        }
    }
}
