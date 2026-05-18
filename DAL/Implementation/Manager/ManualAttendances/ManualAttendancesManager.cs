using BLL.Interfaces.Manager.ManualAttendances;
using BLL.Utility;
using BOL.Models;
using DAL.Data;
using DAL.Implementation.Repository.ManualAttendances;
using EF.Core.Repository.Manager;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Implementation.Manager.ManualAttendances
{
    public class ManualAttendancesManager : CommonManager<Attendance_DbModel>,IManualAttendancesManager
    {
        private readonly Dg_Common _dgCommon;
        private readonly SqlConnection _connection;
        public ManualAttendancesManager(dg_hrpayrollContext context, Dg_Common dgCommon) : base(new ManualAttendancesRepository(context))
        {
            _dgCommon = dgCommon;
            _connection = new SqlConnection(Getway.Dg_Payroll);
        }

        public async Task<DataSet> ManualAttendance(int Emp_serial, DateTime date, decimal intime, DateTime outdate, decimal outtime)
        {
            var data = await _dgCommon.get_InformationDtasetAsync("D_Manual_Attendance "+ Emp_serial + ",'"+ date + "',"+ intime + ",'"+ outdate + "',"+ outtime + "", _connection);
            return data;
        }
        public async Task<DataSet> ManualAttendance_select(int Emp_serial, DateTime date)
        {
            var data = await _dgCommon.get_InformationDtasetAsync("Manual_Attendance_select_DATE_AND_IDWISE "+ Emp_serial + ",'"+ date + "'", _connection);
            return data;
        }
        public async Task<DataSet> ManualAttendance_viw(int comp, DateTime Sdate, DateTime Edate, int IND)
        {
            var data = await _dgCommon.get_InformationDtasetAsync("D_Manual_Attendance_VIW "+ comp + ",'"+ Sdate + "','"+ Edate + "',"+ IND + "", _connection);
            return data;
        }
        public async Task<DataSet> ManualAttendancefilter(int? Compid = null, int? Department = null, int? section = null, int? Building = null, int? Floor = null, int? Line = null, int? salCat = null)
        {
            var data = await _dgCommon.get_InformationDtasetAsync("Emp_filtering_manual_attendance "+ Compid + ","+ Department + ","+ section + ","+ Building + ","+ Floor + ","+ Line + ","+ salCat + "", _connection);
            return data;
        }
        public async Task<DataSet> shiftlist(int CompID)
        {
            var data = await _dgCommon.get_InformationDtasetAsync("Shift_list "+ CompID, _connection);
            return data;
        }
        public async Task<List<ManualAttSeletedList>> GetManualAttList(List<ManualAttPara> para)
        {
            var addList = new List<ManualAttSeletedList>();
            try
            {
                foreach (ManualAttPara item in para)
                {
                    var dt = await _dgCommon.get_InformationDataTableAsync("Emp_list_fromdatetodate_wise '" + item.emp_serial + "','" + item.fromDate + "','" + item.todate + "','" + item.weeklyholiday + "','" + item.leave + "'", _connection);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        addList.Add(new ManualAttSeletedList
                        {
                            compid = Convert.ToInt32(dt.Rows[i]["compid"]),
                            comp_name = dt.Rows[i]["comp_name"].ToString(),
                            emp_serial = Convert.ToInt32(dt.Rows[i]["emp_serial"]),
                            emp_no = Convert.ToInt32(dt.Rows[i]["emp_no"]),
                            emp_proxid = Convert.ToInt32(dt.Rows[i]["emp_proxid"]),
                            pi_fullname = dt.Rows[i]["pi_fullname"].ToString(),
                            //sh_code = Convert.ToInt32(dt.Rows[i]["sh_code"]),
                            sh_name = dt.Rows[i]["sh_name"].ToString(),
                            sh_status = dt.Rows[i]["Sh_statas"].ToString(),
                            //oi_department = Convert.ToInt32(dt.Rows[i]["oi_department"]),
                            oi_departmente_name = dt.Rows[i]["oi_departmente_name"].ToString(),
                            //oi_section = Convert.ToInt32(dt.Rows[i]["oi_section"]),
                            oi_section_name = dt.Rows[i]["oi_section_name"].ToString(),
                            //oi_designation = Convert.ToInt32(dt.Rows[i]["oi_designation"]),
                            oi_designation_name = dt.Rows[i]["oi_designation_name"].ToString(),
                            //oi_bulding = Convert.ToInt32(dt.Rows[i]["oi_bulding"]),
                            oi_bulding_name = dt.Rows[i]["oi_bulding_name"].ToString(),
                            oi_joineddate = dt.Rows[i]["oi_joineddate"].ToString(),
                            at_date = dt.Rows[i]["at_date"].ToString(),
                            at_intime = Convert.ToDecimal(dt.Rows[i]["at_intime"]),
                            at_outdate = dt.Rows[i]["at_outdate"].ToString(),
                            at_outtime = Convert.ToDecimal(dt.Rows[i]["at_outtime"]),
                            at_status = dt.Rows[i]["at_status"].ToString(),
                            sh_InTime = Convert.ToDecimal(dt.Rows[i]["sh_InTime"]),
                            sh_OutTime = Convert.ToDecimal(dt.Rows[i]["sh_OutTime"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return addList;
        }
        public async Task<ReturnObject> SaveManualAtt(List<ManualAttSavePara> para)
        {
            var result = new ReturnObject();
            int loopCount = 0;
            var dtCompid = await _dgCommon.get_InformationDataTableAsync(string.Format("select compid from dg_pay_Employee where emp_serial={0}", para[0].at_emp_serial), _connection);
            int compid = dtCompid.Rows.Count > 0 && !string.IsNullOrEmpty(dtCompid.Rows[0]["compid"].ToString()) ? int.Parse(dtCompid.Rows[0]["compid"].ToString()) : 0;
            try
            {
                var dtCheckSalProc = await _dgCommon.get_InformationDataTableAsync(string.Format("select procs_compid from dg_pay_Process_startOrStop where procs_compid={0} and procs_type in('emp_sal_process_bulk','emp_sal_process_Single') group by procs_compid", compid), _connection);
                if (dtCheckSalProc.Rows.Count > 0)
                {
                    result.Message = "Salary Process In Progress,Please Wait... !!";
                    return result;
                }
                var dtCheckOtProc = await _dgCommon.get_InformationDataTableAsync(string.Format("select procs_compid from dg_pay_Process_startOrStop where procs_compid={0} and procs_type in('att_ot_process','att_Hdot_process') group by procs_compid", compid), _connection);
                if (dtCheckOtProc.Rows.Count > 0)
                {
                    result.Message = "OT Process In Progress,Please Wait... !!";
                    return result;
                }
                await _dgCommon.saveChangesAsync(string.Format("dg_pay_processStartOrStop_insert {0},'{1}',{2},'{3}','{4}'", compid, "att_menual_upload", 1, DateTime.Now.ToString("MM/dd/yyyy"), para[0].User), _connection);
                foreach (var item in para)
                {
                    bool isSave = await _dgCommon.saveChangesAsync("Emp_update_menualAttendance", _connection, item);
                    if (isSave)
                    {
                        loopCount++;
                    }
                }
                if (loopCount > 0)
                {
                    result.IsSuccess = true;
                    result.Message = "Save Successfully !!";
                }
                else
                {
                    result.Message = "Save Fail !!";
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            finally
            {
                await _dgCommon.saveChangesAsync(string.Format("delete from dg_pay_Process_startOrStop where procs_compid={0} and procs_date='{1}' and procs_type='att_menual_upload'", compid, DateTime.Now.ToString("MM/dd/yyyy")), _connection);
            }
            return result;
        }
        public async Task<string> UpdateMenualAttendanceAbsent(List<AttendanceAbsentPaylod> objArr)
        {
            string message = string.Empty;
            try
            {
                //var chk = DateTime.Now.AddDays(-10).Month;
                if ((Convert.ToDateTime(objArr[0].start_date.ToString()).Month == DateTime.Now.AddDays(-20).Month || Convert.ToDateTime(objArr[0].start_date.ToString()).Month == DateTime.Now.Month) && (Convert.ToDateTime(objArr[0].start_date.ToString()).Year == DateTime.Now.AddDays(-20).Year || Convert.ToDateTime(objArr[0].start_date.ToString()).Year == DateTime.Now.Year))
                {
                    foreach (var item in objArr)
                    {
                        await _dgCommon.saveChangesAsync("dg_pay_Update_MenualAbsentAttendance " + item.emp_serial + ",'" + item.start_date + "','" + item.end_date + "','" + item.userName + "'", _connection);
                    }
                    message = "Save Successfully !!";
                }
                else
                {
                    message = "Date Range(" + objArr[0].start_date +" To "+ objArr[0].end_date + ") Is Not Valid !!";
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                message = "Something is wrong,Data not save !!";
            }
            return message;
        }
        public async Task<List<GetListAttendanceRowDel>> GetEmployeeListForAttDel(GetEmployeeListPayload obj)
        {
            var data = new List<GetListAttendanceRowDel>();
            try
            {
                foreach (var item in obj.emp_no)
                {
                    var dataTable = await _dgCommon.get_InformationDataTableAsync("dg_pay_GetEmployeeListForAttDelete "+ obj.compid + ","+ item + "", _connection);
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        data.Add(new GetListAttendanceRowDel
                        {
                            emp_serial = Convert.ToInt32(dataTable.Rows[i]["emp_serial"]),
                            compid = Convert.ToInt32(dataTable.Rows[i]["compid"]),
                            emp_no = Convert.ToInt32(dataTable.Rows[i]["emp_no"]),
                            emp_proxid = dataTable.Rows[i]["emp_proxid"].ToString(),
                            pi_fullname = dataTable.Rows[i]["pi_fullname"].ToString(),
                            oi_departmente_name = dataTable.Rows[i]["oi_departmente_name"].ToString(),
                            oi_section_name = dataTable.Rows[i]["oi_section_name"].ToString(),
                            oi_bulding_name = dataTable.Rows[i]["oi_bulding_name"].ToString(),
                            oi_floor_name = dataTable.Rows[i]["oi_floor_name"].ToString(),
                            oi_line_name = dataTable.Rows[i]["oi_line_name"].ToString(),
                            oi_shift_name = dataTable.Rows[i]["oi_shift_name"].ToString(),
                            oi_joineddate = dataTable.Rows[i]["oi_joineddate"].ToString(),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return data;
        }
        public async Task<ReturnObject> GetEmpFromAttendanceForOutTimeChange(GetEmpAttendanceListPayload obj)
        {
            var result = new ReturnObject();
            var data = await _dgCommon.get_InformationDataTableAsync("Dg_filtering_employee_from_attendance", _connection,obj);
            if (data.Rows.Count > 0)
            {
                result.IsSuccess = true;
                result.dataTable = data;
                return result;
            }
            return result;
        }
        public ReturnObject SaveEmpAttendanceChange(EmpAttendanceChangeSavePayload obj)
        {
            var result = new ReturnObject();
            if (obj.at_emp_serial.Length > 0)
            {
                obj.at_emp_serial.ToList().ForEach(item =>
                {
                    var dbObj = EmpAttendanceChangeSavePayload.MainPayload(item,obj);
                    _dgCommon.saveChanges("Emp_update_Out_time_from_attendance_modification", _connection,dbObj);
                });
                result.IsSuccess = true;
                result.Message = "Outtime Change Successfully !!";
                return result;
            }
            result.Message = "You Can Not Check Any Employee !!";
            return result;
        }
        public async Task<ReturnObject> GetEmpFromAttendanceForInTimeChange(GetEmpAttendanceListPayload obj)
        {
            var result = new ReturnObject();
            var data = await _dgCommon.get_InformationDataTableAsync("Dg_filtering_employee_from_attendance_for_intime_modification", _connection, obj);
            if (data.Rows.Count > 0)
            {
                result.IsSuccess = true;
                result.dataTable = data;
                return result;
            }
            return result;
        }
        public ReturnObject SaveEmpAttendanceInTimeChange(EmpAttendanceChangeSavePayload obj)
        {
            var result = new ReturnObject();
            if (obj.at_emp_serial.Length > 0)
            {
                obj.at_emp_serial.ToList().ForEach(item =>
                {
                    var dbObj = EmpAttendanceChangeSavePayload.MainPayload(item, obj);
                    _dgCommon.saveChanges("Emp_update_In_time_from_attendance_modification", _connection, dbObj);
                });
                result.IsSuccess = true;
                result.Message = "In Time Change Successfully !!";
                return result;
            }
            result.Message = "You Can Not Check Any Employee !!";
            return result;
        }
    }
}
