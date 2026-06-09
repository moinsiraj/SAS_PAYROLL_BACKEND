using BLL.Interfaces.Manager.UploadAttendances;
using BLL.Utility;
using BOL.Models;
using BOL.Models.Hubs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SharpCompress;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace DAL.Implementation.Manager.UploadAttendances
{
    public class UploadAttendancesManager : IUploadAttendancesManager
    {
        private readonly Dg_Common _dgCommon;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHubContext<CustomHubs> _hubContext;
        private readonly SqlConnection _sqlConnection;
        public UploadAttendancesManager(Dg_Common dgCommon, IWebHostEnvironment webHostEnvironment, IHubContext<CustomHubs> hubContext)
        {
            _dgCommon = dgCommon;
            _webHostEnvironment = webHostEnvironment;
            _hubContext = hubContext;
            _sqlConnection = new SqlConnection(Getway.Dg_Payroll);
        }

        public async Task<List<TextUploadMessage>> ReadAttnTextFile_New(UploadFile model)
        {
            var response = new List<TextUploadMessage>();
            int numDateIndex = 0;
            int dateLength = 0;
            int numHourIndex = 0;
            int HourLength = 0;
            int numMinuteIndex = 0;
            int MinuteLength = 0;
            int numProxyIndex = 0;
            int ProxyLength = 0;
            if (model.file.Length > 0)
            {
                await _dgCommon.saveChangesAsync(string.Format("dg_pay_processStartOrStop_insert {0},'{1}',{2},'{3}','{4}'", model.CompanyID, "att_text_upload", 1, DateTime.Now.ToString("MM/dd/yyyy"), model.userName), _sqlConnection);
                var dtAttSetup = await this.GetAttenTextSetupFull(model.CompanyID);
                if (dtAttSetup.Rows.Count > 0)
                {
                    dtAttSetup.AsEnumerable().ToList().ForEach(row =>
                    {
                        string sHead = row["txt_head"].ToString();
                        numHourIndex = sHead == "HOURS" ? int.Parse(row["txt_redStart"].ToString()) : numHourIndex;
                        HourLength = sHead == "HOURS" ? int.Parse(row["txt_redLength"].ToString()) : HourLength;
                        numMinuteIndex = sHead == "MINUTES" ? int.Parse(row["txt_redStart"].ToString()) : numMinuteIndex;
                        MinuteLength = sHead == "MINUTES" ? int.Parse(row["txt_redLength"].ToString()) : MinuteLength;
                        numProxyIndex = sHead == "EMPID" ? int.Parse(row["txt_redStart"].ToString()) : numProxyIndex;
                        ProxyLength = sHead == "EMPID" ? int.Parse(row["txt_redLength"].ToString()) : ProxyLength;
                        numDateIndex = sHead == "ATT_DATE" ? int.Parse(row["txt_redStart"].ToString()) : numDateIndex;
                        dateLength = sHead == "ATT_DATE" ? int.Parse(row["txt_redLength"].ToString()) : ProxyLength;
                    });
                    string filepath = $"{_webHostEnvironment.WebRootPath}\\TextFileUpload\\";
                    string[] fileArr = new string[] { filepath, model.CompanyID.ToString(), "_", null, null, null, null, null, ".txt" };
                    fileArr[3] = DateTime.Now.Day.ToString();
                    fileArr[4] = DateTime.Now.Month.ToString();
                    fileArr[5] = DateTime.Now.Year.ToString();
                    fileArr[6] = DateTime.Now.Minute.ToString();
                    fileArr[7] = DateTime.Now.Second.ToString();
                    string fileFull = string.Concat(fileArr);
                    using (var stream = new FileStream(fileFull, FileMode.Create))
                    {
                        await model.file.CopyToAsync(stream);
                    }
                    var fileLines = File.ReadAllLines(fileFull);
                    //var epmProxid = _dgCommon.get_InformationDataTable("select emp_proxid from dg_pay_Employee where compid=" + model.CompanyID, _sqlConnection)
                    //    .Rows.OfType<DataRow>().Select(k => k[0].ToString().Trim()).ToArray();
                    //var filteredLines = fileLines.Where(line => epmProxid.Any(keyword => line.Contains(keyword.Trim()))).ToArray();

                    var epmProxid = _dgCommon.get_InformationDataTable("select emp_proxid from dg_pay_Employee where compid=" + model.CompanyID, _sqlConnection).AsEnumerable().Select(row => row["emp_proxid"].ToString().Trim()).
                        Where(id => !string.IsNullOrEmpty(id)).ToArray();
                    var filteredLines = fileLines.Where(line => epmProxid.Any(keyword => line.Contains(keyword, StringComparison.OrdinalIgnoreCase))).ToArray();

                    if (filteredLines.Length > 0)
                    {
                        int successCount = 0;
                        foreach (var line in filteredLines)
                        {
                            try
                            {
                                var dtProcessC = await _dgCommon.get_InformationDataTableAsync(string.Format("select procs_status from dg_pay_Process_startOrStop where procs_compid={0} and procs_date='{1}' and procs_type='att_text_upload'", model.CompanyID, DateTime.Now.ToString("MM/dd/yyyy")), _sqlConnection);
                                bool isProcess = (dtProcessC.Rows.Count > 0 && !string.IsNullOrEmpty(dtProcessC.Rows[0]["procs_status"].ToString())) ? bool.Parse(dtProcessC.Rows[0]["procs_status"].ToString()) : false;
                                if (isProcess)
                                {
                                    var connectionId = CustomHubs._userConnections.FirstOrDefault().Key;
                                    int res = Math.Abs((successCount * 100) / filteredLines.Length);
                                    await _hubContext.Clients.Group(string.Concat("bTxtUp_", model.CompanyID, "_", model.userName)).SendAsync("TxtUp_ReceiveProgress", res);

                                    string textFileLine = this.GetCompanyTextFormat(line, model.CompanyID);
                                    string setDate = this.MakeDateFormat(textFileLine.Substring(numDateIndex, dateLength).Trim(), model.DateFormat); //Date Format yyyy/MM/dd
                                    string setProxy = textFileLine.Substring(numProxyIndex, ProxyLength).Trim();
                                    string[] timeArr = new string[] { textFileLine.Substring(numHourIndex, HourLength).Trim(), ".", textFileLine.Substring(numMinuteIndex, MinuteLength).Trim() };
                                    string ntime = string.Concat(timeArr);
                                    if (ntime == "00.00")
                                    {
                                        ntime = ntime.Substring(0, 4);
                                        ntime = ntime.Insert(4, "1");
                                    }
                                    var dtempSerial = await this.GetEmployeeSl(setProxy, model.CompanyID);
                                    string empSerial = dtempSerial.Rows.Count > 0 ? dtempSerial.Rows[0]["emp_serial"].ToString() : string.Empty;
                                    string emp_no = dtempSerial.Rows.Count > 0 ? dtempSerial.Rows[0]["emp_no"].ToString() : string.Empty;
                                    if (empSerial != string.Empty)
                                    {
                                        var output = CheckTextFileData(int.Parse(empSerial), model.CompanyID, setDate, decimal.Parse(ntime));
                                        if (output.at_emp_serial > 0)
                                        {                                           
                                            //bool isValidTime = this.TextFileTimeValid(int.Parse(empSerial), setDate, decimal.Parse(ntime));
                                            bool isSet = await _dgCommon.saveChangesAsync(string.Format("dg_pay_Att_Insert_Textfile {0},'{1}',0,{2},'{3}',{4},'{5}'", int.Parse(empSerial), setProxy, model.CompanyID, setDate, ntime, model.userName), _sqlConnection);
                                            if (isSet)
                                            {
                                                var dtTotalP = _dgCommon.get_InformationDataTable("select isnull(count(dg_pay_Attendance.at_date),0) as ttlP from dg_pay_Attendance where dg_pay_Attendance.at_emp_serial=" + int.Parse(empSerial) + " and (RTRIM(dg_pay_Attendance.at_status_code) ='' or RTRIM(dg_pay_Attendance.at_status_code) ='LA ') and Month(dg_pay_Attendance.at_date)=Month('" + setDate + "') and Year(dg_pay_Attendance.at_date)=Year('" + setDate + "') and RTRIM(dg_pay_Attendance.at_holiday)=''", _sqlConnection);
                                                var dtTotalAB = _dgCommon.get_InformationDataTable("select isnull(count(dg_pay_Attendance.at_date),0) as ttlAB from dg_pay_Attendance  where dg_pay_Attendance.at_emp_serial=" + int.Parse(empSerial) + " and RTRIM(dg_pay_Attendance.at_status_code) ='AB' and Month(dg_pay_Attendance.at_date)=Month('" + setDate + "') and Year(dg_pay_Attendance.at_date)=Year('" + setDate + "')", _sqlConnection);
                                                var dtTotalL = _dgCommon.get_InformationDataTable("select isnull(Count(dg_pay_Attendance.at_date),0) as ttlL from dg_pay_Attendance  where dg_pay_Attendance.at_emp_serial=" + int.Parse(empSerial) + " and (RTRIM(dg_pay_Attendance.at_status_code) ='LA') and Month(dg_pay_Attendance.at_date)=Month('" + setDate + "') and Year(dg_pay_Attendance.at_date)=Year('" + setDate + "') and RTRIM(dg_pay_Attendance.at_holiday)<>'WH'", _sqlConnection);
                                                var dtTotalWH = _dgCommon.get_InformationDataTable("select isnull(count(dg_pay_Attendance.at_date),0) as ttlWH from dg_pay_Attendance  where dg_pay_Attendance.at_emp_serial=" + int.Parse(empSerial) + " and RTRIM(dg_pay_Attendance.at_holiday)='WH'  and Month(dg_pay_Attendance.at_date)=Month('" + setDate + "') and Year(dg_pay_Attendance.at_date)=Year('" + setDate + "')", _sqlConnection);
                                                var dtTotalH = _dgCommon.get_InformationDataTable("select isnull(count(dg_pay_Attendance.at_date),0) as ttlH from dg_pay_Attendance  where dg_pay_Attendance.at_emp_serial=" + int.Parse(empSerial) + " and (RTRIM(dg_pay_Attendance.at_status_code)='SH' OR RTRIM(dg_pay_Attendance.at_holiday)='SH') and Month(dg_pay_Attendance.at_date)=Month('" + setDate + "') and Year(dg_pay_Attendance.at_date)=Year('" + setDate + "')", _sqlConnection);
                                                int Present = (dtTotalP.Rows.Count > 0 && !string.IsNullOrEmpty(dtTotalP.Rows[0]["ttlP"].ToString())) ? Convert.ToInt32(dtTotalP.Rows[0]["ttlP"]) : 0;
                                                int Absent = (dtTotalAB.Rows.Count > 0 && !string.IsNullOrEmpty(dtTotalAB.Rows[0]["ttlAB"].ToString())) ? Convert.ToInt32(dtTotalAB.Rows[0]["ttlAB"]) : 0;
                                                int Late = (dtTotalL.Rows.Count > 0 && !string.IsNullOrEmpty(dtTotalL.Rows[0]["ttlL"].ToString())) ? Convert.ToInt32(dtTotalL.Rows[0]["ttlL"]) : 0;
                                                int WeekHoliday = (dtTotalWH.Rows.Count > 0 && !string.IsNullOrEmpty(dtTotalWH.Rows[0]["ttlWH"].ToString())) ? Convert.ToInt32(dtTotalWH.Rows[0]["ttlWH"]) : 0;
                                                int SpecialHoliday = (dtTotalH.Rows.Count > 0 && !string.IsNullOrEmpty(dtTotalH.Rows[0]["ttlH"].ToString())) ? Convert.ToInt32(dtTotalH.Rows[0]["ttlH"]) : 0;
                                                await _dgCommon.saveChangesAsync(string.Format("update dg_ECard_Total_Info set ttalpresent={0},ttabsent={1},total_late={2},total_WH={3},totalHoliday={4} where comid={5} and emp_id={6} and [month]=month('{7}') and [year]=year('{8}')", Present, Absent, Late, WeekHoliday, SpecialHoliday, model.CompanyID, emp_no, setDate, setDate), _sqlConnection);
                                                successCount++;
                                                response.Add(new TextUploadMessage { IsSuccess = true, Message = "Employee No(" + emp_no + ") Upload Done !!" });
                                            }
                                        }
                                        continue;
                                    }                                   
                                }
                                else
                                {
                                    response.Add(new TextUploadMessage { IsSuccess = false, Message = "Close Anther Tab Use Your UserID !!" });
                                    break;
                                }
                            }
                            catch (Exception ex)
                            {
                                ex.ToString();
                            }
                        }
                        if (successCount == 0)
                        {
                            response.Add(new TextUploadMessage { IsSuccess = false, Message = "Updated Data Not Found In Text File !!" });
                        }
                    }
                    else
                    {
                        response.Add(new TextUploadMessage { IsSuccess = false, Message = "Can Not Find Any Data In Text File !!" });
                    }
                }
                else
                {
                    response.Add(new TextUploadMessage { IsSuccess = false, Message = "Company Text File Not Setup !!" });
                }
                await _dgCommon.saveChangesAsync(string.Format("delete from dg_pay_Process_startOrStop where procs_compid={0} and procs_date='{1}' and procs_type='att_text_upload'", model.CompanyID, DateTime.Now.ToString("MM/dd/yyyy")), _sqlConnection);
            }
            else
            {
                response.Add(new TextUploadMessage { IsSuccess = false, Message = "You Can Not Upload Any File !!" });
            }
            return response.GroupBy(G => G.Message).Select(S => S.First()).OrderBy(O => O.IsSuccess).ToList();
        }
        public async Task<List<TextUploadMessage>> ReadAttnTextFile_New2(UploadFile model)
        {
            var response = new List<TextUploadMessage>();
            if (Path.GetExtension(model.file.FileName) == ".txt" || Path.GetExtension(model.file.FileName) == ".xlsx")
            {
                int numDateIndex = 0;
                int dateLength = 0;
                int numHourIndex = 0;
                int HourLength = 0;
                int numMinuteIndex = 0;
                int MinuteLength = 0;
                int numProxyIndex = 0;
                int ProxyLength = 0;
                if (model.file.Length > 0)
                {
                    var checkProcess = await GetIsAllProcessContinue(model.CompanyID, DateTime.Now.ToString("MM/dd/yyyy"), "att_text_upload", model.userName);
                    if (checkProcess.dataTable != null)
                    {
                        response.Add(new TextUploadMessage { IsSuccess = false, Message = "Someone Is Processing !!" });
                        return response;
                    }
                    await _dgCommon.saveChangesAsync(string.Format("dg_pay_processStartOrStop_insert {0},'{1}',{2},'{3}','{4}'", model.CompanyID, "att_text_upload", 1, DateTime.Now.ToString("MM/dd/yyyy"), model.userName), _sqlConnection);
                    var dtAttSetup = await this.GetAttenTextSetupFull(model.CompanyID);
                    if (dtAttSetup.Rows.Count > 0)
                    {
                        dtAttSetup.AsEnumerable().ToList().ForEach(row =>
                        {
                            string sHead = row["txt_head"].ToString();
                            numHourIndex = sHead == "HOURS" ? int.Parse(row["txt_redStart"].ToString()) : numHourIndex;
                            HourLength = sHead == "HOURS" ? int.Parse(row["txt_redLength"].ToString()) : HourLength;
                            numMinuteIndex = sHead == "MINUTES" ? int.Parse(row["txt_redStart"].ToString()) : numMinuteIndex;
                            MinuteLength = sHead == "MINUTES" ? int.Parse(row["txt_redLength"].ToString()) : MinuteLength;
                            numProxyIndex = sHead == "EMPID" ? int.Parse(row["txt_redStart"].ToString()) : numProxyIndex;
                            ProxyLength = sHead == "EMPID" ? int.Parse(row["txt_redLength"].ToString()) : ProxyLength;
                            numDateIndex = sHead == "ATT_DATE" ? int.Parse(row["txt_redStart"].ToString()) : numDateIndex;
                            dateLength = sHead == "ATT_DATE" ? int.Parse(row["txt_redLength"].ToString()) : ProxyLength;
                        });
                        string filepath = $"{_webHostEnvironment.WebRootPath}\\TextFileUpload\\";
                        string[] fileArr = new string[] { filepath, model.CompanyID.ToString(), "_", null, null, null, null, null, ".txt" };
                        fileArr[3] = DateTime.Now.Day.ToString();
                        fileArr[4] = DateTime.Now.Month.ToString();
                        fileArr[5] = DateTime.Now.Year.ToString();
                        fileArr[6] = DateTime.Now.Minute.ToString();
                        fileArr[7] = DateTime.Now.Second.ToString();
                        string fileFull = string.Concat(fileArr);
                        //using (var stream = new FileStream(fileFull, FileMode.Create))
                        //{
                        //    await model.file.CopyToAsync(stream);
                        //}

                        if (Path.GetExtension(model.file.FileName) == ".xlsx")
                        {
                            var worksheet = await _dgCommon.GetExcelWorkSheet(model.file, 0);
                            var rowCount = worksheet.Dimension.Rows;
                            if (rowCount != 0)
                            {
                                using (FileStream fs = File.Create(fileFull))
                                using (var writer = new StreamWriter(fs))
                                {
                                    for (int row = 2; row <= rowCount; row++)
                                    {
                                        if (!string.IsNullOrEmpty(worksheet.Cells[row, 1].Text) && !string.IsNullOrEmpty(worksheet.Cells[row, 2].Text))
                                        {
                                            var rowValues = string.Concat(worksheet.Cells[row, 1].Value, worksheet.Cells[row, 2].Value);
                                            writer.WriteLine(rowValues);
                                        }
                                    }
                                }
                            }                           

                            //For CSV
                            /*string csvFilePath = $"{_webHostEnvironment.WebRootPath}\\AttenCsvFileUpload\\";
                            if (!Directory.Exists(csvFilePath))
                            {
                                Directory.CreateDirectory(csvFilePath);
                            }
                            string[] filexlsxArr = new string[] { csvFilePath, model.CompanyID.ToString(), "_", null, null, null, null, null, model.file.FileName };
                            fileArr[3] = DateTime.Now.Day.ToString();
                            fileArr[4] = DateTime.Now.Month.ToString();
                            fileArr[5] = DateTime.Now.Year.ToString();
                            fileArr[6] = DateTime.Now.Minute.ToString();
                            fileArr[7] = DateTime.Now.Second.ToString();
                            string csvFilePathFull = string.Concat(filexlsxArr);
                            using (var csvStream = new FileStream(csvFilePathFull, FileMode.Create))
                            {
                                await model.file.CopyToAsync(csvStream);
                            }
                            string[] csvLines = File.ReadAllLines(csvFilePathFull);
                            for (int i = 0; i < csvLines.Length; i++)
                            {
                                csvLines[i] = csvLines[i].Replace("\t", string.Empty).Replace("\r", string.Empty).Replace("\n", string.Empty).Replace(":", string.Empty).Replace(" ", string.Empty).Replace(",", string.Empty).Replace("|", string.Empty).Replace("-", string.Empty).Replace("/", string.Empty).Replace("[", string.Empty).Replace("]", string.Empty);
                            }
                            using (FileStream fs = File.Create(fileFull))
                            using (StreamWriter writer = new StreamWriter(fs))
                            {
                                foreach (string line in csvLines)
                                {
                                    writer.WriteLine(line);
                                }
                            }*/
                        }
                        else
                        {
                            using (var stream = new FileStream(fileFull, FileMode.Create))
                            {
                                await model.file.CopyToAsync(stream);
                            }
                        }


                        var fileLines = File.ReadAllLines(fileFull);
                        //var epmProxid = _dgCommon.get_InformationDataTable("select emp_proxid from dg_pay_Employee where compid=" + model.CompanyID, _sqlConnection)
                        //    .Rows.OfType<DataRow>().Select(k => k[0].ToString().Trim()).ToArray();

                        var epmProxid = _dgCommon.get_InformationDataTable(string.Format("select emp_proxid from dg_pay_Employee where compid={0}", model.CompanyID), _sqlConnection).AsEnumerable().Select(row => row["emp_proxid"].ToString().Trim()).
                            Where(id => !string.IsNullOrEmpty(id)).ToArray();
                        var filteredLines = fileLines.Where(line => epmProxid.Any(keyword => line.Contains(keyword, StringComparison.OrdinalIgnoreCase))).ToArray();
                        if (filteredLines.Length > 0)
                        {
                            var attnTable = AttendanceModel.AddNewDataColumn();
                            foreach (var line in filteredLines)
                            {
                                try
                                {
                                    var dtProcessC = await _dgCommon.get_InformationDataTableAsync(string.Format("select procs_status from dg_pay_Process_startOrStop where procs_compid={0} and procs_date='{1}' and procs_type='att_text_upload'", model.CompanyID, DateTime.Now.ToString("MM/dd/yyyy")), _sqlConnection);
                                    bool isProcess = (dtProcessC.Rows.Count > 0 && !string.IsNullOrEmpty(dtProcessC.Rows[0]["procs_status"].ToString())) ? bool.Parse(dtProcessC.Rows[0]["procs_status"].ToString()) : false;
                                    if (isProcess)
                                    {
                                        await _hubContext.Clients.Group(string.Concat("bTxtUp_", model.CompanyID, "_", model.userName)).SendAsync("TxtUp_ReceiveProgress", 0);
                                        string textFileLine = this.GetCompanyTextFormat(line, model.CompanyID);
                                        string setDate = this.MakeDateFormat(textFileLine.Substring(numDateIndex, dateLength).Trim(), model.DateFormat); //Date Format yyyy/MM/dd
                                        string setProxy = textFileLine.Substring(numProxyIndex, ProxyLength).Trim();
                                        string[] timeArr = new string[] { textFileLine.Substring(numHourIndex, HourLength).Trim(), ".", textFileLine.Substring(numMinuteIndex, MinuteLength).Trim() };
                                        string ntime = string.Concat(timeArr);
                                        if (ntime == "00.00")
                                        {
                                            ntime = ntime.Substring(0, 4);
                                            ntime = ntime.Insert(4, "1");
                                        }
                                        var dtempSerial = await this.GetEmployeeSl(setProxy, model.CompanyID);
                                        string empSerial = dtempSerial.Rows.Count > 0 ? dtempSerial.Rows[0]["emp_serial"].ToString() : string.Empty;
                                        string emp_no = dtempSerial.Rows.Count > 0 ? dtempSerial.Rows[0]["emp_no"].ToString() : string.Empty;
                                        if (empSerial != string.Empty)
                                        {
                                            var output = CheckTextFileData(int.Parse(empSerial), model.CompanyID, setDate, decimal.Parse(ntime));
                                            if (output.at_emp_serial > 0)
                                            {
                                                string resIndate = Convert.ToDateTime(output.at_date).ToString("yyyy/MM/dd");
                                                string resOutdate = Convert.ToDateTime(output.at_outdate).ToString("yyyy/MM/dd");
                                                if (attnTable.AsEnumerable().Any(r => int.Parse(r["at_emp_serial"].ToString()) == output.at_emp_serial && r["at_date"].ToString() == resIndate))
                                                {
                                                    attnTable.AsEnumerable().Where(w => int.Parse(w["at_emp_serial"].ToString()) == output.at_emp_serial && w["at_date"].ToString() == resIndate).ToList().ForEach(row =>
                                                    {
                                                        //For InTime
                                                        if (output.at_intime > 0 && output.at_intime < decimal.Parse(row["at_intime"].ToString()))
                                                        {
                                                            decimal lateTime = CalLateTimeEmployee(int.Parse(row["at_emp_serial"].ToString()), row["at_date"].ToString(), output.at_intime);
                                                            row["at_intime"] = output.at_intime;
                                                            row["at_late"] = lateTime;
                                                            row["at_status"] = lateTime > 0 ? "Late" : string.Empty;
                                                            row["at_status_code"] = lateTime > 0 ? "LA" : string.Empty;
                                                            row["at_text_intime"] = output.at_intime;
                                                            row["at_text_outtime"] = output.at_outtime;
                                                            row["at_text_out_date"] = resOutdate;
                                                            row["at_manual_in"] = output.at_manual_in;
                                                            row["at_manual_in_by"] = output.at_manual_in_by;
                                                            row["at_manual_in_date"] = output.at_manual_in_date;
                                                            row["at_manual_out"] = output.at_manual_out;
                                                            row["at_manual_out_by"] = output.at_manual_out_by;
                                                            row["at_manual_out_date"] = output.at_manual_out_date;
                                                        }
                                                        else
                                                        {
                                                            if (output.at_intime > 0 && decimal.Parse(row["at_intime"].ToString()) == 0 && output.at_intime > decimal.Parse(row["at_intime"].ToString()))
                                                            {
                                                                decimal lateTime = CalLateTimeEmployee(int.Parse(row["at_emp_serial"].ToString()), row["at_date"].ToString(), output.at_intime);
                                                                row["at_intime"] = output.at_intime;
                                                                row["at_late"] = lateTime;
                                                                row["at_status"] = lateTime > 0 ? "Late" : string.Empty;
                                                                row["at_status_code"] = lateTime > 0 ? "LA" : string.Empty;
                                                                row["at_text_intime"] = output.at_intime;
                                                                row["at_text_outtime"] = output.at_outtime;
                                                                row["at_text_out_date"] = resOutdate;
                                                                row["at_manual_in"] = output.at_manual_in;
                                                                row["at_manual_in_by"] = output.at_manual_in_by;
                                                                row["at_manual_in_date"] = output.at_manual_in_date;
                                                                row["at_manual_out"] = output.at_manual_out;
                                                                row["at_manual_out_by"] = output.at_manual_out_by;
                                                                row["at_manual_out_date"] = output.at_manual_out_date;
                                                            }
                                                        }

                                                        //For OutTime
                                                        if (output.at_outtime > 0 && row["at_outdate"].ToString() == resOutdate && output.at_outtime > decimal.Parse(row["at_outtime"].ToString()))
                                                        {
                                                            row["at_outtime"] = output.at_outtime;
                                                            if ((int.Parse(row["at_compid"].ToString()) == 40 || int.Parse(row["at_compid"].ToString()) == 38 || int.Parse(row["at_compid"].ToString()) == 61) && decimal.Parse(row["at_intime"].ToString()) == 0)
                                                            {
                                                                row["at_status"] = "Late";
                                                                row["at_status_code"] = "LA";
                                                            }
                                                            row["at_text_intime"] = output.at_intime;
                                                            row["at_text_outtime"] = output.at_outtime;
                                                            row["at_text_out_date"] = resOutdate;
                                                            row["at_manual_in"] = output.at_manual_in;
                                                            row["at_manual_in_by"] = output.at_manual_in_by;
                                                            row["at_manual_in_date"] = output.at_manual_in_date;
                                                            row["at_manual_out"] = output.at_manual_out;
                                                            row["at_manual_out_by"] = output.at_manual_out_by;
                                                            row["at_manual_out_date"] = output.at_manual_out_date;
                                                        }
                                                        else
                                                        {
                                                            if (output.at_outtime > 0 && row["at_outdate"].ToString() != resOutdate)
                                                            {
                                                                if (row["at_date"].ToString() == row["at_outdate"].ToString() && output.at_outtime < decimal.Parse(row["at_outtime"].ToString()))
                                                                {
                                                                    row["at_outtime"] = output.at_outtime;
                                                                    row["at_outdate"] = resOutdate;
                                                                    if ((int.Parse(row["at_compid"].ToString()) == 40 || int.Parse(row["at_compid"].ToString()) == 38 || int.Parse(row["at_compid"].ToString()) == 61) && decimal.Parse(row["at_intime"].ToString()) == 0)
                                                                    {
                                                                        row["at_status"] = "Late";
                                                                        row["at_status_code"] = "LA";
                                                                    }
                                                                    row["at_text_intime"] = output.at_intime;
                                                                    row["at_text_outtime"] = output.at_outtime;
                                                                    row["at_text_out_date"] = resOutdate;
                                                                    row["at_manual_in"] = output.at_manual_in;
                                                                    row["at_manual_in_by"] = output.at_manual_in_by;
                                                                    row["at_manual_in_date"] = output.at_manual_in_date;
                                                                    row["at_manual_out"] = output.at_manual_out;
                                                                    row["at_manual_out_by"] = output.at_manual_out_by;
                                                                    row["at_manual_out_date"] = output.at_manual_out_date;
                                                                }
                                                                else
                                                                {
                                                                    if (decimal.Parse(row["at_outtime"].ToString()) == 0 && output.at_outtime > decimal.Parse(row["at_outtime"].ToString()))
                                                                    {
                                                                        row["at_outtime"] = output.at_outtime;
                                                                        row["at_outdate"] = resOutdate;
                                                                        if ((int.Parse(row["at_compid"].ToString()) == 40 || int.Parse(row["at_compid"].ToString()) == 38 || int.Parse(row["at_compid"].ToString()) == 61) && decimal.Parse(row["at_intime"].ToString()) == 0)
                                                                        {
                                                                            row["at_status"] = "Late";
                                                                            row["at_status_code"] = "LA";
                                                                        }
                                                                        row["at_text_intime"] = output.at_intime;
                                                                        row["at_text_outtime"] = output.at_outtime;
                                                                        row["at_text_out_date"] = resOutdate;
                                                                        row["at_manual_in"] = output.at_manual_in;
                                                                        row["at_manual_in_by"] = output.at_manual_in_by;
                                                                        row["at_manual_in_date"] = output.at_manual_in_date;
                                                                        row["at_manual_out"] = output.at_manual_out;
                                                                        row["at_manual_out_by"] = output.at_manual_out_by;
                                                                        row["at_manual_out_date"] = output.at_manual_out_date;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    });
                                                }
                                                else
                                                {
                                                    AttendanceModel.SetAttDataColumnVal(attnTable, output, model.userName);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        response.Add(new TextUploadMessage { IsSuccess = false, Message = "Close Anther Tab Use Your UserID !!" });
                                        break;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    ex.ToString();
                                }
                            }

                            try
                            {
                                if (attnTable.Rows.Count > 0)
                                {
                                    var EcardSum = new List<SpecialholidayEcardSum>();
                                    int ProgressResult;
                                    _dgCommon.saveChangesByType("dg_pay_Att_Insert_Textfile_New", _sqlConnection, new SqlParameter("@attUpdateTable", attnTable));
                                    for (int k = 0; k < attnTable.Rows.Count; k++)
                                    {
                                        var dtProcessC2 = await _dgCommon.get_InformationDataTableAsync(string.Format("select procs_status from dg_pay_Process_startOrStop where procs_compid={0} and procs_date='{1}' and procs_type='att_text_upload'", model.CompanyID, DateTime.Now.ToString("MM/dd/yyyy")), _sqlConnection);
                                        bool isProcess2 = (dtProcessC2.Rows.Count > 0 && !string.IsNullOrEmpty(dtProcessC2.Rows[0]["procs_status"].ToString())) ? bool.Parse(dtProcessC2.Rows[0]["procs_status"].ToString()) : false;
                                        if (isProcess2)
                                        {
                                            int Esum_at_emp_serial = int.Parse(attnTable.Rows[k]["at_emp_serial"].ToString());
                                            int Esum_at_emp = 0;
                                            int Esum_at_compid = int.Parse(attnTable.Rows[k]["at_compid"].ToString());
                                            string Esum_at_date = attnTable.Rows[k]["at_date"].ToString();
                                            var dtEmpNo = _dgCommon.get_InformationDataTable(string.Format("select emp_no from dg_pay_Employee where emp_serial={0}", Esum_at_emp_serial), _sqlConnection);
                                            Esum_at_emp = (dtEmpNo.Rows.Count > 0 && !string.IsNullOrEmpty(dtEmpNo.Rows[0]["emp_no"].ToString())) ? int.Parse(dtEmpNo.Rows[0]["emp_no"].ToString()) : Esum_at_emp;
                                            if (_dgCommon.get_InformationDataTable(string.Format("select at_emp_serial from dg_pay_Attendance where at_emp_serial={0} and at_date='{1}'", Esum_at_emp_serial, Esum_at_date), _sqlConnection).Rows.Count > 0)
                                            {
                                                var dtTotalP = _dgCommon.get_InformationDataTable("select isnull(count(dg_pay_Attendance.at_date),0) as ttlP from dg_pay_Attendance where dg_pay_Attendance.at_emp_serial=" + Esum_at_emp_serial + " and (RTRIM(dg_pay_Attendance.at_status_code) ='' or RTRIM(dg_pay_Attendance.at_status_code) ='LA') and Month(dg_pay_Attendance.at_date)=Month('" + Esum_at_date + "') and Year(dg_pay_Attendance.at_date)=Year('" + Esum_at_date + "') and RTRIM(dg_pay_Attendance.at_holiday)=''", _sqlConnection);
                                                var dtTotalAB = _dgCommon.get_InformationDataTable("select isnull(count(dg_pay_Attendance.at_date),0) as ttlAB from dg_pay_Attendance  where dg_pay_Attendance.at_emp_serial=" + Esum_at_emp_serial + " and RTRIM(dg_pay_Attendance.at_status_code) ='AB' and Month(dg_pay_Attendance.at_date)=Month('" + Esum_at_date + "') and Year(dg_pay_Attendance.at_date)=Year('" + Esum_at_date + "')", _sqlConnection);
                                                var dtTotalL = _dgCommon.get_InformationDataTable("select isnull(Count(dg_pay_Attendance.at_date),0) as ttlL from dg_pay_Attendance  where dg_pay_Attendance.at_emp_serial=" + Esum_at_emp_serial + " and (RTRIM(dg_pay_Attendance.at_status_code) ='LA') and Month(dg_pay_Attendance.at_date)=Month('" + Esum_at_date + "') and Year(dg_pay_Attendance.at_date)=Year('" + Esum_at_date + "') and RTRIM(dg_pay_Attendance.at_holiday)<>'WH'", _sqlConnection);
                                                var dtTotalWH = _dgCommon.get_InformationDataTable("select isnull(count(dg_pay_Attendance.at_date),0) as ttlWH from dg_pay_Attendance  where dg_pay_Attendance.at_emp_serial=" + Esum_at_emp_serial + " and RTRIM(dg_pay_Attendance.at_holiday)='WH'  and Month(dg_pay_Attendance.at_date)=Month('" + Esum_at_date + "') and Year(dg_pay_Attendance.at_date)=Year('" + Esum_at_date + "')", _sqlConnection);
                                                var dtTotalH = _dgCommon.get_InformationDataTable("select isnull(count(dg_pay_Attendance.at_date),0) as ttlH from dg_pay_Attendance  where dg_pay_Attendance.at_emp_serial=" + Esum_at_emp_serial + " and (RTRIM(dg_pay_Attendance.at_status_code)='SH' OR RTRIM(dg_pay_Attendance.at_holiday)='SH') and Month(dg_pay_Attendance.at_date)=Month('" + Esum_at_date + "') and Year(dg_pay_Attendance.at_date)=Year('" + Esum_at_date + "')", _sqlConnection);
                                                EcardSum.Add(new SpecialholidayEcardSum
                                                {
                                                    comp_id = Esum_at_compid,
                                                    emp_id = Esum_at_emp,
                                                    Present = (dtTotalP.Rows.Count > 0 && !string.IsNullOrEmpty(dtTotalP.Rows[0]["ttlP"].ToString())) ? Convert.ToInt32(dtTotalP.Rows[0]["ttlP"]) : 0,
                                                    Absent = (dtTotalAB.Rows.Count > 0 && !string.IsNullOrEmpty(dtTotalAB.Rows[0]["ttlAB"].ToString())) ? Convert.ToInt32(dtTotalAB.Rows[0]["ttlAB"]) : 0,
                                                    Late = (dtTotalL.Rows.Count > 0 && !string.IsNullOrEmpty(dtTotalL.Rows[0]["ttlL"].ToString())) ? Convert.ToInt32(dtTotalL.Rows[0]["ttlL"]) : 0,
                                                    WeekHoliday = (dtTotalWH.Rows.Count > 0 && !string.IsNullOrEmpty(dtTotalWH.Rows[0]["ttlWH"].ToString())) ? Convert.ToInt32(dtTotalWH.Rows[0]["ttlWH"]) : 0,
                                                    SpecialHoliday = (dtTotalH.Rows.Count > 0 && !string.IsNullOrEmpty(dtTotalH.Rows[0]["ttlH"].ToString())) ? Convert.ToInt32(dtTotalH.Rows[0]["ttlH"]) : 0,
                                                    shDate = Esum_at_date
                                                });
                                                response.Add(new TextUploadMessage { IsSuccess = true, Message = "Employee No(" + Esum_at_emp + ") Upload Date(" + Convert.ToDateTime(Esum_at_date).ToString("dd/MMM/yyyy") + ") !!" });
                                                ProgressResult = Math.Abs(((k + 1) * 100) / attnTable.Rows.Count);
                                                if (ProgressResult > 1 && ProgressResult <= 99)
                                                    await _hubContext.Clients.Group(string.Concat("bTxtUp_", model.CompanyID, "_", model.userName)).SendAsync("TxtUp_ReceiveProgress", ProgressResult);
                                            }
                                            else
                                            {
                                                response.Add(new TextUploadMessage { IsSuccess = false, Message = "Employee No(" + Esum_at_emp + ") Attendance Row Not Found Date(" + Convert.ToDateTime(Esum_at_date).ToString("dd/MMM/yyyy") + ") !!" });
                                            }
                                            continue;
                                        }
                                        else
                                        {
                                            response.Add(new TextUploadMessage { IsSuccess = false, Message = "Close Anther Tab Use Your UserID !!" });
                                            break;
                                        }
                                    }
                                    var dtEcardSum = _dgCommon.ListToDataTable<SpecialholidayEcardSum>(EcardSum);
                                    if (dtEcardSum.Rows.Count > 0)
                                    {
                                        _dgCommon.saveChangesByType("Dg_Pay_EcardSum_For_SH", _sqlConnection, new SqlParameter("@ecardSumTable", dtEcardSum));
                                    }
                                    await _hubContext.Clients.Group(string.Concat("bTxtUp_", model.CompanyID, "_", model.userName)).SendAsync("TxtUp_ReceiveProgress", 100);
                                }
                                else
                                {
                                    if (File.Exists(fileFull))
                                    {
                                        File.Delete(fileFull);
                                    }
                                    response.Add(new TextUploadMessage { IsSuccess = false, Message = "Updated Data Not Found In Text File !!" });
                                }
                            }
                            catch (Exception ex)
                            {
                                await _dgCommon.saveChangesAsync(string.Format("delete from dg_pay_Process_startOrStop where procs_compid={0} and procs_date='{1}' and procs_type='att_text_upload'", model.CompanyID, DateTime.Now.ToString("MM/dd/yyyy")), _sqlConnection);
                                ex.ToString();
                            }
                        }
                        else
                        {
                            response.Add(new TextUploadMessage { IsSuccess = false, Message = "Can Not Find Any Data In Text File !!" });
                        }
                    }
                    else
                    {
                        response.Add(new TextUploadMessage { IsSuccess = false, Message = "Company Text File Not Setup !!" });
                    }
                    await _dgCommon.saveChangesAsync(string.Format("delete from dg_pay_Process_startOrStop where procs_compid={0} and procs_date='{1}' and procs_type='att_text_upload'", model.CompanyID, DateTime.Now.ToString("MM/dd/yyyy")), _sqlConnection);
                }
                else
                {
                    response.Add(new TextUploadMessage { IsSuccess = false, Message = "You Can Not Upload Any File !!" });
                }
            }
            else
            {
                response.Add(new TextUploadMessage { IsSuccess = false, Message = "File Format Not Valid !!" });
            }
            return response.GroupBy(G => G.Message).Select(S => S.First()).OrderBy(O => O.IsSuccess).ToList();
        }
        public async Task<List<TextUploadMessage>> ReadAttnTextFile_New3(UploadFile model)
        {
            var response = new List<TextUploadMessage>();
            var tasks = new List<Task>();
            var semaphore = new SemaphoreSlim(30);
            try
            {
                if (Path.GetExtension(model.file.FileName) == ".txt" || Path.GetExtension(model.file.FileName) == ".xlsx")
                {
                    int numDateIndex = 0;
                    int dateLength = 0;
                    int numHourIndex = 0;
                    int HourLength = 0;
                    int numMinuteIndex = 0;
                    int MinuteLength = 0;
                    int numProxyIndex = 0;
                    int ProxyLength = 0;
                    if (model.file.Length > 0)
                    {
                        await _hubContext.Clients.Group(string.Concat("bTxtUp_", model.CompanyID, "_", model.userName)).SendAsync("TxtUp_ReceiveProgress", 0);
                        var checkProcess = await GetIsAllProcessContinue(model.CompanyID, DateTime.Now.ToString("MM/dd/yyyy"), "att_text_upload", model.userName);
                        if (checkProcess.dataTable != null)
                        {
                            response.Add(new TextUploadMessage { IsSuccess = false, Message = "Someone Is Processing !!" });
                            return response;
                        }
                        var dtCheckSalProc = await _dgCommon.get_InformationDataTableAsync(string.Format("select procs_compid from dg_pay_Process_startOrStop where procs_compid={0} and procs_type in('emp_sal_process_bulk','emp_sal_process_Single') group by procs_compid", model.CompanyID), _sqlConnection);
                        if (dtCheckSalProc.Rows.Count > 0)
                        {
                            response.Add(new TextUploadMessage { IsSuccess = false, Message = "Salary Process In Progress,Please Wait... !!" });
                            return response;
                        }
                        var dtCheckOtProc = await _dgCommon.get_InformationDataTableAsync(string.Format("select procs_compid from dg_pay_Process_startOrStop where procs_compid={0} and procs_type in('att_ot_process','att_Hdot_process') group by procs_compid", model.CompanyID), _sqlConnection);
                        if (dtCheckOtProc.Rows.Count > 0)
                        {
                            response.Add(new TextUploadMessage { IsSuccess = false, Message = "OT Process In Progress,Please Wait... !!" });
                            return response;
                        }

                        await _dgCommon.saveChangesAsync(string.Format("dg_pay_processStartOrStop_insert {0},'{1}',{2},'{3}','{4}'", model.CompanyID, "att_text_upload", 1, DateTime.Now.ToString("MM/dd/yyyy"), model.userName), _sqlConnection);
                        var dtAttSetup = await this.GetAttenTextSetupFull(model.txt_formatID);
                        if (dtAttSetup.Rows.Count > 0)
                        {
                            numHourIndex = int.Parse(dtAttSetup.Rows[0]["txt_hrs_start"].ToString());
                            HourLength = int.Parse(dtAttSetup.Rows[0]["txt_hrs_end"].ToString());
                            numMinuteIndex = int.Parse(dtAttSetup.Rows[0]["txt_min_start"].ToString());
                            MinuteLength = int.Parse(dtAttSetup.Rows[0]["txt_min_end"].ToString());
                            numProxyIndex = int.Parse(dtAttSetup.Rows[0]["txt_proxid_start"].ToString());
                            ProxyLength = int.Parse(dtAttSetup.Rows[0]["txt_proxid_end"].ToString());
                            numDateIndex = int.Parse(dtAttSetup.Rows[0]["txt_dt_start"].ToString());
                            dateLength = int.Parse(dtAttSetup.Rows[0]["txt_dt_end"].ToString());

                            //dtAttSetup.AsEnumerable().ToList().ForEach(row =>
                            //{
                            //    string sHead = row["txt_head"].ToString();
                            //    numHourIndex = sHead == "HOURS" ? int.Parse(row["txt_redStart"].ToString()) : numHourIndex;
                            //    HourLength = sHead == "HOURS" ? int.Parse(row["txt_redLength"].ToString()) : HourLength;
                            //    numMinuteIndex = sHead == "MINUTES" ? int.Parse(row["txt_redStart"].ToString()) : numMinuteIndex;
                            //    MinuteLength = sHead == "MINUTES" ? int.Parse(row["txt_redLength"].ToString()) : MinuteLength;
                            //    numProxyIndex = sHead == "EMPID" ? int.Parse(row["txt_redStart"].ToString()) : numProxyIndex;
                            //    ProxyLength = sHead == "EMPID" ? int.Parse(row["txt_redLength"].ToString()) : ProxyLength;
                            //    numDateIndex = sHead == "ATT_DATE" ? int.Parse(row["txt_redStart"].ToString()) : numDateIndex;
                            //    dateLength = sHead == "ATT_DATE" ? int.Parse(row["txt_redLength"].ToString()) : ProxyLength;
                            //});


                            string filepath = $"{_webHostEnvironment.WebRootPath}\\TextFileUpload\\";
                            string[] fileArr = new string[] { filepath, model.CompanyID.ToString(), "_", null, null, null, null, null, ".txt" };
                            fileArr[3] = DateTime.Now.Day.ToString();
                            fileArr[4] = DateTime.Now.Month.ToString();
                            fileArr[5] = DateTime.Now.Year.ToString();
                            fileArr[6] = DateTime.Now.Minute.ToString();
                            fileArr[7] = DateTime.Now.Second.ToString();
                            string fileFull = string.Concat(fileArr);

                            if (Path.GetExtension(model.file.FileName) == ".xlsx")
                            {
                                var worksheet = await _dgCommon.GetExcelWorkSheet(model.file, 0);
                                var rowCount = worksheet.Dimension.Rows;
                                if (rowCount != 0)
                                {
                                    using (FileStream fs = File.Create(fileFull))
                                    using (var writer = new StreamWriter(fs))
                                    {
                                        for (int row = 2; row <= rowCount; row++)
                                        {
                                            if (!string.IsNullOrEmpty(worksheet.Cells[row, 1].Text) && !string.IsNullOrEmpty(worksheet.Cells[row, 2].Text))
                                            {
                                                var rowValues = string.Concat(worksheet.Cells[row, 1].Value, worksheet.Cells[row, 2].Value);
                                                writer.WriteLine(rowValues);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                using (var stream = new FileStream(fileFull, FileMode.Create))
                                {
                                    await model.file.CopyToAsync(stream);
                                }
                            }

                            var fileLines = File.ReadAllLines(fileFull);
                            var epmProxid = _dgCommon.get_InformationDataTable(string.Format("select emp_proxid from dg_pay_Employee where compid={0}", model.CompanyID), _sqlConnection).AsEnumerable().Select(row => row["emp_proxid"].ToString().Trim()).
                                Where(id => !string.IsNullOrEmpty(id)).ToArray();
                            var filteredLines = fileLines.Where(line => epmProxid.Any(keyword => line.Contains(keyword, StringComparison.OrdinalIgnoreCase))).ToArray();
                            if (filteredLines.Length > 0)
                            {
                                var employeeSerialMap = _dgCommon.get_InformationDataTable(string.Format("select emp_serial,emp_no,emp_proxid from dg_pay_Employee where compid={0} and oi_active=1", model.CompanyID), _sqlConnection)
                                    .AsEnumerable().ToDictionary(row => row["emp_proxid"].ToString().Trim(), row => new { emp_serial = row["emp_serial"].ToString(), emp_no = row["emp_no"].ToString() });
                                var resEmpNo = new List<TextUploadResponseEmp>();
                                foreach (var line in filteredLines)
                                {
                                    try
                                    {
                                        var dtProcessC = await _dgCommon.get_InformationDataTableAsync(string.Format("select procs_status from dg_pay_Process_startOrStop where procs_compid={0} and procs_date='{1}' and procs_type='att_text_upload'", model.CompanyID, DateTime.Now.ToString("MM/dd/yyyy")), _sqlConnection);
                                        bool isProcess = (dtProcessC.Rows.Count > 0 && !string.IsNullOrEmpty(dtProcessC.Rows[0]["procs_status"].ToString())) ? bool.Parse(dtProcessC.Rows[0]["procs_status"].ToString()) : false;
                                        if (isProcess)
                                        {
                                            //string textFileLine = this.GetCompanyTextFormat(line, model.CompanyID);
                                            string textFileLine = this.GetCompanyTextFormat(line, model.txt_formatID);
                                            string setDate = this.MakeDateFormat(textFileLine.Substring(numDateIndex, dateLength).Trim(), model.DateFormat); //Date Format yyyy/MM/dd
                                            string setProxy = textFileLine.Substring(numProxyIndex, ProxyLength).Trim();
                                            string[] timeArr = new string[] { textFileLine.Substring(numHourIndex, HourLength).Trim(), ".", textFileLine.Substring(numMinuteIndex, MinuteLength).Trim() };
                                            string ntime = string.Concat(timeArr);
                                            if (ntime == "00.00")
                                            {
                                                ntime = ntime.Substring(0, 4);
                                                ntime = ntime.Insert(4, "1");
                                            }
                                            if (employeeSerialMap.TryGetValue(setProxy, out var emp))
                                            {
                                                string empSerial = emp.emp_serial;
                                                string emp_no = emp.emp_no;
                                                await semaphore.WaitAsync();
                                                if (!string.IsNullOrEmpty(empSerial))
                                                {
                                                    var task = Task.Run(async () =>
                                                    {
                                                        try
                                                        {
                                                            bool flag = await _dgCommon.saveChangesUseConStringAsync(string.Format("dg_pay_Att_Insert_Textfile {0},{1},'{2}',{3},'{4}',1", int.Parse(empSerial), model.CompanyID, setDate, ntime, model.userName), Getway.Dg_Payroll);
                                                            if (flag)
                                                            {
                                                                lock (resEmpNo)
                                                                {
                                                                    resEmpNo.Add(new TextUploadResponseEmp { emp_no = int.Parse(emp_no), sDate = setDate });
                                                                }
                                                            }
                                                        }
                                                        finally
                                                        { 
                                                            semaphore.Release();
                                                        }                                                       
                                                    });
                                                    tasks.Add(task);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            response.Add(new TextUploadMessage { IsSuccess = false, Message = "Close Anther Tab Use Your UserID !!" });
                                            break;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        ex.ToString();
                                    }
                                }
                                await Task.WhenAll(tasks);
                                if (resEmpNo.Count > 0)
                                {
                                    var resEmpNoFinal = resEmpNo.DistinctBy(x => x.emp_no).ToList();
                                    var dtEsumTbl = TextUploadResponseEmp.AddNewEcardDataColumn();
                                    tasks = new List<Task>();
                                    for (int i = 0; i < resEmpNoFinal.Count; i++)
                                    {
                                        var dtProcess = await _dgCommon.get_InformationDataTableAsync(string.Format("select procs_status from dg_pay_Process_startOrStop where procs_compid={0} and procs_date='{1}' and procs_type='att_text_upload'", model.CompanyID, DateTime.Now.ToString("MM/dd/yyyy")), _sqlConnection);
                                        bool isProcess = (dtProcess.Rows.Count > 0 && !string.IsNullOrEmpty(dtProcess.Rows[0]["procs_status"].ToString())) ? bool.Parse(dtProcess.Rows[0]["procs_status"].ToString()) : false;
                                        if (isProcess)
                                        {
                                            int emp_no = resEmpNoFinal[i].emp_no;
                                            string procs_date = resEmpNoFinal[i].sDate;
                                            await semaphore.WaitAsync();
                                            var task = Task.Run(async () =>
                                            {
                                                try
                                                {
                                                    var dtAttEsum = await _dgCommon.get_InfoDataTableUseConStringAsync(string.Format("dg_ECardEmpWiseSumm_Indivisual '{0}',{1},{2},1", procs_date, model.CompanyID, emp_no), Getway.Dg_Payroll);
                                                    if (dtAttEsum.Rows.Count > 0)
                                                    {
                                                        lock (dtEsumTbl)
                                                        {
                                                            TextUploadResponseEmp.SetEcardDataColumnVal(dtEsumTbl, dtAttEsum, procs_date);
                                                        }
                                                    }
                                                }
                                                finally
                                                {
                                                    semaphore.Release();
                                                }
                                            });
                                            tasks.Add(task);
                                            response.Add(new TextUploadMessage { IsSuccess = true, Message = "Employee No(" + emp_no + ") Upload Date(" + Convert.ToDateTime(procs_date).ToString("dd/MMM/yyyy") + ") !!" });
                                            int res = Math.Abs(((i + 1) * 100) / resEmpNoFinal.Count);
                                            if (res > 1 && res <= 99)
                                                await _hubContext.Clients.Group(string.Concat("bTxtUp_", model.CompanyID, "_", model.userName)).SendAsync("TxtUp_ReceiveProgress", res);
                                        }
                                        else
                                        {
                                            response.Add(new TextUploadMessage { IsSuccess = false, Message = "Close Anther Tab Use Your UserID !!" });
                                            break;
                                        }
                                    }
                                    await Task.WhenAll(tasks);
                                    if (dtEsumTbl.Rows.Count > 0)
                                    {
                                        _dgCommon.saveChangesByType("Dg_Pay_EcardSum_For_SH", _sqlConnection, new SqlParameter("@ecardSumTable", dtEsumTbl));
                                    }
                                }
                                else
                                {
                                    response.Add(new TextUploadMessage { IsSuccess = false, Message = "Can Not Find Any New Data In Text File !!" });
                                }
                            }
                            else
                            {
                                response.Add(new TextUploadMessage { IsSuccess = false, Message = "Can Not Find Any Data In Text File !!" });
                            }
                        }
                        else
                        {
                            response.Add(new TextUploadMessage { IsSuccess = false, Message = "Company Text File Not Setup !!" });
                        }
                    }
                    else
                    {
                        response.Add(new TextUploadMessage { IsSuccess = false, Message = "You Can Not Upload Any File !!" });
                    }
                }
                else
                {
                    response.Add(new TextUploadMessage { IsSuccess = false, Message = "File Format Not Valid !!" });
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                response.Add(new TextUploadMessage { IsSuccess = false, Message = "Something Went Wrong !!" });
            }
            finally
            {
                await _dgCommon.saveChangesAsync(string.Format("delete from dg_pay_Process_startOrStop where procs_compid={0} and procs_date='{1}' and procs_type='att_text_upload'", model.CompanyID, DateTime.Now.ToString("MM/dd/yyyy")), _sqlConnection);
                await _hubContext.Clients.Group(string.Concat("bTxtUp_", model.CompanyID, "_", model.userName)).SendAsync("TxtUp_ReceiveProgress", 100);
            }
            return response;
        }
        public async Task<List<ReturnObject>> ReadAttnTextFileEmployeeWise(UploadFileEmpWise model)
        {
            //string path = $"{_webHostEnvironment.WebRootPath}\\TextFileUpload\\40_382023946.txt";
            //var lines = File.ReadAllLines(path);
            //string attEntDt = Convert.ToDateTime("07/25/2023").ToString(model.DateFormat);
            //var epmProxid = _dgCommon.get_InformationDataTable("select emp_proxid from dg_pay_Employee where emp_serial in("+ model.EmpSerial + ")", _sqlConnection)
            //    .Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray();
            //string[] arrray = dtEmpProxid.Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray();
            //string[] epmProxid = !string.IsNullOrEmpty(model.EmpSerial) ? model.EmpSerial.Split(',').ToArray() : new string[] { };
            //var filteredLines = lines.Where(line => line.Contains(attEntDt) && epmProxid.Any(keyword => line.Contains(keyword))).ToArray();

            //var filteredLines = lines.Where(line => line.Contains("07035966")).ToArray();



            var response = new List<ReturnObject>();
            int numDateIndex = 0;
            int dateLength = 0;
            int numHourIndex = 0;
            int HourLength = 0;
            int numMinuteIndex = 0;
            int MinuteLength = 0;
            int numProxyIndex = 0;
            int ProxyLength = 0;
            int[] EmpSerialArr = !string.IsNullOrEmpty(model.EmpSerial) ? model.EmpSerial.Split(',').Select(int.Parse).ToArray() : new int[] { };
            try
            {
                if (model.file.Length > 0)
                {
                    if (EmpSerialArr.Length > 0)
                    {
                        string filepath = $"{_webHostEnvironment.WebRootPath}\\TextFileUpload\\";
                        string[] fileArr = new string[] { filepath, model.CompanyID.ToString(), "_", null, null, null, null, null, ".txt" };
                        fileArr[3] = DateTime.Now.Day.ToString();
                        fileArr[4] = DateTime.Now.Month.ToString();
                        fileArr[5] = DateTime.Now.Year.ToString();
                        fileArr[6] = DateTime.Now.Minute.ToString();
                        fileArr[7] = DateTime.Now.Second.ToString();
                        string fileFull = string.Concat(fileArr);
                        using (var stream = new FileStream(fileFull, FileMode.Create))
                        {
                            await model.file.CopyToAsync(stream);
                        }
                        var dtDate = await this.GetPayAttnTextFileRead_Date(model.CompanyID);
                        var dtHour = await this.GetPayAttnTextFileRead_Hour(model.CompanyID);
                        var dtMinute = await this.GetPayAttnTextFileRead_Minute(model.CompanyID);
                        var dtProxy = await this.GetPayAttnTextFileRead_proxy(model.CompanyID);
                        if (dtDate.Rows.Count > 0)
                        {
                            numDateIndex = Convert.ToInt32(dtDate.Rows[0]["txt_redStart"]);
                            dateLength = Convert.ToInt32(dtDate.Rows[0]["txt_redLength"]);
                        }
                        if (dtHour.Rows.Count > 0)
                        {
                            numHourIndex = Convert.ToInt32(dtHour.Rows[0]["txt_redStart"]);
                            HourLength = Convert.ToInt32(dtHour.Rows[0]["txt_redLength"]);
                        }
                        if (dtMinute.Rows.Count > 0)
                        {
                            numMinuteIndex = Convert.ToInt32(dtMinute.Rows[0]["txt_redStart"]);
                            MinuteLength = Convert.ToInt32(dtMinute.Rows[0]["txt_redLength"]);
                        }
                        if (dtProxy.Rows.Count > 0)
                        {
                            numProxyIndex = Convert.ToInt32(dtProxy.Rows[0]["txt_redStart"]);
                            ProxyLength = Convert.ToInt32(dtProxy.Rows[0]["txt_redLength"]);
                        }
                        var fileLines = File.ReadAllLines(fileFull);

                        //string attEntDt = Convert.ToDateTime("07/25/2023").ToString(model.DateFormat);
                        //var epmProxid = _dgCommon.get_InformationDataTable("select emp_proxid from dg_pay_Employee where emp_serial in(" + model.EmpSerial + ")", _sqlConnection)
                        //    .Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray();
                        //var filteredLines = fileLines.Where(line => line.Contains(attEntDt) && epmProxid.Any(keyword => line.Contains(keyword))).ToArray();

                        if (fileLines.Length > 0)
                        //if (filteredLines.Length > 0)
                        {
                            foreach (var line in fileLines)
                            //foreach (var line in filteredLines)
                            {
                                try
                                {
                                    string textFileLine = string.Empty;
                                    string replaceLine = line.Replace("\t", string.Empty).Replace("\r", string.Empty).Replace("\n", string.Empty).Replace(":", string.Empty).Replace(" ", string.Empty).Replace(",", string.Empty).Replace("|", string.Empty).Replace("-", string.Empty).Replace("/", string.Empty).Replace("[", string.Empty).Replace("]", string.Empty);
                                    if (model.CompanyID == 40 || model.CompanyID == 38) //OK
                                    {
                                        textFileLine = replaceLine.Substring(0, 22);
                                    }
                                    else if (model.CompanyID == 53 || model.CompanyID == 37 || model.CompanyID == 51) //OK
                                    {
                                        if (replaceLine.Length == 20)
                                        {
                                            replaceLine = replaceLine.Insert(14, "  ");
                                            textFileLine = replaceLine.Substring(0, 22);
                                        }
                                        else
                                        {
                                            textFileLine = replaceLine.Substring(0, 22);
                                        }
                                    }
                                    else if (model.CompanyID == 49) //OK
                                    {
                                        if (replaceLine.Length < 22)
                                        {
                                            replaceLine = replaceLine.Length == 15 ? replaceLine.Insert(14, "       ") : replaceLine;
                                            replaceLine = replaceLine.Length == 16 ? replaceLine.Insert(14, "      ") : replaceLine;
                                            replaceLine = replaceLine.Length == 17 ? replaceLine.Insert(14, "     ") : replaceLine;
                                            replaceLine = replaceLine.Length == 18 ? replaceLine.Insert(14, "    ") : replaceLine;
                                            replaceLine = replaceLine.Length == 19 ? replaceLine.Insert(14, "   ") : replaceLine;
                                            replaceLine = replaceLine.Length == 20 ? replaceLine.Insert(14, "  ") : replaceLine;
                                            replaceLine = replaceLine.Length == 21 ? replaceLine.Insert(14, " ") : replaceLine;
                                            textFileLine = replaceLine.Substring(0, 22);
                                        }
                                        else
                                        {
                                            textFileLine = replaceLine.Substring(0, 22);
                                        }
                                    }
                                    else if (model.CompanyID == 41) //OK
                                    {
                                        if (replaceLine.Length < 22)
                                        {
                                            replaceLine = replaceLine.Length == 15 ? replaceLine.Insert(14, "       ") : replaceLine;
                                            replaceLine = replaceLine.Length == 16 ? replaceLine.Insert(14, "      ") : replaceLine;
                                            replaceLine = replaceLine.Length == 17 ? replaceLine.Insert(14, "     ") : replaceLine;
                                            replaceLine = replaceLine.Length == 18 ? replaceLine.Insert(14, "    ") : replaceLine;
                                            replaceLine = replaceLine.Length == 19 ? replaceLine.Insert(14, "   ") : replaceLine;
                                            replaceLine = replaceLine.Length == 20 ? replaceLine.Insert(14, "  ") : replaceLine;
                                            replaceLine = replaceLine.Length == 21 ? replaceLine.Insert(14, " ") : replaceLine;
                                            textFileLine = replaceLine.Substring(0, 22);
                                        }
                                        else
                                        {
                                            textFileLine = replaceLine.Substring(0, 22);
                                        }
                                    }
                                    else if (model.CompanyID == 46 || model.CompanyID == 54 || model.CompanyID == 55)
                                    {
                                        string Line1st = string.Empty;
                                        string Line2nd = string.Empty;
                                        string[] Line1st2ndArr;
                                        if (replaceLine.Length < 22)
                                        {
                                            Line1st = new string(replaceLine.Reverse().Take(14).Reverse().ToArray());
                                            Line2nd = replaceLine.Substring(0, replaceLine.Length - Line1st.Length);
                                            Line1st2ndArr = new string[] { Line1st, Line2nd };
                                            replaceLine = string.Concat(Line1st2ndArr);
                                            replaceLine = replaceLine.Length == 15 ? replaceLine.Insert(14, "       ") : replaceLine;
                                            replaceLine = replaceLine.Length == 16 ? replaceLine.Insert(14, "      ") : replaceLine;
                                            replaceLine = replaceLine.Length == 17 ? replaceLine.Insert(14, "     ") : replaceLine;
                                            replaceLine = replaceLine.Length == 18 ? replaceLine.Insert(14, "    ") : replaceLine;
                                            replaceLine = replaceLine.Length == 19 ? replaceLine.Insert(14, "   ") : replaceLine;
                                            replaceLine = replaceLine.Length == 20 ? replaceLine.Insert(14, "  ") : replaceLine;
                                            replaceLine = replaceLine.Length == 21 ? replaceLine.Insert(14, " ") : replaceLine;
                                            textFileLine = replaceLine.Substring(0, 22);
                                        }
                                        else
                                        {
                                            textFileLine = replaceLine.Substring(0, 22);
                                        }
                                    }
                                    string setDate = this.MakeDateFormat(textFileLine.Substring(numDateIndex, dateLength).Trim(), model.DateFormat); //Date Format yyyy/MM/dd
                                    string setProxy = textFileLine.Substring(numProxyIndex, ProxyLength).Trim();
                                    string[] timeArr = new string[] { textFileLine.Substring(numHourIndex, HourLength).Trim(), ".", textFileLine.Substring(numMinuteIndex, MinuteLength).Trim() };
                                    string ntime = string.Concat(timeArr);
                                    if (ntime == "00.00")
                                    {
                                        ntime = ntime.Substring(0, 4);
                                        ntime = ntime.Insert(4, "1");
                                    }
                                    var dtempSerial = await this.GetEmployeeSl(setProxy, model.CompanyID);
                                    string empSerial = dtempSerial.Rows.Count > 0 ? dtempSerial.Rows[0]["emp_serial"].ToString() : string.Empty;
                                    string emp_no = dtempSerial.Rows.Count > 0 ? dtempSerial.Rows[0]["emp_no"].ToString() : string.Empty;
                                    if (empSerial != string.Empty)
                                    {
                                        bool isEmpSerial = EmpSerialArr.Contains(int.Parse(empSerial)) ? true : false;
                                        if (isEmpSerial)
                                        {
                                            var isSave = await _dgCommon.saveChangesAsync("dg_pay_Att_Insert_Textfile " + int.Parse(empSerial) + ",'" + setProxy + "',0," + model.CompanyID + ",'" + setDate + "'," + ntime + "", _sqlConnection);
                                            if (isSave)
                                            {
                                                response.Add(new ReturnObject
                                                {
                                                    IsSuccess = true,
                                                    Message = "Employee No('" + emp_no + "') Upload Successfully !!"
                                                });
                                            }
                                            else
                                            {
                                                response.Add(new ReturnObject
                                                {
                                                    IsSuccess = false,
                                                    Message = "Employee No('" + emp_no + "') Upload Faill !!"
                                                });
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    ex.ToString();
                                    response.Add(new ReturnObject { IsSuccess = false, Message = "Something Went Wrong !!" });
                                    return response;
                                }
                            }
                        }
                    }
                    else
                    {
                        response.Add(new ReturnObject { IsSuccess = false, Message = "You Can Not Check Any Employee !!" });
                        return response;
                    }
                }
                else
                {
                    response.Add(new ReturnObject { IsSuccess = false, Message = "You Can Not Upload Any File !!" });
                    return response;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                response.Add(new ReturnObject { IsSuccess = false, Message = "Something Went Wrong !!" });
                return response;
            }
            if (response.Count > 0)
            {
                return response;
            }
            else
            {
                response.Add(new ReturnObject { IsSuccess = false, Message = "Employee Not Found In Text File !!" });
                return response;
            }
        }
        public async Task<List<ReturnObject>> ReadAttnTextFileEmployeeWise_new(UploadFileEmpWise model)
        {
            var response = new List<TextUploadMessage>();
            try
            {
                if (Path.GetExtension(model.file.FileName) == ".txt" || Path.GetExtension(model.file.FileName) == ".xlsx")
                {
                    int numDateIndex = 0;
                    int dateLength = 0;
                    int numHourIndex = 0;
                    int HourLength = 0;
                    int numMinuteIndex = 0;
                    int MinuteLength = 0;
                    int numProxyIndex = 0;
                    int ProxyLength = 0;
                    int[] EmpSerialArr = !string.IsNullOrEmpty(model.EmpSerial) ? model.EmpSerial.Split(',').Select(int.Parse).ToArray() : new int[] { };
                    if (model.file.Length > 0)
                    {
                        var dtCheckSalProc = await _dgCommon.get_InformationDataTableAsync(string.Format("select procs_compid from dg_pay_Process_startOrStop where procs_compid={0} and procs_type in('emp_sal_process_bulk','emp_sal_process_Single') group by procs_compid", model.CompanyID), _sqlConnection);
                        if (dtCheckSalProc.Rows.Count > 0)
                        {
                            response.Add(new TextUploadMessage { IsSuccess = false, Message = "Salary Process In Progress,Please Wait... !!" });
                            return UploadMessageShow(response);
                        }
                        var dtCheckOtProc = await _dgCommon.get_InformationDataTableAsync(string.Format("select procs_compid from dg_pay_Process_startOrStop where procs_compid={0} and procs_type in('att_ot_process','att_Hdot_process') group by procs_compid", model.CompanyID), _sqlConnection);
                        if (dtCheckOtProc.Rows.Count > 0)
                        {
                            response.Add(new TextUploadMessage { IsSuccess = false, Message = "OT Process In Progress,Please Wait... !!" });
                            return UploadMessageShow(response);
                        }

                        await _dgCommon.saveChangesAsync(string.Format("dg_pay_processStartOrStop_insert {0},'{1}',{2},'{3}','{4}'", model.CompanyID, "att_text_upload_Single", 1, DateTime.Now.ToString("MM/dd/yyyy"), model.userName), _sqlConnection);
                        if (EmpSerialArr.Length > 0)
                        {
                            var dtAttSetup = await this.GetAttenTextSetupFull(model.txt_formatID);
                            if (dtAttSetup.Rows.Count > 0)
                            {
                                numHourIndex = int.Parse(dtAttSetup.Rows[0]["txt_hrs_start"].ToString());
                                HourLength = int.Parse(dtAttSetup.Rows[0]["txt_hrs_end"].ToString());
                                numMinuteIndex = int.Parse(dtAttSetup.Rows[0]["txt_min_start"].ToString());
                                MinuteLength = int.Parse(dtAttSetup.Rows[0]["txt_min_end"].ToString());
                                numProxyIndex = int.Parse(dtAttSetup.Rows[0]["txt_proxid_start"].ToString());
                                ProxyLength = int.Parse(dtAttSetup.Rows[0]["txt_proxid_end"].ToString());
                                numDateIndex = int.Parse(dtAttSetup.Rows[0]["txt_dt_start"].ToString());
                                dateLength = int.Parse(dtAttSetup.Rows[0]["txt_dt_end"].ToString());

                                //dtAttSetup.AsEnumerable().ToList().ForEach(row =>
                                //{
                                //    string sHead = row["txt_head"].ToString();
                                //    numHourIndex = sHead == "HOURS" ? int.Parse(row["txt_redStart"].ToString()) : numHourIndex;
                                //    HourLength = sHead == "HOURS" ? int.Parse(row["txt_redLength"].ToString()) : HourLength;
                                //    numMinuteIndex = sHead == "MINUTES" ? int.Parse(row["txt_redStart"].ToString()) : numMinuteIndex;
                                //    MinuteLength = sHead == "MINUTES" ? int.Parse(row["txt_redLength"].ToString()) : MinuteLength;
                                //    numProxyIndex = sHead == "EMPID" ? int.Parse(row["txt_redStart"].ToString()) : numProxyIndex;
                                //    ProxyLength = sHead == "EMPID" ? int.Parse(row["txt_redLength"].ToString()) : ProxyLength;
                                //    numDateIndex = sHead == "ATT_DATE" ? int.Parse(row["txt_redStart"].ToString()) : numDateIndex;
                                //    dateLength = sHead == "ATT_DATE" ? int.Parse(row["txt_redLength"].ToString()) : ProxyLength;
                                //});

                                string filepath = $"{_webHostEnvironment.WebRootPath}\\TextFileUpload\\";
                                string[] fileArr = new string[] { filepath, model.CompanyID.ToString(), "_", null, null, null, null, null, ".txt" };
                                fileArr[3] = DateTime.Now.Day.ToString();
                                fileArr[4] = DateTime.Now.Month.ToString();
                                fileArr[5] = DateTime.Now.Year.ToString();
                                fileArr[6] = DateTime.Now.Minute.ToString();
                                fileArr[7] = DateTime.Now.Second.ToString();
                                string fileFull = string.Concat(fileArr);
                                if (Path.GetExtension(model.file.FileName) == ".xlsx")
                                {
                                    var worksheet = await _dgCommon.GetExcelWorkSheet(model.file, 0);
                                    var rowCount = worksheet.Dimension.Rows;
                                    if (rowCount != 0)
                                    {
                                        using (FileStream fs = File.Create(fileFull))
                                        using (var writer = new StreamWriter(fs))
                                        {
                                            for (int row = 2; row <= rowCount; row++)
                                            {
                                                if (!string.IsNullOrEmpty(worksheet.Cells[row, 1].Text) && !string.IsNullOrEmpty(worksheet.Cells[row, 2].Text))
                                                {
                                                    var rowValues = string.Concat(worksheet.Cells[row, 1].Value, worksheet.Cells[row, 2].Value);
                                                    writer.WriteLine(rowValues);
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    using (var stream = new FileStream(fileFull, FileMode.Create))
                                    {
                                        await model.file.CopyToAsync(stream);
                                    }
                                }
                                var fileLines = File.ReadAllLines(fileFull);

                                var epmProxid = _dgCommon.get_InformationDataTable("select emp_proxid from dg_pay_Employee where emp_serial in(" + model.EmpSerial + ")", _sqlConnection).AsEnumerable().Select(row => row["emp_proxid"].ToString().Trim()).
                                Where(id => !string.IsNullOrEmpty(id)).ToArray();

                                var filteredLines = fileLines.Where(line => epmProxid.Any(keyword => line.Contains(keyword))).ToArray();
                                var foundProxids = epmProxid.Where(proxid => filteredLines.Any(line => line.Contains(proxid))).ToArray();
                                var notFindProxid = epmProxid.Except(foundProxids).Select(int.Parse).ToArray();
                                if (notFindProxid.Length > 0)
                                {
                                    string notFindProxidString = string.Join(",", notFindProxid);
                                    var NotFindEmp = _dgCommon.get_InformationDataTable("select emp_no from dg_pay_Employee where convert(int,emp_proxid) in(" + notFindProxidString + ") and compid=" + model.CompanyID, _sqlConnection)
                                        .Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray();
                                    string FinalNotFindEmp = string.Join(",", NotFindEmp);
                                    response.Add(new TextUploadMessage { Message = "Employee No(" + FinalNotFindEmp + ") Not Exists In Text File !!" });
                                }
                                if (filteredLines.Length > 0)
                                {
                                    var employeeSerialMap = _dgCommon.get_InformationDataTable(string.Format("select emp_serial,emp_no,emp_proxid from dg_pay_Employee where compid={0} and emp_serial in({1}) and oi_active=1", model.CompanyID, model.EmpSerial), _sqlConnection)
                                        .AsEnumerable().ToDictionary(row => row["emp_proxid"].ToString().Trim(), row => new { emp_serial = row["emp_serial"].ToString(), emp_no = row["emp_no"].ToString() });
                                    foreach (var line in filteredLines)
                                    {
                                        try
                                        {
                                            //string textFileLine = this.GetCompanyTextFormat(line, model.CompanyID);
                                            string textFileLine = this.GetCompanyTextFormat(line, model.txt_formatID);
                                            string setDate = this.MakeDateFormat(textFileLine.Substring(numDateIndex, dateLength).Trim(), model.DateFormat); //Date Format yyyy/MM/dd
                                            string setProxy = textFileLine.Substring(numProxyIndex, ProxyLength).Trim();
                                            string[] timeArr = new string[] { textFileLine.Substring(numHourIndex, HourLength).Trim(), ".", textFileLine.Substring(numMinuteIndex, MinuteLength).Trim() };
                                            string ntime = string.Concat(timeArr);
                                            if (ntime == "00.00")
                                            {
                                                ntime = ntime.Substring(0, 4);
                                                ntime = ntime.Insert(4, "1");
                                            }
                                            if (employeeSerialMap.TryGetValue(setProxy, out var emp))
                                            {
                                                string empSerial = emp.emp_serial;
                                                string emp_no = emp.emp_no;
                                                bool isEmpSerial = EmpSerialArr.Contains(int.Parse(empSerial)) ? true : false;
                                                if (isEmpSerial)
                                                {
                                                    bool isSave = await _dgCommon.saveChangesAsync(string.Format("dg_pay_Att_Insert_Textfile {0},{1},'{2}',{3},'{4}',0", int.Parse(empSerial), model.CompanyID, setDate, ntime, model.userName), _sqlConnection);
                                                    if (isSave)
                                                    {
                                                        response.Add(new TextUploadMessage { IsSuccess = isSave, empNo = int.Parse(emp_no), Message = "Employee No(" + emp_no + ") Upload Successfully !!" });
                                                    }
                                                    else
                                                    {
                                                        response.Add(new TextUploadMessage { empNo = int.Parse(emp_no), Message = "Employee No(" + emp_no + ") New Time Not Found in Text File !!" });
                                                    }
                                                }
                                                else
                                                {
                                                    response.Add(new TextUploadMessage { empNo = int.Parse(emp_no), Message = "Employee No(" + emp_no + ") Not Exists In Text File !!" });
                                                }
                                            }

                                        }
                                        catch (Exception ex)
                                        {
                                            ex.ToString();
                                        }
                                    }
                                }
                                else
                                {
                                    response.Add(new TextUploadMessage { empNo = 0, Message = "Can Not Find Any Data In Text File !!" });
                                }
                            }
                            else
                            {
                                response.Add(new TextUploadMessage { empNo = 0, Message = "Company Text File Not Setup !!" });
                            }
                        }
                        else
                        {
                            response.Add(new TextUploadMessage { empNo = 0, Message = "You Can Not Check Any Employee !!" });
                        }
                    }
                    else
                    {
                        response.Add(new TextUploadMessage { empNo = 0, Message = "You Can Not Upload Any File !!" });
                    }
                    if (response.Count == 0)
                    {
                        response.Add(new TextUploadMessage { empNo = 0, Message = "Not Found Data In Uploaded File !!" });
                    }
                }
                else
                {
                    response.Add(new TextUploadMessage { IsSuccess = false, Message = "File Format Not Valid !!" });
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            finally
            {
                await _dgCommon.saveChangesAsync(string.Format("delete from dg_pay_Process_startOrStop where procs_compid={0} and procs_date='{1}' and procs_type='att_text_upload_Single'", model.CompanyID, DateTime.Now.ToString("MM/dd/yyyy")), _sqlConnection);
            }
            return UploadMessageShow(response).OrderBy(O => O.IsSuccess).ToList();
            //return response.GroupBy(G => G.Message).Select(S => S.First()).OrderBy(O => O.IsSuccess).ToList();
        }
        public async Task<ReturnObject> GetAttendanceOtProcEmp(int compid,string date,int? deptid,int? secid)
        {
            var result = new ReturnObject();
            var dtEmp = await _dgCommon.get_InformationDataTableAsync(string.Format("dg_pay_Att_OT_Process_EmpList {0},'{1}','{2}','{3}'", compid, date, deptid, secid), _sqlConnection);
            if (dtEmp.Rows.Count > 0)
            {
                result.IsSuccess = true;
                result.dataTable = dtEmp;
                return result;
            }
            result.Message = "No Data Available !!";
            return result;
        }
        public async Task<List<TextUploadMessage>> SetAttendanceOtProcess(AttendanceOtProcess obj)
        {
            //Old Function
            /*var response = new List<TextUploadMessage>();
            var checkProcess = await GetIsAllProcessContinue(obj.compid, obj.procs_date, "att_ot_process", obj.userName);
            if (checkProcess.dataTable != null)
            {
                response.Add(new TextUploadMessage { IsSuccess = false, Message = "Someone Is Processing !!" });
                return response;
            }
            if (obj.processType==1)
            {
                if (obj.compid == 0 || string.IsNullOrEmpty(obj.compid.ToString()))
                {
                    response.Add(new TextUploadMessage { IsSuccess = false, Message = "Please Select Company !!" });
                    return response;
                }
                if (string.IsNullOrEmpty(obj.procs_date))
                {
                    response.Add(new TextUploadMessage { IsSuccess = false, Message = "Please Enter Process Date !!" });
                    return response;
                }
                var dtAttInfo = await _dgCommon.get_InformationDataTableAsync(string.Format("dg_pay_Att_OT_Process_EmpList {0},'{1}'", obj.compid, obj.procs_date), _sqlConnection);
                if (dtAttInfo.Rows.Count > 0)
                {
                    try
                    {
                        await _dgCommon.saveChangesAsync(string.Format("dg_pay_processStartOrStop_insert {0},'{1}',{2},'{3}','{4}'", obj.compid, "att_ot_process", 1, obj.procs_date, obj.userName), _sqlConnection);
                        for (int i = 0; i < dtAttInfo.Rows.Count; i++)
                        {
                            var dtProcessC = await _dgCommon.get_InformationDataTableAsync(string.Format("select procs_status from dg_pay_Process_startOrStop where procs_compid={0} and procs_date='{1}' and procs_type='att_ot_process'", obj.compid, obj.procs_date), _sqlConnection);
                            bool isProcess = (dtProcessC.Rows.Count > 0 && !string.IsNullOrEmpty(dtProcessC.Rows[0]["procs_status"].ToString())) ? bool.Parse(dtProcessC.Rows[0]["procs_status"].ToString()) : false;
                            if (isProcess)
                            {
                                int emp_serial = !string.IsNullOrEmpty(dtAttInfo.Rows[i]["emp_serial"].ToString()) ? int.Parse(dtAttInfo.Rows[i]["emp_serial"].ToString()) : 0;
                                int emp_no = !string.IsNullOrEmpty(dtAttInfo.Rows[i]["emp_no"].ToString()) ? int.Parse(dtAttInfo.Rows[i]["emp_no"].ToString()) : 0;
                                if (emp_serial > 0)
                                {
                                    bool isUpdate = _dgCommon.saveChanges(string.Format("update dg_pay_attendance set at_ot_process=1,at_ot_process_by='{0}',at_ot_process_date=getdate() where at_emp_serial={1} and at_date='{2}'", obj.userName, emp_serial, obj.procs_date), _sqlConnection);
                                    if (isUpdate)
                                    {
                                        response.Add(new TextUploadMessage { IsSuccess = true, Message = "Employee No(" + emp_no + ") Process Done !!" });
                                    }
                                    else
                                    {
                                        response.Add(new TextUploadMessage { IsSuccess = false, Message = "Employee No(" + emp_no + ") Process Fail !!" });
                                    }
                                    var res = Math.Abs(((i + 1) * 100) / dtAttInfo.Rows.Count);
                                    await _hubContext.Clients.Group(string.Concat("bAttProc_", obj.compid, "_", obj.userName)).SendAsync("OT_ReceiveProgress", res);
                                }
                                continue;
                            }
                            else
                            {
                                response.Add(new TextUploadMessage { IsSuccess = false, Message = "Close Anther Tab Use Your UserID !!" });
                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ex.ToString();
                        await _dgCommon.saveChangesAsync(string.Format("delete from dg_pay_Process_startOrStop where procs_compid={0} and procs_date='{1}' and procs_type='att_ot_process'", obj.compid, obj.procs_date), _sqlConnection);
                    }
                    finally
                    {
                        await _dgCommon.saveChangesAsync(string.Format("delete from dg_pay_Process_startOrStop where procs_compid={0} and procs_date='{1}' and procs_type='att_ot_process'", obj.compid, obj.procs_date), _sqlConnection);
                    }
                }
                else
                {
                    response.Add(new TextUploadMessage { IsSuccess = false, Message = "Employee Not Found Process Already Done !!" });
                }
            }
            else
            {
                if (obj.empInfo.Count > 0)
                {
                    try
                    {
                        await _dgCommon.saveChangesAsync(string.Format("dg_pay_processStartOrStop_insert {0},'{1}',{2},'{3}','{4}'", obj.compid, "att_ot_process", 1, obj.procs_date, obj.userName), _sqlConnection);
                        for (int i = 0; i < obj.empInfo.Count; i++)
                        {
                            var dtProcessC = await _dgCommon.get_InformationDataTableAsync(string.Format("select procs_status from dg_pay_Process_startOrStop where procs_compid={0} and procs_date='{1}' and procs_type='att_ot_process'", obj.compid, obj.procs_date), _sqlConnection);
                            bool isProcess = (dtProcessC.Rows.Count > 0 && !string.IsNullOrEmpty(dtProcessC.Rows[0]["procs_status"].ToString())) ? bool.Parse(dtProcessC.Rows[0]["procs_status"].ToString()) : false;
                            if (isProcess)
                            {
                                bool isUpdate = _dgCommon.saveChanges(string.Format("update dg_pay_attendance set at_ot_process=1,at_ot_process_by='{0}',at_ot_process_date=getdate() where at_emp_serial={1} and at_date='{2}'", obj.userName, obj.empInfo[i].empserial, obj.procs_date), _sqlConnection);
                                if (isUpdate)
                                {
                                    response.Add(new TextUploadMessage { IsSuccess = true, Message = "Employee No(" + obj.empInfo[i].empNo + ") Process Done !!" });
                                }
                                else
                                {
                                    response.Add(new TextUploadMessage { IsSuccess = false, Message = "Employee No(" + obj.empInfo[i].empNo + ") Process Fail !!" });
                                }
                                var res = Math.Abs(((i + 1) * 100) / obj.empInfo.Count);
                                await _hubContext.Clients.Group(string.Concat("bAttProc_", obj.compid, "_", obj.userName)).SendAsync("OT_ReceiveProgress", res);
                                continue;
                            }
                            else
                            {
                                response.Add(new TextUploadMessage { IsSuccess = false, Message = "Close Anther Tab Use Your UserID !!" });
                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ex.ToString();
                        await _dgCommon.saveChangesAsync(string.Format("delete from dg_pay_Process_startOrStop where procs_compid={0} and procs_date='{1}' and procs_type='att_ot_process'", obj.compid, obj.procs_date), _sqlConnection);
                    }
                    finally
                    {
                        await _dgCommon.saveChangesAsync(string.Format("delete from dg_pay_Process_startOrStop where procs_compid={0} and procs_date='{1}' and procs_type='att_ot_process'", obj.compid, obj.procs_date), _sqlConnection);
                    }
                }
                else
                {
                    response.Add(new TextUploadMessage { IsSuccess = false, Message = "Please Select Employee From List !!" });
                }
            }
            return response.GroupBy(G => G.Message).Select(S => S.First()).OrderBy(O => O.IsSuccess).ToList();*/

            //New Function 2025-4-24
            var response = new List<TextUploadMessage>();
            var tasks = new List<Task>();
            var semaphore = new SemaphoreSlim(20);
            var dtOtProcessTbl = new DataTable();
            var dtAttEsumNew = new DataTable();
            try
            {
                var checkProcess = await GetIsAllProcessContinue(obj.compid, obj.procs_date, "att_ot_process", obj.userName);
                if (checkProcess.dataTable != null)
                {
                    response.Add(new TextUploadMessage { IsSuccess = false, Message = "Someone Is Processing !!" });
                    return response;
                }
                var dtCheckAttProc = await _dgCommon.get_InformationDataTableAsync(string.Format("select procs_compid from dg_pay_Process_startOrStop where procs_compid={0} and procs_type in('att_text_upload','att_text_upload_Single','att_menual_upload') group by procs_compid", obj.compid), _sqlConnection);
                if (dtCheckAttProc.Rows.Count > 0)
                {
                    response.Add(new TextUploadMessage { IsSuccess = false, Message = "Attendance Upload In Progress,Please Wait... !!" });
                    return response;
                }
                var dtCheckSalProc = await _dgCommon.get_InformationDataTableAsync(string.Format("select procs_compid from dg_pay_Process_startOrStop where procs_compid={0} and procs_type in('emp_sal_process_bulk','emp_sal_process_Single') group by procs_compid", obj.compid), _sqlConnection);
                if (dtCheckSalProc.Rows.Count > 0)
                {
                    response.Add(new TextUploadMessage { IsSuccess = false, Message = "Salary Process In Progress,Please Wait... !!" });
                    return response;
                }

                if (obj.processType == 1)
                {
                    if (obj.compid == 0 || string.IsNullOrEmpty(obj.compid.ToString()))
                    {
                        response.Add(new TextUploadMessage { IsSuccess = false, Message = "Please Select Company !!" });
                        return response;
                    }
                    if (string.IsNullOrEmpty(obj.procs_date))
                    {
                        response.Add(new TextUploadMessage { IsSuccess = false, Message = "Please Enter Process Date !!" });
                        return response;
                    }
                    var dtAttInfo = await _dgCommon.get_InformationDataTableAsync(string.Format("dg_pay_Att_OT_Process_EmpList {0},'{1}'", obj.compid, obj.procs_date), _sqlConnection);
                    if (dtAttInfo.Rows.Count > 0)
                    {
                        await _dgCommon.saveChangesAsync(string.Format("dg_pay_processStartOrStop_insert {0},'{1}',{2},'{3}','{4}'", obj.compid, "att_ot_process", 1, obj.procs_date, obj.userName), _sqlConnection);
                        dtOtProcessTbl = AttendanceOtProcess.AddNewDataColumn();
                        await _hubContext.Clients.Group(string.Concat("bAttProc_", obj.compid, "_", obj.userName)).SendAsync("OT_ReceiveProgress", 0);
                        for (int i = 0; i < dtAttInfo.Rows.Count; i++)
                        {
                            var dtProcessC = await _dgCommon.get_InformationDataTableAsync(string.Format("select procs_status from dg_pay_Process_startOrStop where procs_compid={0} and procs_date='{1}' and procs_type='att_ot_process'", obj.compid, obj.procs_date), _sqlConnection);
                            bool isProcess = (dtProcessC.Rows.Count > 0 && !string.IsNullOrEmpty(dtProcessC.Rows[0]["procs_status"].ToString())) ? bool.Parse(dtProcessC.Rows[0]["procs_status"].ToString()) : false;
                            if (isProcess)
                            {
                                int emp_serial = !string.IsNullOrEmpty(dtAttInfo.Rows[i]["emp_serial"].ToString()) ? int.Parse(dtAttInfo.Rows[i]["emp_serial"].ToString()) : 0;
                                await semaphore.WaitAsync();
                                var task = Task.Run(async () =>
                                {
                                    try
                                    {
                                        var dtOtResponse = await _dgCommon.get_InfoDataTableUseConStringAsync(string.Format("dg_Pay_Att_WorkedHrs {0},{1},'{2}',1", emp_serial, obj.compid, obj.procs_date), Getway.Dg_Payroll);
                                        if (dtOtResponse.Rows.Count > 0)
                                        {
                                            lock (dtOtProcessTbl)
                                            {
                                                AttendanceOtProcess.SetAttDataColumnVal(dtOtProcessTbl, dtOtResponse, obj.userName);
                                            }
                                        }
                                    }
                                    finally
                                    {
                                        semaphore.Release();
                                    }
                                });
                                tasks.Add(task);
                            }
                            else
                            {
                                response.Add(new TextUploadMessage { IsSuccess = false, Message = "Close From Anther Tab, Use Your UserID !!" });
                                break;
                            }
                        }
                        await Task.WhenAll(tasks);
                        //Part two
                        if (dtOtProcessTbl.Rows.Count > 0)
                        {
                            var dtProcessCS = await _dgCommon.get_InformationDataTableAsync(string.Format("select procs_status from dg_pay_Process_startOrStop where procs_compid={0} and procs_date='{1}' and procs_type='att_ot_process'", obj.compid, obj.procs_date), _sqlConnection);
                            bool isProcessCS = (dtProcessCS.Rows.Count > 0 && !string.IsNullOrEmpty(dtProcessCS.Rows[0]["procs_status"].ToString())) ? bool.Parse(dtProcessCS.Rows[0]["procs_status"].ToString()) : false;
                            if (isProcessCS)
                            {
                                var empSerialArr = dtAttInfo.AsEnumerable().Select(s => s["emp_serial"].ToString()).ToArray();
                                var dtBackupBeforeOT = await _dgCommon.get_InformationDataTableAsync(string.Format("select at_emp_serial,emp_no as at_emp_no,at_compid,at_date,at_holiday,at_work_hrs,at_work_min,at_ot_hrs,at_ot_min,at_exot_hrs,at_exot_min,at_exot_hrs_2hour_max,at_exot_min_2hour_less,at_ot_ex_ot_hour_min,at_ot_ex_ot_hour_min_with_wh_ot,at_holiday_ot_for_oneday,at_ot_process_by,at_ot_process_date from dg_pay_attendance inner join dg_pay_Employee on at_emp_serial=emp_serial where at_emp_serial in({0}) and at_date='{1}'", string.Join(",", empSerialArr), obj.procs_date), _sqlConnection);
                                bool isUpdate = _dgCommon.saveChangesByType("dg_pay_Att_OT_Process_EmpBatch", _sqlConnection, new SqlParameter("@attUpdateTable", dtOtProcessTbl));
                                if (isUpdate)
                                {
                                    dtAttEsumNew = AttendanceOtProcess.AddNewDataColumnESum();
                                    tasks = new List<Task>();
                                    for (int j = 0; j < dtOtProcessTbl.Rows.Count; j++)
                                    {
                                        var dtProcessC = await _dgCommon.get_InformationDataTableAsync(string.Format("select procs_status from dg_pay_Process_startOrStop where procs_compid={0} and procs_date='{1}' and procs_type='att_ot_process'", obj.compid, obj.procs_date), _sqlConnection);
                                        bool isProcess = (dtProcessC.Rows.Count > 0 && !string.IsNullOrEmpty(dtProcessC.Rows[0]["procs_status"].ToString())) ? bool.Parse(dtProcessC.Rows[0]["procs_status"].ToString()) : false;
                                        if (isProcess)
                                        {
                                            int emp_no = int.Parse(dtOtProcessTbl.Rows[j]["at_emp_no"].ToString());
                                            await semaphore.WaitAsync();
                                            var task = Task.Run(async () =>
                                            {
                                                try
                                                {
                                                    var dtAttEsum = await _dgCommon.get_InfoDataTableUseConStringAsync(string.Format("dg_ECardEmpWiseSumm_Indivisual '{0}',{1},{2},1", obj.procs_date, obj.compid, emp_no), Getway.Dg_Payroll);
                                                    if (dtAttEsum.Rows.Count > 0)
                                                    {
                                                        lock (dtAttEsumNew)
                                                        {
                                                            AttendanceOtProcess.SetAttEsumColumnVal(dtAttEsumNew, dtAttEsum);
                                                        }
                                                    }
                                                }
                                                finally
                                                {
                                                    semaphore.Release();
                                                }
                                            });
                                            tasks.Add(task);
                                            response.Add(new TextUploadMessage { IsSuccess = true, Message = "Employee No(" + emp_no + ") Process Done !!" });
                                            var res = Math.Abs(((j + 1) * 100) / dtOtProcessTbl.Rows.Count);
                                            if (res > 1 && res <= 99)
                                                await _hubContext.Clients.Group(string.Concat("bAttProc_", obj.compid, "_", obj.userName)).SendAsync("OT_ReceiveProgress", res);
                                        }
                                        else
                                        {
                                            response.Clear();
                                            response.Add(new TextUploadMessage { IsSuccess = false, Message = "Close From Anther Tab, Use Your UserID !!" });
                                            break;
                                        }
                                    }
                                    await Task.WhenAll(tasks);
                                    if (dtAttEsumNew.Rows.Count > 0)
                                    {
                                        var dtProcessCSs = await _dgCommon.get_InformationDataTableAsync(string.Format("select procs_status from dg_pay_Process_startOrStop where procs_compid={0} and procs_date='{1}' and procs_type='att_ot_process'", obj.compid, obj.procs_date), _sqlConnection);
                                        bool isProcessCSs = (dtProcessCSs.Rows.Count > 0 && !string.IsNullOrEmpty(dtProcessCSs.Rows[0]["procs_status"].ToString())) ? bool.Parse(dtProcessCSs.Rows[0]["procs_status"].ToString()) : false;
                                        if (isProcessCSs)
                                        {
                                            _dgCommon.saveChangesByType("dg_ECardTotalInfo_type_EmpBatch", _sqlConnection, new SqlParameter("@esumUpdateTable", dtAttEsumNew));
                                        }
                                        else
                                        {
                                            _dgCommon.saveChangesByType("dg_pay_Att_OT_Process_EmpBatch_ReturnOld", _sqlConnection, new SqlParameter("@attUpdateTable", dtBackupBeforeOT));
                                            response.Clear();
                                            response.Add(new TextUploadMessage { IsSuccess = false, Message = "Close From Anther Tab, Use Your UserID !!" });
                                        }
                                    }
                                }
                            }
                            else
                            {
                                response.Clear();
                                response.Add(new TextUploadMessage { IsSuccess = false, Message = "Close From Anther Tab, Use Your UserID !!" });
                            }
                        }
                    }
                    else
                    {
                        response.Add(new TextUploadMessage { IsSuccess = false, Message = "Employee Not Found Process Already Done !!" });
                    }
                }
                else
                {
                    if (obj.empInfo.Count > 0)
                    {
                        await _dgCommon.saveChangesAsync(string.Format("dg_pay_processStartOrStop_insert {0},'{1}',{2},'{3}','{4}'", obj.compid, "att_ot_process", 1, obj.procs_date, obj.userName), _sqlConnection);
                        dtOtProcessTbl = AttendanceOtProcess.AddNewDataColumn();
                        await _hubContext.Clients.Group(string.Concat("bAttProc_", obj.compid, "_", obj.userName)).SendAsync("OT_ReceiveProgress", 0);
                        for (int i = 0; i < obj.empInfo.Count; i++)
                        {
                            var dtProcessC = await _dgCommon.get_InformationDataTableAsync(string.Format("select procs_status from dg_pay_Process_startOrStop where procs_compid={0} and procs_date='{1}' and procs_type='att_ot_process'", obj.compid, obj.procs_date), _sqlConnection);
                            bool isProcess = (dtProcessC.Rows.Count > 0 && !string.IsNullOrEmpty(dtProcessC.Rows[0]["procs_status"].ToString())) ? bool.Parse(dtProcessC.Rows[0]["procs_status"].ToString()) : false;
                            if (isProcess)
                            {
                                int emp_serial = obj.empInfo[i].empserial;
                                await semaphore.WaitAsync();
                                var task = Task.Run(async () =>
                                {
                                    try
                                    {
                                        var dtOtResponse = await _dgCommon.get_InfoDataTableUseConStringAsync(string.Format("dg_Pay_Att_WorkedHrs {0},{1},'{2}',1", emp_serial, obj.compid, obj.procs_date), Getway.Dg_Payroll);
                                        if (dtOtResponse.Rows.Count > 0)
                                        {
                                            lock (dtOtProcessTbl)
                                            {
                                                AttendanceOtProcess.SetAttDataColumnVal(dtOtProcessTbl, dtOtResponse, obj.userName);
                                            }
                                        }
                                    }
                                    finally
                                    {
                                        semaphore.Release();
                                    }
                                });
                                tasks.Add(task);
                            }
                            else
                            {
                                response.Add(new TextUploadMessage { IsSuccess = false, Message = "Close From Anther Tab, Use Your UserID !!" });
                                break;
                            }
                        }
                        await Task.WhenAll(tasks);
                        //Part 2
                        if (dtOtProcessTbl.Rows.Count > 0)
                        {
                            var dtProcessCs = await _dgCommon.get_InformationDataTableAsync(string.Format("select procs_status from dg_pay_Process_startOrStop where procs_compid={0} and procs_date='{1}' and procs_type='att_ot_process'", obj.compid, obj.procs_date), _sqlConnection);
                            bool isProcessCs = (dtProcessCs.Rows.Count > 0 && !string.IsNullOrEmpty(dtProcessCs.Rows[0]["procs_status"].ToString())) ? bool.Parse(dtProcessCs.Rows[0]["procs_status"].ToString()) : false;
                            if (isProcessCs)
                            {
                                var empSerialArr2 = obj.empInfo.Select(s => s.empserial).ToArray();
                                var backupBeforeOT = await _dgCommon.get_InformationDataTableAsync(string.Format("select at_emp_serial,emp_no as at_emp_no,at_compid,at_date,at_holiday,at_work_hrs,at_work_min,at_ot_hrs,at_ot_min,at_exot_hrs,at_exot_min,at_exot_hrs_2hour_max,at_exot_min_2hour_less,at_ot_ex_ot_hour_min,at_ot_ex_ot_hour_min_with_wh_ot,at_holiday_ot_for_oneday,at_ot_process_by,at_ot_process_date from dg_pay_attendance inner join dg_pay_Employee on at_emp_serial=emp_serial where at_emp_serial in({0}) and at_date='{1}'", string.Join(",", empSerialArr2), obj.procs_date), _sqlConnection);
                                bool isUpdate_S = _dgCommon.saveChangesByType("dg_pay_Att_OT_Process_EmpBatch", _sqlConnection, new SqlParameter("@attUpdateTable", dtOtProcessTbl));
                                if (isUpdate_S)
                                {
                                    dtAttEsumNew = AttendanceOtProcess.AddNewDataColumnESum();
                                    tasks = new List<Task>();
                                    for (int j = 0; j < dtOtProcessTbl.Rows.Count; j++)
                                    {
                                        var dtProcessC = await _dgCommon.get_InformationDataTableAsync(string.Format("select procs_status from dg_pay_Process_startOrStop where procs_compid={0} and procs_date='{1}' and procs_type='att_ot_process'", obj.compid, obj.procs_date), _sqlConnection);
                                        bool isProcess = (dtProcessC.Rows.Count > 0 && !string.IsNullOrEmpty(dtProcessC.Rows[0]["procs_status"].ToString())) ? bool.Parse(dtProcessC.Rows[0]["procs_status"].ToString()) : false;
                                        if (isProcess)
                                        {
                                            int emp_no = int.Parse(dtOtProcessTbl.Rows[j]["at_emp_no"].ToString());
                                            await semaphore.WaitAsync();
                                            var task = Task.Run(async () =>
                                            {
                                                try
                                                {
                                                    var dtAttEsum = await _dgCommon.get_InfoDataTableUseConStringAsync(string.Format("dg_ECardEmpWiseSumm_Indivisual '{0}',{1},{2},1", obj.procs_date, obj.compid, emp_no), Getway.Dg_Payroll);
                                                    if (dtAttEsum.Rows.Count > 0)
                                                    {
                                                        lock (dtAttEsumNew)
                                                        {
                                                            AttendanceOtProcess.SetAttEsumColumnVal(dtAttEsumNew, dtAttEsum);
                                                        }
                                                    }
                                                }
                                                finally
                                                {
                                                    semaphore.Release();
                                                }
                                            });
                                            tasks.Add(task);
                                            response.Add(new TextUploadMessage { IsSuccess = true, Message = "Employee No(" + emp_no + ") Process Done !!" });
                                            var res_S = Math.Abs(((j + 1) * 100) / dtOtProcessTbl.Rows.Count);
                                            if (res_S > 1 && res_S <= 99)
                                                await _hubContext.Clients.Group(string.Concat("bAttProc_", obj.compid, "_", obj.userName)).SendAsync("OT_ReceiveProgress", res_S);
                                        }
                                        else
                                        {
                                            response.Add(new TextUploadMessage { IsSuccess = false, Message = "Close From Anther Tab, Use Your UserID !!" });
                                            break;
                                        }
                                    }
                                    await Task.WhenAll(tasks);
                                    if (dtAttEsumNew.Rows.Count > 0)
                                    {
                                        var dtProcessCSs = await _dgCommon.get_InformationDataTableAsync(string.Format("select procs_status from dg_pay_Process_startOrStop where procs_compid={0} and procs_date='{1}' and procs_type='att_ot_process'", obj.compid, obj.procs_date), _sqlConnection);
                                        bool isProcessCSs = (dtProcessCSs.Rows.Count > 0 && !string.IsNullOrEmpty(dtProcessCSs.Rows[0]["procs_status"].ToString())) ? bool.Parse(dtProcessCSs.Rows[0]["procs_status"].ToString()) : false;
                                        if (isProcessCSs)
                                        {
                                            _dgCommon.saveChangesByType("dg_ECardTotalInfo_type_EmpBatch", _sqlConnection, new SqlParameter("@esumUpdateTable", dtAttEsumNew));
                                        }
                                        else
                                        {
                                            _dgCommon.saveChangesByType("dg_pay_Att_OT_Process_EmpBatch_ReturnOld", _sqlConnection, new SqlParameter("@attUpdateTable", backupBeforeOT));
                                            response.Clear();
                                            response.Add(new TextUploadMessage { IsSuccess = false, Message = "Close From Anther Tab, Use Your UserID !!" });
                                        }
                                    }
                                }
                            }
                            else
                            {
                                response.Clear();
                                response.Add(new TextUploadMessage { IsSuccess = false, Message = "Close From Anther Tab, Use Your UserID !!" });
                            }
                        }
                    }
                    else
                    {
                        response.Add(new TextUploadMessage { IsSuccess = false, Message = "Please Select Employee From List !!" });
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                response.Add(new TextUploadMessage { IsSuccess = false, Message = "Something Went Wrong !!" });
            }
            finally
            {
                await _dgCommon.saveChangesAsync(string.Format("delete from dg_pay_Process_startOrStop where procs_compid={0} and procs_date='{1}' and procs_type='att_ot_process'", obj.compid, obj.procs_date), _sqlConnection);
                await _hubContext.Clients.Group(string.Concat("bAttProc_", obj.compid, "_", obj.userName)).SendAsync("OT_ReceiveProgress", 100);
            }
            return response.DistinctBy(G => G.Message).OrderBy(O => O.IsSuccess).ToList();
        }
        public async Task<ReturnObject> GetTextFileFormatInfo(int compid)
        {
            var result = new ReturnObject();
            var dtTxt = await _dgCommon.get_InformationDataTableAsync($"select txt_id,txt_date_format,txt_date_format_p,txt_date_type,txt_date_desc,txt_file_format from dg_pay_attenTextFileFormat where txt_compid={compid}", _sqlConnection);
            if (dtTxt.Rows.Count > 0)
            {
                result.IsSuccess = true;
                result.dataTable = dtTxt;
                return result;
            }
            result.Message = "No Data Available !!";
            return result;
        }

        #region"Setup"
        private string MakeDateFormat(string dateString,string dateFormat)
        {
            string dateResult = string.Empty;
            DateTime customDate;
            DateTime.TryParseExact(dateString, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out customDate);
            dateResult = Convert.ToDateTime(customDate).ToString("yyyy/MM/dd");
            //try
            //{
            //    DateTime customDate;
            //    //string[] formats = { "MMddyyyy", "yyyyMMdd","ddMMyyyy", "MMddyy","yyMMdd","ddMMyy" };
            //    //DateTime.TryParseExact(dateString, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out customDate);
            //    DateTime.TryParseExact(dateString, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out customDate);
            //    dateResult = Convert.ToDateTime(customDate).ToString("yyyy/MM/dd");
            //}
            //catch (Exception ex)
            //{
            //    ex.ToString();
            //}
            return dateResult;
        }
        private bool TextFileTimeValid(int empSerial,string textDate,decimal txtTime)
        {
            bool flag = false;
            var dtAttdata = _dgCommon.get_InformationDataTable("select at_intime,at_outtime,at_date,at_outdate,at_shift from dg_pay_attendance where at_date='"+ textDate + "' and at_emp_serial="+ empSerial, _sqlConnection);
            decimal attIntime = decimal.Parse(dtAttdata.Rows[0]["at_intime"].ToString());
            decimal attOuttime = decimal.Parse(dtAttdata.Rows[0]["at_outtime"].ToString());
            string attIndate = Convert.ToDateTime(dtAttdata.Rows[0]["at_date"]).ToString("MM/dd/yyyy");
            string attOutdate = Convert.ToDateTime(dtAttdata.Rows[0]["at_outdate"]).ToString("MM/dd/yyyy");
            int attShift = int.Parse(dtAttdata.Rows[0]["at_shift"].ToString());
            var dtShiftInfo = _dgCommon.get_InformationDataTable("select intime_start,intime_stop,outtime_start,outtime_stop from dg_pay_shift where sh_code="+ attShift, _sqlConnection);
            decimal shiftIntimeStart = decimal.Parse(dtShiftInfo.Rows[0]["intime_start"].ToString());
            decimal shiftIntimeStop = decimal.Parse(dtShiftInfo.Rows[0]["intime_stop"].ToString());
            decimal shiftOuttimeStart = decimal.Parse(dtShiftInfo.Rows[0]["outtime_start"].ToString());
            decimal shiftOuttimeStop = decimal.Parse(dtShiftInfo.Rows[0]["outtime_stop"].ToString());
            if (attIntime == 0 && txtTime >= shiftIntimeStart && txtTime <= shiftIntimeStop)
            {
                flag = true;
            }
            if (attIntime > 0 && txtTime >= shiftIntimeStart && txtTime<= shiftIntimeStop  && txtTime < attIntime)
            {
                flag = true;
            }
            if (attOuttime == 0 && txtTime >= shiftOuttimeStart)
            {
                flag = true;
            }
            if (attOuttime == 0 && txtTime<= shiftOuttimeStop)
            {
                flag = true;
            }
            if (attOuttime > 0 && txtTime>= shiftOuttimeStart && attIndate == attOutdate && txtTime > attOuttime)
            {
                flag = true;
            }
            if (attOuttime > 0 && txtTime<= shiftOuttimeStop && attIndate!=attOutdate && txtTime> attOuttime)
            {
                flag = true;
            }
            return flag;
        }
        /*private string GetCompanyTextFormat(string line,int CompanyID)
        {
            string textFileLine = string.Empty;
            string replaceLine = line.Replace("\t", string.Empty).Replace("\r", string.Empty).Replace("\n", string.Empty).Replace(":", string.Empty).Replace(" ", string.Empty).Replace(",", string.Empty).Replace("|", string.Empty).Replace("-", string.Empty).Replace("/", string.Empty).Replace("[", string.Empty).Replace("]", string.Empty);
            if (CompanyID == 40 || CompanyID == 38 || CompanyID == 57 || CompanyID == 61 || CompanyID == 60)
            {
                textFileLine = replaceLine.Substring(0, 22);
            }
            else if (CompanyID == 53 || CompanyID == 37 || CompanyID == 51 || CompanyID == 56)
            {
                if (replaceLine.Length == 20)
                {
                    replaceLine = replaceLine.Insert(14, "  ");
                    textFileLine = replaceLine.Substring(0, 22);
                }
                else
                {
                    textFileLine = replaceLine.Substring(0, 22);
                }
            }
            else if (CompanyID == 49)
            {
                if (replaceLine.Length < 22)
                {
                    replaceLine = replaceLine.Length == 15 ? replaceLine.Insert(14, "       ") : replaceLine;
                    replaceLine = replaceLine.Length == 16 ? replaceLine.Insert(14, "      ") : replaceLine;
                    replaceLine = replaceLine.Length == 17 ? replaceLine.Insert(14, "     ") : replaceLine;
                    replaceLine = replaceLine.Length == 18 ? replaceLine.Insert(14, "    ") : replaceLine;
                    replaceLine = replaceLine.Length == 19 ? replaceLine.Insert(14, "   ") : replaceLine;
                    replaceLine = replaceLine.Length == 20 ? replaceLine.Insert(14, "  ") : replaceLine;
                    replaceLine = replaceLine.Length == 21 ? replaceLine.Insert(14, " ") : replaceLine;
                    textFileLine = replaceLine.Substring(0, 22);
                }
                else
                {
                    textFileLine = replaceLine.Substring(0, 22);
                }
            }
            else if (CompanyID == 58 || CompanyID == 46 || CompanyID == 54 || CompanyID == 55) //new
            {
                if (replaceLine.Length < 22)
                {
                    replaceLine = replaceLine.Length == 15 ? replaceLine.Insert(1, "       ") : replaceLine;
                    replaceLine = replaceLine.Length == 16 ? replaceLine.Insert(2, "      ") : replaceLine;
                    replaceLine = replaceLine.Length == 17 ? replaceLine.Insert(3, "     ") : replaceLine;
                    replaceLine = replaceLine.Length == 18 ? replaceLine.Insert(4, "    ") : replaceLine;
                    replaceLine = replaceLine.Length == 19 ? replaceLine.Insert(5, "   ") : replaceLine;
                    replaceLine = replaceLine.Length == 20 ? replaceLine.Insert(6, "  ") : replaceLine;
                    replaceLine = replaceLine.Length == 21 ? replaceLine.Insert(7, " ") : replaceLine;
                    textFileLine = replaceLine.Substring(0, 22);
                }
                else
                {
                    textFileLine = replaceLine.Substring(0, 22);
                }
            }
            else if (CompanyID == 41)
            {
                if (replaceLine.Length < 22)
                {
                    replaceLine = replaceLine.Length == 15 ? replaceLine.Insert(14, "       ") : replaceLine;
                    replaceLine = replaceLine.Length == 16 ? replaceLine.Insert(14, "      ") : replaceLine;
                    replaceLine = replaceLine.Length == 17 ? replaceLine.Insert(14, "     ") : replaceLine;
                    replaceLine = replaceLine.Length == 18 ? replaceLine.Insert(14, "    ") : replaceLine;
                    replaceLine = replaceLine.Length == 19 ? replaceLine.Insert(14, "   ") : replaceLine;
                    replaceLine = replaceLine.Length == 20 ? replaceLine.Insert(14, "  ") : replaceLine;
                    replaceLine = replaceLine.Length == 21 ? replaceLine.Insert(14, " ") : replaceLine;
                    textFileLine = replaceLine.Substring(0, 22);
                }
                else
                {
                    textFileLine = replaceLine.Substring(0, 22);
                }
            }
            /*else if (CompanyID == 46 || CompanyID == 54 || CompanyID == 55)
            {
                string Line1st = string.Empty;
                string Line2nd = string.Empty;
                string[] Line1st2ndArr;
                if (replaceLine.Length < 22)
                {
                    Line1st = new string(replaceLine.Reverse().Take(14).Reverse().ToArray());
                    Line2nd = replaceLine.Substring(0, replaceLine.Length - Line1st.Length);
                    Line1st2ndArr = new string[] { Line1st, Line2nd };
                    replaceLine = string.Concat(Line1st2ndArr);
                    replaceLine = replaceLine.Length == 15 ? replaceLine.Insert(14, "       ") : replaceLine;
                    replaceLine = replaceLine.Length == 16 ? replaceLine.Insert(14, "      ") : replaceLine;
                    replaceLine = replaceLine.Length == 17 ? replaceLine.Insert(14, "     ") : replaceLine;
                    replaceLine = replaceLine.Length == 18 ? replaceLine.Insert(14, "    ") : replaceLine;
                    replaceLine = replaceLine.Length == 19 ? replaceLine.Insert(14, "   ") : replaceLine;
                    replaceLine = replaceLine.Length == 20 ? replaceLine.Insert(14, "  ") : replaceLine;
                    replaceLine = replaceLine.Length == 21 ? replaceLine.Insert(14, " ") : replaceLine;
                    textFileLine = replaceLine.Substring(0, 22);
                }
                else
                {
                    textFileLine = replaceLine.Substring(0, 22);
                }
            }
            return textFileLine;
        }*/


        private string GetCompanyTextFormat(string line, int txt_id)
        {
            string textFileLine = string.Empty;
            string replaceLine = line.Replace("\t", string.Empty).Replace("\r", string.Empty).Replace("\n", string.Empty).Replace(":", string.Empty).Replace(" ", string.Empty).Replace(",", string.Empty).Replace("|", string.Empty).Replace("-", string.Empty).Replace("/", string.Empty).Replace("[", string.Empty).Replace("]", string.Empty);
            if (txt_id == 1 || txt_id == 2 || txt_id == 3 || txt_id == 4 || txt_id == 18 || txt_id == 20 || txt_id == 5 || txt_id == 38 || txt_id == 39)
            {
                textFileLine = replaceLine.Substring(0, 22);
            }
            else if (txt_id == 6 || txt_id == 7 || txt_id == 10 || txt_id == 11 || txt_id == 8 || txt_id == 9 || txt_id == 17)
            {
                if (replaceLine.Length == 20)
                {
                    replaceLine = replaceLine.Insert(14, "  ");
                    textFileLine = replaceLine.Substring(0, 22);
                }
                else
                {
                    textFileLine = replaceLine.Substring(0, 22);
                }
            }
            else if (txt_id == 12)
            {
                if (replaceLine.Length < 22)
                {
                    replaceLine = replaceLine.Length == 15 ? replaceLine.Insert(14, "       ") : replaceLine;
                    replaceLine = replaceLine.Length == 16 ? replaceLine.Insert(14, "      ") : replaceLine;
                    replaceLine = replaceLine.Length == 17 ? replaceLine.Insert(14, "     ") : replaceLine;
                    replaceLine = replaceLine.Length == 18 ? replaceLine.Insert(14, "    ") : replaceLine;
                    replaceLine = replaceLine.Length == 19 ? replaceLine.Insert(14, "   ") : replaceLine;
                    replaceLine = replaceLine.Length == 20 ? replaceLine.Insert(14, "  ") : replaceLine;
                    replaceLine = replaceLine.Length == 21 ? replaceLine.Insert(14, " ") : replaceLine;
                    textFileLine = replaceLine.Substring(0, 22);
                }
                else
                {
                    textFileLine = replaceLine.Substring(0, 22);
                }
            }
            else if (txt_id == 19 || txt_id == 15 || txt_id == 13 || txt_id == 14 || txt_id == 21 || txt_id == 22 || txt_id == 23 || txt_id == 24 || txt_id == 25 || txt_id == 26 || txt_id == 27 || txt_id == 28 || txt_id == 29 || txt_id == 30 || txt_id == 31 || txt_id == 32 || txt_id == 33 || txt_id == 34 || txt_id == 35 || txt_id == 37 || txt_id == 40) //new
            {
                if (replaceLine.Length < 22)
                {
                    replaceLine = replaceLine.Length == 15 ? replaceLine.Insert(1, "       ") : replaceLine;
                    replaceLine = replaceLine.Length == 16 ? replaceLine.Insert(2, "      ") : replaceLine;
                    replaceLine = replaceLine.Length == 17 ? replaceLine.Insert(3, "     ") : replaceLine;
                    replaceLine = replaceLine.Length == 18 ? replaceLine.Insert(4, "    ") : replaceLine;
                    replaceLine = replaceLine.Length == 19 ? replaceLine.Insert(5, "   ") : replaceLine;
                    replaceLine = replaceLine.Length == 20 ? replaceLine.Insert(6, "  ") : replaceLine;
                    replaceLine = replaceLine.Length == 21 ? replaceLine.Insert(7, " ") : replaceLine;
                    textFileLine = replaceLine.Substring(0, 22);
                }
                else
                {
                    textFileLine = replaceLine.Substring(0, 22);
                }
            }
            else if (txt_id == 16)
            {
                if (replaceLine.Length < 22)
                {
                    replaceLine = replaceLine.Length == 15 ? replaceLine.Insert(14, "       ") : replaceLine;
                    replaceLine = replaceLine.Length == 16 ? replaceLine.Insert(14, "      ") : replaceLine;
                    replaceLine = replaceLine.Length == 17 ? replaceLine.Insert(14, "     ") : replaceLine;
                    replaceLine = replaceLine.Length == 18 ? replaceLine.Insert(14, "    ") : replaceLine;
                    replaceLine = replaceLine.Length == 19 ? replaceLine.Insert(14, "   ") : replaceLine;
                    replaceLine = replaceLine.Length == 20 ? replaceLine.Insert(14, "  ") : replaceLine;
                    replaceLine = replaceLine.Length == 21 ? replaceLine.Insert(14, " ") : replaceLine;
                    textFileLine = replaceLine.Substring(0, 22);
                }
                else
                {
                    textFileLine = replaceLine.Substring(0, 22);
                }
            }
            return textFileLine;
        }
        private async Task<DataTable> GetAttenTextSetupFull(int compID)
        {
            //var dt = await _dgCommon.get_InformationDataTableAsync("select rtrim(adi_code) as txt_head,txt_redStart,txt_redLength from dg_pay_attdataImportsetup where adi_company=" + compID + " and rtrim(adi_code) in('HOURS','MINUTES','EMPID','ATT_DATE')", _sqlConnection);
            var dt = await _dgCommon.get_InformationDataTableAsync($"select txt_hrs_start,txt_hrs_end,txt_min_start,txt_min_end,txt_dt_start,txt_dt_end,txt_proxid_start,txt_proxid_end from dg_pay_attenTextFileFormat where txt_id={compID}", _sqlConnection);
            return dt;
        }


        private async Task<DataTable> GetEmployeeSl(string Proxid, int compID)
        {
            var data = await _dgCommon.get_InformationDataTableAsync("select emp_serial,emp_no from dg_pay_Employee where compid=" + compID + " and oi_active=1 and emp_proxid='" + Proxid + "'", _sqlConnection);
            return data;
        }
        private List<ReturnObject> UploadMessageShow(List<TextUploadMessage> employees)
        {
            var groupedMessage = employees.GroupBy(e => e.empNo);
            var result = new List<ReturnObject>();
            foreach (var group in groupedMessage)
            {
                if (group.Any(e => e.Message.Contains("Successfully")))
                {
                    result.Add(new ReturnObject { IsSuccess = true, Message = "Employee No(" + group.Key + ") Upload Successfully !!" });
                }
                else
                {
                    result.AddRange(group);
                }
            }
            return result;
        }

        //New Action 12/04/2023
        private async Task<DataTable> GetPayAttnTextFileRead_Date(int compID)
        {
            var data = await _dgCommon.get_InformationDataTableAsync("select txt_redStart,txt_redLength from dg_pay_attdataImportsetup where adi_code='ATT_DATE' and adi_company=" + compID, _sqlConnection);
            return data;
        }
        private async Task<DataTable> GetPayAttnTextFileRead_Hour(int compID)
        {
            var data = await _dgCommon.get_InformationDataTableAsync("select txt_redStart,txt_redLength from dg_pay_attdataImportsetup where adi_code='HOURS' and adi_company=" + compID, _sqlConnection);
            return data;
        }
        private async Task<DataTable> GetPayAttnTextFileRead_Minute(int compID)
        {
            var data = await _dgCommon.get_InformationDataTableAsync("select txt_redStart,txt_redLength from dg_pay_attdataImportsetup where adi_code='MINUTES' and adi_company=" + compID, _sqlConnection);
            return data;
        }
        private async Task<DataTable> GetPayAttnTextFileRead_proxy(int compID)
        {
            var data = await _dgCommon.get_InformationDataTableAsync("select txt_redStart,txt_redLength from dg_pay_attdataImportsetup where adi_code='EMPID' and adi_company=" + compID, _sqlConnection);
            return data;
        }
        private AttendanceModel CheckTextFileData(int empserial,int compid,string date,decimal time)
        {
            var output = new AttendanceModel();
            string at_outdatedb = null;
            decimal InTime = 0;
            int InTimeHrs = 0;
            int InTimeMin = 0;
            int InTimeTotMin = 0;
            decimal OutTime = 0;
            decimal OutTimeHrs = 0;
            int OutTimeMin = 0;
            int OutTimeTotMin = 0;
            int Shift = 0;
            decimal ShifInTime = 0;
            decimal LateTime = 0;
            decimal LateTimeDB = 0;
            int shiftID = 0;
            decimal ShiftLateAfterTime = 0;
            decimal INTimeStart = 0;
            decimal INTimeStop = 0;
            decimal outtimestart = 0;
            decimal outtimestop = 0;
            string shiftName = string.Empty;
            decimal AfterDate_outtime = 0;
            string at_status_code = string.Empty;
            bool? at_manual_in = null;
            string at_manual_in_by = null;
            string at_manual_in_date = null;
            bool? at_manual_out = null;
            string at_manual_out_by = null;
            string at_manual_out_date = null;
            int latehrs = 0;
            decimal latemin = 0;
            int intimeTotalMin = 0;
            int shiftInTimeTotalMin = 0;
            string at_holiday = string.Empty;
            string beforeoutdateI = string.Empty;
            decimal OutTimeNEW = 0;
            string out_date_I = string.Empty;
            decimal? at_text_intime = null;
            string at_text_out_date = null;
            decimal? at_text_outtime = null;

            // set value
            var dtAfterDate_outtime = _dgCommon.get_InformationDataTable(string.Format("select at_outtime from dg_pay_attendance where at_emp_serial={0} and at_date='{1}' and at_outdate=DATEADD(day, +1, '{2}')", empserial, date, date), _sqlConnection);
            AfterDate_outtime = (dtAfterDate_outtime.Rows.Count> 0 && !string.IsNullOrEmpty(dtAfterDate_outtime.Rows[0]["at_outtime"].ToString())) ? decimal.Parse(dtAfterDate_outtime.Rows[0]["at_outtime"].ToString()) : AfterDate_outtime;

            //Atten Db Value Set
            var dtAtstatus = GetAttendenceTime(empserial, date);
            InTime = (dtAtstatus.Rows.Count > 0 && !string.IsNullOrEmpty(dtAtstatus.Rows[0]["at_intime"].ToString())) ? decimal.Parse(dtAtstatus.Rows[0]["at_intime"].ToString()) : InTime;
            OutTime = (dtAtstatus.Rows.Count > 0 && !string.IsNullOrEmpty(dtAtstatus.Rows[0]["at_outtime"].ToString())) ? decimal.Parse(dtAtstatus.Rows[0]["at_outtime"].ToString()) : OutTime;
            at_outdatedb = (dtAtstatus.Rows.Count > 0 && !string.IsNullOrEmpty(dtAtstatus.Rows[0]["at_outdate"].ToString())) ? dtAtstatus.Rows[0]["at_outdate"].ToString() : at_outdatedb;
            LateTimeDB = (dtAtstatus.Rows.Count > 0 && !string.IsNullOrEmpty(dtAtstatus.Rows[0]["at_late"].ToString())) ? decimal.Parse(dtAtstatus.Rows[0]["at_late"].ToString()) : LateTimeDB;
            at_status_code = (dtAtstatus.Rows.Count > 0 && !string.IsNullOrEmpty(dtAtstatus.Rows[0]["at_status_code"].ToString().Trim())) ? dtAtstatus.Rows[0]["at_status_code"].ToString().Trim() : at_status_code;            
            at_manual_in = (dtAtstatus.Rows.Count > 0 && !string.IsNullOrEmpty(dtAtstatus.Rows[0]["at_manual_in"].ToString())) ? bool.Parse(dtAtstatus.Rows[0]["at_manual_in"].ToString()) : null;
            at_manual_in_by = (dtAtstatus.Rows.Count > 0 && !string.IsNullOrEmpty(dtAtstatus.Rows[0]["at_manual_in_by"].ToString())) ? dtAtstatus.Rows[0]["at_manual_in_by"].ToString() : at_manual_in_by;
            at_manual_in_date = (dtAtstatus.Rows.Count > 0 && !string.IsNullOrEmpty(dtAtstatus.Rows[0]["at_manual_in_date"].ToString())) ? dtAtstatus.Rows[0]["at_manual_in_date"].ToString() : at_manual_in_date;
            at_manual_out = (dtAtstatus.Rows.Count > 0 && !string.IsNullOrEmpty(dtAtstatus.Rows[0]["at_manual_out"].ToString())) ? bool.Parse(dtAtstatus.Rows[0]["at_manual_out"].ToString()) : at_manual_out;
            at_manual_out_by = (dtAtstatus.Rows.Count > 0 && !string.IsNullOrEmpty(dtAtstatus.Rows[0]["at_manual_out_by"].ToString())) ? dtAtstatus.Rows[0]["at_manual_out_by"].ToString() : at_manual_out_by;
            at_manual_out_date = (dtAtstatus.Rows.Count > 0 && !string.IsNullOrEmpty(dtAtstatus.Rows[0]["at_manual_out_date"].ToString())) ? dtAtstatus.Rows[0]["at_manual_out_date"].ToString() : at_manual_out_date;
            at_text_intime = (dtAtstatus.Rows.Count > 0 && !string.IsNullOrEmpty(dtAtstatus.Rows[0]["at_text_intime"].ToString())) ? decimal.Parse(dtAtstatus.Rows[0]["at_text_intime"].ToString()) : at_text_intime;
            at_text_out_date = (dtAtstatus.Rows.Count > 0 && !string.IsNullOrEmpty(dtAtstatus.Rows[0]["at_text_out_date"].ToString())) ? dtAtstatus.Rows[0]["at_text_out_date"].ToString() : at_text_out_date;
            at_text_outtime = (dtAtstatus.Rows.Count > 0 && !string.IsNullOrEmpty(dtAtstatus.Rows[0]["at_text_outtime"].ToString())) ? decimal.Parse(dtAtstatus.Rows[0]["at_text_outtime"].ToString()) : at_text_outtime;
            at_holiday = (dtAtstatus.Rows.Count > 0 && !string.IsNullOrEmpty(dtAtstatus.Rows[0]["at_holiday"].ToString().Trim())) ? dtAtstatus.Rows[0]["at_holiday"].ToString().Trim() : at_holiday;
            Shift = (dtAtstatus.Rows.Count > 0 && !string.IsNullOrEmpty(dtAtstatus.Rows[0]["at_shift"].ToString())) ? int.Parse(dtAtstatus.Rows[0]["at_shift"].ToString()) : Shift;
            
            //Shift Info Set
            var dtShiftInfo = _dgCommon.get_InformationDataTable(string.Format("select intime_start,intime_stop,outtime_start,outtime_stop,RTRIM(sh_name) as sh_name,sh_Lateafter,sh_InTime from dg_pay_shift where sh_code={0} and sh_comp={1}", Shift, compid), _sqlConnection);
            INTimeStart = (dtShiftInfo.Rows.Count > 0 && !string.IsNullOrEmpty(dtShiftInfo.Rows[0]["intime_start"].ToString())) ? decimal.Parse(dtShiftInfo.Rows[0]["intime_start"].ToString()) : INTimeStart;
            INTimeStop = (dtShiftInfo.Rows.Count > 0 && !string.IsNullOrEmpty(dtShiftInfo.Rows[0]["intime_stop"].ToString())) ? decimal.Parse(dtShiftInfo.Rows[0]["intime_stop"].ToString()) : INTimeStop;
            outtimestart = (dtShiftInfo.Rows.Count > 0 && !string.IsNullOrEmpty(dtShiftInfo.Rows[0]["outtime_start"].ToString())) ? decimal.Parse(dtShiftInfo.Rows[0]["outtime_start"].ToString()) : outtimestart;
            outtimestop = (dtShiftInfo.Rows.Count > 0 && !string.IsNullOrEmpty(dtShiftInfo.Rows[0]["outtime_stop"].ToString())) ? decimal.Parse(dtShiftInfo.Rows[0]["outtime_stop"].ToString()) : outtimestop;
            shiftName = (dtShiftInfo.Rows.Count > 0 && !string.IsNullOrEmpty(dtShiftInfo.Rows[0]["sh_name"].ToString())) ? dtShiftInfo.Rows[0]["sh_name"].ToString() : shiftName;
            ShiftLateAfterTime = (dtShiftInfo.Rows.Count > 0 && !string.IsNullOrEmpty(dtShiftInfo.Rows[0]["sh_Lateafter"].ToString())) ? decimal.Parse(dtShiftInfo.Rows[0]["sh_Lateafter"].ToString()) : ShiftLateAfterTime;
           
            if (new[] { "Night Shift","Night Shift-1","Ramadan Night", "Template-Night", "Night Shift-S", "Security-Night", "Shift-Night-1", "CT-3rd Shift","Shift-Loader","Security B", "Day Shift", "Morning","Ramadan-2","General-Shift", "Template-Day", "Shift-Day-Q", "Morning 2", "Shift A (Day)",
            "Shift B (Night)","General","Shift-PAD","Shift-General","CT-1st Shift","General-Ramadan","General-Ramadan-1","Ramadan-HO","G-Ramadan","Ramadan-1","CMS","General-2","General-3","Special Shift","Shift-A","Security-A","Shift-B","CT-2nd Shift","CES","Shift-Night-Q",
            "Shift-C","General-HO","Shift Day-9","General-C","Security-Day","Shift-Day","Shift-Day10","Shift-Day(Security)","Shift-Night","Ramadan-Night","Security-B","Shift-Night-9","Shift-Night(Security)","Template-Night",
            "Security-Night","Shift-Night-1","Shift-Day","Shift-Day(Security)"}.Any(x => x.Equals(shiftName, StringComparison.OrdinalIgnoreCase)))
            {
                if (new[] { "Night Shift","Night Shift-1","Ramadan Night", "Template-Night", "Night Shift-S", "Security-Night", "Shift-Night-1", "CT-3rd Shift","Shift-Loader","Security B", "Shift B (Night)", "Special Shift", "Shift-C", "Shift-Night","Ramadan-Night","Security-B", "Shift-Night-9",
                "Shift-Night(Security)","Template-Night","Security-Night","Shift-Night-1","Shift-B","CT-2nd Shift","CES","Shift-Night-Q","Shift-A","Security-A"}.Any(x => x.Equals(shiftName, StringComparison.OrdinalIgnoreCase)))
                {
                    if (time >= INTimeStart && time<= INTimeStop)
                    {
                        if (InTime == 0 || time < InTime)
                        {
                            if (!(new[] { "AL", "CL", "ML", "M/L", "SL", "LWP" }.Any(x => x.Equals(at_status_code, StringComparison.OrdinalIgnoreCase))))
                            {
                                beforeoutdateI = Convert.ToDateTime(date).AddDays(-1).ToString("MM/dd/yyyy");
                                LateTime = CalLateTimeEmployee(empserial, date, time);
                                output.at_compid = compid;
                                output.at_emp_serial = empserial;
                                output.at_date = date;
                                output.at_intime = time;
                                output.at_outtime = OutTime;
                                output.at_outdate = at_outdatedb;
                                output.at_late = LateTime;
                                output.at_status = LateTime == 0 ? string.Empty : "Late";
                                output.at_status_code = LateTime == 0 ? string.Empty : "LA";
                                output.at_manual_out = at_manual_out;
                                output.at_manual_out_by = at_manual_out_by;
                                output.at_manual_out_date = at_manual_out_date;
                                output.at_text_intime = time;
                                output.at_text_out_date = at_text_out_date;
                                output.at_text_outtime = at_text_outtime;
                            }
                        }
                    }
                    else
                    {
                        if (time >= outtimestart || outtimestop>= time)
                        {
                            string beforeoutdateI2 = string.Empty;
                            decimal beforeouttime = 0;
                            beforeoutdateI2 = Convert.ToDateTime(date).AddDays(-1).ToString("MM/dd/yyyy");
                            var dtbeforeouttime = _dgCommon.get_InformationDataTable(string.Format("select at_outtime from dg_pay_attendance where at_emp_serial={0} and at_date='{1}'", empserial, beforeoutdateI2), _sqlConnection);
                            beforeouttime = (dtbeforeouttime.Rows.Count > 0 && !string.IsNullOrEmpty(dtbeforeouttime.Rows[0]["at_outtime"].ToString())) ? decimal.Parse(dtbeforeouttime.Rows[0]["at_outtime"].ToString()) : beforeouttime;
                            if (outtimestart <= time && (OutTime < time && shiftName != "Shift-C" && shiftName != "Shift-Night" && shiftName != "Ramadan-Night" && shiftName != "Security-B" && shiftName != "Shift-Night-9" && shiftName != "Night Shift" && shiftName != "Night Shift-1" && shiftName!= "Ramadan Night" && shiftName != "Template-Night" && shiftName != "Night Shift-S" && shiftName != "Security-Night" && shiftName != "Shift-Night-1" &&  shiftName != "CT-3rd Shift" && shiftName != "Shift-Loader" && shiftName != "Security B" && shiftName != "Shift-Night(Security)"))
                            {
                                if (!(new[] { "AL", "CL", "ML", "M/L", "SL", "LWP" }.Any(x => x.Equals(at_status_code, StringComparison.OrdinalIgnoreCase))))
                                {
                                    output.at_compid = compid;
                                    output.at_emp_serial = empserial;
                                    output.at_date = date;
                                    output.at_intime = InTime;
                                    output.at_outtime = time;
                                    output.at_outdate = date;
                                    output.at_late = LateTimeDB;
                                    output.at_status = LateTimeDB == 0 ? string.Empty : "Late";
                                    output.at_status_code = LateTimeDB == 0 ? string.Empty : "LA";
                                    output.at_manual_in = at_manual_in;
                                    output.at_manual_in_by = at_manual_in_by;
                                    output.at_manual_in_date = at_manual_in_date;
                                    output.at_text_intime = at_text_intime;
                                    output.at_text_out_date = date;
                                    output.at_text_outtime = time;
                                    if ((compid == 40 || compid == 38 || compid == 61) && InTime == 0)
                                    {
                                        output.at_status = "Late";
                                        output.at_status_code = "LA";
                                    }
                                }
                            }
                            if (time <= outtimestop && beforeouttime < time)
                            {
                                var dtbeforeoutdateI2 = GetAttendenceTime(empserial, beforeoutdateI2);
                                at_status_code = (dtbeforeoutdateI2.Rows.Count > 0 && !string.IsNullOrEmpty(dtbeforeoutdateI2.Rows[0]["at_status_code"].ToString().Trim())) ? dtbeforeoutdateI2.Rows[0]["at_status_code"].ToString().Trim() : string.Empty;
                                if (!(new[] { "AL", "CL", "ML", "M/L", "SL", "LWP" }.Any(x => x.Equals(at_status_code, StringComparison.OrdinalIgnoreCase))))
                                {                               
                                    InTime = (dtbeforeoutdateI2.Rows.Count > 0 && !string.IsNullOrEmpty(dtbeforeoutdateI2.Rows[0]["at_intime"].ToString())) ? decimal.Parse(dtbeforeoutdateI2.Rows[0]["at_intime"].ToString()) : 0;
                                    at_manual_in = (dtbeforeoutdateI2.Rows.Count > 0 && !string.IsNullOrEmpty(dtbeforeoutdateI2.Rows[0]["at_manual_in"].ToString())) ? bool.Parse(dtbeforeoutdateI2.Rows[0]["at_manual_in"].ToString()) : null;
                                    at_manual_in_by = (dtbeforeoutdateI2.Rows.Count > 0 && !string.IsNullOrEmpty(dtbeforeoutdateI2.Rows[0]["at_manual_in_by"].ToString())) ? dtbeforeoutdateI2.Rows[0]["at_manual_in_by"].ToString() : null;
                                    at_manual_in_date = (dtbeforeoutdateI2.Rows.Count > 0 && !string.IsNullOrEmpty(dtbeforeoutdateI2.Rows[0]["at_manual_in_date"].ToString())) ? dtbeforeoutdateI2.Rows[0]["at_manual_in_date"].ToString() : null;
                                    LateTimeDB = (dtbeforeoutdateI2.Rows.Count > 0 && !string.IsNullOrEmpty(dtbeforeoutdateI2.Rows[0]["at_late"].ToString())) ? decimal.Parse(dtbeforeoutdateI2.Rows[0]["at_late"].ToString()) : 0;
                                    at_text_intime = (dtbeforeoutdateI2.Rows.Count > 0 && !string.IsNullOrEmpty(dtbeforeoutdateI2.Rows[0]["at_text_intime"].ToString())) ? decimal.Parse(dtbeforeoutdateI2.Rows[0]["at_text_intime"].ToString()) : 0;

                                    output.at_compid = compid;
                                    output.at_emp_serial = empserial;
                                    output.at_date = beforeoutdateI2;
                                    output.at_intime = InTime;
                                    output.at_outtime = time;
                                    output.at_outdate = date;
                                    output.at_late = LateTimeDB;
                                    output.at_status = LateTimeDB == 0 ? string.Empty : "Late";
                                    output.at_status_code = LateTimeDB == 0 ? string.Empty : "LA";
                                    output.at_manual_in = at_manual_in;
                                    output.at_manual_in_by = at_manual_in_by;
                                    output.at_manual_in_date = at_manual_in_date;
                                    output.at_text_intime = at_text_intime;
                                    output.at_text_out_date = date;
                                    output.at_text_outtime = time;
                                    if ((compid == 40 || compid == 38 || compid == 61) && InTime == 0)
                                    {
                                        output.at_status = "Late";
                                        output.at_status_code = "LA";
                                    }
                                }
                            }
                        }
                    }
                }
                else if (new[] { "Day Shift", "Shift A (Day)", "General","Shift-PAD", "Shift-General", "CT-1st Shift", "General-Ramadan","General-Ramadan-1","Ramadan-HO", "G-Ramadan","Ramadan-1", "CMS", "General-2", "General-3", "Morning","Ramadan-2","General-Shift", "Template-Day", "Shift-Day-Q",
                "Morning-S","Shift-Day-1","Morning 2","General-HO","Shift Day-9","General-C","Security-Day","Shift-Day","Shift-Day10","Shift-Day(Security)","Shift-Day","Shift-B","CT-2nd Shift","CES","Shift-Night-Q"}.Any(x => x.Equals(shiftName, StringComparison.OrdinalIgnoreCase)))
                {
                    if (time >= INTimeStart && time <= INTimeStop)
                    {
                        if (InTime == 0 || time < InTime)
                        {
                            if (!(new[] { "AL", "CL", "ML", "M/L", "SL", "LWP" }.Any(x => x.Equals(at_status_code, StringComparison.OrdinalIgnoreCase))))
                            {
                                LateTime = CalLateTimeEmployee(empserial, date, time);
                                output.at_compid = compid;
                                output.at_emp_serial = empserial;
                                output.at_date = date;
                                output.at_intime = time;
                                output.at_outtime = OutTime;
                                output.at_outdate = at_outdatedb;
                                output.at_late = LateTime;
                                output.at_status = LateTime == 0 ? string.Empty : "Late";
                                output.at_status_code = LateTime == 0 ? string.Empty : "LA";
                                output.at_manual_out = at_manual_out;
                                output.at_manual_out_by = at_manual_out_by;
                                output.at_manual_out_date = at_manual_out_date;
                                output.at_text_intime = time;
                                output.at_text_out_date = at_text_out_date;
                                output.at_text_outtime = at_text_outtime;
                            }
                        }
                    }
                    else
                    {
                        if (outtimestart <= time && OutTime < time && date==Convert.ToDateTime(at_outdatedb).ToString("yyyy/MM/dd"))
                        {
                            if (shiftName == "Day Shift" || shiftName == "Shift A (Day)" || shiftName == "General-2" || shiftName == "General-3" || shiftName == "Morning" || shiftName == "Ramadan-2" || shiftName=="General-Shift" || shiftName == "Template-Day" || shiftName == "Shift-Day-Q" || shiftName == "Morning-S" || shiftName == "Shift-Day-1" || shiftName == "Morning 2" || shiftName == "Shift-A" || shiftName == "Security-A")
                            {
                                if (!(new[] { "AL", "CL", "ML", "M/L", "SL", "LWP" }.Any(x => x.Equals(at_status_code, StringComparison.OrdinalIgnoreCase))))
                                {
                                    output.at_compid = compid;
                                    output.at_emp_serial = empserial;
                                    output.at_date = date;
                                    output.at_intime = InTime;
                                    output.at_outtime = time;
                                    output.at_outdate = date;
                                    output.at_late = LateTimeDB;
                                    output.at_status = LateTimeDB == 0 ? string.Empty : "Late";
                                    output.at_status_code = LateTimeDB == 0 ? string.Empty : "LA";
                                    output.at_manual_in = at_manual_in;
                                    output.at_manual_in_by = at_manual_in_by;
                                    output.at_manual_in_date = at_manual_in_date;
                                    output.at_text_intime = at_text_intime;
                                    output.at_text_out_date = date;
                                    output.at_text_outtime = time;
                                    if ((compid == 40 || compid == 38 || compid == 61) && InTime == 0)
                                    {
                                        output.at_status = "Late";
                                        output.at_status_code = "LA";
                                    }
                                }
                            }
                            if ((shiftName == "General-C" || shiftName == "Security-Day" || shiftName == "Shift-Day" || shiftName == "Shift-Day10" || shiftName == "Shift-Day(Security)" || shiftName == "General-HO" || shiftName == "Shift Day-9" || shiftName == "General" || shiftName=="Shift-PAD" || shiftName == "Shift-General" || shiftName == "CT-1st Shift" || shiftName == "General-Ramadan" || shiftName == "General-Ramadan-1" || shiftName == "Ramadan-HO" || shiftName == "G-Ramadan" || shiftName == "Ramadan-1" || shiftName == "CMS" || shiftName == "Shift-Day" || shiftName == "Shift-Day(Security)") && time > outtimestart)
                            {
                                if (!(new[] { "AL", "CL", "ML", "M/L", "SL", "LWP" }.Any(x => x.Equals(at_status_code, StringComparison.OrdinalIgnoreCase))))
                                {
                                    output.at_compid = compid;
                                    output.at_emp_serial = empserial;
                                    output.at_date = date;
                                    output.at_intime = InTime;
                                    output.at_outtime = time;
                                    output.at_outdate = date;
                                    output.at_late = LateTimeDB;
                                    output.at_status = LateTimeDB == 0 ? string.Empty : "Late";
                                    output.at_status_code = LateTimeDB == 0 ? string.Empty : "LA";
                                    output.at_manual_in = at_manual_in;
                                    output.at_manual_in_by = at_manual_in_by;
                                    output.at_manual_in_date = at_manual_in_date;
                                    output.at_text_intime = at_text_intime;
                                    output.at_text_out_date = date;
                                    output.at_text_outtime = time;
                                    if ((compid == 40 || compid == 38 || compid == 61) && InTime == 0)
                                    {
                                        output.at_status = "Late";
                                        output.at_status_code = "LA";
                                    }
                                }
                            }
                        }
                        string beforeoutdateI5 = string.Empty;
                        string out_date_I5 = string.Empty;
                        decimal OutTimeNEW5 = 0;
                        beforeoutdateI5 = Convert.ToDateTime(date).AddDays(-1).ToString("MM/dd/yyyy");
                        var dtAtstatus2 = GetAttendenceTime(empserial, beforeoutdateI5);
                        decimal OutTimeAfter = 0;
                        OutTimeAfter = (dtAtstatus2.Rows.Count > 0 && !string.IsNullOrEmpty(dtAtstatus2.Rows[0]["at_outtime"].ToString())) ? decimal.Parse(dtAtstatus2.Rows[0]["at_outtime"].ToString()) : OutTimeAfter;
                        if (time <= outtimestop && OutTimeAfter < time)
                        {
                            if ((shiftName == "Shift-B" || shiftName == "CT-2nd Shift" || shiftName == "CES" || shiftName == "Shift-Night-Q")  && Convert.ToDouble(time) > 3.00)
                            {
                                time = 3;
                            }
                            var dtbeforeoutdateI5 = GetAttendenceTime(empserial, beforeoutdateI5);
                            at_status_code = (dtbeforeoutdateI5.Rows.Count > 0 && !string.IsNullOrEmpty(dtbeforeoutdateI5.Rows[0]["at_status_code"].ToString().Trim())) ? dtbeforeoutdateI5.Rows[0]["at_status_code"].ToString().Trim() : string.Empty;
                            if (!(new[] { "AL", "CL", "ML", "M/L", "SL", "LWP" }.Any(x => x.Equals(at_status_code, StringComparison.OrdinalIgnoreCase))))
                            {                               
                                InTime = (dtbeforeoutdateI5.Rows.Count > 0 && !string.IsNullOrEmpty(dtbeforeoutdateI5.Rows[0]["at_intime"].ToString())) ? decimal.Parse(dtbeforeoutdateI5.Rows[0]["at_intime"].ToString()) : 0;
                                at_manual_in = (dtbeforeoutdateI5.Rows.Count > 0 && !string.IsNullOrEmpty(dtbeforeoutdateI5.Rows[0]["at_manual_in"].ToString())) ? bool.Parse(dtbeforeoutdateI5.Rows[0]["at_manual_in"].ToString()) : null;
                                at_manual_in_by = (dtbeforeoutdateI5.Rows.Count > 0 && !string.IsNullOrEmpty(dtbeforeoutdateI5.Rows[0]["at_manual_in_by"].ToString())) ? dtbeforeoutdateI5.Rows[0]["at_manual_in_by"].ToString() : null;
                                at_manual_in_date = (dtbeforeoutdateI5.Rows.Count > 0 && !string.IsNullOrEmpty(dtbeforeoutdateI5.Rows[0]["at_manual_in_date"].ToString())) ? dtbeforeoutdateI5.Rows[0]["at_manual_in_date"].ToString() : null;
                                LateTimeDB = (dtbeforeoutdateI5.Rows.Count > 0 && !string.IsNullOrEmpty(dtbeforeoutdateI5.Rows[0]["at_late"].ToString())) ? decimal.Parse(dtbeforeoutdateI5.Rows[0]["at_late"].ToString()) : 0;
                                at_text_intime = (dtbeforeoutdateI5.Rows.Count > 0 && !string.IsNullOrEmpty(dtbeforeoutdateI5.Rows[0]["at_text_intime"].ToString())) ? decimal.Parse(dtbeforeoutdateI5.Rows[0]["at_text_intime"].ToString()) : 0;

                                output.at_compid = compid;
                                output.at_emp_serial = empserial;
                                output.at_date = beforeoutdateI5;
                                output.at_intime = InTime;
                                output.at_outtime = time;
                                output.at_outdate = date;
                                output.at_late = LateTimeDB;
                                output.at_status = LateTimeDB == 0 ? string.Empty : "Late";
                                output.at_status_code = LateTimeDB == 0 ? string.Empty : "LA";
                                output.at_manual_in = at_manual_in;
                                output.at_manual_in_by = at_manual_in_by;
                                output.at_manual_in_date = at_manual_in_date;
                                output.at_text_intime = at_text_intime;
                                output.at_text_out_date = date;
                                output.at_text_outtime = time;
                                if ((compid == 40 || compid == 38 || compid == 61) && InTime == 0)
                                {
                                    output.at_status = "Late";
                                    output.at_status_code = "LA";
                                }
                            }
                        }
                    }
                }
                else if (shiftName == "Morning" || shiftName == "Ramadan-2" || shiftName == "General-Shift" || shiftName == "Template-Day" || shiftName == "Shift-Day-Q")
                {
                    if (time >= INTimeStart && time<= INTimeStop)
                    {
                        if (InTime == 0 || time < InTime)
                        {
                            if (!(new[] { "AL", "CL", "ML", "M/L", "SL", "LWP" }.Any(x => x.Equals(at_status_code, StringComparison.OrdinalIgnoreCase))))
                            {
                                LateTime = CalLateTimeEmployee(empserial, date, time);
                                output.at_compid = compid;
                                output.at_emp_serial = empserial;
                                output.at_date = date;
                                output.at_intime = time;
                                output.at_outtime = OutTime;
                                output.at_outdate = at_outdatedb;
                                output.at_late = LateTime;
                                output.at_status = LateTime == 0 ? string.Empty : "Late";
                                output.at_status_code = LateTime == 0 ? string.Empty : "LA";
                                output.at_manual_out = at_manual_out;
                                output.at_manual_out_by = at_manual_out_by;
                                output.at_manual_out_date = at_manual_out_date;
                                output.at_text_intime = time;
                                output.at_text_out_date = at_text_out_date;
                                output.at_text_outtime = at_text_outtime;
                            }
                        }
                    }
                    else
                    {
                        if (OutTime == 0 || (time >= outtimestart && outtimestop >= time))
                        {
                            if (!(new[] { "AL", "CL", "ML", "M/L", "SL", "LWP" }.Any(x => x.Equals(at_status_code, StringComparison.OrdinalIgnoreCase))))
                            {
                                output.at_compid = compid;
                                output.at_emp_serial = empserial;
                                output.at_date = date;
                                output.at_intime = InTime;
                                output.at_outtime = time;
                                output.at_outdate = date;
                                output.at_late = LateTimeDB;
                                output.at_status = LateTimeDB == 0 ? string.Empty : "Late";
                                output.at_status_code = LateTimeDB == 0 ? string.Empty : "LA";
                                output.at_manual_in = at_manual_in;
                                output.at_manual_in_by = at_manual_in_by;
                                output.at_manual_in_date = at_manual_in_date;
                                output.at_text_intime = at_text_intime;
                                output.at_text_out_date = date;
                                output.at_text_outtime = time;
                                if ((compid == 40 || compid == 38 || compid == 61) && InTime == 0)
                                {
                                    output.at_status = "Late";
                                    output.at_status_code = "LA";
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (InTime == 0 || InTime > time)
                {
                    ShifInTime = (dtShiftInfo.Rows.Count > 0 && !string.IsNullOrEmpty(dtShiftInfo.Rows[0]["sh_InTime"].ToString())) ? decimal.Parse(dtShiftInfo.Rows[0]["sh_InTime"].ToString()) : ShifInTime;
                    if ((ShifInTime + 3) > time && time >= 6)
                    {
                        if (!(new[] { "AL", "CL", "ML", "M/L", "SL", "LWP" }.Any(x => x.Equals(at_status_code, StringComparison.OrdinalIgnoreCase))))
                        {
                            LateTime = CalLateTimeEmployee(empserial, date, time);
                            output.at_compid = compid;
                            output.at_emp_serial = empserial;
                            output.at_date = date;
                            output.at_intime = time;
                            output.at_outtime = OutTime;
                            output.at_outdate = at_outdatedb;
                            output.at_late = LateTime;
                            output.at_status = LateTime == 0 ? string.Empty : "Late";
                            output.at_status_code = LateTime == 0 ? string.Empty : "LA";
                            output.at_manual_out = at_manual_out;
                            output.at_manual_out_by = at_manual_out_by;
                            output.at_manual_out_date = at_manual_out_date;
                            output.at_text_intime = time;
                            output.at_text_out_date = at_text_out_date;
                            output.at_text_outtime = at_text_outtime;
                        }
                    }
                    else if (time < 6)
                    {
                        beforeoutdateI = Convert.ToDateTime(date).AddDays(-1).ToString("MM/dd/yyyy");
                        var dtNewOutTime = _dgCommon.get_InformationDataTable(string.Format("select at_outtime from dg_pay_attendance where at_emp_serial={0} and at_date='{1}'", empserial, beforeoutdateI), _sqlConnection);
                        OutTimeNEW = (dtNewOutTime.Rows.Count > 0 && !string.IsNullOrEmpty(dtNewOutTime.Rows[0]["at_outtime"].ToString())) ? decimal.Parse(dtNewOutTime.Rows[0]["at_outtime"].ToString()) : OutTimeNEW;
                        out_date_I = Convert.ToDateTime(date).AddDays(1).ToString("MM/dd/yyyy");
                        if (time < 1 && (OutTimeNEW == 0 || OutTimeNEW > time))
                        {
                            decimal OutTimeNEW12after = 0;
                            OutTimeNEW = OutTimeNEW - 24;
                            OutTimeNEW12after = OutTimeNEW - time > 0 ? OutTimeNEW - time : 0;
                            if (time < 1)
                            {
                                var dtbeforeoutdateI = GetAttendenceTime(empserial, beforeoutdateI);
                                at_status_code = (dtbeforeoutdateI.Rows.Count > 0 && !string.IsNullOrEmpty(dtbeforeoutdateI.Rows[0]["at_status_code"].ToString().Trim())) ? dtbeforeoutdateI.Rows[0]["at_status_code"].ToString().Trim() : string.Empty;
                                if (!(new[] { "AL", "CL", "ML", "M/L", "SL", "LWP" }.Any(x => x.Equals(at_status_code, StringComparison.OrdinalIgnoreCase))))
                                {                                    
                                    InTime = (dtbeforeoutdateI.Rows.Count > 0 && !string.IsNullOrEmpty(dtbeforeoutdateI.Rows[0]["at_intime"].ToString())) ? decimal.Parse(dtbeforeoutdateI.Rows[0]["at_intime"].ToString()) : 0;
                                    at_manual_in = (dtbeforeoutdateI.Rows.Count > 0 && !string.IsNullOrEmpty(dtbeforeoutdateI.Rows[0]["at_manual_in"].ToString())) ? bool.Parse(dtbeforeoutdateI.Rows[0]["at_manual_in"].ToString()) : null;
                                    at_manual_in_by = (dtbeforeoutdateI.Rows.Count > 0 && !string.IsNullOrEmpty(dtbeforeoutdateI.Rows[0]["at_manual_in_by"].ToString())) ? dtbeforeoutdateI.Rows[0]["at_manual_in_by"].ToString() : null;
                                    at_manual_in_date = (dtbeforeoutdateI.Rows.Count > 0 && !string.IsNullOrEmpty(dtbeforeoutdateI.Rows[0]["at_manual_in_date"].ToString())) ? dtbeforeoutdateI.Rows[0]["at_manual_in_date"].ToString() : null;
                                    LateTimeDB = (dtbeforeoutdateI.Rows.Count > 0 && !string.IsNullOrEmpty(dtbeforeoutdateI.Rows[0]["at_late"].ToString())) ? decimal.Parse(dtbeforeoutdateI.Rows[0]["at_late"].ToString()) : 0;
                                    at_text_intime = (dtbeforeoutdateI.Rows.Count > 0 && !string.IsNullOrEmpty(dtbeforeoutdateI.Rows[0]["at_text_intime"].ToString())) ? decimal.Parse(dtbeforeoutdateI.Rows[0]["at_text_intime"].ToString()) : 0;

                                    output.at_compid = compid;
                                    output.at_emp_serial = empserial;
                                    output.at_date = beforeoutdateI;
                                    output.at_intime = InTime;
                                    output.at_outtime = time;
                                    output.at_outdate = date;
                                    output.at_late = LateTimeDB;
                                    output.at_status = LateTimeDB == 0 ? string.Empty : "Late";
                                    output.at_status_code = LateTimeDB == 0 ? string.Empty : "LA";
                                    output.at_manual_in = at_manual_in;
                                    output.at_manual_in_by = at_manual_in_by;
                                    output.at_manual_in_date = at_manual_in_date;
                                    output.at_text_intime = at_text_intime;
                                    output.at_text_out_date = date;
                                    output.at_text_outtime = time;
                                    if ((compid == 40 || compid == 38 || compid == 61) && InTime == 0)
                                    {
                                        output.at_status = "Late";
                                        output.at_status_code = "LA";
                                    }
                                }
                            }
                        }
                        else if (OutTimeNEW == 0 || OutTimeNEW > time)
                        {
                            var dtbeforeoutdateI22 = GetAttendenceTime(empserial, beforeoutdateI);
                            at_status_code = (dtbeforeoutdateI22.Rows.Count > 0 && !string.IsNullOrEmpty(dtbeforeoutdateI22.Rows[0]["at_status_code"].ToString().Trim())) ? dtbeforeoutdateI22.Rows[0]["at_status_code"].ToString().Trim() : string.Empty;
                            if (!(new[] { "AL", "CL", "ML", "M/L", "SL", "LWP" }.Any(x => x.Equals(at_status_code, StringComparison.OrdinalIgnoreCase))))
                            {                               
                                InTime = (dtbeforeoutdateI22.Rows.Count > 0 && !string.IsNullOrEmpty(dtbeforeoutdateI22.Rows[0]["at_intime"].ToString())) ? decimal.Parse(dtbeforeoutdateI22.Rows[0]["at_intime"].ToString()) : 0;
                                at_manual_in = (dtbeforeoutdateI22.Rows.Count > 0 && !string.IsNullOrEmpty(dtbeforeoutdateI22.Rows[0]["at_manual_in"].ToString())) ? bool.Parse(dtbeforeoutdateI22.Rows[0]["at_manual_in"].ToString()) : null;
                                at_manual_in_by = (dtbeforeoutdateI22.Rows.Count > 0 && !string.IsNullOrEmpty(dtbeforeoutdateI22.Rows[0]["at_manual_in_by"].ToString())) ? dtbeforeoutdateI22.Rows[0]["at_manual_in_by"].ToString() : null;
                                at_manual_in_date = (dtbeforeoutdateI22.Rows.Count > 0 && !string.IsNullOrEmpty(dtbeforeoutdateI22.Rows[0]["at_manual_in_date"].ToString())) ? dtbeforeoutdateI22.Rows[0]["at_manual_in_date"].ToString() : null;
                                LateTimeDB = (dtbeforeoutdateI22.Rows.Count > 0 && !string.IsNullOrEmpty(dtbeforeoutdateI22.Rows[0]["at_late"].ToString())) ? decimal.Parse(dtbeforeoutdateI22.Rows[0]["at_late"].ToString()) : 0;
                                at_text_intime = (dtbeforeoutdateI22.Rows.Count > 0 && !string.IsNullOrEmpty(dtbeforeoutdateI22.Rows[0]["at_text_intime"].ToString())) ? decimal.Parse(dtbeforeoutdateI22.Rows[0]["at_text_intime"].ToString()) : 0;

                                output.at_compid = compid;
                                output.at_emp_serial = empserial;
                                output.at_date = beforeoutdateI;
                                output.at_intime = InTime;
                                output.at_outtime = time;
                                output.at_outdate = date;
                                output.at_late = LateTimeDB;
                                output.at_status = LateTimeDB == 0 ? string.Empty : "Late";
                                output.at_status_code = LateTimeDB == 0 ? string.Empty : "LA";
                                output.at_manual_in = at_manual_in;
                                output.at_manual_in_by = at_manual_in_by;
                                output.at_manual_in_date = at_manual_in_date;
                                output.at_text_intime = at_text_intime;
                                output.at_text_out_date = date;
                                output.at_text_outtime = time;
                                if ((compid == 40 || compid == 38 || compid == 61) && InTime == 0)
                                {
                                    output.at_status = "Late";
                                    output.at_status_code = "LA";
                                }
                            }
                        }
                    }
                    else
                    {
                        if (!(new[] { "AL", "CL", "ML", "M/L", "SL", "LWP" }.Any(x => x.Equals(at_status_code, StringComparison.OrdinalIgnoreCase))))
                        {
                            output.at_compid = compid;
                            output.at_emp_serial = empserial;
                            output.at_date = date;
                            output.at_intime = InTime;
                            output.at_outtime = time;
                            output.at_outdate = date;
                            output.at_late = LateTimeDB;
                            output.at_status = LateTimeDB == 0 ? string.Empty : "Late";
                            output.at_status_code = LateTimeDB == 0 ? string.Empty : "LA";
                            output.at_manual_in = at_manual_in;
                            output.at_manual_in_by = at_manual_in_by;
                            output.at_manual_in_date = at_manual_in_date;
                            output.at_text_intime = at_text_intime;
                            output.at_text_out_date = date;
                            output.at_text_outtime = time;
                            if ((compid == 40 || compid == 38 || compid == 61) && InTime == 0)
                            {
                                output.at_status = "Late";
                                output.at_status_code = "LA";
                            }
                        }
                    }
                }
                else if (OutTime < time || time < 6 || OutTime == 0)
                {
                    if (time < 6)
                    {
                        string beforeoutdateI1 = string.Empty;
                        string out_date_I1 = string.Empty;
                        decimal OutTimeNEW1 = 0;
                        beforeoutdateI1 = Convert.ToDateTime(date).AddDays(-1).ToString("MM/dd/yyyy");
                        var dtNewOutTime1 = _dgCommon.get_InformationDataTable(string.Format("select at_outtime from dg_pay_attendance where at_emp_serial={0} and at_date='{1}'", empserial, beforeoutdateI1), _sqlConnection);
                        OutTimeNEW1 = (dtNewOutTime1.Rows.Count > 0 && !string.IsNullOrEmpty(dtNewOutTime1.Rows[0]["at_outtime"].ToString())) ? decimal.Parse(dtNewOutTime1.Rows[0]["at_outtime"].ToString()) : OutTimeNEW1;
                        if (time < 1 && (OutTimeNEW1 == 0 || OutTimeNEW1 > time || @OutTimeNEW1 < time))
                        {
                            decimal OutTimeNEW12after1 = 0;
                            OutTimeNEW12after1 = OutTimeNEW1 - time > 0 ? OutTimeNEW1 - time : 0;
                            if (time < 1 && OutTimeNEW12after1> time)
                            {
                                var dtbeforeoutdateI1 = GetAttendenceTime(empserial, beforeoutdateI1);
                                at_status_code = (dtbeforeoutdateI1.Rows.Count > 0 && !string.IsNullOrEmpty(dtbeforeoutdateI1.Rows[0]["at_status_code"].ToString().Trim())) ? dtbeforeoutdateI1.Rows[0]["at_status_code"].ToString().Trim() : string.Empty;
                                if (!(new[] { "AL", "CL", "ML", "M/L", "SL", "LWP" }.Any(x => x.Equals(at_status_code, StringComparison.OrdinalIgnoreCase))))
                                {                       
                                    InTime = (dtbeforeoutdateI1.Rows.Count > 0 && !string.IsNullOrEmpty(dtbeforeoutdateI1.Rows[0]["at_intime"].ToString())) ? decimal.Parse(dtbeforeoutdateI1.Rows[0]["at_intime"].ToString()) : 0;
                                    at_manual_in = (dtbeforeoutdateI1.Rows.Count > 0 && !string.IsNullOrEmpty(dtbeforeoutdateI1.Rows[0]["at_manual_in"].ToString())) ? bool.Parse(dtbeforeoutdateI1.Rows[0]["at_manual_in"].ToString()) : null;
                                    at_manual_in_by = (dtbeforeoutdateI1.Rows.Count > 0 && !string.IsNullOrEmpty(dtbeforeoutdateI1.Rows[0]["at_manual_in_by"].ToString())) ? dtbeforeoutdateI1.Rows[0]["at_manual_in_by"].ToString() : null;
                                    at_manual_in_date = (dtbeforeoutdateI1.Rows.Count > 0 && !string.IsNullOrEmpty(dtbeforeoutdateI1.Rows[0]["at_manual_in_date"].ToString())) ? dtbeforeoutdateI1.Rows[0]["at_manual_in_date"].ToString() : null;
                                    LateTimeDB = (dtbeforeoutdateI1.Rows.Count > 0 && !string.IsNullOrEmpty(dtbeforeoutdateI1.Rows[0]["at_late"].ToString())) ? decimal.Parse(dtbeforeoutdateI1.Rows[0]["at_late"].ToString()) : 0;
                                    at_text_intime = (dtbeforeoutdateI1.Rows.Count > 0 && !string.IsNullOrEmpty(dtbeforeoutdateI1.Rows[0]["at_text_intime"].ToString())) ? decimal.Parse(dtbeforeoutdateI1.Rows[0]["at_text_intime"].ToString()) : 0;

                                    output.at_compid = compid;
                                    output.at_emp_serial = empserial;
                                    output.at_date = beforeoutdateI1;
                                    output.at_intime = InTime;
                                    output.at_outtime = 24 + OutTimeNEW1;
                                    output.at_outdate = date;
                                    output.at_late = LateTimeDB;
                                    output.at_status = LateTimeDB == 0 ? string.Empty : "Late";
                                    output.at_status_code = LateTimeDB == 0 ? string.Empty : "LA";
                                    output.at_manual_in = at_manual_in;
                                    output.at_manual_in_by = at_manual_in_by;
                                    output.at_manual_in_date = at_manual_in_date;
                                    output.at_text_intime = at_text_intime;
                                    output.at_text_out_date = date;
                                    output.at_text_outtime = 24 + OutTimeNEW1;
                                    if ((compid == 40 || compid == 38 || compid == 61) && InTime == 0)
                                    {
                                        output.at_status = "Late";
                                        output.at_status_code = "LA";
                                    }
                                }
                            }
                            else
                            {
                                var dtbeforeoutdateI122 = GetAttendenceTime(empserial, beforeoutdateI1);
                                at_status_code = (dtbeforeoutdateI122.Rows.Count > 0 && !string.IsNullOrEmpty(dtbeforeoutdateI122.Rows[0]["at_status_code"].ToString().Trim())) ? dtbeforeoutdateI122.Rows[0]["at_status_code"].ToString().Trim() : string.Empty;
                                if (!(new[] { "AL", "CL", "ML", "M/L", "SL", "LWP" }.Any(x => x.Equals(at_status_code, StringComparison.OrdinalIgnoreCase))))
                                {                                   
                                    InTime = (dtbeforeoutdateI122.Rows.Count > 0 && !string.IsNullOrEmpty(dtbeforeoutdateI122.Rows[0]["at_intime"].ToString())) ? decimal.Parse(dtbeforeoutdateI122.Rows[0]["at_intime"].ToString()) : 0;
                                    at_manual_in = (dtbeforeoutdateI122.Rows.Count > 0 && !string.IsNullOrEmpty(dtbeforeoutdateI122.Rows[0]["at_manual_in"].ToString())) ? bool.Parse(dtbeforeoutdateI122.Rows[0]["at_manual_in"].ToString()) : null;
                                    at_manual_in_by = (dtbeforeoutdateI122.Rows.Count > 0 && !string.IsNullOrEmpty(dtbeforeoutdateI122.Rows[0]["at_manual_in_by"].ToString())) ? dtbeforeoutdateI122.Rows[0]["at_manual_in_by"].ToString() : null;
                                    at_manual_in_date = (dtbeforeoutdateI122.Rows.Count > 0 && !string.IsNullOrEmpty(dtbeforeoutdateI122.Rows[0]["at_manual_in_date"].ToString())) ? dtbeforeoutdateI122.Rows[0]["at_manual_in_date"].ToString() : null;
                                    LateTimeDB = (dtbeforeoutdateI122.Rows.Count > 0 && !string.IsNullOrEmpty(dtbeforeoutdateI122.Rows[0]["at_late"].ToString())) ? decimal.Parse(dtbeforeoutdateI122.Rows[0]["at_late"].ToString()) : 0;
                                    at_text_intime = (dtbeforeoutdateI122.Rows.Count > 0 && !string.IsNullOrEmpty(dtbeforeoutdateI122.Rows[0]["at_text_intime"].ToString())) ? decimal.Parse(dtbeforeoutdateI122.Rows[0]["at_text_intime"].ToString()) : 0;

                                    output.at_compid = compid;
                                    output.at_emp_serial = empserial;
                                    output.at_date = beforeoutdateI1;
                                    output.at_intime = InTime;
                                    output.at_outtime = 24 + time;
                                    output.at_outdate = date;
                                    output.at_late = LateTimeDB;
                                    output.at_status = LateTimeDB == 0 ? string.Empty : "Late";
                                    output.at_status_code = LateTimeDB == 0 ? string.Empty : "LA";
                                    output.at_manual_in = at_manual_in;
                                    output.at_manual_in_by = at_manual_in_by;
                                    output.at_manual_in_date = at_manual_in_date;
                                    output.at_text_intime = at_text_intime;
                                    output.at_text_out_date = date;
                                    output.at_text_outtime = 24 + time;
                                    if ((compid == 40 || compid == 38 || compid == 61) && InTime == 0)
                                    {
                                        output.at_status = "Late";
                                        output.at_status_code = "LA";
                                    }
                                }
                            }
                        }
                        else if (OutTimeNEW1 == 0 || OutTimeNEW1> time)
                        {
                            var dtbeforeoutdateI1222 = GetAttendenceTime(empserial, beforeoutdateI1);
                            at_status_code = (dtbeforeoutdateI1222.Rows.Count > 0 && !string.IsNullOrEmpty(dtbeforeoutdateI1222.Rows[0]["at_status_code"].ToString().Trim())) ? dtbeforeoutdateI1222.Rows[0]["at_status_code"].ToString().Trim() : string.Empty;
                            if (!(new[] { "AL", "CL", "ML", "M/L", "SL", "LWP" }.Any(x => x.Equals(at_status_code, StringComparison.OrdinalIgnoreCase))))
                            {                               
                                InTime = (dtbeforeoutdateI1222.Rows.Count > 0 && !string.IsNullOrEmpty(dtbeforeoutdateI1222.Rows[0]["at_intime"].ToString())) ? decimal.Parse(dtbeforeoutdateI1222.Rows[0]["at_intime"].ToString()) : 0;
                                at_manual_in = (dtbeforeoutdateI1222.Rows.Count > 0 && !string.IsNullOrEmpty(dtbeforeoutdateI1222.Rows[0]["at_manual_in"].ToString())) ? bool.Parse(dtbeforeoutdateI1222.Rows[0]["at_manual_in"].ToString()) : null;
                                at_manual_in_by = (dtbeforeoutdateI1222.Rows.Count > 0 && !string.IsNullOrEmpty(dtbeforeoutdateI1222.Rows[0]["at_manual_in_by"].ToString())) ? dtbeforeoutdateI1222.Rows[0]["at_manual_in_by"].ToString() : null;
                                at_manual_in_date = (dtbeforeoutdateI1222.Rows.Count > 0 && !string.IsNullOrEmpty(dtbeforeoutdateI1222.Rows[0]["at_manual_in_date"].ToString())) ? dtbeforeoutdateI1222.Rows[0]["at_manual_in_date"].ToString() : null;
                                LateTimeDB = (dtbeforeoutdateI1222.Rows.Count > 0 && !string.IsNullOrEmpty(dtbeforeoutdateI1222.Rows[0]["at_late"].ToString())) ? decimal.Parse(dtbeforeoutdateI1222.Rows[0]["at_late"].ToString()) : 0;
                                at_text_intime = (dtbeforeoutdateI1222.Rows.Count > 0 && !string.IsNullOrEmpty(dtbeforeoutdateI1222.Rows[0]["at_text_intime"].ToString())) ? decimal.Parse(dtbeforeoutdateI1222.Rows[0]["at_text_intime"].ToString()) : 0;

                                output.at_compid = compid;
                                output.at_emp_serial = empserial;
                                output.at_date = beforeoutdateI1;
                                output.at_intime = InTime;
                                output.at_outtime = 24 + time;
                                output.at_outdate = date;
                                output.at_late = LateTimeDB;
                                output.at_status = LateTimeDB == 0 ? string.Empty : "Late";
                                output.at_status_code = LateTimeDB == 0 ? string.Empty : "LA";
                                output.at_manual_in = at_manual_in;
                                output.at_manual_in_by = at_manual_in_by;
                                output.at_manual_in_date = at_manual_in_date;
                                output.at_text_intime = at_text_intime;
                                output.at_text_out_date = date;
                                output.at_text_outtime = 24 + time;
                                if ((compid == 40 || compid == 38 || compid == 61) && InTime == 0)
                                {
                                    output.at_status = "Late";
                                    output.at_status_code = "LA";
                                }
                            }
                        }
                    }
                    else if ((time >= 13 || time>= 12) && OutTime<time)
                    {
                        if (!(new[] { "AL", "CL", "ML", "M/L", "SL", "LWP" }.Any(x => x.Equals(at_status_code, StringComparison.OrdinalIgnoreCase))))
                        {
                            output.at_compid = compid;
                            output.at_emp_serial = empserial;
                            output.at_date = date;
                            output.at_intime = InTime;
                            output.at_outtime = time;
                            output.at_outdate = date;
                            output.at_late = LateTimeDB;
                            output.at_status = LateTimeDB == 0 ? string.Empty : "Late";
                            output.at_status_code = LateTimeDB == 0 ? string.Empty : "LA";
                            output.at_manual_in = at_manual_in;
                            output.at_manual_in_by = at_manual_in_by;
                            output.at_manual_in_date = at_manual_in_date;
                            output.at_text_intime = at_text_intime;
                            output.at_text_out_date = date;
                            output.at_text_outtime = time;
                            if ((compid == 40 || compid == 38 || compid == 61) && InTime == 0)
                            {
                                output.at_status = "Late";
                                output.at_status_code = "LA";
                            }
                        }
                    }
                }
            }
            if (OutTime >= 24)
            {
                if (!(new[] { "AL", "CL", "ML", "M/L", "SL", "LWP" }.Any(x => x.Equals(at_status_code, StringComparison.OrdinalIgnoreCase))))
                {
                    output.at_compid = compid;
                    output.at_emp_serial = empserial;
                    output.at_date = date;
                    output.at_intime = InTime;
                    output.at_outtime = OutTime-24;
                    output.at_outdate = at_outdatedb;
                    output.at_late = LateTimeDB;
                    output.at_status = LateTimeDB == 0 ? string.Empty : "Late";
                    output.at_status_code = LateTimeDB == 0 ? string.Empty : "LA";
                    output.at_manual_in = at_manual_in;
                    output.at_manual_in_by = at_manual_in_by;
                    output.at_manual_in_date = at_manual_in_date;
                    output.at_text_intime = at_text_intime;
                    output.at_text_out_date = at_text_out_date;
                    output.at_text_outtime = at_text_outtime;
                    if ((compid == 40 || compid == 38 || compid == 61) && InTime == 0)
                    {
                        output.at_status = "Late";
                        output.at_status_code = "LA";
                    }
                }
            }
            return output;
        }
        private DataTable GetAttendenceTime(int empserial,string date)
        {
            var dtAttenInfo = _dgCommon.get_InformationDataTable(string.Format("select at_intime,at_outtime,at_outdate,at_late,at_shift,at_status_code,at_manual_in," +
                "at_manual_in_by,at_manual_in_date,at_manual_out,at_manual_out_by,at_manual_out_date,at_holiday,at_text_intime,at_text_out_date,at_text_outtime" +
                " from dg_pay_attendance where at_emp_serial={0} AND at_date='{1}'", empserial, date), _sqlConnection);
            return dtAttenInfo;
        }
        private decimal CalLateTimeEmployee(int empSerial,string date,decimal time)
        {
            decimal LateTime = new decimal(0);
            var dtAttInfo = _dgCommon.get_InformationDataTable(string.Format("select at_holiday,at_shift from dg_pay_attendance where at_emp_serial={0} and at_date='{1}'", empSerial, date), _sqlConnection);
            string at_holiday = dtAttInfo.Rows.Count > 0 ? dtAttInfo.Rows[0]["at_holiday"].ToString().Trim() : string.Empty;
            int at_shift = dtAttInfo.Rows.Count > 0 ? int.Parse(dtAttInfo.Rows[0]["at_shift"].ToString()) : 0;
            var dtShiftInfo = _dgCommon.get_InformationDataTable(string.Format("select sh_Lateafter from dg_pay_shift where sh_code={0}", at_shift), _sqlConnection);
            decimal sh_Lateafter = dtShiftInfo.Rows.Count > 0 ? decimal.Parse(dtShiftInfo.Rows[0]["sh_Lateafter"].ToString()) : 0;

            int ShiftTimeHrs = Convert.ToInt32(Math.Floor(sh_Lateafter));
            int ShiftTimeMin = Convert.ToInt32((sh_Lateafter - ShiftTimeHrs) * 100);
            int ShiftTimeTotMin = (ShiftTimeHrs * 60) + ShiftTimeMin;

            int InTimeHrs = Convert.ToInt32(Math.Floor(time));
            int InTimeMin = Convert.ToInt32((time - InTimeHrs) * 100);
            int InTimeTotMin = (InTimeHrs * 60) + InTimeMin;

            int LateHrs = Convert.ToInt32((InTimeTotMin - ShiftTimeTotMin) / 60);
            decimal LateMin = ((Convert.ToDecimal(InTimeTotMin) - Convert.ToDecimal(ShiftTimeTotMin)) - (Convert.ToDecimal(LateHrs) * 60)) / 100;
            LateHrs = LateHrs < 0 ? 0 : LateHrs;
            LateMin = LateMin < 0 ? 0 : LateMin;

            if (string.IsNullOrEmpty(at_holiday))
            {
                LateTime = ((LateHrs > 0 ? LateHrs : 0) + (LateMin > 0 ? LateMin : 0));
            }
            return LateTime;
        }
        private async Task<ReturnObject> GetIsAllProcessContinue(int compid, string procsDt, string processTP, string userName)
        {
            var result = new ReturnObject();
            if (_dgCommon.get_InformationDataTable(string.Format("select procs_compid from dg_pay_Process_startOrStop where procs_compid={0} and procs_date='{1}' and procs_type='{2}' and procs_startBy='{3}'", compid, procsDt, processTP, userName), _sqlConnection).Rows.Count > 0)
                return result;

            var dtIsProcess = await _dgCommon.get_InformationDataTableAsync(string.Format("select procs_compid,procs_type,procs_status from dg_pay_Process_startOrStop where procs_compid={0} and procs_date='{1}' and procs_type='{2}'", compid, procsDt, processTP), _sqlConnection);
            if (dtIsProcess.Rows.Count > 0)
            {
                result.IsSuccess = true;
                result.dataTable = dtIsProcess;
                return result;
            }
            return result;
        }
        private void CreateTxtFromCsv(string csvFilePath, string txtFilePath)
        {
            try
            {
                // Check if the CSV file exists
                if (!File.Exists(csvFilePath))
                {
                    Console.WriteLine($"CSV file not found at {csvFilePath}");
                    return;
                }

                // Read all lines from the CSV file
                var csvLines = File.ReadAllLines(csvFilePath);

                // Write lines to a TXT file
                using (var writer = new StreamWriter(txtFilePath))
                {
                    foreach (var line in csvLines)
                    {
                        // Optional: Replace commas with tabs or other delimiters
                        string processedLine = line.Replace(",", "\t");

                        // Write the processed line to the TXT file
                        writer.WriteLine(processedLine);
                    }
                }

                Console.WriteLine($"File successfully created at: {txtFilePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
            }
        }
        //End New Action 12/04/2023

        /*private DateTime GetCurrentDateTime()
        {
            return DateTime.Now;
        }
        private async Task<DataTable> GetPayAttnClock(int compID)
        {
            var data = await _dgCommon.get_InformationDataTableAsync("select adi_stposition,adi_noofposition from dg_pay_attdataImportsetup where adi_code='CLOCK' and adi_company=" + compID, _sqlConnection);
            return data;
        }
        private async Task<DataTable> GetPayAttnDay(int compID)
        {
            var data = await _dgCommon.get_InformationDataTableAsync("select adi_stposition,adi_noofposition from dg_pay_attdataImportsetup where adi_code='DAY' and adi_company=" + compID, _sqlConnection);
            return data;
        }
        private async Task<DataTable> GetPayAttnMonth(int compID)
        {
            var data = await _dgCommon.get_InformationDataTableAsync("select adi_stposition,adi_noofposition from dg_pay_attdataImportsetup where adi_code='MONTH' and adi_company=" + compID, _sqlConnection);
            return data;
        }
        private async Task<DataTable> GetPayAttnYear(int compID)
        {
            var data = await _dgCommon.get_InformationDataTableAsync("select adi_stposition,adi_noofposition from dg_pay_attdataImportsetup where adi_code='YEAR' and adi_company=" + compID, _sqlConnection);
            return data;
        }
        private async Task<DataTable> GetPayAttnHours(int compID)
        {
            var data = await _dgCommon.get_InformationDataTableAsync("select adi_stposition,adi_noofposition from dg_pay_attdataImportsetup where adi_code='HOURS' and adi_company=" + compID, _sqlConnection);
            return data;
        }
        private async Task<DataTable> GetPayAttnMinutes(int compID)
        {
            var data = await _dgCommon.get_InformationDataTableAsync("select adi_stposition,adi_noofposition from dg_pay_attdataImportsetup where adi_code='MINUTES' and adi_company=" + compID, _sqlConnection);
            return data;
        }
        private async Task<DataTable> GetPayAttnProxID(int compID)
        {
            var data = await _dgCommon.get_InformationDataTableAsync("select adi_stposition,adi_noofposition from dg_pay_attdataImportsetup where adi_code='EMPID' and adi_company=" + compID, _sqlConnection);
            return data;
        }

        //2nd
        private async Task<DataTable> GetPayAttnClock1(int compID)
        {
            var data = await _dgCommon.get_InformationDataTableAsync("select adi_stposition1,adi_noofposition1 from dg_pay_attdataImportsetup where adi_code='CLOCK' and adi_company=" + compID, _sqlConnection);
            return data;
        }
        private async Task<DataTable> GetPayAttnDay1(int compID)
        {
            var data = await _dgCommon.get_InformationDataTableAsync("select adi_stposition1,adi_noofposition1 from dg_pay_attdataImportsetup where adi_code='DAY' and adi_company=" + compID, _sqlConnection);
            return data;
        }
        private async Task<DataTable> GetPayAttnMonth1(int compID)
        {
            var data = await _dgCommon.get_InformationDataTableAsync("select adi_stposition1,adi_noofposition1 from dg_pay_attdataImportsetup where adi_code='MONTH' and adi_company=" + compID, _sqlConnection);
            return data;
        }
        private async Task<DataTable> GetPayAttnYear1(int compID)
        {
            var data = await _dgCommon.get_InformationDataTableAsync("select adi_stposition1,adi_noofposition1 from dg_pay_attdataImportsetup where adi_code='YEAR' and adi_company=" + compID, _sqlConnection);
            return data;
        }
        private async Task<DataTable> GetPayAttnHours1(int compID)
        {
            var data = await _dgCommon.get_InformationDataTableAsync("select adi_stposition1,adi_noofposition1 from dg_pay_attdataImportsetup where adi_code='HOURS' and adi_company=" + compID, _sqlConnection);
            return data;
        }
        private async Task<DataTable> GetPayAttnMinutes1(int compID)
        {
            var data = await _dgCommon.get_InformationDataTableAsync("select adi_stposition1,adi_noofposition1 from dg_pay_attdataImportsetup where adi_code='MINUTES' and adi_company=" + compID, _sqlConnection);
            return data;
        }
        private async Task<DataTable> GetPayAttnProxID1(int compID)
        {
            var data = await _dgCommon.get_InformationDataTableAsync("select adi_stposition1,adi_noofposition1 from dg_pay_attdataImportsetup where adi_code='EMPID' and adi_company=" + compID, _sqlConnection);
            return data;
        }
        private async Task<bool> InsertDataAttn(string empSerial, string proxID, string nDate, string ntime)
        {
            bool flag = false;
            try
            {
                try
                {
                    SqlCommand command = new SqlCommand("dg_pay_Att_Insert_Textfile", _sqlConnection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@at_emp_serial", empSerial);
                    command.Parameters.AddWithValue("@at_proxid", proxID);
                    command.Parameters.AddWithValue("@at_groupid", 0);
                    command.Parameters.AddWithValue("@at_compid", 0);
                    command.Parameters.AddWithValue("@at_date", nDate);
                    command.Parameters.AddWithValue("@at_intime", ntime);
                    await _sqlConnection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                    flag = true;
                }
                catch (Exception ex)
                {
                    ex.ToString();
                    flag = false;
                }
            }
            finally
            {
                await _sqlConnection.CloseAsync();
            }
            return flag;
        }*/
            #endregion
        }
    }
