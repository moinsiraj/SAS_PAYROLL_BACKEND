using BLL.Interfaces;
using BLL.Utility;
using BOL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Payroll_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IGlobalMaster _globalMaster;
        private readonly Dg_Common _dgCommon;
        private readonly ILogger<ReportController> _logger;
        public ReportController(IGlobalMaster globalMaster, Dg_Common dgCommon, ILogger<ReportController> logger)
        {
            _globalMaster = globalMaster;
            _dgCommon = dgCommon;
            _logger = logger;

        }

        #region"Employee"
        [HttpGet("InActiveList")]
        public IActionResult EmpINActiveList(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Export_Report_Employee_Details_InActive(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("ActiveList")]
        public IActionResult EmpActiveList(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Export_Report_Employee_Details_Active(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Dg_ActiveEmployeeImage")]
        public IActionResult Dg_ActiveEmployeesWithImage(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Dg_ActiveEmployeesWithImage(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("ActiveList_With_image")]
        public IActionResult ActiveList_With_image(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Export_Report_Employee_Details_Active_With_Image(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("In_ActiveList_With_image")]
        public IActionResult In_ActiveList_With_image(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Export_Report_Employee_Details_In_Active_With_Image(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Dg_JoinDateWiseEmployeeDetails")]
        public IActionResult JoinDateWiseEmployee_Details_InActive(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Dg_JoinDateWise_Employee_Details_Active(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Dg_EmployeeIdCard")]
        public IActionResult Dg_EmployeeIdCard(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Dg_CreateReportFileIDCARD(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Dg_EmployeeIdCard_Bangla")]
        public IActionResult Dg_EmployeeIdCard_Bangla(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Dg_CreateReportFileIDCARD_BN(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Dg_EmpAgeCertificate")]
        public IActionResult Dg_EmpAgeCertificate(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Dg_EmpAgeCertificate(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Dg_IncrementLetter")]
        public IActionResult Dg_IncrementLetter(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Dg_IncrementLetter(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Dg_PromotionWithIncrementLetter")]
        public IActionResult Dg_PromotionWithIncrementLetter(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Dg_PromotionWithIncrementLetter(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Dg_IncrementLetter_Bangla")]
        public IActionResult Dg_IncrementLetter_Bangla(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Dg_IncrementLetter_Bangla(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Dg_PromotionWithIncrementLetter_Bangla")]
        public IActionResult Dg_PromotionWithIncrementLetter_Bangla(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Dg_PromotionWithIncrementLetter_Bangla(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Dg_AppointmentLetter")]
        public IActionResult Dg_AppointmentLetter(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Dg_AppointmentLetter(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Dg_AppointmentLetter_IFL")]
        public IActionResult Dg_AppointmentLetter_IFL(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Dg_AppointmentLetter_IFL(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Dg_AppointmentLetter_StaffBangla")]
        public IActionResult Dg_AppointmentLetter_StaffBangla(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Dg_AppointmentLetter_StaffBangla(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Dg_AppointmentLetter_EN")]
        public IActionResult Dg_AppointmentLetter_EN(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Dg_AppointmentLetter_EN(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Dg_NoticeLetter_1st")]
        public IActionResult Dg_NoticeLetter_1st(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Dg_NoticeLetter_1st(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Dg_NoticeLetter_2nd")]
        public IActionResult Dg_NoticeLetter_2nd(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Dg_NoticeLetter_2nd(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Dg_voluntarily_resign_letter")]
        public IActionResult Dg_voluntarily_resign_letter(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Dg_voluntarily_resign_letter(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Dg_MaleFemaleDetails")]
        public IActionResult Dg_MaleFemaleDetails(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Dg_MaleFemaleDetails(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Dg_MaleFemaleSummary")]
        public IActionResult Dg_MaleFemaleSummary(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Dg_MaleFemaleSummary(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Dg_EmployeeDetailsRegligion")]
        public IActionResult Dg_EmployeeDetailsRegligion(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Dg_EmployeeDetailsRegligion(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Dg_ProximityCardChecklist")]
        public IActionResult Dg_ProximityCardChecklist(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Dg_ProximityCardChecklist(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Dg_EmployeewiseIncrementDetails")]
        public IActionResult Dg_EmployeewiseIncrementDetails(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Dg_EmployeewiseIncrementDetails(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Dg_EmployeewiseIncrement_Sheet_Details")]
        public IActionResult Dg_EmployeewiseIncrement_Sheet_Details(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Dg_EmployeewiseIncremen_Sheet_Details(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Dg_CutoffDatewiseIncrementPendingList")]
        public IActionResult Dg_CutoffDatewiseIncrementPendingList(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Dg_CutoffDatewiseIncrementPendingList(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet]
        [Route("Dg_CutoffDatewiseIncrementApprovedList")]
        public IActionResult Dg_CutoffDatewiseIncrementApprovedList(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Dg_CutoffDatewiseIncrementApprovedList(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Dg_EmployeeWiseDetailedInformation")]
        public IActionResult Dg_EmployeeWiseDetailedInformation(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Dg_EmployeeWiseDetailedInformation(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Dg_EmployeeInformation_BankAccForm")]
        public IActionResult Dg_EmployeeInformation_BankAccForm(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Dg_EmployeeInformation_BankAccForm(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Dg_EmpTiffinBillStatus")]
        public IActionResult Dg_EmpTiffinBillStatus(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Dg_Emp_TiffinBillStatus(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Dg_EmpNightBillStatus")]
        public IActionResult Dg_Emp_NightBillStatus(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Dg_Emp_NightBillStatus(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("shift_change_history")]
        public IActionResult EMPshift_change_history(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Export_Report_Employee_shiftchange_history(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost("shift_change_history_new")]
        public IActionResult shift_change_history_new([Bind(nameof(ReportParameterModel))] ReportParameterModel obj)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Export_Report_Employee_shiftchange_history_New(obj);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(obj.reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Dg_EmpShiftGroupChecklist")]
        public IActionResult Dg_EmpShiftGroupChecklist(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Dg_EmpShiftGroupChecklist(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("EmployeeNominationForm")]
        public IActionResult EmployeeNominationForm(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.EmployeeNominationForm(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("EmployeeJoiningLetter")]
        public IActionResult EmployeeJoiningLetter(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.EmployeeJoiningLetter(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("EmployeeJobApplication")]
        public IActionResult EmployeeJobApplication(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.EmployeeJobApplication(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("EmployeeDetailsExcel")]
        public IActionResult EmployeeDetailsExcel(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.EmployeeDetailsExcel(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("EmployeeBackgroundCheckBN")]
        public IActionResult EmployeeBackgroundCheckBN(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.EmployeeBackgroundCheck(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("EmployeeIvelatution_From")]
        public IActionResult EmployeeIvelatution_From(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.EmployeeIvelatution_From(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("EmployeeBudgetReport")]
        public IActionResult EmployeeBudgetReport(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.EmployeeBudgetReport(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }


        [HttpGet("EmployeeServiceBookReport")]
        public IActionResult EmployeeServiceBookReport(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.EmployeeServiceBookReport(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }


        [HttpGet("Dg_Ecard_datetodate_complaince_2hour")]
        public IActionResult Dg_CreateReportFile_Ecard_Date_To_Date_2hour(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Dg_CreateReportFile_Ecard_Date_To_Date_2hour(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpGet("Dg_Ecard_date_todate_bayer_4hour")]
        public IActionResult Dg_CreateReportFile_Ecard_Date_To_Date_4hour(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Dg_CreateReportFile_Ecard_Date_To_Date_4hour(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Separation_Employee")]
        public IActionResult Report_In_Activeemp_Date_wise_Excel(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Report_In_Activeemp_Date_wise_Excel(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("EmployeeInternalTransfer_info_D2D")]
        public IActionResult Report_EmployeeInternalTransfer_info_D2D(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Report_EmployeeInternalTransfer_info_D2D(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Employee_info_bangla")]
        public IActionResult Export_Report_Employee_info_bangla(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Export_Report_Employee_info_bangla(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        //Audit
        [HttpGet("excel_Audit_Attendance_preriodical")]
        public IActionResult Report_preriodical_present_absent_leave_Weekly_holiday_special_holiday(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Report_preriodical_present_absent_leave_Weekly_holiday_special_holiday(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Report_salary_Info_Audit_Excel")]
        public IActionResult Report_Salary_info_Audit_Excel(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Report_Salary_info_Audit_Excel(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpGet("Report_Joindate_wise_Emp_info_Audit_Excel")]
        public IActionResult Report_Joindate_wise_info_Audit_Excel(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Report_Joindate_wise_info_Audit_Excel(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Report_manual_attendsance_Audit_Excel")]
        public IActionResult Report_manual_attendsance_Audit_Excel(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Report_manual_attendsance_Audit_Excel(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Report_TiffinBill_Audit_Excel")]
        public IActionResult Report_TiffinBill_Audit_Excel(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Report_TiffinBill_Audit_Excel(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Report_NightBill_Audit_Excel")]
        public IActionResult Report_Night_Audit_Excel(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Report_Night_Audit_Excel(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpGet("Report_LeaveTransction_Audit_Excel")]
        public IActionResult Report_Leave_transction_Audit_Excel(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Report_Leave_transction_Audit_Excel(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("EmployeeDetailsExcel_audit")]
        public IActionResult EmployeeDetailsExcel_audit(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.EmployeeDetailsExcel(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Separation_Employee_Audit")]
        public IActionResult Report_In_Activeemp_Date_wise_Audit_Excel(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Report_In_Activeemp_Date_wise_Audit_Excel(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Date_wise_total_ot_details_Audit_Excel")]
        public IActionResult Date_wise_total_ot_details_Audit_Excel(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Date_wise_total_ot_details_Audit_Excel(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Employee_envelope")]
        public IActionResult Employee_envelope(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Report_Employee_envelope(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Report_TiffinBill_Audit_date_to_date_Excel")]
        public IActionResult Report_TiffinBill_Audit_date_to_date_Excel(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Report_TiffinBill_Audit_date_to_date_Excel(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Report_manual_attendsance_Audit_in_out_Excel")]
        public IActionResult Report_manual_attendsance_Audit_in_out_Excel(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Report_manual_attendsance_Audit_in_out_Excel(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        //Audit
        #endregion

        #region"Leave"
        [HttpGet("Dg_LeaveBalances")]
        public IActionResult Dg_LeaveBalances(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Dg_LeaveBalances(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Dg_LeaveTransactions")]
        public IActionResult Dg_LeaveTransactions(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Dg_LeaveTransactions(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Dg_IndividualLeaveStatement")]
        public IActionResult Dg_IndividualLeaveStatement(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Dg_IndividualLeaveStatement(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Dg_MaternityLeaveList")]
        public IActionResult Dg_MaternityLeaveList(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Dg_MaternityLeaveList(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Dg_MaternityLeavePayment")]
        public IActionResult Dg_MaternityLeavePayment(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Dg_MaternityLeavePayment(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Dg_LeaveForm")]
        public IActionResult Dg_LeaveForm(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Dg_LeaveForm(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Dg_LeaveFormEmpWise")]
        public IActionResult Dg_LeaveFormEmpWise(int empSerial, int leaveID, string reportType)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Dg_LeaveFormEmpWise(empSerial, leaveID, reportType);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Dg_IndividualLeave_Register")]
        public IActionResult Dg_IndividualLeave_Register(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Dg_IndividualLeave_Register(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        #endregion

        #region"Attendance"
        [HttpGet("Present")]
        public IActionResult Present(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.CreateReportFile_Attendance_Present(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Dg_Att_PresentWithImages")]
        public IActionResult Dg_Att_PresentWithImages(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Dg_Att_PresentWithImages(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Absent")]
        public IActionResult Absent(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.CreateReportFile_Attendance_Absent(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Dg_Att_AbsentWithImages")]
        public IActionResult Dg_Att_AbsentWithImages(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Dg_Att_AbsentWithImages(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Late")]
        public IActionResult Late(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.CreateReportFile_Attendance_Late(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Innotpunch")]
        public IActionResult Innotpunch(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.CreateReportFile_Attendance_Innotpunch(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Outnotpunch")]
        public IActionResult Outnotpunch(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.CreateReportFile_Attendance_Outnotpunch(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("InOutnotpunch")]
        public IActionResult InOutnotpunch(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.CreateReportFile_Attendance_InOutnotpunch(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Dg_Att_SectionWiseSummary")]
        public IActionResult Dg_Att_SectionWiseSummary(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Dg_Att_SectionWiseSummary(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Dg_CreateReportFile_Ecard_u1")]
        public IActionResult Dg_CreateReportFile_Ecard_u1(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Dg_CreateReportFile_Ecard_u1(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Dg_Ecard_Date_To_Date")]
        public IActionResult Dg_CreateReportFile_Ecard_Date_To_Date(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Dg_CreateReportFile_Ecard_Date_To_Date(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Dg_CreateReportFile_Ecard_complaince_2hour")]
        public IActionResult Dg_CreateReportFile_Ecard_complaince_2hour(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Dg_CreateReportFile_Ecard_complaince_2hour(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Dg_CreateReportFile_Ecard_bayer_4hour")]
        public IActionResult Dg_CreateReportFile_Ecard_bayer_4hour(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Dg_CreateReportFile_Ecard_bayer_4hour(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("MonthlyAttendance")]
        public IActionResult Dg_MonthlyAttendance(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Dg_Att_MonthlyAttendance(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("MonthlyAttendance_Absent")]
        public IActionResult MonthlyAttendance_Absent(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Dg_Att_MonthlyAttendance_Absent(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("EmployeeMenualAttnList")]
        public IActionResult EmployeeMenualAttnList(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.EmployeeMenualAttnList(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Date_wise_total_ot_details")]
        public IActionResult Date_wise_total_ot_details(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Date_wise_total_ot_details(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Floor_wise_manpower_comparison")]
        public IActionResult Floor_wise_manpower_comparison(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Floor_wise_manpower_comparison(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("EmployeeAutoShiftHistory")]
        public IActionResult EmployeeAutoShiftHistory(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.EmployeeAutoShiftHistory(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }       
        [HttpGet("excel_Audit_Attendance_preriodicall")]
        public IActionResult Report_preriodical_present_absent_leave_Weekly_holiday_special_holidays(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Report_preriodical_present_absent_leave_Weekly_holiday_special_holidays(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Dg_Att_hourly_manpower")]
        public IActionResult Dg_Att_hourly_manpower(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Dg_Att_hourly_manpower(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Dg_Att_hourly_manpower_d2d")]
        public IActionResult Dg_Att_hourly_manpower_d2d(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Dg_Att_hourly_manpower_d2d(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Dg_Att_hourly_manpower_otandvalue")]
        public IActionResult Dg_Att_hourly_manpower_otandvalue(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Dg_Att_hourly_manpower_otandvalue(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Dg_Att_single_hourwise_manpower_otandvalue")]
        public IActionResult Dg_Att_single_hourwise_manpower_otandvalue(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Dg_Att_single_hourwise_manpower_otandvalue(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("LunchOut")]
        public IActionResult LunchOut(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.CreateReportFile_Attendance_LunchOut(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Warning")]
        public IActionResult Warning(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.CreateReportFile_Weekly_Warning(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Continuous_Absenteeism")]
        public IActionResult Continuous_Absenteeism(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.CreateReportFile_Continuous_Absenteeism(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        #endregion

        #region"Salary"
        [HttpGet("SalaryShert")]
        public IActionResult SalaryShert(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.CreateReportFile_salarysheet_D(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Sal_salarysheet_Details")]
        public IActionResult SalaryShert_Details_Actual(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Sal_salarysheet_Details(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Sal_salarysheet_Details_Excel")]
        public IActionResult Sal_salarysheet_Details_Excel(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Sal_salarysheet_Details_Excel(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Sal_salarysheet_DetailsReport")]
        public IActionResult SalaryShert_Detail_2Hour(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Sal_salarysheet_DetailsReport(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Sal_salarysheet_ReportDetails")]
        public IActionResult SalaryShert_Detail_4Hour(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Sal_salarysheet_ReportDetails(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("SalaryShert_LineWise_Summary")]
        public IActionResult SalaryShert_LineWise_Summary(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.SalaryShert_LineWise_Summary(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("SalaryShert_53")]
        public IActionResult SalaryShert_53(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.CreateReportFile_salarysheet_D_53(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("SalaryShert_53_complaince_2hour")]
        public IActionResult SalaryShert_53_complaince_2hour(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.CreateReportFile_salarysheet_D_53_2(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("SalaryShert_53_complaince_4hour")]
        public IActionResult SalaryShert_53_complaince_4hour(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.CreateReportFile_salarysheet_D_53_4(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Payslip")]
        public IActionResult Payslip(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.CreateReportFile_Payslip(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Payslip_bangla")]
        public IActionResult Payslip_bangla(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.CreateReportFile_Payslip_bangl(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Payslip_complaince_2hour")]
        public IActionResult Payslip_complaince_2hour(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.CreateReportFile_Payslip_complaince_2hour(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Payslip_bayer_2hour_bangla")]
        public IActionResult Payslip_bayer_2hour_bangla(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.CreateReportFile_Payslip_bayer_2hour_Bangla(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Payslip_bayer_4hour")]
        public IActionResult Payslip_bayer_4hour(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.CreateReportFile_Payslip_bayer_4hour(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Payslip_bayer_4hour_bangla")]
        public IActionResult Payslip_bayer_4hour_bangla(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.CreateReportFile_Payslip_bayer_4hour_Bangla(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("SalarySheet_Bank")]
        public IActionResult SalarySheet_Bank(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.CreateReportFile_SalarySheetBank(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("SalarySheetEXOT")]
        public IActionResult SalarySheetEXOT(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.CreateReportFile_SalarySheetEXOT(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("SalarySheetOTEXOT")]
        public IActionResult SalarySheetOTEXOT(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.CreateReportFile_SalarySheetOTEXOT(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("OTDetails")]
        public IActionResult SalarySheetOTDetails(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.CreateReportFile_OTDetails(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("OTSummary")]
        public IActionResult SalarySheetOTSummary(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.CreateReportFile_OTSummary(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("SalarySheetSummary")]
        public IActionResult SalarySheetSummary(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.CreateReportFile_SalarySheetSummary(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("SalarySheetSummaryYearly")]
        public IActionResult SalarySheetSummaryYearly(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.CreateReportFile_SalarySheetSummaryYearly(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("SalarySheetSummarySalCategoryWise")]
        public IActionResult SalarySheetSummarySalCategoryWise(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.SalarySheetSummarySalCategoryWise(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("SalarySheetSummary_Floor_SalCategoryWise")]
        public IActionResult SalarySheetSummary_Floor_SalCategoryWise(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.SalarySheetSummary_Floor_SalCategoryWise(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("SalarySheetSummary_FloorAndLine_SalCategoryWise")]
        public IActionResult SalarySheetSummary_FloorAndLine_SalCategoryWise(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.SalarySheetSummary_FloorAndLine_SalCategoryWise(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("SalarySheetOtExotSummary")]
        public IActionResult SalarySheetOtExotSummary(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.CreateReportFile_SalarySheetOtExotSummary(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("SalarySheetBank_Excel")]
        public IActionResult Sal_SalarySheetBankExcel(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Sal_SalarySheetBankExcel(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType("excel"));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Sal_SalarySheetAccount")]
        public IActionResult Sal_SalarySheetAccount(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Sal_SalarySheetAccount(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Sal_TiffinBillSummary")]
        public IActionResult Sal_TiffinBillSummary(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Sal_EmployeeTiffinBillSummary(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Sal_NightBillSummary")]
        public IActionResult Sal_NightBillSummary(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Sal_EmployeeNightBillSummary(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Sal_EmpTiffinBillAmount")]
        public IActionResult Sal_TiffinBillAmount(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Sal_EmployeeTiffinBillAmount(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Sal_EmpNightBillAmount_date_to_date")]
        public IActionResult Sal_EmpNightBillAmount_date_to_date(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Sal_EmployeeNightBillAmount_date_to_date(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Sal_EmpNightBillAmount")]
        public IActionResult Sal_NightBillAmount(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Sal_EmployeeNightBillAmount(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Sal_Eid_bonus")]
        public IActionResult Sal_Eid_bonus(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Sal_Eid_bonus(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Sal_Eid_bonus_excel")]
        public IActionResult Sal_Eid_bonus_excel(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Sal_Eid_bonus_excel(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Sal_EidBonusSummaryLineWise")]
        public IActionResult Sal_EidBonusSummaryLineWise(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Sal_EidBonusSummaryLineWise(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Sal_Eid_bonus_bank")]
        public IActionResult Sal_Eid_bonus_bank(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Sal_Eid_bonus_bank(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Sal_EidBonusSummaryCatagoryWise")]
        public IActionResult Sal_EidBonusSummaryCatagoryWise(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Sal_EidBonusSummaryHead(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Sal_SalaryAdvance")]
        public IActionResult Sal_SalaryAdvance(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Sal_SalaryAdvance(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("SalarySheet_Advance_Bank")]
        public IActionResult CreateReportFile_SalarySheet_advanceBank(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.CreateReportFile_SalarySheet_advanceBank(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("SalarySheet_Advance_Summary")]
        public IActionResult CreateReportFile_SalarySheet_Advance_Summary(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.CreateReportFile_SalarySheet_Advance_Summary(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("SalarySheet_Advance_LineWiseSummary")]
        public IActionResult SalaryShert_LineWise_Advance_Summary(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.SalaryShert_LineWise_Advance_Summary(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("SalarySheetcomplaince2hour")]
        public IActionResult CreateReportFile_SalarySheetcomplaince2hour(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.CreateReportFile_SalarySheetcomplaince2hour(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("SalarySheetcomplaince4hour")]
        public IActionResult CreateReportFile_SalarySheetcomplaince4hour(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.CreateReportFile_SalarySheetcomplaince4hour(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Date_TO_Date_OTSummary")]
        public IActionResult Date_TO_Date_OTSummary(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.CreateReportFile_Date_TO_Date_OTSummary(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Date_TO_Date_OT_4_hour")]
        public IActionResult Date_TO_Date_OT_4_hour(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.CreateReportFile_Date_TO_Date_OT_4_hour(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("CreateReportFile_Date_TO_Date_OTSummary_subscetion_wise")]
        public IActionResult CreateReportFile_Date_TO_Date_OTSummary_subscetion_wise(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.CreateReportFile_Date_TO_Date_OTSummary_subscetion_wise(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("SalarySheetSummary_2hour")]
        public IActionResult SalarySheetSummary_2hour(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.CreateReportFile_SalarySheetSummary_2hour(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("SalarySheetSummary_4hour")]
        public IActionResult SalarySheetSummary_4hour(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.CreateReportFile_SalarySheetSummary_4hour(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("SalaryShert_LineWise_Summary_2hour")]
        public IActionResult SalaryShert_LineWise_Summary_2hour(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.SalaryShert_LineWise_Summary_2hour(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("SalaryShert_LineWise_Summary_4hour")]
        public IActionResult SalaryShert_LineWise_Summary_4hour(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.SalaryShert_LineWise_Summary_4hour(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Date_TO_Date_2hourorabove_2hourOT")]
        public IActionResult Date_TO_Date_2hourorabove_2hourOT(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.CreateReportFile_Date_TO_Date_2hourorabove_2hourOT(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("SalarySheet_Bank_Report")]
        public IActionResult SalarySheet_Bank_2hrs(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.CreateReportFile_SalarySheetBank_2hrs(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("SalarySheet_Report_Bank")]
        public IActionResult SalarySheet_Bank_4hrs(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.CreateReportFile_SalarySheetBank_4hrs(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Sal_SalarySheetBank_Forwarding")]
        public IActionResult Sal_SalarySheetBank_Forwarding(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Sal_SalarySheetBank_Forwarding(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Date_TO_Date_4hourabove_OT")]
        public IActionResult Date_TO_Date_4hourorabove_OT(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.CreateReportFile_Date_TO_Date_4hourorabove_OT(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("R_OT")]
        public IActionResult R_OT(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.CreateReportFile_R_OT(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Date_TO_Date_4hourorabove_OT_sum")]
        public IActionResult Date_TO_Date_4hourorabove_OT_sum(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.CreateReportFile_Date_TO_Date_4hourorabove_OT_linewise_sum(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Anual_Leave_payment_rpt")]
        public IActionResult Report_Anual_Leave_payment_rpt(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Report_Anual_Leave_payment_rpt(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Sal_EmpNightBillAmount_date_to_date_General_Shift")]
        public IActionResult Sal_EmpNightBillAmount_date_to_date_General_Shift(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Sal_EmployeeNightBillAmount_date_to_date_General_Shift(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Comparative_Salary_Summary_2Month")]
        public IActionResult Comparative_Salary_Summary_2Month(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Comparative_Salary_Summary_2Month(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Sal_EmpNightBillAmount_date_to_date_General_Shift_Bank")]
        public IActionResult Sal_EmpNightBillAmount_date_to_date_General_Shift_Bank(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Sal_EmpNightBillAmount_date_to_date_General_Shift_Bank(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Sal_EmpNightBillAmount_date_to_date_General_Shift_Summary")]
        public IActionResult Sal_EmpNightBillAmount_date_to_date_General_Shift_Summary(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Sal_EmployeeNightBillAmount_date_to_date_General_Shift_Summary(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Sal_EidBonusSummaryLineWise_excel")]
        public IActionResult Sal_EidBonusSummaryLineWiseExcel(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Sal_EidBonusSummaryLineWiseExcel(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        #endregion

        #region"Others"
        [HttpGet("SalaryShert_Bank")]
        public IActionResult SalaryShert_Bank(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.CreateReportFile_salarysheet_D_Bank(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Ecard")]
        public IActionResult Ecard(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.CreateReportFile_Ecard(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        
        [HttpGet("Payslip_U1")]
        public IActionResult Payslip_U1(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.CreateReportFile_Payslip_U1(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }      
        [HttpGet("SalarySheet-NC")]
        public IActionResult Export_Report_SalarySheet(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.Export_Report_SalarySheet(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("testBarcode")]
        public IActionResult testBarcodeRDLC(string reportType, int companyID, string userName)
        {
            try
            {
                var reportBytes = _globalMaster.reportManager.testBarcode(reportType, companyID, userName);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        #endregion

        [HttpPost("PostPayReportView")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostPayReportView(PostReportViewPayload obj)
        {
            try
            {
                var response = await _globalMaster.reportManager.PostPayReportView(obj);
                string fileName = $"Report_{DateTime.Now:yyyyMMddHHmmss}.{obj.reportType.ToLower()}";
                return File(response, _dgCommon.GetContentType(obj.reportType), fileName);
                //return File(response, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(obj.reportType));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PostPayReportView)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
    }
}