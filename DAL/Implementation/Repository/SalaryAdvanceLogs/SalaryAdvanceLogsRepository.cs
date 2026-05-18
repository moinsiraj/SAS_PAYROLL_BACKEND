using BLL.Interfaces.Repository.SalaryAdvanceLogs;
using BOL.Models;
using DAL.Data;
using EF.Core.Repository.Repository;

namespace DAL.Implementation.Repository.SalaryAdvanceLogs
{
    public class SalaryAdvanceLogsRepository : CommonRepository<SalaryAdvanceLog_DbModel>,ISalaryAdvanceLogsRepository
    {
        public SalaryAdvanceLogsRepository(dg_hrpayrollContext context):base(context)
        {            
        }
    }
}
