using BLL.Interfaces;
using BOL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Payroll_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecialholidaysController : ControllerBase
    {
        private readonly IGlobalMaster _globalMaster;
        private readonly ILogger<SpecialholidaysController> _logger;
        public SpecialholidaysController(IGlobalMaster globalMaster, ILogger<SpecialholidaysController> logger)
        {
            _globalMaster = globalMaster;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetDgPaySpecialholidays()
        {
            try
            {
                var data = _globalMaster.specialholidays.GetAll().Select(x => DgPaySpecialholiday.DbToCustomModel(x)).ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgPaySpecialholidays)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDgPaySpecialholiday(int id)
        {
            try
            {
                var data = await _globalMaster.specialholidays.GetFirstOrDefaultAsync(x => x.serial == id);
                var nData = DgPaySpecialholiday.DbToCustomModel(data);
                return Ok(nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgPaySpecialholiday)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDgPaySpecialholiday(int id, DgPaySpecialholiday obj)
        {
            try
            {
                var nData = DgPaySpecialholiday.CustomToDbModel(obj);
                await _globalMaster.specialholidays.UpdateAsync(nData);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PutDgPaySpecialholiday)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost]
        public async Task<IActionResult> PostDgPaySpecialholiday(DgPaySpecialholiday obj)
        {
            try
            {
                var nData = DgPaySpecialholiday.CustomToDbModel(obj);
                await _globalMaster.specialholidays.AddAsync(nData);
                return CreatedAtAction("GetDgPaySpecialholiday", new { id = nData.serial }, nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PostDgPaySpecialholiday)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        //New Action 1/19/2024 By Forhad
        [HttpGet("FilterBase_employeelist_forSpecialholiday")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetEmpListForSpecialholiday(int Compid, int Department,
            int section, int Building, int Floor, int Line,
            int Shift, int Grade, int salcat, string formDate, string toDate)
        {
            try
            {
                var data = await _globalMaster.specialholidays.GetEmployeeListForSpecialholiday(Compid, Department, section, Building, Floor, Line, Shift, Grade, salcat, formDate, toDate);
                if (data.Rows.Count > 0)
                {
                    return Ok(data);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetEmpListForSpecialholiday)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpPost("SaveEmployeeSpecialholiday")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult SaveSpecialholiday(EmpWiseSpecialholidayPayload obj)
        {
            try
            {
                var response = _globalMaster.specialholidays.SaveEmpWiseSpecialholiday(obj);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(SaveSpecialholiday)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("GetCreatedSpecialholidays")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCreatedSpecialholidays(int compid, string formDate, string toDate)
        {
            try
            {
                var response = await _globalMaster.specialholidays.GetCreatedSpecialholidays_List(compid, formDate, toDate);
                if (response.IsSuccess == true)
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetCreatedSpecialholidays)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost("DeleteSpecialHoliday")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteSpecialHoliday(DeleteSpecialholidayPayload obj)
        {
            try
            {
                var response = _globalMaster.specialholidays.DeleteEmpWiseSpecialholiday(obj);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteSpecialHoliday)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost("SpecialHolidayProcess")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SpecialHolidayProcess(SpecialholidayProcessPayload obj)
        {
            try
            {
                var response = await _globalMaster.specialholidays.SpecialholidaysProcess(obj);
                if (response.IsSuccess)
                {
                    return Ok(new { response.IsSuccess, response.Message });
                }
                return BadRequest(new { response.IsSuccess, response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(SpecialHolidayProcess)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpPost("SaveHolidayProcess")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult SaveHolidayProcess(HolidayProcessPayload obj)
        {
            try
            {
                var response = _globalMaster.specialholidays.SaveHolidayProcess(obj);
                if (response.IsSuccess)
                {
                    return Ok(new { response.IsSuccess, response.Message });
                }
                return BadRequest(new { response.IsSuccess, response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(SaveHolidayProcess)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpGet("GetEmployeeHolidayBillProcess")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetEmployeeHolidayBillProcess(int companyID, string monthYear)
        {
            try
            {
                var response = await _globalMaster.specialholidays.GetEmployeeHolidayBillProcess(companyID, monthYear);
                if (response.IsSuccess)
                {
                    return Ok(new { response.IsSuccess, response.Message,response.dataTable });
                }
                return NotFound(new { response.IsSuccess, response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetEmployeeHolidayBillProcess)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpPost("DeleteEmployeeHolidayBillProcess")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteEmployeeHolidayBillProcess(HolidayBillProcessDelPayload obj)
        {
            try
            {
                var response = _globalMaster.specialholidays.DeleteEmployeeHolidayBillProcess(obj);
                if (response.IsSuccess)
                {
                    return Ok(new { response.IsSuccess, response.Message });
                }
                return BadRequest(new { response.IsSuccess, response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteEmployeeHolidayBillProcess)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDgPaySpecialholiday(int id)
        {
            try
            {
                var obj = await _globalMaster.specialholidays.GetFirstOrDefaultAsync(x => x.serial == id);
                if (obj == null)
                {
                    _logger.LogError($"Invalid Delete Attempt In {nameof(DeleteDgPaySpecialholiday)}");
                    return BadRequest("Submitted Data Is Invalid");
                }
                await _globalMaster.specialholidays.DeleteAsync(obj);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteDgPaySpecialholiday)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Forhad_Test")]
        public IActionResult forhadTest()
        {
            try
            {
                var data = _globalMaster.specialholidays.test_save();
                return Ok(data);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return BadRequest();
        }
    }
}
