using BLL.Interfaces;
using BOL.Models;
using CoreApiResponse;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Payroll_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AllProcessesController : BaseController
    {
        private readonly IGlobalMaster _globalMaster;
        private readonly ILogger<AllProcessesController> _logger;
        public AllProcessesController(IGlobalMaster globalMaster, ILogger<AllProcessesController> logger)
        {
            _globalMaster = globalMaster;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetAllProcesses()
        {
            try
            {
                var data = _globalMaster.allProcess.GetAll().Select(n => AllProcess.DbToCustomModel(n)).ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetAllProcesses)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAllProcess(int id)
        {
            try
            {
                var data = await _globalMaster.allProcess.GetFirstOrDefaultAsync(n => n.ID == id);
                var nData = AllProcess.DbToCustomModel(data);
                return Ok(nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetAllProcess)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAllProcess(int id, AllProcess obj)
        {
            try
            {
                var nData = AllProcess.CustonToDbModel(obj);
                await _globalMaster.allProcess.UpdateAsync(nData);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PutAllProcess)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost]
        public async Task<IActionResult> PostAllProcess(AllProcess obj)
        {
            try
            {
                var nData = AllProcess.CustonToDbModel(obj);
                await _globalMaster.allProcess.AddAsync(nData);
                return CreatedAtAction("GetAllProcess", new { id = nData.ID }, nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PostAllProcess)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAllProcess(int id)
        {
            try
            {
                var obj = await _globalMaster.allProcess.GetFirstOrDefaultAsync(c => c.ID == id);
                if (obj == null)
                {
                    _logger.LogError($"Invalid Delete Attempt In {nameof(DeleteAllProcess)}");
                    return BadRequest("Submitted Data Is Invalid");
                }
                await _globalMaster.allProcess.DeleteAsync(obj);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteAllProcess)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpGet("Deshboard")]
        public async Task<IActionResult> GetDeshboard(int compid)
        {
            try
            {
                var data = await _globalMaster.allProcess.GetDeshboard(compid);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDeshboard)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Attendance_Process")]
        public async Task<IActionResult> GetAttendance_Process(DateTime SDate, int CompID)
        {
            try
            {
                var data = await _globalMaster.allProcess.GetAttendance_Process(SDate, CompID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetAttendance_Process)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpPost("Salary_Process")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSalary_Process(int groupid, int compid, string pDate, string userName)
        {
            try
            {
                var response = await _globalMaster.allProcess.SalaryProcessSave_New(groupid, compid, pDate, userName);
                //var response = _globalMaster.allProcess.SalaryProcessSave(groupid, compid, pDate);
                if (response.IsSuccess)
                {
                    return Ok(new { response.IsSuccess, response.Message });
                }
                return BadRequest(new { response.IsSuccess, response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetSalary_Process)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpPost("Salary_Process_Single")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Salary_Process_Single(SingleSalaryProcess obj)
        {
            try
            {
                var response = await _globalMaster.allProcess.Salary_Process_Single(obj);
                if (response.IsSuccess==true)
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(Salary_Process_Single)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Salary_Confarmations")]
        public async Task<IActionResult> GetSalary_Confarmations(int com_id, int month, int year, string userName)
        {
            try
            {
                var data = await _globalMaster.allProcess.GetSalary_Confarmations(com_id, month, year, userName);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetSalary_Confarmations)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Create_User")]
        public async Task<IActionResult> GetCreate_User(string name, string EmailId, string Password, int Designation,
            DateTime Getdate, int CompId, int Emp_ID, string Active_status, int Emp_serial, int Compliance)
        {
            try
            {
                var data = await _globalMaster.allProcess.GetCreate_User(name, EmailId, Password, Designation, Getdate, CompId, Emp_ID, Active_status, Emp_serial, Compliance);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetCreate_User)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("SearchUserlist")]
        public async Task<IActionResult> GetSearchUserlist()
        {
            try
            {
                var data = await _globalMaster.allProcess.GetSearchUserlist();
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetSearchUserlist)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPut("UpdateSalaryPaymentDate")]
        public async Task<IActionResult> UpdateSalaryPaymentDate(int compid, string pDate, string salMonth)
        {
            try
            {
                string message = await _globalMaster.allProcess.SalaryPaymentDate(compid, pDate, salMonth);
                if (message== "Update Successfully !!")
                {
                    return CustomResult(message, HttpStatusCode.OK);
                }
                return CustomResult(message, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(UpdateSalaryPaymentDate)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost("PostALLProcessClose")]
        public async Task<IActionResult> PostAllProcessClose(int compid, string procsDt, string processTP, string userName)
        {
            var response = await _globalMaster.allProcess.PostAllProcessClose(compid, procsDt, processTP, userName);
            if (response.IsSuccess)
            {
                return Ok(new { response.IsSuccess });
            }
            return BadRequest(new { response.IsSuccess });
        }
        [HttpGet("GetIsOtProcessContinue")]
        public async Task<IActionResult> GetIsOtProcessContinue(int compid, string procsDt, string processTP, string userName)
        {
            var response = await _globalMaster.allProcess.GetIsAllProcessContinue(compid, procsDt, processTP, userName);
            if (response.IsSuccess)
            {
                return Ok(new { response.IsSuccess, response.dataTable });
            }
            return NotFound();
        }
    }
}
