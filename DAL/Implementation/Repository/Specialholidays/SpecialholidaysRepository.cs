using BLL.Interfaces.Repository.Specialholidays;
using BOL.Models;
using DAL.Data;
using EF.Core.Repository.Repository;

namespace DAL.Implementation.Repository.Specialholidays
{
    public class SpecialholidaysRepository : CommonRepository<Specialholiday_DbModel>,ISpecialholidaysRepository
    {
        public SpecialholidaysRepository(dg_hrpayrollContext context):base(context)
        {
        }
    }
}
