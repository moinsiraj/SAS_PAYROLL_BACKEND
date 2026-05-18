using BLL.Interfaces;
using BOL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Payroll_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaternityLeaveInfoesController : ControllerBase
    {
        private readonly IGlobalMaster _globalMaster;
        private readonly ILogger<MaternityLeaveInfoesController> _logger;
        public MaternityLeaveInfoesController(IGlobalMaster globalMaster, ILogger<MaternityLeaveInfoesController> logger)
        {
            _globalMaster = globalMaster;
            _logger = logger;
        }

        [HttpGet("get-MaternityLeaveInfo")]
        public async Task<IActionResult> GetMaternityLeaveInfo(int CompID, DateTime Startdate, DateTime Enddate)
        {
            try
            {
                var data = await _globalMaster.maternityLeaveInfo.GetMaternityLeaveInfo(CompID, Startdate, Enddate);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetMaternityLeaveInfo)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet]
        public IActionResult GetDgPayMaternityLeaveInfos()
        {
            try
            {
                var data = _globalMaster.maternityLeaveInfo.GetAll().Select(x => DgPayMaternityLeaveInfo.DbToCustomModel(x)).ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgPayMaternityLeaveInfos)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDgPayMaternityLeaveInfo(int id)
        {
            try
            {
                var data = await _globalMaster.maternityLeaveInfo.GetFirstOrDefaultAsync(x => x.serialNo == id);
                var nData = DgPayMaternityLeaveInfo.DbToCustomModel(data);
                return Ok(nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgPayMaternityLeaveInfo)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDgPayMaternityLeaveInfo(int id, DgPayMaternityLeaveInfo obj)
        {
            try
            {
                var nData = DgPayMaternityLeaveInfo.CustomToDbModel(obj);
                await _globalMaster.maternityLeaveInfo.UpdateAsync(nData);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PutDgPayMaternityLeaveInfo)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }       
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDgPayMaternityLeaveInfo(int id)
        {
            try
            {
                var obj = await _globalMaster.maternityLeaveInfo.GetFirstOrDefaultAsync(c => c.serialNo == id);
                if (obj == null)
                {
                    _logger.LogError($"Invalid Delete Attempt In {nameof(DeleteDgPayMaternityLeaveInfo)}");
                    return BadRequest("Submitted Data Is Invalid");
                }
                await _globalMaster.maternityLeaveInfo.DeleteAsync(obj);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteDgPayMaternityLeaveInfo)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        //New Action 2/03/2024 By Forhad
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostDgPayMaternityLeaveInfo(DgPayMaternityLeaveInfo obj)
        {
            try
            {
                var response = await _globalMaster.maternityLeaveInfo.SaveMaternityLeaveInfo(obj);
                if (response.IsSuccess == true)
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PostDgPayMaternityLeaveInfo)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("GetMaternityLeavePayment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMaternityLeavePayment(int companyID, string formDate, string toDate)
        {
            try
            {
                var response = await _globalMaster.maternityLeaveInfo.GetMaternityLeaveEmpByCompany(companyID, formDate, toDate);
                if (response.IsSuccess == true)
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetMaternityLeavePayment)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost("UpdateMaternityLeave_NotPayment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateMaternityLeave_NotPayment(MaternityPaymentModification obj)
        {
            try
            {
                var response = _globalMaster.maternityLeaveInfo.UpdateArrowRight(obj);
                if (response.IsSuccess == true)
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(UpdateMaternityLeave_NotPayment)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost("UpdateMaternityLeave_Payment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateMaternityLeave_Payment(MaternityPaymentModification obj)
        {
            try
            {
                var response = _globalMaster.maternityLeaveInfo.UpdateArrowLeft(obj);
                if (response.IsSuccess == true)
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(UpdateMaternityLeave_Payment)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
    }
}
