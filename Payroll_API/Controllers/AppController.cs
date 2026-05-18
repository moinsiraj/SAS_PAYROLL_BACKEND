using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Payroll_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppController : ControllerBase
    {
        private readonly IGlobalMaster _globalMaster;
        private readonly ILogger<AppController> _logger;

        public AppController(IGlobalMaster globalMaster, ILogger<AppController> logger)
        {
            _globalMaster = globalMaster;
            _logger = logger;
        }

        [HttpGet("AppDeshboard")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDeshboard(int compid)
        {
            try
            {
                var response = await _globalMaster.appManager.GetAppDeshboard(compid);
                if (response.IsSuccess)
                {
                    return Ok(new { isSuccess = response.IsSuccess, statusCode = 200, data = response.dataTable });
                }
                return NotFound(new { isSuccess = response.IsSuccess, statusCode = 404, message = response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDeshboard)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpGet("AppHourlyManpower")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAppHourlyManpower(string compid, string fromdate, string todate)
        {
            try
            {
                var response = await _globalMaster.appManager.GetAppHourlyManpower(compid, fromdate, todate);
                if (response.IsSuccess)
                {
                    return Ok(new { isSuccess = response.IsSuccess, statusCode = 200, data = response.dataTable });
                }
                return NotFound(new { isSuccess = response.IsSuccess, statusCode = 404, message = response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDeshboard)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpGet("AppHourlyManpower_sectionwise")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAppHourlyManpower_sectionwise(int compid, string fromdate, string todate)
        {
            try
            {
                var response = await _globalMaster.appManager.GetAppHourlyManpower_sectionwise(compid, fromdate, todate);
                if (response.IsSuccess)
                {
                    return Ok(new { isSuccess = response.IsSuccess, statusCode = 200, data = response.dataTable });
                }
                return NotFound(new { isSuccess = response.IsSuccess, statusCode = 404, message = response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDeshboard)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpGet("AppHourlyManpower_sectionwise_emplist")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAppHour_sectionwise_lyManpower_list(int compid, string fromdate, string todate, int section, int eight, int eight_pointfive,
            int nine, int nine_pointfive, int ten, int ten_pointfive, int eliven, int eliven_pointfive, int twelve, int AfterTwelve)
        {
            try
            {
                var response = await _globalMaster.appManager.GetAppHour_sectionwise_lyManpower_list(compid, fromdate, todate, section, eight, eight_pointfive, nine, nine_pointfive, ten, ten_pointfive, eliven, eliven_pointfive, twelve, AfterTwelve);
                if (response.IsSuccess)
                {
                    return Ok(new { isSuccess = response.IsSuccess, statusCode = 200, data = response.dataTable });
                }
                return NotFound(new { isSuccess = response.IsSuccess, statusCode = 404, message = response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDeshboard)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

    }
}
