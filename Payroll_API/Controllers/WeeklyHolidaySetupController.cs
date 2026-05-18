using BLL.Interfaces;
using BOL.Models;
using CoreApiResponse;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Payroll_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeeklyHolidaySetupController : BaseController
    {
        private readonly IGlobalMaster _globalMaster;
        private readonly ILogger<WeeklyHolidaySetupController> _logger;
        public WeeklyHolidaySetupController(IGlobalMaster globalMaster, ILogger<WeeklyHolidaySetupController> logger)
        {
            _globalMaster = globalMaster;
            _logger = logger;

        }

        [HttpPost("SaveWeeklyHolidaySetup")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult SaveWeeklyHolidaySetup(WeeklyHolidayPayload obj)
        {
            try
            {
                var response = _globalMaster.weeklyHolidaySetup.SaveWeeklyHolidaySetup(obj);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(SaveWeeklyHolidaySetup)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("GetEmployeeHoliDay")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetEmployeeHoliDay(string monthYear, int empSerial)
        {
            try
            {
                var response = await _globalMaster.weeklyHolidaySetup.GetWeeklyHoliday(monthYear, empSerial);
                if (response.IsSuccess==true)
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(SaveWeeklyHolidaySetup)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }
    }
}
