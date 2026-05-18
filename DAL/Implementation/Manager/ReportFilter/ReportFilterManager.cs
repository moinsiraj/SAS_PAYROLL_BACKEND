using BLL.Interfaces.Manager.ReportFilter;
using BLL.Utility;
using BOL.Models;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Implementation.Manager.ReportFilter
{
    public class ReportFilterManager : IReportFilterManager
    {
        private readonly Dg_Common _dgCommon;
        private readonly SqlConnection _connection;
        public ReportFilterManager(Dg_Common dgCommon)
        {
            _dgCommon = dgCommon;
            _connection = new SqlConnection(Getway.Dg_Payroll);
        }

        public async Task<ReturnObject> GetFilterEmpData(EmployeeCheckPaylod obj)
        {
            var result = new ReturnObject();
            if (obj.compid != 0 && !string.IsNullOrEmpty(obj.compid.ToString()))
            {
                string salCatId = string.Empty;
                string empIn = string.Empty;
                string empNotIn = string.Empty;

                if (obj.salCategory.Length > 0)
                {                    
                    var salCatArr = new List<int>();
                    foreach (var salCat in obj.salCategory)
                    {
                        salCatArr.Add(salCat);
                    }
                    salCatId = " and oi_salcategory in(" + string.Join(",", salCatArr) + ")";
                }
                int[] empNo = !string.IsNullOrEmpty(obj.employees) ? obj.employees.Split(",").Select(int.Parse).ToArray() : null;
                if (empNo != null)
                {
                    foreach (var emp in empNo)
                    {
                        var listEmp = await _dgCommon.get_InformationDataTableAsync(string.Format("dg_pay_filtering_reportByEmpNo {0},{1},{2},{3},{4},{5},'{6}'", obj.compid, obj.department, obj.section, obj.building, obj.floor, obj.line, salCatId), _connection);
                        bool exists = listEmp.AsEnumerable().Where(c => c.Field<int>("emp_no").Equals(emp)).Count() > 0;
                        if (exists)
                        {
                            empIn = string.IsNullOrEmpty(empIn) ? emp.ToString() : string.Concat(empIn, ",", emp);
                        }
                        else
                        {
                            empNotIn = string.IsNullOrEmpty(empNotIn) ? emp.ToString() : string.Concat(empNotIn, ",", emp);
                        }
                    }
                    if (!string.IsNullOrEmpty(empIn) || !string.IsNullOrEmpty(empNotIn))
                    {
                        result.IsSuccess = true;
                        result.dataTable = await _dgCommon.get_InformationDataTableAsync("select emp_no as EmpNo,pi_fullname as empName,cast(1 as bit) as isGet  from dg_pay_Employee where compid=" + obj.compid + " and emp_no in(" + empIn + ")", _connection);
                        result.Message = !string.IsNullOrEmpty(empNotIn) ? string.Format("Employee({0}) Not Valid !!", empNotIn) : string.Empty;
                        return result;
                    }
                }
            }
            return result;
        }
        public async Task<bool> Save_EmpReportParameter(ReportParameterModel obj)
        {
            bool flag = false;
            string condition = string.Empty;
            string conditionSalCat = string.Empty;
            try
            {
                if (obj.empNoFilter.Count >0)
                {
                    var myArray = new List<int>();
                    string arrayParameter = string.Empty;
                    foreach (var item in obj.empNoFilter)
                    {
                        if (item.isGet == true)
                        {
                            myArray.Add(item.EmpNo);
                            arrayParameter = string.Join(",", myArray);
                            condition = "and emp_no in(" + arrayParameter + ")";
                        }
                        else
                        {
                            myArray.Add(item.EmpNo);
                            arrayParameter = string.Join(",", myArray);
                            condition = "and emp_no not in(" + arrayParameter + ")";
                        }
                    }
                }
                if (obj.salcat.Length > 0)
                {
                    var catList = new List<int>();
                    string catParameter = string.Empty;
                    foreach (var itemSalCat in obj.salcat)
                    {
                        catList.Add(itemSalCat);
                        catParameter = string.Join(",", catList);
                        conditionSalCat = " and oi_salcategory in(" + catParameter + ")";
                    }
                }
                await _dgCommon.saveChangesAsync("Emp_filtering '"+ obj.Compid + "','"+ obj.Department + "','"+ obj.section + "','"+ obj.Building + "','"+ obj.Floor + "','"+ obj.Line + "','"+ obj.Shift + "','"+ obj.Grade + "','"+ obj.Start_date + "','"+ obj.End_date + "','"+ obj.Inactive_reson + "','"+ obj.User + "','"+ condition + "','"+ conditionSalCat + "'", _connection);
                return true;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return flag;
        }
        public async Task<bool> Save_LevReportParameter(ReportParameterModel obj)
        {
            bool flag = false;
            string condition = string.Empty;
            string conditionSalCat = string.Empty;
            try
            {
                if (obj.empNoFilter.Count > 0)
                {
                    var myArray = new List<int>();
                    string arrayParameter = string.Empty;
                    foreach (var item in obj.empNoFilter)
                    {
                        if (item.isGet == true)
                        {
                            myArray.Add(item.EmpNo);
                            arrayParameter = string.Join(",", myArray);
                            condition = "and emp_no in(" + arrayParameter + ")";
                        }
                        else
                        {
                            myArray.Add(item.EmpNo);
                            arrayParameter = string.Join(",", myArray);
                            condition = "and emp_no not in(" + arrayParameter + ")";
                        }
                    }
                }
                if (obj.salcat.Length > 0)
                {
                    var catList = new List<int>();
                    string catParameter = string.Empty;
                    foreach (var itemSalCat in obj.salcat)
                    {
                        catList.Add(itemSalCat);
                        catParameter = string.Join(",", catList);
                        conditionSalCat = " and oi_salcategory in(" + catParameter + ")";
                    }
                }
                await _dgCommon.saveChangesAsync("Emp_filtering_Leave '" + obj.Compid + "','" + obj.Department + "','" + obj.section + "','" + obj.Building + "','" + obj.Floor + "','" + obj.Line + "','" + obj.Shift + "','" + obj.Grade + "','" + obj.Start_date + "','" + obj.End_date + "','" + obj.User + "','" + condition + "','"+ conditionSalCat + "'", _connection);
                return true;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return flag;            
        }
        public async Task<bool> Save_AttReportParameter(ReportPrintPayload obj)
        {
            //Old Code
            /*bool flag = false;
            string condition = string.Empty;
            string conditionSalCat = string.Empty;
            try
            {
                if (obj.empNoFilter.Count > 0)
                {
                    var myArray = new List<int>();
                    string arrayParameter = string.Empty;
                    foreach (var item in obj.empNoFilter)
                    {
                        if (item.isGet == true)
                        {
                            myArray.Add(item.EmpNo);
                            arrayParameter = string.Join(",", myArray);
                            condition = "and emp_no in(" + arrayParameter + ")";
                        }
                        else
                        {
                            myArray.Add(item.EmpNo);
                            arrayParameter = string.Join(",", myArray);
                            condition = "and emp_no not in(" + arrayParameter + ")";
                        }
                    }
                }
                if (obj.salcat.Length > 0)
                {
                    var catList = new List<int>();
                    string catParameter = string.Empty;
                    foreach (var itemSalCat in obj.salcat)
                    {
                        catList.Add(itemSalCat);
                        catParameter = string.Join(",", catList);
                        conditionSalCat = " and oi_salcategory in(" + catParameter + ")";
                    }
                }
                await _dgCommon.saveChangesAsync("Emp_filtering_Attendance '" + obj.Compid + "','" + obj.Department + "','" + obj.section + "','" + obj.Building + "','" + obj.Floor + "','" + obj.Line + "','" + obj.Shift + "','" + obj.Grade + "','" + obj.Start_date + "','" + obj.End_date + "','" + obj.User + "','" + condition + "','"+ conditionSalCat + "'", _connection);
                return true;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return flag;*/

            //New Code 12/12/2024
            bool flag = false;

            try
            {
                string empID = string.Empty;
                string departmentId = string.Empty;
                string sectionId = string.Empty;
                string floorId = string.Empty;
                string lineId = string.Empty;
                string salCatID = string.Empty;
                string dateRange = string.Empty;
                if (obj.Compid != 0)
                {
                    if (obj.empNoFilter.Count > 0)
                    {
                        var empArr = new List<int>();
                        foreach (var item in obj.empNoFilter)
                        {
                            if (item.isGet == true)
                            {
                                empArr.Add(item.EmpNo);
                                empID = "and emp_no in(" + string.Join(",", empArr) + ")";
                            }
                            else
                            {
                                empArr.Add(item.EmpNo);
                                empID = "and emp_no not in(" + string.Join(",", empArr) + ")";
                            }
                        }
                    }
                    if (obj.Department.Length > 0)
                    {
                        var dptArr = new List<int>();
                        foreach (var department in obj.Department)
                        {
                            dptArr.Add(department);
                        }
                        departmentId = " and oi_department in(" + string.Join(",", dptArr) + ")";
                    }
                    if (obj.section.Length > 0)
                    {
                        var sectionArr = new List<int>();
                        foreach (var section in obj.section)
                        {
                            sectionArr.Add(section);
                        }
                        sectionId = " and oi_section in(" + string.Join(",", sectionArr) + ")";
                    }
                    if (obj.Floor.Length > 0)
                    {
                        var floorArr = new List<int>();
                        foreach (var floor in obj.Floor)
                        {
                            floorArr.Add(floor);
                        }
                        floorId = " and oi_floor in(" + string.Join(",", floorArr) + ")";
                    }
                    if (obj.Line.Length > 0)
                    {
                        var lineArr = new List<int>();
                        foreach (var line in obj.Line)
                        {
                            lineArr.Add(line);
                        }
                        lineId = " and oi_line in(" + string.Join(",", lineArr) + ")";
                    }
                    if (obj.salcat.Length > 0)
                    {
                        var salCatArr = new List<int>();
                        foreach (var itemSalCat in obj.salcat)
                        {
                            salCatArr.Add(itemSalCat);
                        }
                        salCatID = " and oi_salcategory in(" + string.Join(",", salCatArr) + ")";
                    }
                    if (obj.Shift > 0)
                    {
                        dateRange = string.Format(" and at_date between ''{0}'' and ''{1}''", obj.Start_date.ToString("MM-dd-yyyy"), obj.End_date.ToString("MM-dd-yyyy"));
                    }
                    await _dgCommon.saveChangesAsync("Emp_filtering_Attendance_New '" + obj.Compid + "','" + departmentId + "','" + sectionId + "','" + obj.Building + "','" + floorId + "','" + lineId + "','" + obj.Shift + "','" + obj.Grade + "','" + obj.Start_date + "','" + obj.End_date + "','" + obj.User + "','" + empID + "','" + salCatID + "','" + dateRange + "'", _connection);
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return flag;
        }
        public async Task<bool> Save_SalReportParameter(ReportParameterModel obj)
        {
            bool flag = false;
            string condition = string.Empty;
            string conditionSalCat = string.Empty;
            try
            {
                if (obj.empNoFilter.Count > 0)
                {
                    var myArray = new List<int>();
                    string arrayParameter = string.Empty;
                    foreach (var item in obj.empNoFilter)
                    {
                        if (item.isGet == true)
                        {
                            myArray.Add(item.EmpNo);
                            arrayParameter = string.Join(",", myArray);
                            condition = "and emp_no in(" + arrayParameter + ")";
                        }
                        else
                        {
                            myArray.Add(item.EmpNo);
                            arrayParameter = string.Join(",", myArray);
                            condition = "and emp_no not in(" + arrayParameter + ")";
                        }
                    }
                }
                if (obj.salcat.Length > 0)
                {
                    var catList = new List<int>();
                    string catParameter = string.Empty;
                    foreach(var itemSalCat in obj.salcat)
                    {
                        catList.Add(itemSalCat);
                        catParameter = string.Join(",", catList);
                        conditionSalCat = " and oi_salcategory in("+ catParameter + ")";
                    }
                }
                await _dgCommon.saveChangesAsync("Emp_filtering_salary '" + obj.Compid + "','" + obj.Department + "','" + obj.section + "','" + obj.Building + "','" + obj.Floor + "','" + obj.Line + "','" + obj.Shift + "','" + obj.Grade + "','" + obj.Start_date + "','" + obj.End_date + "','"+ obj.oi_bank + "','"+ obj.Inactive_reson + "','" + obj.User + "','" + condition + "','"+ conditionSalCat + "'", _connection);
                return true;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return flag;
        }

        // New Action 11/23/2023 by Forhad************
        public async Task<DataTable> GetDepartmentForRepFiltter(int companyID)
        {
            var dptData = new DataTable();
            try
            {
                dptData = await _dgCommon.get_InformationDataTableAsync("D_Department_ForRepFiltter "+ companyID, _connection);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return dptData;
        }
        public async Task<DataTable> GetSectionsForRepFiltter(DepartmenIdtArr obj)
        {
            var secData = new DataTable();
            try
            {
                string department = string.Empty;
                if (obj.dptID.Length > 0)
                {
                    var dptArr = new List<int>();
                    foreach (var dptItem in obj.dptID)
                    {
                        dptArr.Add(dptItem);
                    }
                    department = string.Join(",", dptArr);                    
                }
                secData = await _dgCommon.get_InformationDataTableAsync("D_Sections_ForRepFiltter " + obj.companyID + ",'" + department + "'", _connection);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return secData;
        }
        public async Task<DataTable> GetBuldingForRepFiltter(int companyID)
        {
            var dptData = new DataTable();
            try
            {
                dptData = await _dgCommon.get_InformationDataTableAsync("D_bulding_ForRepFiltter " + companyID, _connection);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return dptData;
        }
        public async Task<DataTable> GetFloorForRepFiltter(floorIdPayload obj)
        {
            var dptData = new DataTable();
            try
            {
                string departmentID = string.Empty;
                string sectionID = string.Empty;
                if (obj.departmentID.Length >0)
                {
                    var departmentArr = new List<int>();
                    obj.departmentID.ToList().ForEach(departmentID =>
                    {
                        departmentArr.Add(departmentID);
                    });
                    departmentID = " and oi_department in(" + string.Join(",", departmentArr) + ")";
                }
                if (obj.sectionID.Length >0)
                {
                    var sectionArr = new List<int>();
                    obj.sectionID.ToList().ForEach(sectionID =>
                    {
                        sectionArr.Add(sectionID);
                    });
                    sectionID = " and oi_section in(" + string.Join(",", sectionArr) + ")";
                }
                dptData = await _dgCommon.get_InformationDataTableAsync("D_Floor_ForRepFiltter " + obj.companyID + "," + obj.buldingID + ",'"+ departmentID + "','"+ sectionID + "'", _connection);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return dptData;
        }
        public async Task<DataTable> GetLineForRepFiltter(lineIdPayload obj)
        {
            var lineData = new DataTable();
            try
            {
                string departmentID = string.Empty;
                string sectionID = string.Empty;
                string floorID = string.Empty;
                if(obj.departmentID.Length >0)
                {
                    var departmentArr = new List<int>();
                    obj.departmentID.ToList().ForEach(departmentID =>
                    {
                        departmentArr.Add(departmentID);
                    });
                    departmentID = " and oi_department in("+ string.Join(",", departmentArr) + ")";
                }
                if (obj.sectionID.Length > 0)
                {
                    var sectionArr = new List<int>();
                    obj.sectionID.ToList().ForEach(sectionID =>
                    {
                        sectionArr.Add(sectionID);
                    });
                    sectionID = " and oi_section in("+ string.Join(",", sectionArr) + ")";
                }
                if (obj.floorID.Length > 0)
                {
                    var floorArr = new List<int>();
                    foreach (var floor in obj.floorID)
                    {
                        floorArr.Add(floor);
                    }
                    floorID = string.Join(",", floorArr);
                }
                lineData = await _dgCommon.get_InformationDataTableAsync("D_Lines_ForRepFiltter " + obj.companyID + ",'" + departmentID + "','" + sectionID + "','" + floorID + "'", _connection);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return lineData;
        }
        public async Task<DataTable> GetShiftForRepFiltter(ShiftIdPayload obj)
        {
            var lineData = new DataTable();
            try
            {
                string departmentID = string.Empty;
                string sectionID = string.Empty;
                string floorID = string.Empty;
                string lineID = string.Empty;
                if (obj.departmentID.Length >0)
                {
                    var departmentArr = new List<int>();
                    obj.departmentID.ToList().ForEach(departmentID =>
                    {
                        departmentArr.Add(departmentID);
                    });
                    departmentID = " and oi_department in(" + string.Join(",", departmentArr) + ")";
                }
                if (obj.sectionID.Length >0)
                {
                    var sectionArr = new List<int>();
                    obj.sectionID.ToList().ForEach(sectionID =>
                    {
                        sectionArr.Add(sectionID);
                    });
                    sectionID = " and oi_section in(" + string.Join(",", sectionArr) + ")";
                }
                if (obj.floorID.Length >0)
                {
                    var floorArr = new List<int>();
                    obj.floorID.ToList().ForEach(floorID =>
                    {
                        floorArr.Add(floorID);
                    });
                    floorID = " and oi_floor in(" + string.Join(",", floorArr) + ")";
                }
                if(obj.lineID.Length > 0)
                {
                    var lineArr = new List<int>();
                    obj.lineID.ToList().ForEach(lineID =>
                    {
                        lineArr.Add(lineID);
                    });
                    lineID = " and oi_line in(" + string.Join(",", lineArr) + ")";
                }
                lineData = await _dgCommon.get_InformationDataTableAsync("D_Shift_ForRepFiltter " + obj.companyID + ",'" + departmentID + "','" + sectionID + "','" + floorID + "','" + lineID + "'", _connection);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return lineData;
        }
        public async Task<DataTable> GetGradeForRepFiltter(GradeIdPayload obj)
        {
            var lineData = new DataTable();
            try
            {
                string departmentID = string.Empty;
                string sectionID = string.Empty;
                string floorID = string.Empty;
                string lineID = string.Empty;
                string shiftID = string.Empty;
                if (obj.departmentID.Length > 0)
                {
                    var departmentArr = new List<int>();
                    obj.departmentID.ToList().ForEach(departmentID =>
                    {
                        departmentArr.Add(departmentID);
                    });
                    departmentID = " and oi_department in(" + string.Join(",", departmentArr) + ")";
                }
                if (obj.sectionID.Length > 0)
                {
                    var sectionArr = new List<int>();
                    obj.sectionID.ToList().ForEach(sectionID =>
                    {
                        sectionArr.Add(sectionID);
                    });
                    sectionID = " and oi_section in(" + string.Join(",", sectionArr) + ")";
                }
                if (obj.floorID.Length > 0)
                {
                    var floorArr = new List<int>();
                    obj.floorID.ToList().ForEach(floorID =>
                    {
                        floorArr.Add(floorID);
                    });
                    floorID = " and oi_floor in(" + string.Join(",", floorArr) + ")";
                }
                if (obj.lineID.Length > 0)
                {
                    var lineArr = new List<int>();
                    obj.lineID.ToList().ForEach(lineID =>
                    {
                        lineArr.Add(lineID);
                    });
                    lineID = " and oi_line in(" + string.Join(",", lineArr) + ")";
                }
                if (obj.shiftID.Length > 0)
                {
                    var shiftArr = new List<int>();
                    obj.shiftID.ToList().ForEach(shiftID =>
                    {
                        shiftArr.Add(shiftID);
                    });
                    shiftID = " and oi_shift in(" + string.Join(",", shiftArr) + ")";
                }
                lineData = await _dgCommon.get_InformationDataTableAsync("D_Grade_ForRepFiltter " + obj.companyID + ",'" + departmentID + "','" + sectionID + "','" + floorID + "','" + lineID + "','"+ shiftID + "'", _connection);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return lineData;
        }
        public async Task<DataTable> GetSalCatForRepFiltter(SalCatIdPayload obj)
        {
            var lineData = new DataTable();
            try
            {
                string departmentID = string.Empty;
                string sectionID = string.Empty;
                string floorID = string.Empty;
                string lineID = string.Empty;
                string shiftID = string.Empty;
                string gradeID = string.Empty;
                if (obj.departmentID.Length > 0)
                {
                    var departmentArr = new List<int>();
                    obj.departmentID.ToList().ForEach(departmentID =>
                    {
                        departmentArr.Add(departmentID);
                    });
                    departmentID = " and oi_department in(" + string.Join(",", departmentArr) + ")";
                }
                if (obj.sectionID.Length > 0)
                {
                    var sectionArr = new List<int>();
                    obj.sectionID.ToList().ForEach(sectionID =>
                    {
                        sectionArr.Add(sectionID);
                    });
                    sectionID = " and oi_section in(" + string.Join(",", sectionArr) + ")";
                }
                if (obj.floorID.Length > 0)
                {
                    var floorArr = new List<int>();
                    obj.floorID.ToList().ForEach(floorID =>
                    {
                        floorArr.Add(floorID);
                    });
                    floorID = " and oi_floor in(" + string.Join(",", floorArr) + ")";
                }
                if (obj.lineID.Length > 0)
                {
                    var lineArr = new List<int>();
                    obj.lineID.ToList().ForEach(lineID =>
                    {
                        lineArr.Add(lineID);
                    });
                    lineID = " and oi_line in(" + string.Join(",", lineArr) + ")";
                }
                if (obj.shiftID.Length > 0)
                {
                    var shiftArr = new List<int>();
                    obj.shiftID.ToList().ForEach(shiftID =>
                    {
                        shiftArr.Add(shiftID);
                    });
                    shiftID = " and oi_shift in(" + string.Join(",", shiftArr) + ")";
                }
                if (obj.gradeID.Length > 0)
                {
                    var gradetArr = new List<int>();
                    obj.gradeID.ToList().ForEach(gradeID =>
                    {
                        gradetArr.Add(gradeID);
                    });
                    gradeID = " and oi_garde in(" + string.Join(",", gradetArr) + ")";
                }
                lineData = await _dgCommon.get_InformationDataTableAsync("D_SalCat_ForRepFiltter " + obj.companyID + ",'" + departmentID + "','" + sectionID + "','" + floorID + "','" + lineID + "','" + shiftID + "','"+ gradeID + "'", _connection);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return lineData;
        }


        public async Task<DataTable> GetRepFiltterSection(DepartmenIdtArr obj)
        {
            var secData = new DataTable();
            try
            {
                if (obj.dptID.Length > 0)
                {
                    var dptArr = new List<int>();
                    foreach (var dptItem in obj.dptID)
                    {
                        dptArr.Add(dptItem);
                    }
                    secData = await _dgCommon.get_InformationDataTableAsync("D_Section_ForRepFiltter '"+ string.Join(",", dptArr) + "'", _connection);
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return secData;
        }
        public async Task<DataTable> GetRepFiltterLine(floorIdArr obj)
        {
            var lineData = new DataTable();
            try
            {
                if (obj.floorID.Length > 0)
                {
                    var lineArr = new List<int>();
                    foreach (var floor in obj.floorID)
                    {
                        lineArr.Add(floor);
                    }
                    lineData = await _dgCommon.get_InformationDataTableAsync("D_Line_ForRepFiltter '" + string.Join(",", lineArr) + "'", _connection);
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return lineData;
        }
        public async Task<ReturnObject> GetFilterCheckEmpNo(Rep_EmpIdNoCheckPaylod obj)
        {
            var result = new ReturnObject();
            try
            {
                if (obj.compid != 0 && !string.IsNullOrEmpty(obj.compid.ToString()))
                {
                    string departmentId = string.Empty;
                    string sectionId = string.Empty;
                    string floorId = string.Empty;
                    string lineId = string.Empty;
                    string salCatId = string.Empty;
                    string empIn = string.Empty;
                    string empNotIn = string.Empty;
                    if (obj.department.Length > 0)
                    {
                        var dptArr = new List<int>();
                        foreach (var department in obj.department)
                        {
                            dptArr.Add(department);
                        }
                        departmentId = " and oi_department in(" + string.Join(",", dptArr) + ")";
                    }
                    if (obj.section.Length > 0)
                    {
                        var sectionArr = new List<int>();
                        foreach (var section in obj.section)
                        {
                            sectionArr.Add(section);
                        }
                        sectionId = " and oi_section in(" + string.Join(",", sectionArr) + ")";
                    }
                    if (obj.floor.Length > 0)
                    {
                        var floorArr = new List<int>();
                        foreach (var floor in obj.floor)
                        {
                            floorArr.Add(floor);
                        }
                        floorId = " and oi_floor in(" + string.Join(",", floorArr) + ")";
                    }
                    if (obj.line.Length > 0)
                    {
                        var lineArr = new List<int>();
                        foreach (var line in obj.line)
                        {
                            lineArr.Add(line);
                        }
                        lineId = " and oi_line in(" + string.Join(",", lineArr) + ")";
                    }
                    if (obj.salCategory.Length > 0)
                    {
                        var salCatArr = new List<int>();
                        foreach (var salCat in obj.salCategory)
                        {
                            salCatArr.Add(salCat);
                        }
                        salCatId = " and oi_salcategory in(" + string.Join(",", salCatArr) + ")";
                    }

                    int[] empNo = !string.IsNullOrEmpty(obj.employees) ? obj.employees.Split(",").Select(int.Parse).ToArray() : null;
                    if (empNo != null)
                    {
                        foreach (var emp in empNo)
                        {
                            var listEmp = await _dgCommon.get_InformationDataTableAsync(string.Format("dg_pay_filtering_reportByEmpNo_New {0},'{1}','{2}',{3},'{4}','{5}','{6}'", obj.compid, departmentId, sectionId, obj.building, floorId, lineId, salCatId), _connection);
                            bool exists = listEmp.AsEnumerable().Where(c => c.Field<int>("emp_no").Equals(emp)).Count() > 0;
                            if (exists)
                            {
                                empIn = string.IsNullOrEmpty(empIn) ? emp.ToString() : string.Concat(empIn, ",", emp);
                            }
                            else
                            {
                                empNotIn = string.IsNullOrEmpty(empNotIn) ? emp.ToString() : string.Concat(empNotIn, ",", emp);
                            }
                        }
                        if (!string.IsNullOrEmpty(empIn) || !string.IsNullOrEmpty(empNotIn))
                        {
                            result.IsSuccess = true;
                            result.dataTable = await _dgCommon.get_InformationDataTableAsync("select emp_no as EmpNo,pi_fullname as empName,cast(1 as bit) as isGet  from dg_pay_Employee where compid=" + obj.compid + " and emp_no in(" + empIn + ")", _connection);
                            result.Message = !string.IsNullOrEmpty(empNotIn) ? string.Format("Employee({0}) Not Valid !!", empNotIn) : string.Empty;
                            return result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }            
            return result;
        }
        public async Task<bool> Print_SalReportData(ReportPrintPayload obj)
        {
            bool flag = false;
            
            try
            {
                string empID = string.Empty;
                string departmentId = string.Empty;
                string sectionId = string.Empty;
                string floorId = string.Empty;
                string lineId = string.Empty;
                string salCatID = string.Empty;
                if (obj.Compid !=0)
                {
                    if (obj.empNoFilter.Count > 0)
                    {
                        var empArr = new List<int>();
                        foreach (var item in obj.empNoFilter)
                        {
                            if (item.isGet == true)
                            {
                                empArr.Add(item.EmpNo);
                                empID = "and emp_no in(" + string.Join(",", empArr) + ")";
                            }
                            else
                            {
                                empArr.Add(item.EmpNo);
                                empID = "and emp_no not in(" + string.Join(",", empArr) + ")";
                            }
                        }
                    }
                    if (obj.Department.Length > 0)
                    {
                        var dptArr = new List<int>();
                        foreach (var department in obj.Department)
                        {
                            dptArr.Add(department);
                        }
                        departmentId = " and oi_department in(" + string.Join(",", dptArr) + ")";
                    }
                    if (obj.section.Length > 0)
                    {
                        var sectionArr = new List<int>();
                        foreach (var section in obj.section)
                        {
                            sectionArr.Add(section);
                        }
                        sectionId = " and oi_section in(" + string.Join(",", sectionArr) + ")";
                    }
                    if (obj.Floor.Length > 0)
                    {
                        var floorArr = new List<int>();
                        foreach (var floor in obj.Floor)
                        {
                            floorArr.Add(floor);
                        }
                        floorId = " and oi_floor in(" + string.Join(",", floorArr) + ")";
                    }
                    if (obj.Line.Length > 0)
                    {
                        var lineArr = new List<int>();
                        foreach (var line in obj.Line)
                        {
                            lineArr.Add(line);
                        }
                        lineId = " and oi_line in(" + string.Join(",", lineArr) + ")";
                    }
                    if (obj.salcat.Length > 0)
                    {
                        var salCatArr = new List<int>();
                        foreach (var itemSalCat in obj.salcat)
                        {
                            salCatArr.Add(itemSalCat);
                        }
                        salCatID = " and oi_salcategory in(" + string.Join(",", salCatArr) + ")";
                    }
                    await _dgCommon.saveChangesAsync("Emp_filtering_salary_New '" + obj.Compid + "','"+departmentId+"','" + sectionId + "','" + obj.Building + "','" + floorId + "','" + lineId + "','" + obj.Shift + "','" + obj.Grade + "','" + obj.Start_date + "','" + obj.End_date + "','" + obj.oi_bank + "','" + obj.Inactive_reson + "','" + obj.User + "','" + empID + "','" + salCatID + "'", _connection);
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return flag;
        }

        public async Task<DataTable> GetEmployeeInactiveReason()
        {
            var data = await _dgCommon.get_InformationDataTableAsync("select atv_id,atv_reason from dg_pay_EmpInactiveReason", _connection);
            return data;
        }
    }
}
