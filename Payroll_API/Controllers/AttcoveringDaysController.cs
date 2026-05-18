using BLL.Interfaces;
using BOL.Models;
using DAL.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using Microsoft.VisualBasic;
using System.Xml.Linq;

namespace Payroll_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttcoveringDaysController : ControllerBase
    {
        private readonly IGlobalMaster _globalMaster;
        private readonly ILogger<AttcoveringDaysController> _logger;
        public AttcoveringDaysController(IGlobalMaster globalMaster, ILogger<AttcoveringDaysController> logger)
        {
            _globalMaster = globalMaster;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetDgPayAttcoveringDays()
        {
            try
            {
                var data = _globalMaster.attcoveringDay.GetAll().Select(n => DgPayAttcoveringDay.DbToCustomModel(n)).ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgPayAttcoveringDays)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDgPayAttcoveringDay(int id)
        {
            try
            {
                var data = await _globalMaster.attcoveringDay.GetFirstOrDefaultAsync(x => x.cd_serial == id);
                var nData = DgPayAttcoveringDay.DbToCustomModel(data);
                return Ok(nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgPayAttcoveringDay)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDgPayAttcoveringDay(DgPayAttcoveringDay obj)
        {
            try
            {
                var nData = DgPayAttcoveringDay.CustonToDbModel(obj);
                await _globalMaster.attcoveringDay.UpdateAsync(nData);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PutDgPayAttcoveringDay)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost]
        public async Task<IActionResult> PostDgPayAttcoveringDay(DgPayAttcoveringDay obj)
        {
            try
            {
                var nData = DgPayAttcoveringDay.CustonToDbModel(obj);
                await _globalMaster.attcoveringDay.AddAsync(nData);
                return CreatedAtAction("GetDgPayAttcoveringDay", new { id = nData.cd_serial, }, nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PostDgPayAttcoveringDay)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDgPayAttcoveringDay(int id)
        {
            try
            {
                var obj = await _globalMaster.attcoveringDay.GetFirstOrDefaultAsync(c => c.cd_serial == id);
                if (obj == null)
                {
                    _logger.LogError($"Invalid Delete Attempt In {nameof(DeleteDgPayAttcoveringDay)}");
                    return BadRequest("Submitted Data Is Invalid");
                }
                await _globalMaster.attcoveringDay.DeleteAsync(obj);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteDgPayAttcoveringDay)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        //New Action 2-7-2024 By forhad
        [HttpGet("FilterBase_employeelist_forCoverdingday")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetEmpListForCoverdingDay(int Compid, int Department,
            int section, int Building, int Floor, int Line,
            int Shift, int Grade, int salcat, string formDate, string toDate)
        {
            try
            {
                var data = await _globalMaster.attcoveringDay.GetEmployeeListForCoverday(Compid, Department, section, Building, Floor, Line, Shift, Grade, salcat, formDate, toDate);
                if (data.Rows.Count > 0)
                {
                    return Ok(data);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetEmpListForCoverdingDay)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpPost("SaveCoverdingDayEmpWise")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult SaveCoverdingDayEmpWise(EmpWiseCoveringdayPayload obj)
        {
            try
            {
                var response = _globalMaster.attcoveringDay.SaveEmpWiseCoveringDay(obj);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(SaveCoverdingDayEmpWise)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpGet("GetCreatedCoveringdays")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCreatedCoveringdays(int compid, string formDate, string toDate)
        {
            try
            {
                var response = await _globalMaster.attcoveringDay.GetCreatedCoverdingDays_List(compid, formDate, toDate);
                if (response.IsSuccess == true)
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetCreatedCoveringdays)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpPost("DeleteCoverdingDayEmpWise")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteCoverdingDayEmpWise(DeleteCoveringdayPayload obj)
        {
            try
            {
                var response = _globalMaster.attcoveringDay.DeleteEmpWiseSpecialholiday(obj);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteCoverdingDayEmpWise)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
    }
}
