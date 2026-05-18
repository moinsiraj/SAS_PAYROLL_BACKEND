using BLL.Interfaces;
using BOL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Payroll_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LunchInoutSetupsController : ControllerBase
    {
        private readonly IGlobalMaster _globalMaster;
        private readonly ILogger<LunchInoutSetupsController> _logger;
        public LunchInoutSetupsController(IGlobalMaster globalMaster, ILogger<LunchInoutSetupsController> logger)
        {
            _globalMaster = globalMaster;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetDgLunchInoutSetups()
        {
            try
            {
                var data = _globalMaster.lunchInoutSetups.GetAll().Select(x => DgLunchInoutSetup.DbToCustomModel(x)).ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgLunchInoutSetups)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDgLunchInoutSetup(int id)
        {
            try
            {
                var data = await _globalMaster.lunchInoutSetups.GetFirstOrDefaultAsync(x => x.l_serial == id);
                var nData = DgLunchInoutSetup.DbToCustomModel(data);
                return Ok(nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgLunchInoutSetup)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDgLunchInoutSetup(int id, DgLunchInoutSetup obj)
        {
            try
            {
                var nData = DgLunchInoutSetup.CustomToDbModel(obj);
                await _globalMaster.lunchInoutSetups.UpdateAsync(nData);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PutDgLunchInoutSetup)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost]
        public async Task<IActionResult> PostDgLunchInoutSetup(DgLunchInoutSetup obj)
        {
            try
            {
                var nData = DgLunchInoutSetup.CustomToDbModel(obj);
                await _globalMaster.lunchInoutSetups.AddAsync(nData);
                return CreatedAtAction("GetDgLunchInoutSetup", new { id = nData.l_serial }, nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PostDgLunchInoutSetup)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDgLunchInoutSetup(int id)
        {
            try
            {
                var obj = await _globalMaster.lunchInoutSetups.GetFirstOrDefaultAsync(c => c.l_serial == id);
                if (obj == null)
                {
                    _logger.LogError($"Invalid Delete Attempt In {nameof(DeleteDgLunchInoutSetup)}");
                    return BadRequest("Submitted Data Is Invalid");
                }
                await _globalMaster.lunchInoutSetups.DeleteAsync(obj);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteDgLunchInoutSetup)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        //New Action 2/27/2024 by Forhad
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("SaveEmployeeLunchOutDate")]
        public IActionResult SaveLunchOutDate(EmployeeLunchOutPayload obj)
        {
            try
            {
                var response = _globalMaster.lunchInoutSetups.SaveEmployeeLunchOutHistory(obj);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(SaveLunchOutDate)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost("GetEmployeeInfoForLunchOut")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetEmployeeInfoForLunchOut(LunchOutEmployeeInfo obj)
        {
            try
            {
                var response = await _globalMaster.lunchInoutSetups.GetEmployeeInfoForLunchOut(obj);
                if (response.IsSuccess)
                {
                    return Ok(new{response.IsSuccess,response.Message,response.dataTable});
                }
                return NotFound(new { response.IsSuccess, response.Message, response.dataTable });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetEmployeeInfoForLunchOut)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("DeleteEmployeeLunchOut")]
        public IActionResult DeleteEmployeeLunchOut(EmployeeLunchOutPayload obj)
        {
            try
            {
                var response = _globalMaster.lunchInoutSetups.DeleteEmployeeLunchOut(obj);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteEmployeeLunchOut)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
    }
}
