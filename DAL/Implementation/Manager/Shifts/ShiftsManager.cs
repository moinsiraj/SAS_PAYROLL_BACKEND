using BLL.Interfaces.Manager.Shifts;
using BOL.Models;
using DAL.Data;
using DAL.Implementation.Repository.Shifts;
using EF.Core.Repository.Manager;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;

namespace DAL.Implementation.Manager.Shifts
{
    public class ShiftsManager : CommonManager<Shift_DbModel>,IShiftsManager
    {
        public ShiftsManager(dg_hrpayrollContext context):base(new ShiftsRepository(context))
        {            
        }       
    }
}
