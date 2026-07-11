using BLL.Interfaces.Manager.ShiftChanges;
using BLL.Utility;
using BOL.Models;
using DAL.Data;
using DAL.Implementation.Repository.ShiftChanges;
using EF.Core.Repository.Manager;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Implementation.Manager.ShiftChanges
{
    public class ShiftChangesManager : CommonManager<ShiftChange_DbModel>,IShiftChangesManager
    {
        private readonly Dg_Common _dgCommon;
        private readonly SqlConnection _connection;
        public ShiftChangesManager(dg_hrpayrollContext context, Dg_Common dgCommon) :base(new ShiftChangesRepository(context))
        {
            _dgCommon = dgCommon;
            _connection = new SqlConnection(Getway.Dg_Payroll);
        }

        public async Task<DataSet> FilterBase_employeelist(int? Compid = null, int? Department = null,
            int? section = null, int? Building = null, int? Floor = null, int? Line = null,
            int? Shift = null, int? Grade = null, int? salcat = null)
        {
            var data = await _dgCommon.get_InformationDtasetAsync("Emp_filtering_list "+ Compid + ","+ Department + "," +
                ""+ section + ","+ Building + ","+ Floor + ","+ Line + ","+ Shift + ","+ Grade + ","+ salcat + "", _connection);
            return data;
        }
        public async Task<DataSet> ShiftChanges_Batch(int CompID, int emp_no)
        {
            var data = await _dgCommon.get_InformationDtasetAsync("D_ShiftChange "+ CompID + ","+ emp_no + "", _connection);
            return data;
        }
        public async Task<DataSet> ShiftSearch(int compid, DateTime s_date, DateTime E_date)
        {
            var data = await _dgCommon.get_InformationDtasetAsync("D_ShiftChange_search "+ compid + ",'"+ s_date + "','"+ E_date + "'", _connection);
            return data;
        }
        public async Task<DataSet> GetShift(int compid)
        {
            var data = await _dgCommon.get_InformationDtasetAsync("D_Shift "+ compid, _connection);
            return data;
        }
        public async Task<DataSet> Getshiftlist_alldata(int compid)
        {
            var data = await _dgCommon.get_InformationDtasetAsync("D_shiftlist_alldata "+ compid, _connection);
            return data;
        }
        public async Task<DataSet> ShiftChanges_Batch(int emp_serial, int oi_shift_OLD, int oi_shift, DateTime effectDate, String User, DateTime Udate)
        {
            var data = await _dgCommon.get_InformationDtasetAsync("dg_pay_Emp_Change_Shift "+ emp_serial + "," +
                ""+ oi_shift_OLD + ","+ oi_shift + ",'"+ effectDate + "','"+ User + "','"+ Udate + "'", _connection);
            return data;
        }
        public async Task<DataSet> ShiftChanges_Batch(int oi_shift, DateTime effectDate, string User, DateTime Udate, int comid)
        {
            var data = await _dgCommon.get_InformationDtasetAsync("dg_pay_Emp_Change_Shift_Batch "+ oi_shift + "," +
                "'"+ effectDate + "','"+ User + "','"+ Udate + "',"+ comid + "", _connection);
            return data;
        }
        public async Task<DataSet> employee_Info(int? Compid = null, int? Department = null, 
            int? section = null, int? Building = null, int? Floor = null, int? Line = null, int? 
            Shift = null, int? Grade = null, int? salcat = null, int? Newshift = null, DateTime? EffectDate = null)
        {
            var data = await _dgCommon.get_InformationDtasetAsync("Emp_filtering "+ Compid + "," +
                ""+ Department + ","+ section + ","+ Building + ","+ Floor + ","+ Line + ","+ Shift + "," +
                ""+ Grade + ","+ salcat + ","+ Newshift + ",'"+ EffectDate + "'", _connection);
            return data;
        }
        public async Task<DataSet> Getrosterhistory(int comid)
        {
            var data = await _dgCommon.get_InformationDtasetAsync("Shift_rostered_information "+ comid, _connection);
            return data;
        }
        public Task<DataTable> GetRosterInfoFdateTdateWise(int comid, string from_date = null, string to_date = null)
        {
            var data = _dgCommon.get_InformationDataTableAsync("Shift_rostered_information_fromdate_to_todate " + comid + ",'"+ from_date + "','"+ to_date + "'", _connection);
            return data;
        }
        public async Task<bool> Dg_Save_ShiftRostaring_info(List<ShiftRostaring> srg)
        {
            bool flag = false;
            try
            {
                foreach (ShiftRostaring item in srg)
                {
                    SqlCommand cmd = new SqlCommand("Emp_roster_process", _connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@emp_serial", item.sc_empserial);
                    cmd.Parameters.AddWithValue("@from_date", item.sc_start_date.ToString("yyyy/MM/dd"));
                    cmd.Parameters.AddWithValue("@to_date", item.sc_end_date.ToString("yyyy/MM/dd"));
                    cmd.Parameters.AddWithValue("@Old_shift", item.sc_old_shift);
                    cmd.Parameters.AddWithValue("@shift", item.sc_new_shift);
                    cmd.Parameters.AddWithValue("@sc_user", item.sc_user);
                    cmd.Parameters.AddWithValue("@sc_addate", DateTime.Now);
                    await _connection.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    await _connection.CloseAsync();
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                flag = false;
            }
            finally
            {
                await _connection.CloseAsync();
            }
            return flag;
        }
        public List<object> Dg_Save_ShiftRostaring_info2(List<ShiftRostaring2> srg)
        {
            var result = new List<object>();
            try
            {
                if (srg[0].sc_transfer_type == "permanent")
                {
                    srg.ToList().ForEach(ptItem =>
                    {
                        var dtEmployee = _dgCommon.get_InformationDataTable("select compid,emp_no from dg_pay_Employee where emp_serial=" + ptItem.sc_empserial, _connection);
                        int compNo = Convert.ToInt32(dtEmployee.Rows[0]["emp_no"]);
                        bool isSaveRT = _dgCommon.saveChanges("Dg_Pay_SaveEmployee_ShiftChange_Permanent "+ ptItem.sc_empserial + ",'"+ ptItem.sc_start_date + "',"+ ptItem.sc_new_shift + ",'"+ ptItem.sc_transfer_type + "','"+ ptItem.sc_user + "'", _connection);
                        if (isSaveRT)
                        {
                            result.Add(new { MessageType = "Success", Message = "Employee No(" + compNo + ") Shift Change Successfully !!" });
                        }
                        else
                        {
                            result.Add(new { MessageType = "Error", Message = "Employee No(" + compNo + ") Shift Not Change !!" });
                        }
                    });
                }
                else
                {
                    int currentMonth = DateTime.Now.AddDays(-20).Month;
                    int currentYear = DateTime.Now.AddDays(-20).Year;
                    int inputMonth = srg[0].sc_start_date.Month;
                    int inputYear = srg[0].sc_start_date.Year;
                    if (((currentMonth <= inputMonth) || (inputMonth > currentMonth) || (inputYear != currentYear)) && (currentYear <= inputYear))
                    {
                        var dtSalConf = _dgCommon.get_InformationDataTable("SELECT ss_emp_serial FROM dg_pay_salarysheet WHERE ss_compid=" + srg[0].sc_compid + " and MONTH(ss_date)=" + inputMonth + " AND YEAR(ss_date)=" + inputYear + " AND ss_confirmed=1", _connection);
                        bool isSalConf = dtSalConf.Rows.Count > 0 ? true : false;
                        if (!isSalConf)
                        {
                            var shiftChange = new List<ShiftRosterPaloadList>();
                            srg.ToList().ForEach(item =>
                            {
                                var dtEmployee = _dgCommon.get_InformationDataTable("select compid,emp_no from dg_pay_Employee where emp_serial=" + item.sc_empserial, _connection);
                                int compID = Convert.ToInt32(dtEmployee.Rows[0]["compid"]);
                                int compNo = Convert.ToInt32(dtEmployee.Rows[0]["emp_no"]);
                                for (var sDate = Convert.ToDateTime(item.sc_start_date); sDate <= Convert.ToDateTime(item.sc_end_date); sDate = sDate.AddDays(1))
                                {
                                    var dtLeave = _dgCommon.get_InformationDataTable("SELECT at_date FROM dg_pay_attendance WHERE at_emp_serial=" + item.sc_empserial + " and at_date='" + sDate + "' and (at_status_code='ML' or at_status_code='AL' or at_status_code='CL' or at_status_code='SL' or at_status_code='LWP' or at_status_code='M/L' or at_status_code='M/C' or at_status_code='ML' or at_status_code='COM')", _connection);
                                    bool isLeave = dtLeave.Rows.Count > 0 ? true : false;
                                    if (!isLeave)
                                    {
                                        var dtPresent = _dgCommon.get_InformationDataTable("SELECT at_emp_serial FROM dg_pay_attendance WHERE at_date='" + sDate + "' AND at_emp_serial=" + item.sc_empserial + " AND (RTRIM(at_status_code)='' OR RTRIM(at_status_code)='LA')", _connection);
                                        bool isPresent = dtPresent.Rows.Count > 0 ? true : false;
                                        if (!isPresent)
                                        {
                                            var dtAbsentManually = _dgCommon.get_InformationDataTable("SELECT at_emp_serial FROM dg_pay_attendance WHERE at_emp_serial=" + item.sc_empserial + " AND at_date='" + sDate + "' AND at_menual_absent=1", _connection);
                                            bool isAbsentManually = dtAbsentManually.Rows.Count > 0 ? true : false;
                                            if (!isAbsentManually || isAbsentManually)
                                            {
                                                shiftChange.Add(new ShiftRosterPaloadList
                                                {
                                                    sc_compID = compID,
                                                    sc_empserial = item.sc_empserial,
                                                    sc_effect_date = sDate.ToString(),
                                                    sc_start_date = item.sc_start_date.ToString(),
                                                    sc_end_date = item.sc_end_date.ToString(),
                                                    sc_old_shift = item.sc_old_shift,
                                                    sc_new_shift = item.sc_new_shift,
                                                    sc_transfer_type = item.sc_transfer_type,
                                                    sc_user = item.sc_user
                                                });
                                                result.Add(new { MessageType = "Success", Message = "Employee No(" + compNo + ") Date(" + sDate.ToString("dd-MMM-yyyy") + ") Shift Change Successfully !!" });
                                            }
                                            else
                                            {
                                                result.Add(new { MessageType = "Error", Message = "Employee No(" + compNo + " Date(" + sDate.ToString("dd-MMM-yyyy") + ")) Manually Absent Shift Not Change !!" });
                                            }
                                        }
                                        else
                                        {
                                            if (sDate.Date == DateTime.Now.Date)
                                            {
                                                shiftChange.Add(new ShiftRosterPaloadList
                                                {
                                                    sc_compID = compID,
                                                    sc_empserial = item.sc_empserial,
                                                    sc_effect_date = sDate.ToString(),
                                                    sc_start_date = item.sc_start_date.ToString(),
                                                    sc_end_date = item.sc_end_date.ToString(),
                                                    sc_old_shift = item.sc_old_shift,
                                                    sc_new_shift = item.sc_new_shift,
                                                    sc_transfer_type = item.sc_transfer_type,
                                                    sc_user = item.sc_user
                                                });
                                                result.Add(new { MessageType = "Success", Message = "Employee No(" + compNo + " Date(" + sDate.ToString("dd-MMM-yyyy") + ")) Shift Change Successfully !!" });
                                            }
                                            else
                                            {
                                                result.Add(new { MessageType = "Error", Message = "Employee No(" + compNo + " Date(" + sDate.ToString("dd-MMM-yyyy") + ")) Already Present Shift Not Change !!" });
                                            }
                                        }
                                    }
                                    else
                                    {
                                        result.Add(new { MessageType = "Error", Message = "Employee No(" + compNo + " Date(" + sDate.ToString("dd-MMM-yyyy") + ")) Leave Shift Not Change !!" });
                                    }
                                }
                            });
                            var dtShiftChange = _dgCommon.ListToDataTable<ShiftRosterPaloadList>(shiftChange);
                            if (dtShiftChange.Rows.Count > 0)
                            {
                                var attUpData = _dgCommon.get_InformationDataTableByType("Dg_Pay_GetShiftChangeAttenData", _connection, new SqlParameter("@shiftTable", dtShiftChange));
                                if (attUpData.Rows.Count > 0)
                                {
                                    attUpData.AsEnumerable().ToList().ForEach(att =>
                                    {
                                        int at_emp_serial = Convert.ToInt32(att.Field<int>("at_emp_serial"));
                                        string at_status_code = att.Field<string>("at_status_code").ToString().Trim();
                                        string at_holiday = att.Field<string>("at_holiday").ToString().Trim();
                                        string at_date = att.Field<DateTime>("at_date").ToString().Trim();
                                        if (at_status_code == "AB" || at_status_code == "WH" || at_status_code == "SH")
                                        {
                                            att["at_shift"] = srg[0].sc_new_shift;
                                        }
                                        else if ((at_status_code == "" || at_status_code == "LA") && at_holiday == "")
                                        {
                                            att["at_status"] = "Absent";
                                            att["at_status_code"] = "AB";
                                            att["at_shift"] = srg[0].sc_new_shift;
                                        }
                                        else if ((at_status_code == "" || at_status_code == "LA") && at_holiday == "WH")
                                        {
                                            att["at_status"] = Convert.ToDateTime(at_date).ToString("dddd");
                                            att["at_status_code"] = "WH";
                                            att["at_shift"] = srg[0].sc_new_shift;
                                        }
                                        else if ((at_status_code == "" || at_status_code == "LA") && at_holiday == "SH")
                                        {
                                            var dtShDisc = _dgCommon.get_InformationDataTable("SELECT sh_description FROM dg_pay_specialholidays_empWise WHERE sh_emp_serial=" + at_emp_serial + " AND sh_date='" + at_date + "'", _connection);
                                            string sh_Description = dtShDisc.Rows[0]["sh_description"].ToString();
                                            att["at_status"] = sh_Description;
                                            att["at_status_code"] = "SH";
                                            att["at_shift"] = srg[0].sc_new_shift;
                                        }
                                    });
                                    _dgCommon.saveChangesByType("Dg_Pay_SaveAttenDataAfterShiftChange", _connection, new SqlParameter("@attUpdateTable", attUpData));
                                }
                                _dgCommon.saveChangesByType("Dg_Pay_SaveShiftChangeRoster", _connection, new SqlParameter("@shiftTable", dtShiftChange));
                            }
                        }
                        else
                        {
                            result.Add(new { MessageType = "Error", Message = "Salary Already Confirmed This Month(" + srg[0].sc_start_date.ToString("MMMM-yyyy") + ") Shift Can Not Be Changed !!" });
                        }
                    }
                    else
                    {
                        result.Add(new { MessageType = "Error", Message = "Previous Month Date Range(" + srg[0].sc_start_date.ToString("dd-MMM-yyyy") + " To " + srg[0].sc_end_date.ToString("dd-MMM-yyyy") + ") Shift Can Not Be Changed !!" });
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return result;
        }
        public List<ReturnObject> Save_AutoShiftRostaring(ShiftRostaringAuto obj)
        {
            var result = new List<ReturnObject>();
            try
            {
                if (obj.rosterChild.Count > 0)
                {
                    obj.rosterChild.ToList().ForEach(item =>
                    {
                        if (item.rGroupId > 0)
                        {
                            bool isSave = _dgCommon.saveChanges("Dg_Pay_Save_ShiftRoll_EmpWise " + obj.companyID + "," + item.rGroupId + "," + item.empSerial + "," + item.empId + ",'" + obj.fDate + "','" + obj.userName + "'", _connection);
                            if (isSave)
                            {
                                result.Add(new ReturnObject
                                {
                                    IsSuccess = true,
                                    Message = "Employee No(" + item.empId + ") Save Successfully !!"
                                });
                            }
                            else
                            {
                                result.Add(new ReturnObject
                                {
                                    IsSuccess = false,
                                    Message = "Employee No(" + item.empId + ") Save Fail !!"
                                });
                            }
                        }
                        else
                        {
                            result.Add(new ReturnObject
                            {
                                IsSuccess = false,
                                Message = "Employee No(" + item.empId + ") Save Fail Shift Group Id Zero !!"
                            });
                        }
                    });
                    

                    //string endDate = Convert.ToDateTime(obj.fDate).AddDays(7).ToString("MM-dd-yyyy");
                    //obj.rosterChild.ToList().ForEach(item =>
                    //{
                    //    var data = _dgCommon.get_InformationDataTable("Dg_Pay_Save_ShiftRoll_EmpWise " + obj.companyID + "," + item.rGroupId + "," + item.empSerial + "," + item.empId + ",'" + obj.fDate + "','" + obj.userName + "'", _connection);
                    //    bool exists = data.AsEnumerable().Where(c => c.Field<string>("mType").Equals("Success")).Count() > 0;
                    //    exists = true && _dgCommon.saveChanges("update dg_pay_Employee set oi_shift_groupid=" + item.rGroupId + " where emp_serial=" + item.empSerial, _connection);
                    //    data.AsEnumerable().ToList().ForEach(x =>
                    //    {
                    //        result.Add(new ReturnObject
                    //        {
                    //            IsSuccess = Convert.ToBoolean(x.Field<string>("mType").Equals("Success") ? true : false),
                    //            Message = x.Field<string>("msg").ToString()
                    //        });
                    //    });
                    //});
                }
                else
                {
                    result.Add(new ReturnObject
                    {
                        IsSuccess = false,
                        Message = "You Can Not Chack Any Employee !!"
                    });
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                result.Add(new ReturnObject
                {
                    IsSuccess = false,
                    Message = "Something Went Wrong !!"
                });
            }
            return result;
        }
        public List<ReturnObject> Save_AutoShiftRosterProcess(ShiftRostaringAuto obj)
        {
            var result = new List<ReturnObject>();
            try
            {
                if (obj.rosterChild.Count > 0)
                {
                    obj.rosterChild.ToList().ForEach(item =>
                    {
                        var data = _dgCommon.get_InformationDataTable("Dg_Pay_ShiftAutoRoll_Save " + item.empSerial + "," + item.rGroupId + ",'" + obj.fDate + "','" + obj.tDate + "','" + obj.userName + "'", _connection);
                        data.AsEnumerable().ToList().ForEach(x =>
                        {
                            result.Add(new ReturnObject
                            {
                                IsSuccess = Convert.ToBoolean(x.Field<string>("mType").Equals("Success") ? true : false),
                                Message = x.Field<string>("msg").ToString()
                            });
                        });
                    });
                }
                else
                {
                    result.Add(new ReturnObject
                    {
                        Message = "You Can Not Chack Any Employee !!"
                    });
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                result.Add(new ReturnObject
                {
                    Message = "Something Went Wrong !!"
                });
            }
            return result;
        }
        public async Task<DataTable> GetShiftGroupList(int companyID)
        {
            var data = await _dgCommon.get_InformationDataTableAsync("select sRGroup,sRGrpDescription from Dg_Pay_ShiftRollGroup where sRCompid=" + companyID, _connection);
            return data;
        }
        public async Task<DataTable> GetShiftName(int compID, int deptID = 0, int secID = 0)
        {
            var data = await _dgCommon.get_InformationDataTableAsync("Dg_GetShiftName_ForShiftRosterhistory "+ compID + ","+ deptID + ","+ secID, _connection);
            return data;
        }
    }
}
