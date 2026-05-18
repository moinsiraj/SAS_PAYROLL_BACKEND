using BLL.Interfaces;
using BOL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Payroll_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadAttendancesController : ControllerBase
    {
        private readonly IGlobalMaster _globalMaster;
        private readonly ILogger<UploadAttendancesController> _logger;
        public UploadAttendancesController(IGlobalMaster globalMaster, ILogger<UploadAttendancesController> logger)
        {
            _globalMaster = globalMaster;
            _logger = logger;
        }

        [HttpPost("UploadFile3")]
        public async Task<IActionResult> UploadFile3([FromForm] UploadFile model)
        {
            try
            {
                var result = await _globalMaster.uploadAttendances.ReadAttnTextFile_New3(model);
                if (result.Count > 0)
                {
                    return Ok(result);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(UploadFile3)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpPost("UploadFileEmpWise")]
        public async Task<IActionResult> ReadAttnTextFileEmployeeWise([FromForm] UploadFileEmpWise model)
        {
            try
            {
                var result = await _globalMaster.uploadAttendances.ReadAttnTextFileEmployeeWise_new(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(ReadAttnTextFileEmployeeWise)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpGet("TestString")]
        public IActionResult TestString(string testString)
        {
            string sValue = testString.Reverse().ToString();
            var alt_right6 = new string(testString.Reverse().Take(14).Reverse().ToArray());
            string tst = testString.Substring(0, testString.Length - alt_right6.Length);
            var newArr = new string[] { alt_right6, tst };
            string newString = string.Concat(newArr);
            return Ok(newString);
        }

        [HttpGet("GetAttendanceOtProcEmp")]
        public async Task<IActionResult> GetAttendanceOtProcEmp(int compid, string date, int? deptid=null, int? secid=null)
        {
            try
            {
                var response = await _globalMaster.uploadAttendances.GetAttendanceOtProcEmp(compid, date, deptid, secid);
                if (response.IsSuccess)
                {
                    return Ok(new { response.IsSuccess, response.dataTable });
                }
                return NotFound(new { response.IsSuccess, response.Message, response.dataTable });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetAttendanceOtProcEmp)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpPost("SetAttendanceOtProcess")]
        public async Task<IActionResult> SetAttendanceOtProcess(AttendanceOtProcess obj)
        {
            try
            {
                var result = await _globalMaster.uploadAttendances.SetAttendanceOtProcess(obj);
                if (result.Count > 0)
                {
                    return Ok(result);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(SetAttendanceOtProcess)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpGet("GetTextFileFormatInfo")]
        public async Task<IActionResult> GetTextFileFormatInfo(int compid)
        {
            try
            {
                var response = await _globalMaster.uploadAttendances.GetTextFileFormatInfo(compid);
                if (response.IsSuccess)
                {
                    return Ok(new { response.IsSuccess, response.dataTable });
                }
                return NotFound(new { response.IsSuccess, response.Message, response.dataTable });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetTextFileFormatInfo)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
    }
}