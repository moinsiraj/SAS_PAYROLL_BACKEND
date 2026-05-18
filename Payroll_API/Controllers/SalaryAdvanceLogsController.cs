using BLL.Interfaces;
using BOL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Payroll_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalaryAdvanceLogsController : ControllerBase
    {
        private readonly IGlobalMaster _globalMaster;
        private readonly ILogger<SalaryAdvanceLogsController> _logger;
        public SalaryAdvanceLogsController(IGlobalMaster globalMaster, ILogger<SalaryAdvanceLogsController> logger)
        {
            _globalMaster = globalMaster;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetDgPaySalaryAdvanceLogs()
        {
            try
            {
                var data = _globalMaster.salaryAdvanceLogs.GetAll().Select(x => DgPaySalaryAdvanceLog.DbToCustomModel(x)).ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgPaySalaryAdvanceLogs)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDgPaySalaryAdvanceLog(int id)
        {
            try
            {
                var data = await _globalMaster.salaryAdvanceLogs.GetFirstOrDefaultAsync(x => x.serial == id);
                var nData = DgPaySalaryAdvanceLog.DbToCustomModel(data);
                return Ok(nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgPaySalaryAdvanceLog)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDgPaySalaryAdvanceLog(int id, DgPaySalaryAdvanceLog obj)
        {
            try
            {
                var nData = DgPaySalaryAdvanceLog.CustomToDbModel(obj);
                await _globalMaster.salaryAdvanceLogs.UpdateAsync(nData);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PutDgPaySalaryAdvanceLog)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost]
        public async Task<IActionResult> PostDgPaySalaryAdvanceLog(DgPaySalaryAdvanceLog obj)
        {
            try
            {
                //var nData = DgPaySalaryAdvanceLog.CustomToDbModel(obj);
                var response = await _globalMaster.salaryAdvanceLogs.SaveAdvanceProcess(obj);
                if (response.IsSuccess)
                {
                    return Ok(new { response.IsSuccess, response.Message });
                }
                return BadRequest(new { response.IsSuccess, response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PostDgPaySalaryAdvanceLog)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDgPaySalaryAdvanceLog(int id)
        {
            try
            {
                var obj = await _globalMaster.salaryAdvanceLogs.GetFirstOrDefaultAsync(x => x.serial == id);
                if (obj == null)
                {
                    _logger.LogError($"Invalid Delete Attempt In {nameof(DeleteDgPaySalaryAdvanceLog)}");
                    return BadRequest("Submitted Data Is Invalid");
                }
                await _globalMaster.salaryAdvanceLogs.DeleteAsync(obj);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteDgPaySalaryAdvanceLog)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("get-AdvanceProcess")]
        public async Task<IActionResult> AdvanceProcess(int SAMonth, int SAYear, int sp_groupid, int sp_compid, int days)
        {
            try
            {
                var data = await _globalMaster.salaryAdvanceLogs.AdvanceProcess(SAMonth, SAYear, sp_groupid, sp_compid, days);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(AdvanceProcess)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("get-AdvanceScearch")]
        public async Task<IActionResult> AdvanceProcess(int CompId, int month, int year)
        {
            try
            {
                var data = await _globalMaster.salaryAdvanceLogs.AdvanceProcess(CompId, month, year);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(AdvanceProcess)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("get-AdvanceProcessSum")]
        public async Task<IActionResult> AdvanceProcessSum(int CompId, int month, int year)
        {
            try
            {
                var data = await _globalMaster.salaryAdvanceLogs.AdvanceProcessSum(CompId, month, year);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(AdvanceProcessSum)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost("Save_SalaryAdvanceConfirm")]
        public async Task<IActionResult> Save_SalaryAdvanceConfirm(DgPaySalaryAdvanceLog obj)
        {
            try
            {
                var response = await _globalMaster.salaryAdvanceLogs.SaveAdvanceConfirmed(obj);
                if (response.IsSuccess)
                {
                    return Ok(new { response.IsSuccess, response.Message });
                }
                return BadRequest(new { response.IsSuccess, response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(AdvanceProcessSum)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost("Salary_Advance_PaymentDate")]
        public async Task<IActionResult> Salary_Advance_PaymentDate(PaySalaryAdvancePayment obj)
        {
            try
            {
                var response = await _globalMaster.salaryAdvanceLogs.Salary_Advance_PaymentDate(obj);
                if (response.IsSuccess)
                {
                    return Ok(new { response.IsSuccess, response.Message });
                }
                return BadRequest(new { response.IsSuccess, response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(Salary_Advance_PaymentDate)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
    }
}
