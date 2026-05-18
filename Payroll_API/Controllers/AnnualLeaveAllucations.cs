using BLL.Interfaces;
using BOL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace Payroll_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnnualLeaveAllucations : ControllerBase
    {
        private readonly IGlobalMaster _globalMaster;
        private readonly ILogger<AnnualLeaveAllucations> _logger;
        public AnnualLeaveAllucations(IGlobalMaster globalMaster, ILogger<AnnualLeaveAllucations> logger)
        {
            _globalMaster = globalMaster;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetDgAnnualLeaveAllucations()
        {
            try
            {
                var data = _globalMaster.annualLeaveAllo.GetAll().Select(x => DgAnnualLeaveAllucation.DbToCustomModel(x)).ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgAnnualLeaveAllucations)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDgAnnualLeaveAllucation(int id)
        {
            try
            {
                var data = await _globalMaster.annualLeaveAllo.GetFirstOrDefaultAsync(x => x.serial == id);
                var nData = DgAnnualLeaveAllucation.DbToCustomModel(data);
                return Ok(nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgAnnualLeaveAllucation)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDgAnnualLeaveAllucation(int id, DgAnnualLeaveAllucation obj)
        {
            try
            {
                var nData = DgAnnualLeaveAllucation.CustonToDbModel(obj);
                await _globalMaster.annualLeaveAllo.UpdateAsync(nData);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PutDgAnnualLeaveAllucation)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost]
        public async Task<IActionResult> PostDgAnnualLeaveAllucation(DgAnnualLeaveAllucation obj)
        {
            try
            {
                var nData = DgAnnualLeaveAllucation.CustonToDbModel(obj);
                await _globalMaster.annualLeaveAllo.AddAsync(nData);
                return CreatedAtAction("GetDgAnnualLeaveAllucation", new { id = nData.serial }, nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PostDgAnnualLeaveAllucation)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDgAnnualLeaveAllucation(int id)
        {
            try
            {
                var obj = await _globalMaster.annualLeaveAllo.GetFirstOrDefaultAsync(c => c.serial == id);
                if (obj == null)
                {
                    _logger.LogError($"Invalid Delete Attempt In {nameof(DeleteDgAnnualLeaveAllucation)}");
                    return BadRequest("Submitted Data Is Invalid");
                }
                await _globalMaster.annualLeaveAllo.DeleteAsync(obj);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteDgAnnualLeaveAllucation)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("get-AnnualLeave_process")]
        public async Task<IActionResult> GetAnnualLeave_process(int year, int casual, int medical, int annul, int comID)
        {
            try
            {
                var data = await _globalMaster.annualLeaveAllo.GetAnnualLeave_process(year, casual, medical, annul, comID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetAnnualLeave_process)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        //New Create 3/15/2025
        [HttpGet("GetAnnualLeaveEmployee")]
        public async Task<IActionResult> GetAnnualLeaveEmployee(int compid, int? deptid = null, int? secid = null)
        {
            try
            {
                var response = await _globalMaster.annualLeaveAllo.GetAnnualLeaveEmployee(compid, deptid, secid);
                if (response.IsSuccess)
                {
                    return Ok(new { response.IsSuccess, response.dataTable });
                }
                return NotFound(new { response.IsSuccess, response.Message, response.dataTable });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetAnnualLeaveEmployee)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost("AnnualLeave_process_save")]
        public async Task<IActionResult> AnnualLeave_process_save(AnnualLeave obj)
        {
            try
            {
                var result = await _globalMaster.annualLeaveAllo.AnnualLeave_process_save(obj);
                if (result.Count > 0)
                {
                    return Ok(result.OrderBy(o => o.IsSuccess));
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(AnnualLeave_process_save)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
    }
}
