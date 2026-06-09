using BLL.Interfaces.Manager.Report;
using BLL.Utility;
using BOL.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Reporting.NETCore;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.Drawing;
using BarcodeLib;
using SharpCompress;

namespace DAL.Implementation.Manager.Report
{
    public class ReportManager : IReportManager
    {
        private readonly Dg_Common _dgCommon;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly SqlConnection _connection;
        public ReportManager(Dg_Common dgCommon, IWebHostEnvironment webHostEnvironment)
        {
            _dgCommon = dgCommon;
            _webHostEnvironment = webHostEnvironment;
            _connection = new SqlConnection(Getway.Dg_Payroll);
        }

        #region"Employee"
        public byte[] Export_Report_Employee_Details_InActive(string reportType, int companyID, string userName) //ok
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_EmployeeDetails_inactive "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "DataSet1";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Cd_Inactiveemp_details.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("Title",string.Concat("Employee InActive List - ",ReportTitle.DepartmentName,ReportTitle.SectionName,ReportTitle.BuildingName,ReportTitle.LineName)),
                new ReportParameter("PrintUser",ReportTitle.PrintUser)
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Export_Report_Employee_Details_Active(string reportType, int companyID, string userName) //ok
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_EmployeeDetails "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "DataSet1";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Cd_emp_details.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("Employee Active List - ",ReportTitle.DepartmentName,ReportTitle.SectionName,ReportTitle.BuildingName,ReportTitle.LineName))
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Dg_ActiveEmployeesWithImage(string reportType, int companyID, string userName) //ok
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_rpt_activeEmployeeListImg "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "Attinout";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Dg_ActiveEmployeeImage.rdlc";
            string imgPath = new Uri($"{_webHostEnvironment.WebRootPath}\\EmployeeImage\\").AbsoluteUri;
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("EmpImagePath",imgPath),
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("Active Employee Image - ",ReportTitle.DepartmentName,ReportTitle.SectionName,ReportTitle.BuildingName,ReportTitle.LineName)),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }

        public byte[] Export_Report_Employee_Details_Active_With_Image(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable(string.Format("Dg_Pay_Rep_EmployeeDetails {0},'{1}'", companyID, userName), _connection);
            if (data.Rows.Count > 0)
            {                
                string baseImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "EmployeeImage", companyID.ToString());               
                var existingFiles = new HashSet<string>(Directory.GetFiles(baseImagePath, "*.jpg").Select(Path.GetFileNameWithoutExtension), StringComparer.OrdinalIgnoreCase);
                string baseUri = new Uri(baseImagePath).AbsoluteUri;
                data.AsEnumerable().Where(row => existingFiles.Contains(row["emp_no"]?.ToString())).ToList().ForEach(row => row["empImage"] = $"{baseUri}/{row["emp_no"]}.jpg");
            }
            string dataset = "DataSet1";
            string reportPath = Path.Combine(_webHostEnvironment.WebRootPath, "Report", "dg_emp_details_with_image.rdlc");
            string imgPath = new Uri(Path.Combine(_webHostEnvironment.WebRootPath, "EmployeeImage")).AbsoluteUri;
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("EmpImagePath", imgPath),
                new ReportParameter("PrintUser", ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("Employee Active List With Image - ",ReportTitle.DepartmentName,ReportTitle.SectionName,ReportTitle.BuildingName,ReportTitle.LineName))
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, reportPath, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }

        public byte[] Export_Report_Employee_Details_In_Active_With_Image(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_EmployeeDetails_inactive_image " + companyID + ",'" + userName + "'", _connection);
            if (data.Rows.Count > 0)
            {
                string baseImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "EmployeeImage", companyID.ToString());
                var existingFiles = new HashSet<string>(Directory.GetFiles(baseImagePath, "*.jpg").Select(Path.GetFileNameWithoutExtension), StringComparer.OrdinalIgnoreCase);
                string baseUri = new Uri(baseImagePath).AbsoluteUri;
                data.AsEnumerable().Where(row => existingFiles.Contains(row["emp_no"]?.ToString())).ToList().ForEach(row => row["empImage"] = $"{baseUri}/{row["emp_no"]}.jpg");
            }
            string dataset = "DataSet1";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\dg_emp_details_with_image_inactive.rdlc";
            string imgPath = new Uri($"{_webHostEnvironment.WebRootPath}\\EmployeeImage\\").AbsoluteUri;
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("EmpImagePath",imgPath),
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("Employee In Active List With Image - ",ReportTitle.DepartmentName,ReportTitle.SectionName,ReportTitle.BuildingName,ReportTitle.LineName))
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Dg_JoinDateWise_Employee_Details_Active(string reportType, int companyID, string userName) //ok
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_JoinDateWiseEmployeeDetails "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "DataSet1";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Cd_JoinDateWiseEmp_details.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("Title",string.Concat("Join Date wise Employee Active List - ",ReportTitle.DepartmentName,ReportTitle.SectionName,ReportTitle.BuildingName,ReportTitle.LineName)),
                new ReportParameter("PrintUser",ReportTitle.PrintUser)
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Dg_CreateReportFileIDCARD(string reportType, int companyID, string userName) //ok
        {
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_EmployeeDetails "+ companyID + ",'"+ userName + "'", _connection);
            this.AddDataColumnWithBarcode(data, 4);
            string dataset = "ID";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Cd_IDCard.rdlc";
            string imgPath = new Uri($"{_webHostEnvironment.WebRootPath}\\EmployeeImage\\").AbsoluteUri;
            string imgPathQR = new Uri($"{_webHostEnvironment.WebRootPath}\\Employee_QRCode\\DG_QR_Code.jpg").AbsoluteUri;
            string imgCompLogo = new Uri($"{_webHostEnvironment.WebRootPath}\\AuthSign\\comp_logo.png").AbsoluteUri;
            string imgPathEmpSign = new Uri($"{_webHostEnvironment.WebRootPath}\\EmployeeSignature\\").AbsoluteUri;
            string imgPathAuthSign = new Uri($"{_webHostEnvironment.WebRootPath}\\AuthSign\\").AbsoluteUri;
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter ("compLogo", imgCompLogo),
                new ReportParameter("EmpImagePath",imgPath),
                new ReportParameter("QR",imgPathQR),
                new ReportParameter("EmpSign",imgPathEmpSign),
                new ReportParameter("AuthSign",imgPathAuthSign)
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Dg_CreateReportFileIDCARD_BN(string reportType, int companyID, string userName) //ok
        {
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_EmpIdCardBangla "+ companyID + ",'"+ userName + "'", _connection);
            this.AddDataColumnWithBarcode(data, 6);
            string dataset = "IDCard";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Cd_IDCardBangla.rdlc";
            string imgPath = new Uri($"{_webHostEnvironment.WebRootPath}\\EmployeeImage\\").AbsoluteUri;
            string imgPathEmpSign = new Uri($"{_webHostEnvironment.WebRootPath}\\EmployeeSignature\\").AbsoluteUri;
            string imgCompLogo = new Uri($"{_webHostEnvironment.WebRootPath}\\AuthSign\\comp_logo.png").AbsoluteUri;
            string imgPathAuthSign = new Uri($"{_webHostEnvironment.WebRootPath}\\AuthSign\\").AbsoluteUri;
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter ("compLogo", imgCompLogo),
                new ReportParameter("EmpImagePath",imgPath),
                new ReportParameter("EmpSign",imgPathEmpSign),
                new ReportParameter("AuthSign",imgPathAuthSign)
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Dg_EmpAgeCertificate(string reportType, int companyID, string userName) //ok
        {
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_AgeCertificate "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "AgeCertificate";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Dg_Emp_AgeCertificate.rdlc";
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType);
            _dgCommon.saveChanges("delete dg_print_employeelist where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Dg_IncrementLetter(string reportType, int companyID, string userName) //ok
        {
            var data = _dgCommon.get_InformationDataTable("Dg_Rep_IncrementLetter "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "IncrementLetter";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Dg_IncrementLetter.rdlc";
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType);
            _dgCommon.saveChanges("delete dg_print_employeelist where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Dg_PromotionWithIncrementLetter(string reportType, int companyID, string userName) //ok
        {
            var data = _dgCommon.get_InformationDataTable("Dg_Rep_PromotionWithIncrementLetter "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "IncrementLetter";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Dg_PromotionWithIncrementLetter.rdlc";
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType);
            _dgCommon.saveChanges("delete dg_print_employeelist where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Dg_IncrementLetter_Bangla(string reportType, int companyID, string userName) //ok
        {
            var data = _dgCommon.get_InformationDataTable("Dg_Rep_IncrementLetter "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "IncrementLetter";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Dg_IncrementLetter_Bangla.rdlc";
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType);
            _dgCommon.saveChanges("delete dg_print_employeelist where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Dg_PromotionWithIncrementLetter_Bangla(string reportType, int companyID, string userName) //ok
        {
            var data = _dgCommon.get_InformationDataTable("Dg_Rep_PromotionWithIncrementLetter "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "IncrementLetter";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Dg_PromotionWithIncrementLetter_Bangla.rdlc";
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType);
            _dgCommon.saveChanges("delete dg_print_employeelist where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Dg_AppointmentLetter(string reportType, int companyID, string userName) //ok
        {
            var data = _dgCommon.get_InformationDataTable("Dg_Rep_AppointmentLetter "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "AppLttrWorker";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Cd_AppointmentLetter.rdlc";
            string groupLogo = new Uri($"{_webHostEnvironment.WebRootPath}\\AuthSign\\GroupLogo.png").AbsoluteUri;
            string authSign = new Uri($"{_webHostEnvironment.WebRootPath}\\AuthSign\\").AbsoluteUri;
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("groupLogo",groupLogo),
                new ReportParameter("authSign",authSign)
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Dg_AppointmentLetter_IFL(string reportType, int companyID, string userName) //ok
        {
            var data = _dgCommon.get_InformationDataTable("Dg_Rep_AppointmentLetter "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "AppLttrWorker";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Dg_AppointmentLetter_IFL.rdlc";
            string groupLogo = new Uri($"{_webHostEnvironment.WebRootPath}\\AuthSign\\GroupLogo.png").AbsoluteUri;
            string autSign = new Uri($"{_webHostEnvironment.WebRootPath}\\AuthSign\\53.jpg").AbsoluteUri;
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("groupLogo",groupLogo),
                new ReportParameter("authSign",autSign)
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Dg_AppointmentLetter_StaffBangla(string reportType, int companyID, string userName) //ok
        {
            var data = _dgCommon.get_InformationDataTable("Dg_Rep_AppointmentLetter_StaffBN "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "AppLttrWorker";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Cd_AppointmentLetter_StaffBN.rdlc";
            string groupLogo = new Uri($"{_webHostEnvironment.WebRootPath}\\AuthSign\\GroupLogo.png").AbsoluteUri;
            string authSign = new Uri($"{_webHostEnvironment.WebRootPath}\\AuthSign\\").AbsoluteUri;
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("groupLogo",groupLogo),
                new ReportParameter("AuthSign",authSign)
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Dg_AppointmentLetter_EN(string reportType, int companyID, string userName)
        {
            var data = _dgCommon.get_InformationDataTable("Dg_Rep_AppointmentLetter "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "AppLttrWorker";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Dg_AppointmentLetter_EN.rdlc";
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType);
            _dgCommon.saveChanges("delete dg_print_employeelist where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Dg_NoticeLetter_1st(string reportType, int companyID, string userName) //ok
        {
            var data = _dgCommon.get_InformationDataTable("Dg_Rep_NoticeLetter "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "NoticeLetter";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Dg_NoticeLetter_1st.rdlc";
            string groupLogo = new Uri($"{_webHostEnvironment.WebRootPath}\\AuthSign\\GroupLogo.png").AbsoluteUri;
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("groupLogo",groupLogo)
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Dg_NoticeLetter_2nd(string reportType, int companyID, string userName) //ok
        {
            var data = _dgCommon.get_InformationDataTable("Dg_Rep_NoticeLetter "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "NoticeLetter";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Dg_NoticeLetter_2nd.rdlc";
            string groupLogo = new Uri($"{_webHostEnvironment.WebRootPath}\\AuthSign\\GroupLogo.png").AbsoluteUri;
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("groupLogo",groupLogo)
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Dg_voluntarily_resign_letter(string reportType, int companyID, string userName) //ok
        {
            var data = _dgCommon.get_InformationDataTable("Dg_Rep_NoticeLetter " + companyID + ",'" + userName + "'", _connection);
            string dataset = "NoticeLetter";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Dg_voluntarily_resign_letter.rdlc";
            string groupLogo = new Uri($"{_webHostEnvironment.WebRootPath}\\AuthSign\\GroupLogo.png").AbsoluteUri;
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("groupLogo",groupLogo)
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Dg_MaleFemaleDetails(string reportType, int companyID, string userName) //ok
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Rep_MaleFemale_Detailed "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "MaleFemaleDetailed";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Dg_MaleFemaleDetails.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("Male / Female Detailed Report - ",ReportTitle.DepartmentName,ReportTitle.SectionName,ReportTitle.BuildingName,ReportTitle.LineName)),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Dg_MaleFemaleSummary(string reportType, int companyID, string userName) //ok
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Rep_MaleFemale_Summary "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "MaleFemale";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Dg_MaleFemaleSummary.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("Male / Female Summary Report - ",ReportTitle.DepartmentName,ReportTitle.SectionName,ReportTitle.BuildingName,ReportTitle.LineName)),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Dg_EmployeeDetailsRegligion(string reportType, int companyID, string userName) //ok
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Rep_EmployeeDetails_Regligion "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "EmployeeDetails_Religion";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Dg_EmployeeDetailsRegligion.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("EmployeeDetails_Religion - ",ReportTitle.DepartmentName,ReportTitle.SectionName,ReportTitle.BuildingName,ReportTitle.LineName)),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Dg_ProximityCardChecklist(string reportType, int companyID, string userName) //ok
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Rep_EmployeeProxIDChkList "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "ProxIDList";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Cd_ProximityCardChecklist.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("Proximity Card Checklist - ",ReportTitle.DepartmentName,ReportTitle.SectionName,ReportTitle.BuildingName,ReportTitle.LineName)),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Dg_EmployeewiseIncrementDetails(string reportType, int companyID, string userName) //ok
        {
            var data = _dgCommon.get_InformationDataTable("Dg_Rep_EmpIncrement_Details "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "rpt_empIncrement_Details";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Dg_EmployeewiseIncrementDetails.rdlc";
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType);
            _dgCommon.saveChanges("delete dg_print_employeelist where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Dg_EmployeewiseIncremen_Sheet_Details(string reportType, int companyID, string userName) //ok
        {
            var data = _dgCommon.get_InformationDataTable("Dg_Rep_EmpIncrement_Sheet " + companyID + ",'" + userName + "'", _connection);
            string dataset = "rpt_empIncrement_Details";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Dg_EmployeewiseIncrement_Sheet_Details.rdlc";
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType);
            _dgCommon.saveChanges("delete dg_print_employeelist where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Dg_CutoffDatewiseIncrementPendingList(string reportType, int companyID, string userName) //ok
        {
            var data = _dgCommon.get_InformationDataTable("Dg_Rep_empIncrement_PendingList "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "rpt_empIncrement_PendingList";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Dg_CutoffDatewiseIncrementPendingList.rdlc";
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType);
            _dgCommon.saveChanges("delete dg_print_employeelist where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Dg_CutoffDatewiseIncrementApprovedList(string reportType, int companyID, string userName) //ok
        {
            var data = _dgCommon.get_InformationDataTable("Dg_Rep_empIncremented_List "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "rpt_empIncrement_ApproveList";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Dg_CutoffDatewiseIncrementApprove.rdlc";
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType);
            _dgCommon.saveChanges("delete dg_print_employeelist where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Dg_EmployeeWiseDetailedInformation(string reportType, int companyID, string userName) //ok
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Rep_emp_CV_Single 0,'" + userName + "'," + companyID + "", _connection);
            string dataset = "rpt_emp_CV";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Dg_EmployeeInfo.rdlc";
            string imgPath = new Uri($"{_webHostEnvironment.WebRootPath}\\EmployeeImage\\").AbsoluteUri;            
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("EmpImagePath",imgPath),
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Dg_EmployeeInformation_BankAccForm(string reportType, int companyID, string userName) //ok
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Rep_emp_CV_Single 0,'" + userName + "'," + companyID + "", _connection);
            DataColumn[] newCol = new DataColumn[]
            {
                new DataColumn("IsShowEmpImg", typeof(bool)),
                new DataColumn("IsShowEmpSignImg", typeof(bool))
            };
            data.Columns.AddRange(newCol);
            for (int i = 0; i < data.Rows.Count; i++)
            {
                string img_PathEmp = $"{_webHostEnvironment.WebRootPath}\\EmployeeImage\\" + data.Rows[i]["compid"].ToString() + "\\" + data.Rows[i]["emp_no"].ToString() + ".jpg";
                string img_PathSign = $"{_webHostEnvironment.WebRootPath}\\EmployeeSignature\\" + data.Rows[i]["compid"].ToString() + "\\" + data.Rows[i]["emp_no"].ToString() + ".png";
                data.Rows[i]["IsShowEmpImg"] = File.Exists(img_PathEmp) ? true : false;
                data.Rows[i]["IsShowEmpSignImg"] = File.Exists(img_PathSign) ? true : false;
            }
            string dataset = "rpt_emp_bnkAccForm";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Dg_EmployeeInfo_bankAccForm.rdlc";
            string imgPath = new Uri($"{_webHostEnvironment.WebRootPath}\\EmployeeImage\\").AbsoluteUri;
            string imgPathSign = new Uri($"{_webHostEnvironment.WebRootPath}\\EmployeeSignature\\").AbsoluteUri;
            string imgBankLogo = new Uri($"{_webHostEnvironment.WebRootPath}\\BankLogo\\DBBL.jpeg").AbsoluteUri;
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("EmpImagePath", imgPath),
                new ReportParameter("EmpImagePathSign", imgPathSign),
                new ReportParameter("PrintUser", ReportTitle.PrintUser),
                new ReportParameter("BankLogo", imgBankLogo)
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Dg_Emp_TiffinBillStatus(string reportType, int companyID, string userName) //ok
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("dg_rpt_tiffinbill_status "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "DataSet1";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Cd_EmpTiffinBillStatus.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("Employee Tiffin Bill Status - ",ReportTitle.DepartmentName,ReportTitle.SectionName,ReportTitle.BuildingName,ReportTitle.LineName)),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Dg_Emp_NightBillStatus(string reportType, int companyID, string userName) //ok
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("dg_rpt_nightbill_status "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "DataSet1";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Cd_EmpNightBillStatus.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("Employee Night Bill Status - ",ReportTitle.DepartmentName,ReportTitle.SectionName,ReportTitle.BuildingName,ReportTitle.LineName)),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Export_Report_Employee_shiftchange_history(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_Shift_change_history "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "DataSet1";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Cd_emp_shiftchange_history.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("Employee shiftchange history - ",ReportTitle.DepartmentName,ReportTitle.SectionName,ReportTitle.BuildingName,ReportTitle.LineName))
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Export_Report_Employee_shiftchange_history_New(ReportParameterModel obj)
        {
            string empID = string.Empty;
            string salCatID = string.Empty;
            if (obj.empNoFilter.Count > 0)
            {
                var empArr = new List<int>();
                foreach (var item in obj.empNoFilter)
                {
                    if (item.isGet == true)
                    {
                        empArr.Add(item.EmpNo);
                        empID = " and emp_no in(" + string.Join(",", empArr) + ")";
                    }
                    else
                    {
                        empArr.Add(item.EmpNo);
                        empID = " and emp_no not in(" + string.Join(",", empArr) + ")";
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
                    salCatID = " and oi_salcategory in(" + catParameter + ")";
                }
            }

            var data = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_Shift_change_history_New '" + obj.Compid + "','"+ obj.Department + "','" + obj.section + "','" + obj.Building + "','" + obj.Floor + "','" + obj.Line + "','" + obj.Shift + "','" + obj.Grade + "','" + obj.Start_date + "','" + obj.End_date + "','" + obj.User + "','" + empID + "','" + salCatID + "'", _connection);
            string dataset = "DataSet1";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\dg_emp_shiftchange_history.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",obj.User),
                new ReportParameter("Title",string.Concat("Employee shiftchange history - From : ",Convert.ToDateTime(obj.Start_date).ToString("dd/MMM/yyyy")," To : ",Convert.ToDateTime(obj.End_date).ToString("dd/MMM/yyyy")))
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, obj.reportType, reportParameters);
            return reportBytes;
        }
        public byte[] Dg_EmpShiftGroupChecklist(string reportType, int companyID, string userName)
        {
            var data = _dgCommon.get_InformationDataTable(string.Format("Dg_Rep_Employee_ShiftGroupCheckList {0},'{1}'", companyID, userName), _connection);
            string dataset = "shiftGroupChkList";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Dg_EmpShiftGroupChecklist.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",userName),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] EmployeeNominationForm(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_NominationForm "+ companyID + ",'"+ userName + "'", _connection);
            DataColumn nomineeCol = new DataColumn("IsShowNomineeImg",typeof(bool));
            //DataColumn authSignCol = new DataColumn("IsAuthSing",typeof(bool));
            data.Columns.Add(nomineeCol);
            for (int i = 0; i < data.Rows.Count; i++)
            {
                string nomineeImagePath = $"{_webHostEnvironment.WebRootPath}\\NomineeImage\\" + data.Rows[i]["compid"].ToString() + "\\" + data.Rows[i]["emp_no"].ToString() + ".jpg";
                if (File.Exists(nomineeImagePath))
                {
                    data.Rows[i]["IsShowNomineeImg"] = true;
                }
                else
                {
                    data.Rows[i]["IsShowNomineeImg"] = false;
                }
            }
            string dataset = "NominationForm";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Dg_Pay_NominationForm.rdlc";
            string imgPath = new Uri($"{_webHostEnvironment.WebRootPath}\\NomineeImage\\").AbsoluteUri;
            string groupLogo = new Uri($"{_webHostEnvironment.WebRootPath}\\AuthSign\\GroupLogo.png").AbsoluteUri;
            string signAuth = new Uri($"{_webHostEnvironment.WebRootPath}\\AuthSign\\").AbsoluteUri;
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("EmpImagePath",imgPath),
                new ReportParameter("groupLogo",groupLogo),
                new ReportParameter("authSignPath",signAuth)
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] EmployeeJoiningLetter(string reportType, int companyID, string userName)
        {
            var data = _dgCommon.get_InformationDataTable("dg_pay_rep_JoiningLetter "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "joiningLater";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Dg_Pay_JoiningLetter_Bangla.rdlc";
            string groupLogoImgPath = new Uri($"{_webHostEnvironment.WebRootPath}\\AuthSign\\GroupLogo.png").AbsoluteUri;
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("GroupLogo",groupLogoImgPath)
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] EmployeeJobApplication(string reportType, int companyID, string userName)
        {
            var data = _dgCommon.get_InformationDataTable("dg_pay_rep_JobApplication "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "jobApplicationBN";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Dg_Pay_JobApplicationForm_Bangla.rdlc";
            string groupLogoImgPath = new Uri($"{_webHostEnvironment.WebRootPath}\\AuthSign\\GroupLogo.png").AbsoluteUri;
            string empImage = new Uri($"{_webHostEnvironment.WebRootPath}\\EmployeeImage\\").AbsoluteUri;
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("EmpImage",empImage),
                new ReportParameter("GroupLogo",groupLogoImgPath)
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] EmployeeDetailsExcel(string reportType, int companyID, string userName)
        {
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_EmployeeExcel "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "EmpDetailsExcel";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Dg_Pay_Rep_EmployeeDetailsExcel.rdlc";
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType);
            _dgCommon.saveChanges("delete dg_print_employeelist where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] EmployeeBackgroundCheck(string reportType, int companyID, string userName)
        {
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_Emp_background "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "BackgroundCheckBN";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Dg_Pay_Rep_EmpBackgroundChk.rdlc";
            string groupLogoImgPath = new Uri($"{_webHostEnvironment.WebRootPath}\\AuthSign\\GroupLogo.png").AbsoluteUri;
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("GroupLogo",groupLogoImgPath)
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] EmployeeIvelatution_From(string reportType, int companyID, string userName)
        {
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_Emp_the_interview_Ivelatution_From "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "Ivelatution_From";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Dg_Pay_rep_Emp_Ivelatution_From.rdlc";
            string groupLogoImgPath = new Uri($"{_webHostEnvironment.WebRootPath}\\AuthSign\\GroupLogo.png").AbsoluteUri;
            string authSign = new Uri($"{_webHostEnvironment.WebRootPath}\\AuthSign\\").AbsoluteUri;
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("GroupLogo",groupLogoImgPath),
                new ReportParameter("AuthSign",authSign)
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] EmployeeBudgetReport(string reportType, int companyID, string userName)
        {
            var data = _dgCommon.get_InformationDataTable("dg_pay_Employee_BudgetList " + companyID, _connection);
            string dataset = "budget";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\dg_emp_budget_history.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",userName)
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            return reportBytes;
        }
        public byte[] EmployeeServiceBookReport(string reportType, int companyID, string userName)
        {
            var data = _dgCommon.get_InformationDataTable(string.Format("dg_pay_Emp_serviceBook {0},'{1}'", companyID, userName), _connection);
            string dataset = "serviceBook";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\dg_emp_service_book.rdlc";
            string authSignPath = $"{_webHostEnvironment.WebRootPath}\\AuthSign\\" + companyID + ".jpg";
            data.AsEnumerable().ToList().ForEach(row =>
            {
                string imagePathEmp = $"{_webHostEnvironment.WebRootPath}\\EmployeeImage\\" + companyID + "\\" + row["emp_no"] +".jpg";
                row["authImage"] = File.Exists(authSignPath) ? new Uri(authSignPath).AbsoluteUri : string.Empty;
                row["employeeImage"] = File.Exists(imagePathEmp) ? new Uri(imagePathEmp).AbsoluteUri : string.Empty;
            });
            var reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",userName)
            };
            SubreportProcessingEventHandler sReportHandler = (sender, e) =>
            {
                int emp_serial = int.Parse(e.Parameters["empSerial"].Values[0].ToString());
                var data = _dgCommon.get_InformationDataTable(string.Format("dg_pay_Emp_serviceBook_info {0}", emp_serial), _connection);
                var data2 = _dgCommon.get_InformationDataTable(string.Format("dg_pay_Emp_serviceBook_Annual_Leave_info {0}", emp_serial), _connection);
                e.DataSources.Add(new ReportDataSource("serviceIncInfo", data));
                e.DataSources.Add(new ReportDataSource("serviceIncInfo2", data2));
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters, sReportHandler);
            _dgCommon.saveChanges(string.Format("delete dg_print_employeelist where pl_user='{0}'", userName), _connection);
            return reportBytes;
        }
        //DateOnly todate 2 hour
        public byte[] Dg_CreateReportFile_Ecard_Date_To_Date_2hour(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_atttendance", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("dg_Ecard_Report_date_to_date_2hour " + companyID + ",'" + userName + "'", _connection);
            string dataset = "Ecard";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Ecard_Date_To_Date_complaince.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",userName),
                new ReportParameter("FromDate",ReportTitle.StartDate),
                new ReportParameter("ToDate",ReportTitle.EndDate)
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_atttendance where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }

        //DateOnly todate 4 hour
        public byte[] Dg_CreateReportFile_Ecard_Date_To_Date_4hour(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_atttendance", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("dg_Ecard_Report_date_to_date_4hour " + companyID + ",'" + userName + "'", _connection);
            string dataset = "Ecard";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Ecard_Date_To_Date_bayer.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",userName),
                new ReportParameter("FromDate",ReportTitle.StartDate),
                new ReportParameter("ToDate",ReportTitle.EndDate)
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_atttendance where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Report_In_Activeemp_Date_wise_Excel(string reportType, int companyID, string userName)
        {
            var data = _dgCommon.get_InformationDataTable("dg_rpt_excel_Audit_In_active_emp_info_dateTOdate " + companyID + ",'" + userName + "'", _connection);
            string dataset = "Audit_In_active";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Dg_Pay_Rep_In_Active_DatewiseEmp_Excel.rdlc";
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType);
            _dgCommon.saveChanges("delete dg_print_employeelist where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Report_EmployeeInternalTransfer_info_D2D(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_EmployeeInternalTransfer_info " + companyID + ",'" + userName + "'", _connection);
            string dataset = "InternalTransfer_info";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Dg_Pay_Rep_EmployeeInternalTransfer_info.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",userName),
                new ReportParameter("Title",string.Concat("Employee Internal Transfer history - ",ReportTitle.StartDate," To ",ReportTitle.EndDate))
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Export_Report_Employee_info_bangla(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_emp_info_bangla " + companyID + ",'" + userName + "'", _connection);
            string dataset = "DataSet1";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\dg_emp_info_bangla.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("Employee Bangla Information - ",ReportTitle.DepartmentName,ReportTitle.SectionName,ReportTitle.BuildingName,ReportTitle.LineName))
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }

        //Audit
        public byte[] Report_preriodical_present_absent_leave_Weekly_holiday_special_holiday(string reportType, int companyID, string userName)
        {
            var data = _dgCommon.get_InformationDataTable("dg_rpt_excel_Audit_Attendance_Report_preriodical_present_absent_leave_Weekly_holiday_special_holiday "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "Audit_AttendanceExcel";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Dg_Pay_Rep_AttendanceAudit_Excel.rdlc";
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType);
            _dgCommon.saveChanges("delete dg_print_employeelist where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Report_Salary_info_Audit_Excel(string reportType, int companyID, string userName)
        {
            var data = _dgCommon.get_InformationDataTable("dg_rpt_excel_Audit_Salary_info "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "Audit_salary_info";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Dg_Pay_Rep_Salary_info_Audit_Excel.rdlc";
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType);
            _dgCommon.saveChanges("delete dg_print_employeelist where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Report_Joindate_wise_info_Audit_Excel(string reportType, int companyID, string userName)
        {
            var data = _dgCommon.get_InformationDataTable("dg_rpt_excel_Audit_joindate_wise_emp_info "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "Audit_Joindate_wise_Empinfo";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Dg_Pay_Rep_joindate_wise_Audit_Excel.rdlc";
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType);
            _dgCommon.saveChanges("delete dg_print_employeelist where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Report_manual_attendsance_Audit_Excel(string reportType, int companyID, string userName)
        {
            var data = _dgCommon.get_InformationDataTable("dg_rpt_excel_Audit_manual_attendance "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "Audit_manualAttendance_wise_Empinfo";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Dg_Pay_Rep_manual_attendance_Audit_Excel.rdlc";
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType);
            _dgCommon.saveChanges("delete dg_print_employeelist where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Report_TiffinBill_Audit_Excel(string reportType, int companyID, string userName)
        {
            var data = _dgCommon.get_InformationDataTable("dg_rpt_excel_Audit_Tiffin_bill_Night_bill_Periodical "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "Audit_TiffinBill_Empinfo";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Dg_Pay_Rep_Tiffin_Bill_Audit_Excel.rdlc";
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType);
            _dgCommon.saveChanges("delete dg_print_employeelist where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Report_Night_Audit_Excel(string reportType, int companyID, string userName)
        {
            var data = _dgCommon.get_InformationDataTable("dg_rpt_excel_Audit_Tiffin_bill_Night_bill_Periodical "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "Audit_TiffinBill_Empinfo";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Dg_Pay_Rep_Night_Bill_Audit_Excel.rdlc";
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType);
            _dgCommon.saveChanges("delete dg_print_employeelist where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Report_Leave_transction_Audit_Excel(string reportType, int companyID, string userName)
        {
            var data = _dgCommon.get_InformationDataTable("dg_rpt_excel_Audit_leave_transection "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "Audit_LeaveTransction";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Dg_Pay_Rep_LeaveTransaction_Audit_Excel.rdlc";
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType);
            _dgCommon.saveChanges("delete dg_print_employeelist where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Report_In_Activeemp_Date_wise_Audit_Excel(string reportType, int companyID, string userName)
        {
            var data = _dgCommon.get_InformationDataTable("dg_rpt_excel_Audit_In_active_emp_info_dateTOdate "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "Audit_In_active";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Dg_Pay_Rep_In_Active_DatewiseEmp_Audit_Excel.rdlc";
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType);
            _dgCommon.saveChanges("delete dg_print_employeelist where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Date_wise_total_ot_details_Audit_Excel(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("dg_rpt_excel_Audit_date_to_date_OT_Exot_TotalOT_with_value " + companyID + ",'" + userName + "'", _connection);
            string dataset = "OT_Exot_TotalOT_Audit";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Att_Total_OT_details_Audit_Excel.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("OT Details Excel")),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Report_Employee_envelope(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("dg_rpt_emp_envelope " + companyID + ",'" + userName + "'", _connection);
            string dataset = "DataSet1";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\dg_emp_envelope.rdlc";
            string groupLogo = new Uri($"{_webHostEnvironment.WebRootPath}\\AuthSign\\GroupLogo.png").AbsoluteUri;
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("GroupLogo",groupLogo),
                new ReportParameter("Title",string.Concat("Employee Active List - ",ReportTitle.DepartmentName,ReportTitle.SectionName,ReportTitle.BuildingName,ReportTitle.LineName))
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Report_TiffinBill_Audit_date_to_date_Excel(string reportType, int companyID, string userName)
        {
            _dgCommon.saveChanges(string.Format("Dg_Pay_tiffinbill_nightbill_date_to_date_process {0},'{1}'", companyID, userName), _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_tiffin_bill_night_bill_date_to_date " + companyID + ",'" + userName + "'", _connection);
            string dataset = "Audit_TiffinBill_Empinfo";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Dg_Pay_Rep_Tiffin_Bill_Audit_date_to_date_Excel.rdlc";
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType);
            _dgCommon.saveChanges("delete dg_print_employeelist where pl_user='" + userName + "'", _connection);
            _dgCommon.saveChanges("delete dg_pay_tiffinbill_nightbill_date_to_date where tn_compid=" + companyID + " and tn_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Report_manual_attendsance_Audit_in_out_Excel(string reportType, int companyID, string userName)
        {
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_EmpMenualAtt_List_in_out " + companyID + ",'" + userName + "'", _connection);
            string dataset = "Audit_manualAttendance_wise_Empinfo";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Dg_Pay_Rep_manual_attendance_Audit_in_out_Excel.rdlc";
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType);
            _dgCommon.saveChanges("delete dg_print_employeelist where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }

        #endregion

        #region"Leave"
        public byte[] Dg_LeaveBalances(string reportType, int companyID, string userName) //ok
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_leave", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Rep_Lev_Balances "+ companyID + ",'"+ userName + "'", _connection);
            var year = Convert.ToDateTime(ReportTitle.StartDate).Year;
            string dataset = "DataSet1";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Cd_LeaveBalances.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("Leave Balances - ",year," ",ReportTitle.DepartmentName,ReportTitle.SectionName,ReportTitle.BuildingName,ReportTitle.LineName)),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("DELETE dg_print_employeelist_leave WHERE pl_user='"+ userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Dg_LeaveTransactions(string reportType, int companyID, string userName) //ok
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_leave", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Rep_Lev_Trans "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "LeaveTrans";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Dg_LeaveTransactions.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("Leave Transactions - ",ReportTitle.StartDate," To ",ReportTitle.EndDate," ",ReportTitle.DepartmentName,ReportTitle.SectionName,ReportTitle.BuildingName,ReportTitle.LineName)),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("DELETE dg_print_employeelist_leave WHERE pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Dg_IndividualLeaveStatement(string reportType, int companyID, string userName) //ok
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_leave", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Rep_Lev_Balances "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "LeaveBal";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Cd_IndividualLeaveStatement.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("DELETE dg_print_employeelist_leave WHERE pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Dg_MaternityLeaveList(string reportType, int companyID, string userName) //ok
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_leave", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Rep_maternity_leave "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "rpt_maternity_leave";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Dg_MaternityLeaveList.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("Maternity Leave Details- From Date:",ReportTitle.StartDate," End Date:",ReportTitle.EndDate)),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("DELETE dg_print_employeelist_leave WHERE pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Dg_MaternityLeavePayment(string reportType, int companyID, string userName) //ok
        {
            var data = _dgCommon.get_InformationDataTable("Dg_Rep_maternityLeave_payment "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "maternityLeave_payment";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Dg_MaternityLeavePayment.rdlc";
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType);
            _dgCommon.saveChanges("DELETE dg_print_employeelist_leave WHERE pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Dg_LeaveForm(string reportType, int companyID, string userName) //ok
        {
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_Leave_from "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "leaveForm";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Cd_LeaveForm.rdlc";
            string groupLogoImgPath = new Uri($"{_webHostEnvironment.WebRootPath}\\AuthSign\\comp_logo.png").AbsoluteUri;
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("groupLogo",groupLogoImgPath)
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("DELETE dg_print_employeelist_leave WHERE pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Dg_LeaveFormEmpWise(int empSerial, int leaveID, string reportType) //ok
        {
            var data = _dgCommon.get_InformationDataTable(string.Format("Dg_Pay_Rep_Leave_from_empWise {0},{1}", empSerial, leaveID), _connection);
            if (data.Rows.Count > 0)
            {
                int applicantCompid = int.Parse(data.Rows[0]["compid"].ToString());
                int applicantEmpid = int.Parse(data.Rows[0]["emp_no"].ToString());
                string applicantSignPath = $"{_webHostEnvironment.WebRootPath}\\EmployeeSignature\\{applicantCompid}\\{applicantEmpid}.png";
                data.Rows[0]["applicantEmp_imgPath"] = File.Exists(applicantSignPath) ? new Uri(applicantSignPath).AbsoluteUri : string.Empty;

                int transCompid = !string.IsNullOrEmpty(data.Rows[0]["transEmp_compid"].ToString()) ? int.Parse(data.Rows[0]["transEmp_compid"].ToString()) : 0;
                int transEmpid = !string.IsNullOrEmpty(data.Rows[0]["transEmp_no"].ToString()) ? int.Parse(data.Rows[0]["transEmp_no"].ToString()) : 0;
                string transSignPath = $"{_webHostEnvironment.WebRootPath}\\EmployeeSignature\\{transCompid}\\{transEmpid}.png";
                data.Rows[0]["transEmp_imgPath"] = File.Exists(transSignPath) ? new Uri(transSignPath).AbsoluteUri : string.Empty;

                int recomCompid = !string.IsNullOrEmpty(data.Rows[0]["recomEmp_compid"].ToString()) ? int.Parse(data.Rows[0]["recomEmp_compid"].ToString()) : 0;
                int recomEmpid = !string.IsNullOrEmpty(data.Rows[0]["recomEmp_empno"].ToString()) ? int.Parse(data.Rows[0]["recomEmp_empno"].ToString()) : 0;
                string recomSignPath = $"{_webHostEnvironment.WebRootPath}\\EmployeeSignature\\{recomCompid}\\{recomEmpid}.png";
                data.Rows[0]["recomEmp_imgPath"] = File.Exists(recomSignPath) ? new Uri(recomSignPath).AbsoluteUri : string.Empty;

                int deptCompid = !string.IsNullOrEmpty(data.Rows[0]["deptEmp_compid"].ToString()) ? int.Parse(data.Rows[0]["deptEmp_compid"].ToString()) : 0;
                int deptEmpid = !string.IsNullOrEmpty(data.Rows[0]["deptEmp_empno"].ToString()) ? int.Parse(data.Rows[0]["deptEmp_empno"].ToString()) : 0;
                string deptSignPath = $"{_webHostEnvironment.WebRootPath}\\EmployeeSignature\\{deptCompid}\\{deptEmpid}.png";
                data.Rows[0]["deptEmp_imgPath"] = File.Exists(deptSignPath) ? new Uri(deptSignPath).AbsoluteUri : string.Empty;

                int hrCompid = !string.IsNullOrEmpty(data.Rows[0]["hrEmp_compid"].ToString()) ? int.Parse(data.Rows[0]["hrEmp_compid"].ToString()) : 0;
                int hrEmpid = !string.IsNullOrEmpty(data.Rows[0]["hrEmp_empno"].ToString()) ? int.Parse(data.Rows[0]["hrEmp_empno"].ToString()) : 0;
                string hrSignPath = $"{_webHostEnvironment.WebRootPath}\\EmployeeSignature\\{hrCompid}\\{hrEmpid}.png";
                data.Rows[0]["hrEmp_imgPath"] = File.Exists(hrSignPath) ? new Uri(hrSignPath).AbsoluteUri : string.Empty;

                int appCompid = !string.IsNullOrEmpty(data.Rows[0]["appEmp_compid"].ToString()) ? int.Parse(data.Rows[0]["appEmp_compid"].ToString()) : 0;
                int appEmpid = !string.IsNullOrEmpty(data.Rows[0]["appEmp_empno"].ToString()) ? int.Parse(data.Rows[0]["appEmp_empno"].ToString()) : 0;
                string appSignPath = $"{_webHostEnvironment.WebRootPath}\\EmployeeSignature\\{appCompid}\\{appEmpid}.png";
                data.Rows[0]["appEmp_imgPath"] = File.Exists(appSignPath) ? new Uri(appSignPath).AbsoluteUri : string.Empty;
            }
            
            string dataset = "leaveForm";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Dg_LeaveFormEmpWise.rdlc";
            string groupLogoImgPath = new Uri($"{_webHostEnvironment.WebRootPath}\\AuthSign\\GroupLogo.png").AbsoluteUri;
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("groupLogo",groupLogoImgPath)
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            return reportBytes;
        }
        public byte[] Dg_IndividualLeave_Register(string reportType, int companyID, string userName) //ok
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_leave", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Rep_Lev_Register " + companyID + ",'" + userName + "'", _connection);
            string dataset = "Register";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Dg_IndividualLeave_Register.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
            };
            SubreportProcessingEventHandler sReportHandler = (sender, e) =>
            {
                int emp_serial = int.Parse(e.Parameters["empSerial"].Values[0].ToString());
                var data = _dgCommon.get_InformationDataTable(string.Format("Dg_Rep_Lev_Register_leavedate {0}", emp_serial), _connection);
                e.DataSources.Add(new ReportDataSource("register_date", data));
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters, sReportHandler);
            _dgCommon.saveChanges("DELETE dg_print_employeelist_leave WHERE pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        #endregion

        #region"Attendance"
        public byte[] CreateReportFile_Attendance_Present(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_atttendance", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_Att_InOut "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "DataSet1";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Att_inout.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("Attendance Present - ",ReportTitle.DepartmentName,ReportTitle.SectionName,ReportTitle.BuildingName,ReportTitle.LineName)),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_atttendance where pl_user='"+ userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Dg_Att_PresentWithImages(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_atttendance",companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("dg_pay_empPresentWithImg "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "Attinout";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Dg_Att_PresentWithImages.rdlc";
            string imgPath = new Uri($"{_webHostEnvironment.WebRootPath}\\EmployeeImage\\").AbsoluteUri;
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("EmpImagePath",imgPath),
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat(ReportTitle.DepartmentName,ReportTitle.SectionName,ReportTitle.BuildingName,ReportTitle.LineName," - From - ",ReportTitle.StartDate," To ",ReportTitle.EndDate)),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_atttendance where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] CreateReportFile_Attendance_Absent(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_atttendance", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_Att_Absent "+ companyID + ",'"+ userName + "'", _connection);
            data.Columns.Add("lastPresent", typeof(string));
            data.AsEnumerable().ToList().ForEach(item =>
            {
                var dtDate = _dgCommon.get_InformationDataTable("select max(at_date) as at_date from dg_pay_attendance where at_emp_serial=" + int.Parse(item["at_emp_serial"].ToString()) + " and (RTRIM(dg_Pay_Attendance.at_status_code)='' or RTRIM(dg_Pay_Attendance.at_status_code)='LA') and at_holiday='' and at_date between '" + Convert.ToDateTime(ReportTitle.StartDate).ToString("MM-dd-yyyy") + "' and '" + Convert.ToDateTime(ReportTitle.EndDate).ToString("MM-dd-yyyy") + "'", _connection);
                item["lastPresent"] = !string.IsNullOrEmpty(dtDate.Rows[0]["at_date"].ToString()) ? Convert.ToDateTime(dtDate.Rows[0]["at_date"]).ToString("dd-MMM-yyyy") : string.Empty;
            });
            string dataset = "DataSet1";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Attendance_Absent.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("Attendance Absent - ",ReportTitle.DepartmentName,ReportTitle.SectionName,ReportTitle.BuildingName,ReportTitle.LineName)),
                new ReportParameter("Title2",string.Concat("Form - ",Convert.ToDateTime(ReportTitle.StartDate).ToString("dd-MMM-yyyy")," To - ",Convert.ToDateTime(ReportTitle.EndDate).ToString("dd-MMM-yyyy")))
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_atttendance where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Dg_Att_AbsentWithImages(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_atttendance", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("dg_pay_empAbsentWithImg " + companyID + ",'"+ userName + "'", _connection);
            string dataset = "Attinout";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Dg_Att_AbsentWithImages.rdlc";
            string imgPath = new Uri($"{_webHostEnvironment.WebRootPath}\\EmployeeImage\\").AbsoluteUri;
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("EmpImagePath",imgPath),
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat(ReportTitle.DepartmentName,ReportTitle.SectionName,ReportTitle.BuildingName,ReportTitle.LineName," - From - ",ReportTitle.StartDate," To ",ReportTitle.EndDate)),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_atttendance where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] CreateReportFile_Attendance_Late(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_atttendance", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_Att_Late "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "DataSet1";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Attendance_Late.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("Attendance Late From-",ReportTitle.StartDate," To-",ReportTitle.EndDate))
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_atttendance where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] CreateReportFile_Attendance_Innotpunch(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_atttendance", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_Att_InTimeNotPunch "+ companyID + ",'"+ userName + "'", _connection); //Dg_Pay_Rep_Att_NoInPunch
            string dataset = "DataSet1";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Attendance_In_Notpunch.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_atttendance where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] CreateReportFile_Attendance_Outnotpunch(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_atttendance", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_Att_OutTimeNotPunch "+ companyID + ",'"+ userName + "'", _connection); //Dg_Pay_Rep_Att_NoOutPunch
            string dataset = "DataSet1";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Attendance_Out_Notpunch.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_atttendance where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] CreateReportFile_Attendance_InOutnotpunch(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_atttendance", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_Att_InOutNotPunch "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "DataSet1";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Attendance_InOut_Notpunch.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_atttendance where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Dg_Att_SectionWiseSummary(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_atttendance", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Rep_Att_SecSummary "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "AttSecSum";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Cd_Att_SectionWiseSummary.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("Section wise Attendance Summary - ",ReportTitle.StartDate," - ",ReportTitle.DepartmentName,ReportTitle.SectionName,ReportTitle.BuildingName,ReportTitle.LineName)),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_atttendance where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Dg_CreateReportFile_Ecard_u1(string reportType, int companyID, string userName)
        {
            var data = _dgCommon.get_InformationDataTable("dg_Ecard_Report "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "Ecard";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Ecard_u1.rdlc";
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType);
            _dgCommon.saveChanges("delete dg_print_employeelist_atttendance where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        //For Test
        public async Task<byte[]> Dg_CreateReportFile_Ecard_u1_test(PostReportViewPayload obj)
        {            
            var data = await _dgCommon.get_InformationDataTableAsync("dg_Ecard_Report_v1", _connection, obj);
            string dataset = "Ecard";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Ecard_u1.rdlc";
            return _dgCommon.GenerateReport(data, dataset, path, obj.reportType);           
        }


        public byte[] Dg_CreateReportFile_Ecard_Date_To_Date(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_atttendance", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("dg_Ecard_Report_date_to_date "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "Ecard";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Ecard_Date_To_Date.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",userName),
                new ReportParameter("fromDate",ReportTitle.StartDate),
                new ReportParameter("ToDate",ReportTitle.EndDate)
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType,reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_atttendance where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Dg_CreateReportFile_Ecard_complaince_2hour(string reportType, int companyID, string userName)
        {
            var data = _dgCommon.get_InformationDataTable("dg_Ecard_Report_complaince "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "Ecard";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Ecard_c.rdlc";
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType);
            _dgCommon.saveChanges("delete dg_print_employeelist_atttendance where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Dg_CreateReportFile_Ecard_bayer_4hour(string reportType, int companyID, string userName)
        {
            var data = _dgCommon.get_InformationDataTable("dg_Ecard_Report_bayer "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "Ecard";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Ecard_b.rdlc";
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType);
            _dgCommon.saveChanges("delete dg_print_employeelist_atttendance where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Dg_Att_MonthlyAttendance(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_atttendance", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_Att_AttMonthly "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "AttMonthly";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Cd_Att_MonthlyAttendance.rdlc";
            int num = 1;
            do
            {
                if (!data.Columns.Contains(string.Format("D{0}", num)))
                {
                    data.Columns.Add(string.Format("D{0}", num), typeof(string));
                }
                num = checked(num + 1);
            }
            while (num <= 31);
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("TotDays",Convert.ToString(GetMonthDays(Convert.ToDateTime(ReportTitle.StartDate)))),
                new ReportParameter("Title",string.Concat("Monthly Attendance Summary - ",ReportTitle.StartDate," - ",ReportTitle.DepartmentName,ReportTitle.SectionName,ReportTitle.BuildingName,ReportTitle.LineName)),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_atttendance where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Dg_Att_MonthlyAttendance_Absent(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_atttendance", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_Att_AttMonthly_Absent " + companyID + ",'" + userName + "'", _connection);
            string dataset = "AttMonthly";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Dg_Att_MonthlyAttendance_Absent.rdlc";
            int num = 1;
            do
            {
                if (!data.Columns.Contains(string.Format("D{0}", num)))
                {
                    data.Columns.Add(string.Format("D{0}", num), typeof(string));
                }
                num = checked(num + 1);
            }
            while (num <= 31);
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("TotDays",Convert.ToString(GetMonthDays(Convert.ToDateTime(ReportTitle.StartDate)))),
                new ReportParameter("Title",string.Concat("Monthly Attendance Absent Summary - ",ReportTitle.StartDate," - ",ReportTitle.DepartmentName,ReportTitle.SectionName,ReportTitle.BuildingName,ReportTitle.LineName)),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_atttendance where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] EmployeeMenualAttnList(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_atttendance", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_EmpMenualAtt_List "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "MenualAttnList";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Dg_Pay_Rep_EmpMenualAtt_List.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("Employee Menual Attendance From ",ReportTitle.StartDate," To ",ReportTitle.EndDate)),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_atttendance where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Date_wise_total_ot_details(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_atttendance", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_Att_OT_Exot_TotalOT_Details "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "OT_Exot_TotalOT";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Att_Total_OT_details.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("OT Details Excel")),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_atttendance where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Floor_wise_manpower_comparison(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_atttendance", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_Att_floor_wise_manpower_comparison " + companyID + ",'" + userName + "'", _connection);
            string dataset = "manpower_comparison";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Floor_wise_manpower_comparison.rdlc";
            string fdate = ReportTitle.StartDate;
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("fromdate",ReportTitle.StartDate),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_atttendance where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] EmployeeAutoShiftHistory(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_atttendance", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_EmployeeAutoShiftInfo " + companyID + ",'" + userName + "'", _connection);
            string dataset = "autoShiftH";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\EmployeeAutoShiftInfo.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("Auto Shift History From-",ReportTitle.StartDate," To-",ReportTitle.EndDate))
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_atttendance where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }       
        public byte[] Report_preriodical_present_absent_leave_Weekly_holiday_special_holidays(string reportType, int companyID, string userName)
        {
            var data = _dgCommon.get_InformationDataTable("dg_rpt_excel_Audit_Attendance_Report_preriodical_present_absent_leave_Weekly_holiday_special_holidays " + companyID + ",'" + userName + "'", _connection);
            string dataset = "Audit_AttendanceExcel";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Dg_Pay_Rep_AttendanceAudit_Excell.rdlc";
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType);
            _dgCommon.saveChanges("delete dg_print_employeelist_atttendance where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Dg_Att_hourly_manpower(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_atttendance", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("dg_pay_rep_hour_wise_manpower " + companyID + ",'" + userName + "'", _connection);
            string dataset = "OT_Exot_TotalOT";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Att_hourly_man_power.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("Section wise Attendance Summary - ",ReportTitle.StartDate," - ",ReportTitle.DepartmentName,ReportTitle.SectionName,ReportTitle.BuildingName,ReportTitle.LineName)),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_atttendance where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Dg_Att_hourly_manpower_d2d(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_atttendance", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("dg_pay_rep_hour_wise_manpower_d2d " + companyID + ",'" + userName + "'", _connection);
            string dataset = "OT_Exot_TotalOT";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Att_hourly_man_power_d2d.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("Section wise Attendance Summary - ",ReportTitle.StartDate," - ",ReportTitle.DepartmentName,ReportTitle.SectionName,ReportTitle.BuildingName,ReportTitle.LineName)),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_atttendance where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Dg_Att_hourly_manpower_otandvalue(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_atttendance", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("dg_pay_rep_hour_wise_manpower_with_value_f " + companyID + ",'" + userName + "'", _connection);
            string dataset = "OT_Exot_TotalOT";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Att_hourly_man_power_with_ot_values.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("Section wise Attendance Summary - ",ReportTitle.StartDate," - ",ReportTitle.DepartmentName,ReportTitle.SectionName,ReportTitle.BuildingName,ReportTitle.LineName)),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_atttendance where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Dg_Att_single_hourwise_manpower_otandvalue(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_atttendance", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("dg_pay_rep_single_hour_wise_manpower_with_value_ff " + companyID + ",'" + userName + "'", _connection);
            string dataset = "OT_Exot_TotalOT";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Att_single_hourwise_man_power_with_ot_values.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("Section wise Attendance Summary - ",ReportTitle.StartDate," - ",ReportTitle.DepartmentName,ReportTitle.SectionName,ReportTitle.BuildingName,ReportTitle.LineName)),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_atttendance where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] CreateReportFile_Attendance_LunchOut(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_atttendance", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_Att_LunchOut " + companyID + ",'" + userName + "'", _connection);
            string dataset = "DataSet1";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Att_LunchOut.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("Attendance Present - ",ReportTitle.DepartmentName,ReportTitle.SectionName,ReportTitle.BuildingName,ReportTitle.LineName)),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_atttendance where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] CreateReportFile_Weekly_Warning(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_atttendance", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_Att_weekly_warning " + companyID + ",'" + userName + "'", _connection); //Dg_Pay_Rep_Att_NoInPunch
            string dataset = "DataSet1";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Weekly_warning.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_atttendance where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] CreateReportFile_Continuous_Absenteeism(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_atttendance", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_Att_Continuous_Absenteeism " + companyID + ",'" + userName + "'", _connection);
            string dataset = "Continuous_Absenteeism";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Continuous_Absenteeism.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("Continuous Absenteeism - ",ReportTitle.DepartmentName,ReportTitle.SectionName,ReportTitle.BuildingName,ReportTitle.LineName)),
                new ReportParameter("Title2",string.Concat("Form - ",Convert.ToDateTime(ReportTitle.StartDate).ToString("dd-MMM-yyyy")," To - ",Convert.ToDateTime(ReportTitle.EndDate).ToString("dd-MMM-yyyy")))
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_atttendance where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        #endregion

        #region"Salary"
        public byte[] CreateReportFile_salarysheet_D(string reportType, int companyID, string userName)
        {
            var data = _dgCommon.get_InformationDataTable("dg_Pay_Rep_Sal_SalarySheet_D "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "DataSet1";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Sal_Salarysheet.rdlc";
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Sal_salarysheet_Details(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_salary", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("dg_Pay_Rep_Sal_SalarySheet_D "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "DataSet1";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Sal_Salarysheet_Details.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("For The Month Of - ",Convert.ToDateTime(ReportTitle.StartDate).ToString("MMMM-yyyy"))),
                new ReportParameter("TotDays",Convert.ToString(GetMonthDays(Convert.ToDateTime(ReportTitle.StartDate)))),                
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Sal_salarysheet_Details_Excel(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_salary", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("dg_Pay_Rep_Sal_SalarySheet_D "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "SalDetailsExcel";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Sal_Salarysheet_DetailsExcel.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("For The Month Of - ",Convert.ToDateTime(ReportTitle.StartDate).ToString("MMMM-yyyy"))),
                new ReportParameter("TotDays",Convert.ToString(GetMonthDays(Convert.ToDateTime(ReportTitle.StartDate)))),
                //new ReportParameter("TotWorkDays",Convert.ToString(GetTotWorkingDays(Convert.ToDateTime(ReportTitle.StartDate),ReportTitle.Compid))),
                //new ReportParameter("TotWLH",Convert.ToString(GetTotWeeklyHolidays(Convert.ToDateTime(ReportTitle.StartDate),ReportTitle.Compid))),
                //new ReportParameter("TotSPH",Convert.ToString(GetTotSpecialHolidays(Convert.ToDateTime(ReportTitle.StartDate),ReportTitle.Compid)))
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Sal_salarysheet_DetailsReport(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_salary", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("dg_Pay_Rep_Sal_SalarySheet_D "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = reportType == "excel" ? "SalDetailsExcel" : "DataSet1";
            string path = reportType=="excel" ? $"{_webHostEnvironment.WebRootPath}\\Report\\Sal_Salarysheet_DetailsReportExcel.rdlc" : $"{_webHostEnvironment.WebRootPath}\\Report\\Sal_Salarysheet_Details_Report.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("For The Month Of - ",Convert.ToDateTime(ReportTitle.StartDate).ToString("MMMM-yyyy"))),
                new ReportParameter("TotDays",Convert.ToString(GetMonthDays(Convert.ToDateTime(ReportTitle.StartDate)))),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Sal_salarysheet_ReportDetails(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_salary", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("dg_Pay_Rep_Sal_SalarySheet_D "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = reportType == "excel" ? "SalDetailsExcel" : "DataSet1";
            string path = reportType == "excel" ? $"{_webHostEnvironment.WebRootPath}\\Report\\Sal_Salarysheet_ReportDetailsExcel.rdlc" : $"{_webHostEnvironment.WebRootPath}\\Report\\Sal_Salarysheet_Report_Details.rdlc";
            //string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Sal_Salarysheet_Report_Details.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("For The Month Of - ",Convert.ToDateTime(ReportTitle.StartDate).ToString("MMMM-yyyy"))),
                new ReportParameter("TotDays",Convert.ToString(GetMonthDays(Convert.ToDateTime(ReportTitle.StartDate)))),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] SalaryShert_LineWise_Summary(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_salary", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("dg_Pay_Rep_Sal_SalarySheet_summary "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "DataSet1";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Sal_Salarysheet_LineWise_Summary.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("For The Month Of - ",Convert.ToDateTime(ReportTitle.StartDate).ToString("MMMM-yyyy"))),
                new ReportParameter("TotDays",Convert.ToString(GetMonthDays(Convert.ToDateTime(ReportTitle.StartDate)))),
                new ReportParameter("TotWorkDays",Convert.ToString(GetTotWorkingDays(Convert.ToDateTime(ReportTitle.StartDate),ReportTitle.Compid))),
                new ReportParameter("TotWLH",Convert.ToString(GetTotWeeklyHolidays(Convert.ToDateTime(ReportTitle.StartDate),ReportTitle.Compid))),
                new ReportParameter("TotSPH",Convert.ToString(GetTotSpecialHolidays(Convert.ToDateTime(ReportTitle.StartDate),ReportTitle.Compid)))
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] CreateReportFile_salarysheet_D_53(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_salary", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("dg_Pay_Rep_Sal_SalarySheet_D "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "DataSet1";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Sal_Salarysheet_53.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("Monthly Salary Sheet Month Of - ",Convert.ToDateTime(ReportTitle.StartDate).ToString("MMMM-yyyy")," - ",ReportTitle.DepartmentName,ReportTitle.SectionName,ReportTitle.BuildingName,ReportTitle.LineName)),
                new ReportParameter("TotDays",Convert.ToString(GetMonthDays(Convert.ToDateTime(ReportTitle.StartDate)))),
                new ReportParameter("TotWorkDays",Convert.ToString(GetTotWorkingDays(Convert.ToDateTime(ReportTitle.StartDate),ReportTitle.Compid))),
                new ReportParameter("TotWLH",Convert.ToString(GetTotWeeklyHolidays(Convert.ToDateTime(ReportTitle.StartDate),ReportTitle.Compid))),
                new ReportParameter("TotSPH",Convert.ToString(GetTotSpecialHolidays(Convert.ToDateTime(ReportTitle.StartDate),ReportTitle.Compid)))
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] CreateReportFile_salarysheet_D_53_2(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_salary", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("dg_Pay_Rep_Sal_SalarySheet_D "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "DataSet1";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Sal_Salarysheet_53_2.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("Monthly Salary Sheet Month Of - ",Convert.ToDateTime(ReportTitle.StartDate).ToString("MMMM-yyyy")," - ",ReportTitle.DepartmentName,ReportTitle.SectionName,ReportTitle.BuildingName,ReportTitle.LineName)),
                new ReportParameter("TotDays",Convert.ToString(GetMonthDays(Convert.ToDateTime(ReportTitle.StartDate)))),
                new ReportParameter("TotWorkDays",Convert.ToString(GetTotWorkingDays(Convert.ToDateTime(ReportTitle.StartDate),ReportTitle.Compid))),
                new ReportParameter("TotWLH",Convert.ToString(GetTotWeeklyHolidays(Convert.ToDateTime(ReportTitle.StartDate),ReportTitle.Compid))),
                new ReportParameter("TotSPH",Convert.ToString(GetTotSpecialHolidays(Convert.ToDateTime(ReportTitle.StartDate),ReportTitle.Compid)))
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] CreateReportFile_salarysheet_D_53_4(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_salary", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("dg_Pay_Rep_Sal_SalarySheet_D "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "DataSet1";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Sal_Salarysheet_53_4.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("Monthly Salary Sheet Month Of - ",Convert.ToDateTime(ReportTitle.StartDate).ToString("MMMM-yyyy")," - ",ReportTitle.DepartmentName,ReportTitle.SectionName,ReportTitle.BuildingName,ReportTitle.LineName)),
                new ReportParameter("TotDays",Convert.ToString(GetMonthDays(Convert.ToDateTime(ReportTitle.StartDate)))),
                new ReportParameter("TotWorkDays",Convert.ToString(GetTotWorkingDays(Convert.ToDateTime(ReportTitle.StartDate),ReportTitle.Compid))),
                new ReportParameter("TotWLH",Convert.ToString(GetTotWeeklyHolidays(Convert.ToDateTime(ReportTitle.StartDate),ReportTitle.Compid))),
                new ReportParameter("TotSPH",Convert.ToString(GetTotSpecialHolidays(Convert.ToDateTime(ReportTitle.StartDate),ReportTitle.Compid)))
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] CreateReportFile_Payslip(string reportType, int companyID, string userName)
        {
            var data = _dgCommon.get_InformationDataTable("dg_Rep_Sal_PaySlip "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "PaySlip";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Sal_PaySlip_Debonir.rdlc";
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] CreateReportFile_Payslip_bangl(string reportType, int companyID, string userName)
        {
            var data = _dgCommon.get_InformationDataTable("dg_Rep_Sal_PaySlip " + companyID + ",'" + userName + "'", _connection);
            string dataset = "PaySlip";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Sal_PaySlip_Bangla.rdlc";
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] CreateReportFile_Payslip_complaince_2hour(string reportType, int companyID, string userName)
        {
            var data = _dgCommon.get_InformationDataTable("dg_Rep_Sal_PaySlip "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "PaySlip";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Sal_PaySlip_ReportDetails.rdlc";
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] CreateReportFile_Payslip_bayer_2hour_Bangla(string reportType, int companyID, string userName)
        {
            var data = _dgCommon.get_InformationDataTable("dg_Rep_Sal_PaySlip " + companyID + ",'" + userName + "'", _connection);
            string dataset = "PaySlip";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Sal_PaySlip_report_Bangla.rdlc";
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] CreateReportFile_Payslip_bayer_4hour(string reportType, int companyID, string userName)
        {
            var data = _dgCommon.get_InformationDataTable("dg_Rep_Sal_PaySlip "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "PaySlip";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Sal_PaySlip_Debonirrr.rdlc";
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] CreateReportFile_Payslip_bayer_4hour_Bangla(string reportType, int companyID, string userName)
        {
            var data = _dgCommon.get_InformationDataTable("dg_Rep_Sal_PaySlip " + companyID + ",'" + userName + "'", _connection);
            string dataset = "PaySlip";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Sal_PaySlip_Bangla_report.rdlc";
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] CreateReportFile_SalarySheetBank(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_salary", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("dg_Pay_Rep_Sal_SalarySheet_Bank "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "SalarySheet_Bank";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\SalarySheet_Bank.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] CreateReportFile_SalarySheetEXOT(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_salary", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_Att_EXTODetails "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "EXOT_Sheet";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Sal_EXOT_Sheet.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("EXOT Sheet - ",Convert.ToDateTime(ReportTitle.StartDate).ToString("MMMM-yyyy")," - ",ReportTitle.DepartmentName,ReportTitle.SectionName,ReportTitle.BuildingName,ReportTitle.LineName)),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] CreateReportFile_SalarySheetOTEXOT(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_salary", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_Sal_SalarySheet_D_EXOT "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "SalSheet";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Sal_OTEXOT_Sheet.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("Salary Sheet - ",Convert.ToDateTime(ReportTitle.StartDate).ToString("MMMM-yyyy")," - ",ReportTitle.DepartmentName,ReportTitle.SectionName,ReportTitle.BuildingName,ReportTitle.LineName)),
                new ReportParameter("TotDays",Convert.ToString(GetMonthDays(Convert.ToDateTime(ReportTitle.StartDate)))),
                new ReportParameter("TotWorkDays",Convert.ToString(GetTotWorkingDays(Convert.ToDateTime(ReportTitle.StartDate),ReportTitle.Compid))),
                new ReportParameter("TotWLH",Convert.ToString(GetTotWeeklyHolidays(Convert.ToDateTime(ReportTitle.StartDate),ReportTitle.Compid))),
                new ReportParameter("TotSPH",Convert.ToString(GetTotSpecialHolidays(Convert.ToDateTime(ReportTitle.StartDate),ReportTitle.Compid)))
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] CreateReportFile_OTDetails(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_salary", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_Att_OTDetails "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "OT";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Sal_OTDetails.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("OT Detailed - ",Convert.ToDateTime(ReportTitle.StartDate).ToString("dd-MMM-yyyy")," - ",ReportTitle.DepartmentName,ReportTitle.SectionName,ReportTitle.BuildingName,ReportTitle.LineName)),         
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] CreateReportFile_OTSummary(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_salary", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_Sal_OT "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "OT";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Sal_OTSummary.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("OT Monthly Summary -  Month of ",Convert.ToDateTime(ReportTitle.StartDate).ToString("MMMM-yyyy")," - ",ReportTitle.DepartmentName,ReportTitle.SectionName,ReportTitle.BuildingName,ReportTitle.LineName)),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] CreateReportFile_SalarySheetSummary(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_salary", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_Sal_SalarySheet_S "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "DataSet1";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\SalarySheet_Summary.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("Monthly Section Wise Salary Summary for Month of ",Convert.ToDateTime(ReportTitle.StartDate).ToString("MMMM-yyyy"))),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] CreateReportFile_SalarySheetSummaryYearly(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_salary", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_Sal_SalarySheet_S_yearly " + companyID + ",'" + userName + "'", _connection);
            string dataset = "DataSet1";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\SalarySheet_SummaryYearly.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("Monthly Year Wise Salary Summary for Year of ",Convert.ToDateTime(ReportTitle.StartDate).ToString("yyyy"))),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] SalarySheetSummarySalCategoryWise(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_salary", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_Sal_SalarySheet_S_SalCatWise " + companyID + ",'" + userName + "'", _connection);
            string dataset = "DataSet1";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\SalarySheet_Summary_SalCatWise.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("Monthly Section & Category Wise Salary Summary for Month of ",Convert.ToDateTime(ReportTitle.StartDate).ToString("MMMM-yyyy"))),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] SalarySheetSummary_Floor_SalCategoryWise(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_salary", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_Sal_SalarySheet_S_floor_SalCatWise " + companyID + ",'" + userName + "'", _connection);
            string dataset = "DataSet1";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\SalarySheet_Summary_Floor_SalCatWise.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("Monthly Floor Wise Salary Summary for Month of ",Convert.ToDateTime(ReportTitle.StartDate).ToString("MMMM-yyyy"))),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] SalarySheetSummary_FloorAndLine_SalCategoryWise(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_salary", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_Sal_SalarySheet_S_floorAndLine_SalCatWise " + companyID + ",'" + userName + "'", _connection);
            string dataset = "DataSet1";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\SalarySheet_Summary_FloorAndLine_SalCatWise.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("Monthly Floor And Line Wise Salary Summary for Month of ",Convert.ToDateTime(ReportTitle.StartDate).ToString("MMMM-yyyy"))),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] CreateReportFile_SalarySheetOtExotSummary(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_salary", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_Sal_SalarySheet_S_OT_EXOT "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "DataSet1";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\SalarySheet_OTEXOTSum_Summary.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("Section Wise OT Summary for Month of ",Convert.ToDateTime(ReportTitle.StartDate).ToString("MMMM-yyyy")," - ",ReportTitle.DepartmentName,ReportTitle.SectionName,ReportTitle.BuildingName,ReportTitle.LineName)),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Sal_SalarySheetBankExcel(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_salary", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_SalarySheetBankFor_Excel "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "SalBankExcel";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Sal_SalarySheetBank_Excel.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("Bank List for the Month of ",Convert.ToDateTime(ReportTitle.StartDate).ToString("MMMM-yyyy"))),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, "excel", reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Sal_SalarySheetAccount(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_salary", companyID, userName, _connection);
            var dataDetails = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_eidbonus_Acc_BankFor " + companyID + ",'" + userName + "'", _connection);
            var dataTopsheet = _dgCommon.get_InformationDataTable("dg_pay_bounsTopSheet " + companyID + ",'" + userName + "'", _connection);
            var dataTable = new DataTable[] { dataDetails, dataTopsheet };
            var dataset = new string[] { "SalBankExcel","topSheet" };
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Sal_SalarySheetAcc_Excel.rdlc";
            
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("Bank List for the Month of ",Convert.ToDateTime(ReportTitle.StartDate).ToString("MMMM-yyyy"))),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(dataTable, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Sal_EmployeeTiffinBillSummary(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_salary", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_EmployeeTiffinBill_Summary "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "TiffinBillSum";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Sal_TiffinBillSummary.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("Tiffin Bill Summary for Month of ",Convert.ToDateTime(ReportTitle.StartDate).ToString("MMMM-yyyy"))),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Sal_EmployeeNightBillSummary(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_salary", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_EmployeeNightBill_Summary "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "NightBillSum";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Sal_NightBillSummary.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("Night Bill Summary for Month of ",Convert.ToDateTime(ReportTitle.StartDate).ToString("MMMM-yyyy"))),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Sal_EmployeeTiffinBillAmount(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_salary", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_EmpWiseTiffinBillAmt "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "TiffinBillAmt";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Sal_EmpTiffinBillAmt.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("Employee Wise Tiffin Bill for Month of ",Convert.ToDateTime(ReportTitle.StartDate).ToString("MMMM-yyyy"))),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Sal_EmployeeNightBillAmount(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_salary", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_EmpWiseNightBillAmt "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "NightBillAmt";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Sal_EmpNightBillAmt.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("Employee Wise Night Bill for Month of ",Convert.ToDateTime(ReportTitle.StartDate).ToString("MMMM-yyyy"))),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Sal_EmployeeNightBillAmount_date_to_date(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_salary", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_EmpWiseNightBillAmt_for_41_Template_Night " + companyID + ",'" + userName + "'", _connection);
            string dataset = "NightBillAmt";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Sal_EmpNightBillAmt_date_to_date.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("Employee Wise Night Bill for Month of ",Convert.ToDateTime(ReportTitle.StartDate).ToString("MMMM-yyyy"))),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Sal_Eid_bonus(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_salary", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("dg_pay_rep_Eid_Bonus "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "Eid_Bonus";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Sal_Eid_Bonus.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser)
            };
            byte[]  reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);          
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Sal_Eid_bonus_excel(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_salary", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("dg_pay_rep_Eid_Bonus " + companyID + ",'" + userName + "'", _connection);
            string dataset = "Eid_Bonus";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Sal_Eid_Bonus_excel.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser)
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Sal_EidBonusSummaryLineWise(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_salary", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("dg_pay_rep_Eid_Bonus_line_wise_Summary "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "eidBonusSummary";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Sal_EidBonusSummary.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser)
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Sal_Eid_bonus_bank(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_salary", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("dg_pay_rep_Eid_Bonus "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "Eid_Bonus";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Sal_Eid_Bonus_bank.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser)
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Sal_EidBonusSummaryHead(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_salary", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("dg_pay_rep_Eid_Bonus_salary_category_wise "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "eidBonusSummary";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Sal_EidBonusSummaryHead.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser)
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Sal_SalaryAdvance(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_salary", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("dg_Pay_Rep_Sal_SalarySheet_Advance " + companyID + ",'" + userName + "'", _connection);
            string dataset = "DataSet1";
            string path = reportType!="excel" ? $"{_webHostEnvironment.WebRootPath}\\Report\\Sal_SalaryAdvance.rdlc" : $"{_webHostEnvironment.WebRootPath}\\Report\\Sal_SalaryAdvance_Excel.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("Advance Salary Payment Sheet For The Month Of - ",Convert.ToDateTime(ReportTitle.StartDate).ToString("MMMM-yyyy"))),
                new ReportParameter("TotDays",Convert.ToString(GetMonthDays(Convert.ToDateTime(ReportTitle.StartDate)))),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] CreateReportFile_SalarySheet_advanceBank(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_salary", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("dg_Pay_Rep_Sal_SalarySheet_Advance_Bank " + companyID + ",'" + userName + "'", _connection);
            string dataset = "SalarySheet_AdvanceBank";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\SalarySheet_AdvanceBank.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] CreateReportFile_SalarySheet_Advance_Summary(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_salary", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_Sal_SalarySheet_Advance_S " + companyID + ",'" + userName + "'", _connection);
            string dataset = "Advance_summary";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\SalarySheet_Advance_Summary.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("Salary Advance Summary for Month of ",Convert.ToDateTime(ReportTitle.StartDate).ToString("MMMM-yyyy"))),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] SalaryShert_LineWise_Advance_Summary(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_salary", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("dg_Pay_Rep_Sal_SalarySheet_Advance_summary " + companyID + ",'" + userName + "'", _connection);
            string dataset = "Line_wise_advance_summary";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Sal_Salarysheet_LineWise_Advance_Summary.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("For The Month Of - ",Convert.ToDateTime(ReportTitle.StartDate).ToString("MMMM-yyyy"))),
                new ReportParameter("TotDays",Convert.ToString(GetMonthDays(Convert.ToDateTime(ReportTitle.StartDate)))),
                new ReportParameter("TotWorkDays",Convert.ToString(GetTotWorkingDays(Convert.ToDateTime(ReportTitle.StartDate),ReportTitle.Compid))),
                new ReportParameter("TotWLH",Convert.ToString(GetTotWeeklyHolidays(Convert.ToDateTime(ReportTitle.StartDate),ReportTitle.Compid))),
                new ReportParameter("TotSPH",Convert.ToString(GetTotSpecialHolidays(Convert.ToDateTime(ReportTitle.StartDate),ReportTitle.Compid)))
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        // ot report complaince 2 hour mm
        public byte[] CreateReportFile_SalarySheetcomplaince2hour(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_salary", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_sal_complaince_2hour_4hour_ot " + companyID + ",'" + userName + "'", _connection);
            string dataset = "Sheet_ot_2hour_4hour";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Sal_ot_report.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("Report OT - ",Convert.ToDateTime(ReportTitle.StartDate).ToString("MMMM-yyyy")," - ",ReportTitle.DepartmentName,ReportTitle.SectionName,ReportTitle.BuildingName,ReportTitle.LineName)),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        // ot report complaince 4 hour mm
        public byte[] CreateReportFile_SalarySheetcomplaince4hour(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_salary", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_sal_complaince_2hour_4hour_ot " + companyID + ",'" + userName + "'", _connection);
            string dataset = "Sheet_ot_2hour_4hour";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Report_sal_ot.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("Report OT - ",Convert.ToDateTime(ReportTitle.StartDate).ToString("MMMM-yyyy")," - ",ReportTitle.DepartmentName,ReportTitle.SectionName,ReportTitle.BuildingName,ReportTitle.LineName)),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] CreateReportFile_Date_TO_Date_OTSummary(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_salary", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_Sal_Date_To_Date_OT " + companyID + ",'" + userName + "'", _connection);
            string dataset = "Date_TO_Date_OT";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Date_TO_Date_OTSummary.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("StartDate",ReportTitle.StartDate),
                new ReportParameter("EndDate",ReportTitle.EndDate),
                new ReportParameter("Title",string.Concat("OT Summary -  Month of ",Convert.ToDateTime(ReportTitle.StartDate).ToString("MMMM-yyyy")," - ",ReportTitle.DepartmentName,ReportTitle.SectionName,ReportTitle.BuildingName,ReportTitle.LineName)),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] CreateReportFile_Date_TO_Date_OT_4_hour(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_salary", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_Sal_Date_To_Date_OT_4_hour " + companyID + ",'" + userName + "'", _connection);
            string dataset = "Date_TO_Date_OT";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Report_date_to_date_ot.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("StartDate",ReportTitle.StartDate),
                new ReportParameter("EndDate",ReportTitle.EndDate),
                new ReportParameter("Title",string.Concat("OT Summary -  Month of ",Convert.ToDateTime(ReportTitle.StartDate).ToString("MMMM-yyyy")," - ",ReportTitle.DepartmentName,ReportTitle.SectionName,ReportTitle.BuildingName,ReportTitle.LineName)),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] CreateReportFile_Date_TO_Date_OTSummary_subscetion_wise(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_salary", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_Sal_Date_To_Date_OT_Sub_Section_Wise_Summary " + companyID + ",'" + userName + "'", _connection);
            string dataset = "Date_TO_Date_OT";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Date_TO_Date_OT_SubsectionWise_Summary.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("StartDate",ReportTitle.StartDate),
                new ReportParameter("EndDate",ReportTitle.EndDate),
                new ReportParameter("Title",string.Concat("OT Summary -  Month of ",Convert.ToDateTime(ReportTitle.StartDate).ToString("MMMM-yyyy")," - ",ReportTitle.DepartmentName,ReportTitle.SectionName,ReportTitle.BuildingName,ReportTitle.LineName)),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] CreateReportFile_SalarySheetSummary_2hour(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_salary", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_Sal_SalarySheet_S " + companyID + ",'" + userName + "'", _connection);
            string dataset = "DataSet1";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\SalarySheet_Summary_Report.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("Salary Summary for Month of ",Convert.ToDateTime(ReportTitle.StartDate).ToString("MMMM-yyyy"))),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] CreateReportFile_SalarySheetSummary_4hour(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_salary", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_Sal_SalarySheet_S " + companyID + ",'" + userName + "'", _connection);
            string dataset = "DataSet1";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\SalarySheet_Report_Summary.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("Salary Summary for Month of ",Convert.ToDateTime(ReportTitle.StartDate).ToString("MMMM-yyyy"))),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] SalaryShert_LineWise_Summary_2hour(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_salary", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("dg_Pay_Rep_Sal_SalarySheet_summary " + companyID + ",'" + userName + "'", _connection);
            string dataset = "DataSet1";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Sal_Salarysheet_LineWise_Summary_Report.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("For The Month Of - ",Convert.ToDateTime(ReportTitle.StartDate).ToString("MMMM-yyyy"))),
                new ReportParameter("TotDays",Convert.ToString(GetMonthDays(Convert.ToDateTime(ReportTitle.StartDate)))),
                new ReportParameter("TotWorkDays",Convert.ToString(GetTotWorkingDays(Convert.ToDateTime(ReportTitle.StartDate),ReportTitle.Compid))),
                new ReportParameter("TotWLH",Convert.ToString(GetTotWeeklyHolidays(Convert.ToDateTime(ReportTitle.StartDate),ReportTitle.Compid))),
                new ReportParameter("TotSPH",Convert.ToString(GetTotSpecialHolidays(Convert.ToDateTime(ReportTitle.StartDate),ReportTitle.Compid)))
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] SalaryShert_LineWise_Summary_4hour(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_salary", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("dg_Pay_Rep_Sal_SalarySheet_summary " + companyID + ",'" + userName + "'", _connection);
            string dataset = "DataSet1";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Sal_Salarysheet_LineWise_Report_Summary.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("For The Month Of - ",Convert.ToDateTime(ReportTitle.StartDate).ToString("MMMM-yyyy"))),
                new ReportParameter("TotDays",Convert.ToString(GetMonthDays(Convert.ToDateTime(ReportTitle.StartDate)))),
                new ReportParameter("TotWorkDays",Convert.ToString(GetTotWorkingDays(Convert.ToDateTime(ReportTitle.StartDate),ReportTitle.Compid))),
                new ReportParameter("TotWLH",Convert.ToString(GetTotWeeklyHolidays(Convert.ToDateTime(ReportTitle.StartDate),ReportTitle.Compid))),
                new ReportParameter("TotSPH",Convert.ToString(GetTotSpecialHolidays(Convert.ToDateTime(ReportTitle.StartDate),ReportTitle.Compid)))
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] CreateReportFile_Date_TO_Date_2hourorabove_2hourOT(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_salary", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_Sal_Date_To_Date_2hourORabove2hour_OT " + companyID + ",'" + userName + "'", _connection);
            string dataset = "Date_TO_Date_OT";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Date_TO_Date_2hour_or_above_2hourOTSummary.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("StartDate",ReportTitle.StartDate),
                new ReportParameter("EndDate",ReportTitle.EndDate),
                new ReportParameter("Title",string.Concat("OT Summary -  Month of ",Convert.ToDateTime(ReportTitle.StartDate).ToString("MMMM-yyyy")," - ",ReportTitle.DepartmentName,ReportTitle.SectionName,ReportTitle.BuildingName,ReportTitle.LineName)),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] CreateReportFile_SalarySheetBank_2hrs(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_salary", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("dg_Pay_Rep_Sal_SalarySheet_Bank " + companyID + ",'" + userName + "'", _connection);
            string dataset = "SalarySheet_Bank";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\SalarySheet_Bank_Report.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] CreateReportFile_SalarySheetBank_4hrs(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_salary", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("dg_Pay_Rep_Sal_SalarySheet_Bank " + companyID + ",'" + userName + "'", _connection);
            string dataset = "SalarySheet_Bank";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\SalarySheet_Report_Bank.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Sal_SalarySheetBank_Forwarding(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_salary", companyID, userName, _connection);
            var dataDetails = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_SalarySheetBankFor_Excel " + companyID + ",'" + userName + "'", _connection);
            var dataTopsheet = _dgCommon.get_InformationDataTable("dg_pay_bounsTopSheet " + companyID + ",'" + userName + "'", _connection);
            var dataTable = new DataTable[] { dataDetails, dataTopsheet };
            var dataset = new string[] { "SalBankExcel", "topSheet" };
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Sal_SalarySheet_Bank_Forwarding.rdlc";

            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("Bank List for the Month of ",Convert.ToDateTime(ReportTitle.StartDate).ToString("MMMM-yyyy"))),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(dataTable, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] CreateReportFile_Date_TO_Date_4hourorabove_OT(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_salary", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_Sal_Date_To_Date_4hour_above_OT " + companyID + ",'" + userName + "'", _connection);
            string dataset = "Date_TO_Date_OT";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Date_TO_Date_above_4hourOT.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("StartDate",ReportTitle.StartDate),
                new ReportParameter("EndDate",ReportTitle.EndDate),
                new ReportParameter("Title",string.Concat("OT-  Month of ",Convert.ToDateTime(ReportTitle.StartDate).ToString("MMMM-yyyy")," - ",ReportTitle.DepartmentName,ReportTitle.SectionName,ReportTitle.BuildingName,ReportTitle.LineName)),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] CreateReportFile_R_OT(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_salary", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_Sal_R_ot " + companyID + ",'" + userName + "'", _connection);
            string dataset = "Date_TO_Date_OT";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\R_OT.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("StartDate",ReportTitle.StartDate),
                new ReportParameter("EndDate",ReportTitle.EndDate),
                new ReportParameter("Title",string.Concat("OT-  Month of ",Convert.ToDateTime(ReportTitle.StartDate).ToString("MMMM-yyyy")," - ",ReportTitle.DepartmentName,ReportTitle.SectionName,ReportTitle.BuildingName,ReportTitle.LineName)),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] CreateReportFile_Date_TO_Date_4hourorabove_OT_linewise_sum(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_salary", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_Sal_Date_To_Date_4hour_above_OT_linewise_summary " + companyID + ",'" + userName + "'", _connection);
            string dataset = "Date_TO_Date_OT";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Date_TO_Date_above_4hourOT_linewise_sum.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("StartDate",ReportTitle.StartDate),
                new ReportParameter("EndDate",ReportTitle.EndDate),
                new ReportParameter("Title",string.Concat("OT-  Month of ",Convert.ToDateTime(ReportTitle.StartDate).ToString("MMMM-yyyy")," - ",ReportTitle.DepartmentName,ReportTitle.SectionName,ReportTitle.BuildingName,ReportTitle.LineName)),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        
        public byte[] Report_Anual_Leave_payment_rpt(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_salary", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Anual_Leave_payment_rpt " + companyID + ",'" + userName + "'", _connection);
            string dataset = "Anual_Leave_pmt";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Dg_Pay_Rep_Anual_Leave_payment.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("Annual Leave Payment for the Year of ",Convert.ToDateTime(ReportTitle.StartDate).ToString("yyyy"))),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Sal_EmployeeNightBillAmount_date_to_date_General_Shift(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_salary", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_EmpWiseNightBillAmt_for_41_Template_Night_GENARAL_SHIFT " + companyID + ",'" + userName + "'", _connection);
            string dataset = "NightBillAmt";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Sal_EmpNightBillAmt_date_to_date_General.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("Employee Wise Night Bill for Month of ",Convert.ToDateTime(ReportTitle.StartDate).ToString("MMMM-yyyy"))),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Comparative_Salary_Summary_2Month(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_salary", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_Sal_Comparative_salary_Summary_last_2month " + companyID + ",'" + userName + "'", _connection);
            string dataset = "DataSet1";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Comparative_salary_2month.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("For The Month Of - ",Convert.ToDateTime(ReportTitle.StartDate).ToString("MMMM-yyyy"))),
                new ReportParameter("TotDays",Convert.ToString(GetMonthDays(Convert.ToDateTime(ReportTitle.StartDate)))),
                new ReportParameter("TotWorkDays",Convert.ToString(GetTotWorkingDays(Convert.ToDateTime(ReportTitle.StartDate),ReportTitle.Compid))),
                new ReportParameter("TotWLH",Convert.ToString(GetTotWeeklyHolidays(Convert.ToDateTime(ReportTitle.StartDate),ReportTitle.Compid))),
                new ReportParameter("TotSPH",Convert.ToString(GetTotSpecialHolidays(Convert.ToDateTime(ReportTitle.StartDate),ReportTitle.Compid)))
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Sal_EmpNightBillAmount_date_to_date_General_Shift_Bank(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_salary", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_EmpWiseNightBillAmt_for_41_Template_Night_GENARAL_SHIFT_Bank " + companyID + ",'" + userName + "'", _connection);
            string dataset = "NightBillAmt";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Sal_EmpNightBillAmt_date_to_date_General_Bank.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("Employee Wise Night Bill for Month of ",Convert.ToDateTime(ReportTitle.StartDate).ToString("MMMM-yyyy"))),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Sal_EmployeeNightBillAmount_date_to_date_General_Shift_Summary(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_salary", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("Dg_Pay_Rep_EmpWiseNightBillAmt_for_41_Template_Night_GENARAL_SHIFT_Summary " + companyID + ",'" + userName + "'", _connection);
            string dataset = "NightBillAmt";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Sal_EmpNightBillAmt_date_to_date_General_Summary.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser),
                new ReportParameter("Title",string.Concat("Employee Wise Night Bill for Month of ",Convert.ToDateTime(ReportTitle.StartDate).ToString("MMMM-yyyy"))),
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Sal_EidBonusSummaryLineWiseExcel(string reportType, int companyID, string userName)
        {
            var ReportTitle = this.ReportTitle("dg_print_employeelist_salary", companyID, userName, _connection);
            var data = _dgCommon.get_InformationDataTable("dg_pay_rep_Eid_Bonus_line_wise_Summary_Excel " + companyID + ",'" + userName + "'", _connection);
            string dataset = "eidBonusSummary";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Sal_EidBonusSummary_Excel.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("PrintUser",ReportTitle.PrintUser)
            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        #endregion

        #region"Others"
        public byte[] CreateReportFile_salarysheet_D_Bank(string reportType, int companyID, string userName)
        {
            var data = _dgCommon.get_InformationDataTable("dg_Pay_Rep_Sal_SalarySheet_D "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "DataSet1";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\SalarySheet_Bank_DG.rdlc";
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] CreateReportFile_Ecard(string reportType, int companyID, string userName)
        {
            var data = _dgCommon.get_InformationDataTable("dg_Ecard_Report "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "Ecard";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Ecard.rdlc";
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType);
            _dgCommon.saveChanges("delete dg_print_employeelist_atttendance where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] CreateReportFile_Payslip_U1(string reportType, int companyID, string userName)
        {
            var data = _dgCommon.get_InformationDataTable("dg_Rep_Sal_PaySlip "+ companyID + ",'"+ userName + "'", _connection);
            string dataset = "PaySlip";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Sal_PaySlip_Debonir_U1.rdlc";
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] Export_Report_SalarySheet(string reportType, int companyID, string userName)
        {
            var data = _dgCommon.get_InformationDataTable("dg_Pay_Rep_Sal_SalarySheet_D "+ companyID + ",'"+ userName + "'", _connection);
            var data2 = _dgCommon.get_InformationDataTable("select com_name from dg_pay_company", _connection);
            string dataset = "DataSet1";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\Sal_Salarysheet_U1.rdlc";
            ReportParameterCollection reportParameters = new ReportParameterCollection
            {
                new ReportParameter("comp",data2.Rows[0]["com_name"].ToString()),

            };
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType, reportParameters);
            _dgCommon.saveChanges("delete dg_print_employeelist_salary where pl_user='" + userName + "'", _connection);
            return reportBytes;
        }
        public byte[] testBarcode(string reportType, int companyID, string userName)
        {
            DataTable data = new DataTable();
            DataColumn newCol = new DataColumn("BarcodeGen", typeof(byte[]));
            DataColumn newCol1 = new DataColumn("sl_no", typeof(int));
            newCol.AllowDBNull = true;
            data.Columns.Add(newCol);
            data.Columns.Add(newCol1);
            for (int i = 0; i < 1000; i++)
            {
                var row = data.NewRow();
                row["BarcodeGen"] = this.barcodeGenerator("12345");
                row["sl_no"] = i;
                data.Rows.Add(row);
            }
            string dataset = "testBarcode";
            string path = $"{_webHostEnvironment.WebRootPath}\\Report\\testBarcode.rdlc";
            byte[] reportBytes = _dgCommon.GenerateReport(data, dataset, path, reportType);
            return reportBytes;
        }
        #endregion

        //New Report Post
        public async Task<byte[]> PostPayReportView(PostReportViewPayload obj)
        {
            var result = new byte[] {};
            switch (obj.reportID)
            {
                case 318:
                    result = await Dg_CreateReportFile_Ecard_u1_test(obj);
                    break;
                default:
                    throw new ArgumentException("Report ID Not Configure !!");
            }
            return result;
        }

        //For Report Title Details
        private Dg_ReportTitle ReportTitle(string tableName, int companyID, string userName, SqlConnection sqlCon)
        {
            Dg_ReportTitle titleList;
            DataTable dt = _dgCommon.get_InformationDataTable("select * from " + tableName + " where pl_compid="+ companyID + " and pl_user='"+ userName + "'", sqlCon);
            if (dt.Rows.Count > 0)
            {
                titleList = new Dg_ReportTitle()
                {
                    Compid = Convert.ToInt32(dt.Rows[0]["pl_compid"]),
                    DepartmentName = !string.IsNullOrEmpty(dt.Rows[0]["pl_department_name"].ToString()) ? "Department : " + dt.Rows[0]["pl_department_name"].ToString() + "" : "ALL",
                    SectionName = !string.IsNullOrEmpty(dt.Rows[0]["pl_section_name"].ToString()) ? ", Section : " + dt.Rows[0]["pl_section_name"].ToString() + "" : string.Empty,
                    BuildingName = !string.IsNullOrEmpty(dt.Rows[0]["pl_building_name"].ToString()) ? ", Building : " + dt.Rows[0]["pl_building_name"].ToString() + "" : string.Empty,
                    FloorName = !string.IsNullOrEmpty(dt.Rows[0]["pl_floor_name"].ToString()) ? ", Floor : " + dt.Rows[0]["pl_floor_name"].ToString() + "" : string.Empty,
                    LineName = !string.IsNullOrEmpty(dt.Rows[0]["pl_line_name"].ToString()) ? ", Line : " + dt.Rows[0]["pl_line_name"].ToString() + "" : string.Empty,
                    ShiftName = !string.IsNullOrEmpty(dt.Rows[0]["pl_shift_name"].ToString()) ? ", Shift : " + dt.Rows[0]["pl_shift_name"].ToString() + "" : string.Empty,
                    SalcatName = !string.IsNullOrEmpty(dt.Rows[0]["pl_salcat_name"].ToString()) ? "Salary Category : " + dt.Rows[0]["pl_salcat_name"].ToString() + "" : string.Empty,
                    StartDate = Convert.ToDateTime(dt.Rows[0]["pl_Startdate"]).ToString("dd-MMM-yyyy"),
                    EndDate = Convert.ToDateTime(dt.Rows[0]["pl_Enddate"]).ToString("dd-MMM-yyyy"),
                    PrintUser = dt.Rows[0]["pl_user"].ToString()
                };
                return titleList;
            }           
            return null;
        }
        private int GetMonthDays(DateTime xDate)
        {
            return DateTime.DaysInMonth(xDate.Year, xDate.Month);
        }
        private int GetTotWorkingDays(DateTime xDate, int CompID)
        {
            int num;
            DateTime.DaysInMonth(xDate.Year, xDate.Month);
            string[] str = new string[] { "SELECT COUNT(distinct sh_date) AS SH FROM dg_pay_specialholidays_empWise WHERE month(sh_date) =", Convert.ToString(xDate.Month), " AND YEAR(sh_date) =", Convert.ToString(xDate.Year), " AND sh_compid =", Convert.ToString(CompID) };
            SqlCommand sqlCommand = new SqlCommand(string.Concat(str), _connection);
            _connection.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            sqlDataReader.Read();
            num = (sqlDataReader.IsDBNull(0) ? 0 : Convert.ToInt32(sqlDataReader[0]));
            _connection.Close();
            int num1 = 0;
            int num2 = DateTime.DaysInMonth(xDate.Year, xDate.Month);
            for (int i = 1; i <= num2; i = checked(i + 1))
            {
                str = new string[] { i.ToString(), "/", Convert.ToString(xDate.Month), "/", Convert.ToString(xDate.Year) };
                if (DateTime.ParseExact(string.Concat(str), "d/M/yyyy", null).DayOfWeek == DayOfWeek.Friday)
                {
                    num1 = checked(num1 + 1);
                }
            }
            int num3 = checked(checked(DateTime.DaysInMonth(xDate.Year, xDate.Month) - (checked(num + num1))) + GetCoveringDays(xDate, CompID));
            return num3;
        }
        private int GetTotWeeklyHolidays(DateTime xDate, int CompID)
        {
            DateTime.DaysInMonth(xDate.Year, xDate.Month);
            int num = 0;
            int num1 = DateTime.DaysInMonth(xDate.Year, xDate.Month);
            for (int i = 1; i <= num1; i = checked(i + 1))
            {
                string[] str = new string[] { i.ToString(), "/", Convert.ToString(xDate.Month), "/", Convert.ToString(xDate.Year) };
                if (DateTime.ParseExact(string.Concat(str), "d/M/yyyy", null).DayOfWeek == DayOfWeek.Friday)
                {
                    num = checked(num + 1);
                }
            }
            return checked(num - GetCoveringDays(xDate, CompID));
        }
        private int GetTotSpecialHolidays(DateTime xDate, int CompID)
        {
            int num;
            DateTime.DaysInMonth(xDate.Year, xDate.Month);
            string[] str = new string[] { "SELECT COUNT(distinct sh_date) AS SH FROM dg_pay_specialholidays_empWise WHERE month(sh_date) =", Convert.ToString(xDate.Month), " AND YEAR(sh_date) =", Convert.ToString(xDate.Year), " AND sh_compid =", Convert.ToString(CompID) };
            SqlCommand sqlCommand = new SqlCommand(string.Concat(str), _connection);
            _connection.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            sqlDataReader.Read();
            num = (sqlDataReader.IsDBNull(0) ? 0 : Convert.ToInt32(sqlDataReader[0]));
            _connection.Close();
            return num;
        }        
        private int GetCoveringDays(DateTime xDate, int Compid)
        {
            int num;
            string[] str = new string[] { "SELECT COUNT(distinct cd_covDate) AS CD FROM dg_pay_attcovering_days_empWise WHERE month(cd_covDate) =month('", Convert.ToString(xDate), "') and year(cd_covDate) =year('", Convert.ToString(xDate), "') and cd_compid =", Convert.ToString(Compid) };
            SqlCommand sqlCommand = new SqlCommand(string.Concat(str), _connection);
            _connection.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            sqlDataReader.Read();
            num = (sqlDataReader.IsDBNull(0) ? 0 : Convert.ToInt32(sqlDataReader[0]));
            _connection.Close();
            return num;
        }




        private byte[] barcodeGenerator(string bData)
        {
            Barcode barcode = new Barcode();
            Image img = barcode.Encode(TYPE.CODE128, bData, Color.Black, Color.White, 300, 100);
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, ImageFormat.Jpeg);
                byte[] imageBytes = ms.ToArray();
                return imageBytes;
            }
        }
        private void AddDataColumnWithBarcode(DataTable dt, int columnIndex)
        {
            DataColumn[] newCol = new DataColumn[]
            {
                new DataColumn("BarcodeGen", typeof(byte[])),
                new DataColumn("IsShowEmpSign", typeof(bool)),
                new DataColumn("IsShowAuthSign", typeof(bool))
            };
            dt.Columns.AddRange(newCol);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string imagePathEmp = $"{_webHostEnvironment.WebRootPath}\\EmployeeSignature\\" + dt.Rows[i]["compid"].ToString() + "\\" + dt.Rows[i]["emp_no"].ToString() + ".png";
                if (File.Exists(imagePathEmp))
                {
                    dt.Rows[i]["IsShowEmpSign"] = true;
                }
                else
                {
                    dt.Rows[i]["IsShowEmpSign"] = false;
                }
                string imagePathAuth = $"{_webHostEnvironment.WebRootPath}\\AuthSign\\" + dt.Rows[i]["compid"].ToString() + ".jpg";
                if (File.Exists(imagePathAuth))
                {
                    dt.Rows[i]["IsShowAuthSign"] = true;
                }
                else
                {
                    dt.Rows[i]["IsShowAuthSign"] = false;
                }
                Barcode barcode = new Barcode();
                Image img = barcode.Encode(TYPE.CODE128, dt.Rows[i][columnIndex].ToString(), Color.Black, Color.White, 300, 100);
                using (MemoryStream ms = new MemoryStream())
                {
                    img.Save(ms, ImageFormat.Jpeg);
                    byte[] imageBytes = ms.ToArray();
                    dt.Rows[i]["BarcodeGen"] = imageBytes;
                }
            }
        }
        //private void isShowImage(DataTable dt,DataColumn[] columnName,string imagePath)
        //{
        //    if (dt.Rows.Count > 0)
        //    {
        //        dt.Columns.AddRange(columnName);
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            if (File.Exists(imagePath))
        //            {

        //            }
        //        }
        //    }
        //}
    }
}