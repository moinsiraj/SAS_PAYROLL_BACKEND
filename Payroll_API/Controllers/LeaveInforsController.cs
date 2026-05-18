using BLL.Interfaces;
using BOL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Payroll_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveInforsController : ControllerBase
    {
        private readonly IGlobalMaster _globalMaster;
        private readonly ILogger<LeaveInforsController> _logger;
        public LeaveInforsController(IGlobalMaster globalMaster, ILogger<LeaveInforsController> logger)
        {
            _globalMaster = globalMaster;
            _logger = logger;
        }

        [HttpGet("get-LeaveInfo")]
        public async Task<IActionResult> GetCompanyName(int CompID, int EmpNO, int year)
        {
            try
            {
                var data = await _globalMaster.leaveInfor.GetCompanyName(CompID, EmpNO, year);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetCompanyName)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Get_LeaveBalanceInfo")]
        public async Task<IActionResult> GetLeaveBalanceInfo(int EmpSerial, int year)
        {
            try
            {
                var data = await _globalMaster.leaveInfor.GetLeaveBalanceInfo(EmpSerial, year);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetLeaveBalanceInfo)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Get_LeaveTransInfo")]
        public ActionResult LeaveTransInfo(int EmpSerial, int year)
        {
            try
            {
                var data = _globalMaster.levTransFdtTdt.GetAll().Where(x => x.lev_emp_serial == EmpSerial && x.lev_from_date.Year == year).ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(LeaveTransInfo)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet]
        public IActionResult GetDgPayLeaveInfors()
        {
            try
            {
                var data = _globalMaster.leaveInfor.GetAll().Select(x => DgPayLeaveInfor.DbToCustomModel(x)).ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgPayLeaveInfors)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDgPayLeaveInfor(int id)
        {
            try
            {
                var data = await _globalMaster.leaveInfor.GetFirstOrDefaultAsync(x => x.lev_serial == id);
                var nData = DgPayLeaveInfor.DbToCustomModel(data);
                return Ok(nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgPayLeaveInfor)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDgPayLeaveInfor(int id, DgPayLeaveInfor obj)
        {
            try
            {
                var nData = DgPayLeaveInfor.CustomToDbModel(obj);
                await _globalMaster.leaveInfor.UpdateAsync(nData);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PutDgPayLeaveInfor)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost]
        public async Task<IActionResult> PostDgPayLeaveInfor(DgPayLeaveInfor obj)
        {
            try
            {
                var nData = DgPayLeaveInfor.CustomToDbModel(obj);
                await _globalMaster.leaveInfor.AddAsync(nData);
                return CreatedAtAction("GetDgPayLeaveInfor", new { id = nData.lev_serial }, nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PostDgPayLeaveInfor)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDgPayLeaveInfor(int id)
        {
            try
            {
                var obj = await _globalMaster.leaveInfor.GetFirstOrDefaultAsync(c => c.lev_serial == id);
                if (obj == null)
                {
                    _logger.LogError($"Invalid Delete Attempt In {nameof(DeleteDgPayLeaveInfor)}");
                    return BadRequest("Submitted Data Is Invalid");
                }
                await _globalMaster.leaveInfor.DeleteAsync(obj);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteDgPayLeaveInfor)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost("UpdateLeaveInfo")]
        public async Task<IActionResult> UpdateLeaveInfo(int empSl, decimal anualBal)
        {
            try
            {
                var response = await _globalMaster.leaveInfor.UpdateLeaveInfo(empSl, anualBal);
                if (response.IsSuccess)
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteDgPayLeaveInfor)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
    }
}
