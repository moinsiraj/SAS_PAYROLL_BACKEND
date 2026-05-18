using BOL.Models;
using EF.Core.Repository.Interface.Manager;
using System.Data;

namespace BLL.Interfaces.Manager.ShiftChanges
{
    public interface IShiftChangesManager : ICommonManager<ShiftChange_DbModel>
    {
        Task<DataSet> FilterBase_employeelist(int? Compid = null, int? Department = null, int? section = null, int? Building = null, int? Floor = null, int? Line = null, int? Shift = null, int? Grade = null, int? salcat = null);
        Task<DataSet> ShiftChanges_Batch(int CompID, int emp_no);
        Task<DataSet> ShiftSearch(int compid, DateTime s_date, DateTime E_date);
        Task<DataSet> GetShift(int compid);
        Task<DataSet> Getshiftlist_alldata(int compid);
        Task<DataSet> ShiftChanges_Batch(int emp_serial, int oi_shift_OLD, int oi_shift, DateTime effectDate, String User, DateTime Udate);
        Task<DataSet> ShiftChanges_Batch(int oi_shift, DateTime effectDate, string User, DateTime Udate, int comid);
        Task<DataSet> employee_Info(int? Compid = null, int? Department = null, int? section = null, int? Building = null, int? Floor = null, int? Line = null, int? Shift = null, int? Grade = null, int? salcat = null, int? Newshift = null, DateTime? EffectDate = null);
        Task<bool> Dg_Save_ShiftRostaring_info(List<ShiftRostaring> srg);
        List<object> Dg_Save_ShiftRostaring_info2(List<ShiftRostaring2> srg);
        List<ReturnObject> Save_AutoShiftRostaring(ShiftRostaringAuto obj);
        List<ReturnObject> Save_AutoShiftRosterProcess(ShiftRostaringAuto obj);
        Task<DataTable> GetShiftGroupList(int companyID);
        Task<DataSet> Getrosterhistory(int comid);
        Task<DataTable> GetRosterInfoFdateTdateWise(int comid, string from_date = null, string to_date = null);
        Task<DataTable> GetShiftName(int compID, int deptID = 0, int secID = 0);
        //ShiftChange_DbModel CustomToDbModel(DgPayShiftChange obj);
        //DgPayShiftChange DbToCustomModel(ShiftChange_DbModel obj);
    }
}
