using BLL.Interfaces;
using BLL.Utility;
using BOL.Models;
using CoreApiResponse;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net;
using System.Net.Mime;

namespace Payroll_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : BaseController
    {
        private readonly IGlobalMaster _globalMaster;
        private readonly IWebHostEnvironment _environment;
        private readonly Dg_Common _sqlCommon;
        private readonly ILogger<EmployeesController> _logger;
        public EmployeesController(IGlobalMaster globalMaster, IWebHostEnvironment environment, Dg_Common sqlCommon,ILogger<EmployeesController> logger)
        {
            _globalMaster = globalMaster;
            _environment = environment;
            _sqlCommon = sqlCommon;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetDgPayEmployees()
        {
            try
            {
                var data = _globalMaster.employees.GetAll().Select(data => DgPayEmployee.DbToCustom(data)).ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgPayEmployees)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDgPayEmployee(int id)
        {
            try
            {
                var data = await _globalMaster.employees.GetFirstOrDefaultAsync(x => x.emp_serial == id);
                var nData = DgPayEmployee.DbToCustom(data);
                return Ok(nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgPayEmployee)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDgPayEmployee(int id, DgPayEmployee obj)
        {
            try
            {
                var nData = DgPayEmployee.CustomToDb(obj);
                bool isUpdate = await _globalMaster.employees.UpdateAsync(nData);
                if (isUpdate)
                {
                    var compInfo = await _globalMaster.employees.GetCompanyAddress(int.Parse(obj.Compid.ToString()));
                    var compName = compInfo.Rows[0]["com_name"].ToString();
                    var compAddName = compInfo.Rows[0]["com_ad1"].ToString();
                    var joinDate = Convert.ToDateTime(obj.OiJoineddate).ToString("dd-MMM-yyyy");
                    var QrInfo = new string[] { "ID No:" + obj.EmpNo.ToString() + ",", " Name:" + obj.PiFullname + ",", " Join Date:" + joinDate + ",", " Designation:" + obj.OiDesignationName + ",", " Company:" + compName + ",", " Address:" + compAddName + ",", "Email:info@debonairgroupdb.com,", " Website:https://www.debonairgroupdb.com" };
                    QrInfo = QrInfo.Select(x => x.EndsWith(Environment.NewLine) ? x : x + Environment.NewLine).ToArray();
                    var FilePath = new string[] { $"{_environment.WebRootPath}\\Employee_QRCode\\" + obj.Compid + "\\", "" + obj.EmpNo + "", ".jpg" };
                    _globalMaster.employees.QR_Generator(QrInfo, FilePath);
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PutDgPayEmployee)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost]
        public async Task<IActionResult> PostDgPayEmployee(DgPayEmployee obj)
        {
            try
            {
                var nData = DgPayEmployee.CustomToDb(obj);
                bool isSave = await _globalMaster.employees.AddAsync(nData);
                if (isSave)
                {
                    var compInfo = await _globalMaster.employees.GetCompanyAddress(int.Parse(obj.Compid.ToString()));
                    var compName = compInfo.Rows[0]["com_name"].ToString();
                    var compAddName = compInfo.Rows[0]["com_ad1"].ToString();
                    var joinDate = Convert.ToDateTime(obj.OiJoineddate).ToString("dd-MMM-yyyy");
                    var QrInfo = new string[] { "ID No:" + obj.EmpNo.ToString() + ",", " Name:" + obj.PiFullname + ",", " Join Date:" + joinDate + ",", " Designation:" + obj.OiDesignationName + ",", " Company:" + compName + ",", " Address:" + compAddName + ",", "Email:info@debonairgroupdb.com,", " Website:https://www.debonairgroupdb.com" };
                    QrInfo = QrInfo.Select(x => x.EndsWith(Environment.NewLine) ? x : x + Environment.NewLine).ToArray();
                    var FilePath = new string[] { $"{_environment.WebRootPath}\\Employee_QRCode\\" + obj.Compid + "\\", "" + obj.EmpNo + "", ".jpg" };
                    //Leave Generate
                    await _globalMaster.employees.EmpLeaveGenerate(obj);
                }
                return CreatedAtAction("GetDgPayEmployee", new { id = nData.emp_serial }, nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PostDgPayEmployee)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDgPayEmployee(int id)
        {
            try
            {
                var obj = await _globalMaster.employees.GetFirstOrDefaultAsync(c => c.emp_serial == id);
                if (obj == null)
                {
                    _logger.LogError($"Invalid Delete Attempt In {nameof(DeleteDgPayEmployee)}");
                    return BadRequest("Submitted Data Is Invalid");
                }
                await _globalMaster.employees.DeleteAsync(obj);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteDgPayEmployee)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpPost("GenQR_code")]
        public async Task<IActionResult> GenQR_code(EmpQRCodeGenerator emp)
        {
            var compInfo = await _globalMaster.employees.GetCompanyAddress(int.Parse(emp.CompanyID.ToString()));
            var compName = compInfo.Rows[0]["com_name"].ToString();
            var compAddName = compInfo.Rows[0]["com_ad1"].ToString();
            var joinDate = Convert.ToDateTime(emp.JoinDate).ToString("dd-MMM-yyyy");
            var QrInfo = new string[] { "ID No:" + emp.EmpNo.ToString() + ",", "Name:" + emp.FullName + ",", "Join Date:" + joinDate + ",", "Designation:" + emp.DesignationName + ",", "Company:" + compName + ",", "Address:" + compAddName + ",", "Email:info@debonairgroupdb.com,", "Website:https://www.debonairgroupdb.com" };
            QrInfo = QrInfo.Select(x => x.EndsWith(Environment.NewLine) ? x : x + Environment.NewLine).ToArray();
            var FilePath = new string[] { $"{_environment.WebRootPath}\\Employee_QRCode\\" + emp.CompanyID + "\\", "" + emp.EmpNo + "", ".jpg" };
            _globalMaster.employees.QR_Generator(QrInfo, FilePath);
            return Ok();
        }
        [HttpPost("Employee_active_status")]
        public async Task<IActionResult> Employee_active_status(int Emp_serial, int Active_status,string active_Date, string Inactive_Date, string in_active_Resion, string userName)
        {
            try
            {
                var data = await _globalMaster.employees.Employee_active_status(Emp_serial, Active_status, active_Date, Inactive_Date, in_active_Resion, userName);
                if (data[0].ToString() == "success")
                {
                    return CustomResult(data[1].ToString(), HttpStatusCode.OK);
                }
                return CustomResult(data[1].ToString(), HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(Employee_active_status)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }
        [HttpGet("GetActiveInactiveHistory")]
        public async Task<IActionResult> GetActiveInactiveHistory(int empSerial)
        {
            try
            {
                var data = await _globalMaster.employees.GetActiveInactiveHistory(empSerial);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetActiveInactiveHistory)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("textfile")]
        public async Task<IActionResult> textfile(int nempSerial, int Emp_ID, int ngroupid, int ncompid, DateTime nDate, string ntime)
        {
            try
            {
                var data = await _globalMaster.employees.textfile(nempSerial, Emp_ID, ngroupid, ncompid, nDate, ntime);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(textfile)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpGet("get-FilterBase_ShiftChange")]
        public async Task<IActionResult> employee_ShiftChange(int? Compid = null, int? Department = null,
            int? section = null, int? Building = null, int? Floor = null, int? Line = null, int? Shift = null,
            int? Grade = null, int? salcat = null, int? Newshift = null, DateTime? EffectDate = null, string user = null)
        {
            try
            {
                var data = await _globalMaster.employees.employee_ShiftChange(Compid, Department, section, Building, Floor, Line, Shift, Grade, salcat, Newshift, EffectDate, user);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(employee_ShiftChange)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("get")]
        public async Task<IActionResult> get()
        {
            try
            {
                var data = await _globalMaster.employees.get();
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(get)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("get-FilterBase_individual")]
        public async Task<IActionResult> employee_Info(int emp_serial)
        {
            try
            {
                var data = await _globalMaster.employees.employee_Info(emp_serial);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(employee_Info)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("get-idcard-bangla")]
        public async Task<IActionResult> employee_Info()
        {
            try
            {
                var data = await _globalMaster.employees.employee_Info();
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(employee_Info)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("get-CompanyName-list")]
        public async Task<IActionResult> GetCompanyName()
        {
            try
            {
                var data = await _globalMaster.employees.GetCompanyName();
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetCompanyName)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("get-divisionName-list")]
        public async Task<IActionResult> GetdivisionName()
        {
            try
            {
                var data = await _globalMaster.employees.GetdivisionName();
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetdivisionName)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("get-Distict-list")]
        public async Task<IActionResult> GetDistict(int di_id)
        {
            try
            {
                var data = await _globalMaster.employees.GetDistict(di_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDistict)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("get-Thana-list")]
        public async Task<IActionResult> GetThana(int th_id)
        {
            try
            {
                var data = await _globalMaster.employees.GetThana(th_id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetThana)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("get-postoffice-list")]
        public async Task<IActionResult> Getpostoffice(int ThanaID)
        {
            try
            {
                var data = await _globalMaster.employees.Getpostoffice(ThanaID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(Getpostoffice)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("get-Village-list")]
        public async Task<IActionResult> Getvillage(int thanaID)
        {
            try
            {
                var data = await _globalMaster.employees.Getvillage(thanaID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(Getvillage)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("get-Companywiseemployee-list")]
        public async Task<IActionResult> Companywiseemployee(int compid)
        {
            try
            {
                var data = await _globalMaster.employees.Companywiseemployee(compid);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(Companywiseemployee)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("get-Employee_ID-list")]
        public async Task<IActionResult> Employee_ID(int compid)
        {
            try
            {
                var data = await _globalMaster.employees.Employee_ID(compid);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(Employee_ID)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("get-singleEmployee")]
        public async Task<IActionResult> singleEmployee(int comId, int EmpId)
        {
            try
            {
                var data = await _globalMaster.employees.singleEmployee(comId, EmpId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(singleEmployee)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("get-Employee_ID-list_formeternity")]
        public async Task<IActionResult> formeternity(int compid)
        {
            try
            {
                var data = await _globalMaster.employees.formeternity(compid);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(formeternity)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("get-Proxmity_ID-list")]
        public async Task<IActionResult> Proxmity_ID(int compid)
        {
            try
            {
                var data = await _globalMaster.employees.Proxmity_ID(compid);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(Proxmity_ID)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("get-Designation-list")]
        public async Task<IActionResult> GetDesignation()
        {
            try
            {
                var data = await _globalMaster.employees.GetDesignation();
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDesignation)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("get-Department-list")]
        public async Task<IActionResult> Department(int compid)
        {
            try
            {
                var data = await _globalMaster.employees.Department(compid);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(Department)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("get-Section-list")]
        public async Task<IActionResult> GetSection(int Department)
        {
            try
            {
                var data = await _globalMaster.employees.GetSection(Department);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetSection)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("get-Building-list")]
        public async Task<IActionResult> Getbuilding(int compid)
        {
            try
            {
                var data = await _globalMaster.employees.Getbuilding(compid);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(Getbuilding)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("get-Floor-list")]
        public async Task<IActionResult> GetFloor(int compid, int Building)
        {
            try
            {
                var data = await _globalMaster.employees.GetFloor(compid, Building);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetFloor)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("get-Line-list")]
        public async Task<IActionResult> GetLine(int compid, int Building, int Floor)
        {
            try
            {
                var data = await _globalMaster.employees.GetLine(compid, Building, Floor);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetLine)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("get-salarycategory")]
        public async Task<IActionResult> Getsalarycategory(int compID)
        {
            try
            {
                var data = await _globalMaster.employees.Getsalarycategory(compID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(Getsalarycategory)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("get-Shift-list")]
        public async Task<IActionResult> GetShift(int compid)
        {
            try
            {
                var data = await _globalMaster.employees.GetShift(compid);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetShift)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("get-Grade-list")]
        public async Task<IActionResult> GetGrade()
        {
            try
            {
                var data = await _globalMaster.employees.GetGrade();
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetGrade)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("get-Bank-list")]
        public async Task<IActionResult> GetBank()
        {
            try
            {
                var data = await _globalMaster.employees.GetBank();
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetBank)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost("UploadImage")]
        public async Task<IActionResult> UploadImage(IFormFile fileEmp, IFormFile fileNominee, IFormFile fileSign, IFormFile fileEmpNid, IFormFile fileNomineeNid, string compid, int empid)
        {
            try
            {
                var flag = await _globalMaster.employees.UploadEmpInfoImage(fileEmp, fileNominee, fileSign, fileEmpNid, fileNomineeNid, compid, empid);
                if (flag)
                {
                    return Ok();
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(UploadImage)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("viewEmpDocFile")]
        public IActionResult viewEmpDocFile(int compid, int empid, string vFrom)
        {
            try
            {
                var response = _globalMaster.employees.viewEmpDocFile(compid, empid, vFrom);
                if (response.IsSuccess)
                {
                    return File((byte[])response.dataTable, MediaTypeNames.Application.Octet, $"{empid}_Nid.pdf");
                }
                return NotFound(new { isSuccess = response.IsSuccess, statusCode = 200, message = response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(viewEmpDocFile)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }           
        }

        [HttpPost("uploadEmployeeImage_zipOrRar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> uploadEmployeeImage_zipOrRar(IFormFile zipFile, int compid, string imgType)
        {
            try
            {
                var response = await _globalMaster.employees.UploadEmployeeImageFolder(zipFile, compid, imgType);
                if (response.IsSuccess)
                {
                    return Ok(new { IsSuccess = response.IsSuccess, StatusCode = 200, Message = response.Message });
                }
                return BadRequest(new { IsSuccess = response.IsSuccess, StatusCode = 400, Message = response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(uploadEmployeeImage_zipOrRar)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        //Employee Education Action
        [HttpGet("getEmpNoForEDU")]
        public IActionResult GetEmployeeNoForEducation(int compID)
        {
            try
            {
                var data = _globalMaster.employees.GetAll().Where(e => e.compid == compID).
                    Select(e => new { EmpNo = e.emp_no}).OrderByDescending(e => e.EmpNo).ToList();
                return new JsonResult(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetBank)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("getEducationInfoByEmpNo")]
        public async Task<IActionResult> GetEmployeeEducationInfo(int EmpNo)
        {
            try
            {
                var data = await _globalMaster.education.GetEduSingel(EmpNo);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetEmployeeEducationInfo)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost("AddOrEditEducationInfo")]
        public async Task<IActionResult> AddOrEditEDU(EmpEducation edu)
        {
            try
            {
                if (edu != null)
                {
                    var ChkEmpNo = await _globalMaster.education.GetFirstOrDefaultAsync(e => e.comid == edu.comid && e.empno == edu.empno);
                    if (ChkEmpNo == null)
                    {
                        edu.addtime = DateTime.Now;
                        await _globalMaster.education.AddAsync(edu);
                        return CreatedAtAction("GetEmployeeEducationInfo", new { id = edu.empno }, edu);
                    }
                    else
                    {
                        await _globalMaster.education.UpdateEduInfo(edu);
                        return CreatedAtAction("GetEmployeeEducationInfo", new { id = edu.empno }, edu);
                    }
                }
                return Problem("Company Or Employee Number Empty");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(AddOrEditEDU)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        //Employee Experince Action
        [HttpGet("getExperinceInfoByEmpNo")]
        public async Task<IActionResult> GetEmployeeExperinceInfo(int EmpNo)
        {
            try
            {
                var data = await _globalMaster.experince.GetExpcSingel(EmpNo);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetEmployeeExperinceInfo)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost("AddOrEditExperince")]
        public async Task<IActionResult> AddOrEditExperince(EmpExperince expc)
        {
            try
            {
                if (expc != null)
                {
                    var ChkEmpNo = await _globalMaster.experince.GetFirstOrDefaultAsync(e => e.compid == expc.compid && e.emp_id == expc.emp_id);
                    if (ChkEmpNo == null)
                    {
                        expc.addtime = DateTime.Now;
                        await _globalMaster.experince.AddAsync(expc);
                        return CreatedAtAction("GetEmployeeExperinceInfo", new { id = expc.emp_id }, expc);
                    }
                    else
                    {
                        await _globalMaster.experince.UpdateExpcInfo(expc);
                        return CreatedAtAction("GetEmployeeExperinceInfo", new { id = expc.emp_id }, expc);
                    }
                }
                return Problem("Company Or Employee Number Empty");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(AddOrEditExperince)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        //New Version
        [HttpGet("GetEmpPersonalInfoByEmpNo")]
        public async Task<IActionResult> GetEmpPersonalInfoByEmpNo(int compid, int emp_no)
        {
            try
            {
                var data = await _globalMaster.employees.GetEmpPersonalInfo_ByEmpNo(compid, emp_no);
                if (data != null)
                {
                    return CustomResult("Data Has Been Loaded Successfully !!", data, HttpStatusCode.OK);
                }
                return CustomResult("Data Not Found !!", data, HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetEmpPersonalInfoByEmpNo)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }
        [HttpPost("addOrEditEmpPersonal_Info")]
        public async Task<IActionResult> addOrEditEmpPersonal(Employee_Personal_Info obj)
        {
            try
            {
                var response = await _globalMaster.employees.AddOrEditEmpPersonalInfo(obj);
                if (response.IsSuccess)
                {
                    return Ok(new { isSuccess = response.IsSuccess, statusCode = 200, message = response.Message });
                }
                return BadRequest(new { isSuccess = response.IsSuccess, statusCode = 400, message = response.Message, data = response.dataTable });
                //if (result == "Save Successfully !!" || result == "Update Successfully !!")
                //{
                //    return CustomResult(result, HttpStatusCode.OK);
                //}
                //return CustomResult(result, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(addOrEditEmpPersonal)}");
                return CustomResult("Internal Server Error, Please Try Again Later!",HttpStatusCode.InternalServerError);
            }
        }
        [HttpGet("GetEmpOfficialInfoByEmpSerial")]
        public async Task<IActionResult> GetEmpOfficialInfo_ByEmpSerial(int emp_serial)
        {
            try
            {
                var data = await _globalMaster.employees.GetEmpOfficialInfo_ByEmpSerial(emp_serial);
                if (data != null)
                {
                    return CustomResult("Data Has Been Loaded Successfully !!", data, HttpStatusCode.OK);
                }
                return CustomResult("Data Not Found !!", data, HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetEmpOfficialInfo_ByEmpSerial)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }
        [HttpPost("UpdateEmpOfficial_Info")]
        public async Task<IActionResult> UpdateEmpOfficial(Employee_Office_Info obj)
        {
            try
            {
                var result = await _globalMaster.employees.UpdateEmpOfficialInfo(obj);
                if (result[0].ToString() == "success")
                {
                    await _globalMaster.employees.SaveEmployeeAttenData(obj.emp_serial);
                    return CustomResult(result[1].ToString(), HttpStatusCode.OK);
                }
                //if (result == "Save Successfully !!")
                //{
                //    await _globalMaster.employees.SaveEmployeeAttenData(obj.emp_serial);
                //    return CustomResult(result, HttpStatusCode.OK);
                //}
                return CustomResult(result[1].ToString(), HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(UpdateEmpOfficial)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }
        [HttpGet("GetEmpNomineeInfoByEmpSerial")]
        public async Task<IActionResult> GetEmpNomineeInfoByEmpSerial(int emp_serial)
        {
            try
            {
                var data = await _globalMaster.employees.GetEmpNomineeInfo_ByEmpSerial(emp_serial);
                if (data != null)
                {
                    return CustomResult("Data Has Been Loaded Successfully !!", data, HttpStatusCode.OK);
                }
                return CustomResult("Data Not Found !!", data, HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetEmpNomineeInfoByEmpSerial)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }
        [HttpPost("UpdateEmpNominee_Info")]
        public async Task<IActionResult> UpdateEmpNominee(Employee_Nominee_Info obj)
        {
            try
            {
                string result = await _globalMaster.employees.UpdateEmpNomineeInfo(obj);
                if (result == "Save Successfully !!")
                {
                    return CustomResult(result, HttpStatusCode.OK);
                }
                return CustomResult(result, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(UpdateEmpNominee)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }
        [HttpGet("GetEmpEducationInfoByEmpNo")]
        public async Task<IActionResult> GetEmpEducationInfoByEmpNo(int compid, int emp_no)
        {
            try
            {
                var data = await _globalMaster.employees.GetEmpEducationInfo_ByEmpNo(compid, emp_no);
                if (data != null)
                {
                    return CustomResult("Data Has Been Loaded Successfully !!", data, HttpStatusCode.OK);
                }
                return CustomResult("Data Not Found !!", data, HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetEmpEducationInfoByEmpNo)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }
        [HttpPost("addOrEditEmpEducation_Info")]
        public async Task<IActionResult> addOrEditEmpEducation(Employee_Education_Info obj)
        {
            try
            {
                string result = await _globalMaster.employees.UpdateEmpEducationInfo(obj);
                if (result == "Save Successfully !!")
                {
                    return CustomResult(result, HttpStatusCode.OK);
                }
                return CustomResult(result, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(addOrEditEmpEducation)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }
        [HttpGet("GetEmpExperinceInfoByEmpNo")]
        public async Task<IActionResult> GetEmpExperinceInfoByEmpNo(int compid, int emp_no)
        {
            try
            {
                var data = await _globalMaster.employees.GetEmpExperinceInfo_ByEmpNo(compid, emp_no);
                if (data != null)
                {
                    return CustomResult("Data Has Been Loaded Successfully !!", data, HttpStatusCode.OK);
                }
                return CustomResult("Data Not Found !!", data, HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetEmpExperinceInfoByEmpNo)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }
        [HttpPost("addOrEditEmpExperince_Info")]
        public async Task<IActionResult> addOrEditEmpExperince_Info(Employee_Experince_Info obj)
        {
            try
            {
                string result = await _globalMaster.employees.addOrEditEmpExperince_Info(obj);
                if (result == "Save Successfully !!")
                {
                    return CustomResult(result, HttpStatusCode.OK);
                }
                return CustomResult(result, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(addOrEditEmpExperince_Info)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }
        [HttpGet("GetEmployeeFullName")]
        public async Task<IActionResult> GetEmployeeFullName(int compid, int emp_no)
        {
            try
            {
                var data = await _globalMaster.employees.GetEmployeeFullName(compid, emp_no);
                if (data != null)
                {
                    return CustomResult("Data Has Been Loaded Successfully !!", data, HttpStatusCode.OK);
                }
                return CustomResult("Data Not Found !!", data, HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetEmployeeFullName)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }
        [HttpGet("GetAllDegree_Title")]
        public async Task<IActionResult> GetAllDegree_Title()
        {
            try
            {
                var data = await _globalMaster.employees.GetAllDegree_Title();
                if (data != null)
                {
                    return CustomResult("Data Has Been Loaded Successfully !!", data, HttpStatusCode.OK);
                }
                return CustomResult("Data Not Found !!", data, HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetAllDegree_Title)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }
        [HttpGet("GetAllExam_Board")]
        public async Task<IActionResult> GetAllExam_Board()
        {
            try
            {
                var data = await _globalMaster.employees.GetAllExam_Board();
                if (data != null)
                {
                    return CustomResult("Data Has Been Loaded Successfully !!", data, HttpStatusCode.OK);
                }
                return CustomResult("Data Not Found !!", data, HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetAllExam_Board)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }
        [HttpGet("GetEmployeeNo")]
        public async Task<IActionResult> GetEmployeeNo(int compid)
        {
            try
            {
                var data = await _globalMaster.employees.GetEmployeeNo(compid);
                if (data != null)
                {
                    return CustomResult("Data Has Been Loaded Successfully !!", data, HttpStatusCode.OK);
                }
                return CustomResult("Data Not Found !!", data, HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetEmployeeNo)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }
        [HttpGet("GetEmployeeSingelInfoRDLC")]
        public IActionResult GetEmployeeSingelInfoRDLC(int emp_serial,string userName,string reportType="pdf")
        {
            try
            {
                var reportBytes = _globalMaster.employees.Dg_EmployeeSingleDetailedInformation(emp_serial, userName, reportType);
                if (reportBytes == null)
                {
                    return NotFound();
                }
                return File(reportBytes, MediaTypeNames.Application.Octet, _sqlCommon.GetContentType(reportType));
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost("GetEmployeeInfoFilter")]
        public async Task<IActionResult> GetEmployeeInfoFilter(EmployeeFilterPayload obj)
        {
            try
            {
                var response = await _globalMaster.employees.GetEmployeeInfoFilter(obj);
                if (response.IsSuccess)
                {
                    return Ok(new { response.IsSuccess, response.Message, response.dataTable });
                }
                return NotFound(new { response.IsSuccess, response.Message });
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpGet("GetEmployeeInfo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetEmployeeInfo(int companyID)
        {
            var response = await _globalMaster.employees.GetCompanyWiseEmployee_ForPMS(companyID);
            if (response.IsSuccess == true)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost("GetEmployeeInfoLeaveFilter")]
        public async Task<IActionResult> GetEmployeeInfoLeaveFilter(EmployeeLeaveFilterPayload obj)
        {
            try
            {
                var response = await _globalMaster.employees.GetEmployeeInfoLeaveFilter(obj);
                if (response.IsSuccess)
                {
                    return Ok(new { response.IsSuccess, response.Message, response.dataTable });
                }
                return NotFound(new { response.IsSuccess, response.Message });
            }
            catch (Exception ex)
            {
                ex.ToString();
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
    }
}