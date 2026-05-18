using BLL.Interfaces;
using BOL.Models;
using CoreApiResponse;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Payroll_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DgPayDeducationsController : BaseController
    {
        private readonly IGlobalMaster _globalMaster;
        private readonly ILogger<DgPayDeducationsController> _logger;
        public DgPayDeducationsController(IGlobalMaster globalMaster, ILogger<DgPayDeducationsController> logger)
        {
            _globalMaster = globalMaster;
            _logger = logger;
        }

        [HttpGet("get-DeductionType")]
        public async Task<IActionResult> GetDeductionType(string DeductionType)
        {
            try
            {
                var data = await _globalMaster.dgPayDeducations.GetDeductionType(DeductionType);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDeductionType)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("get-EmpwiseDeduction")]
        public async Task<IActionResult> GetEmpwiseDeduction(string al_emp_serial, string dDate)
        {
            try
            {
                var data = await _globalMaster.dgPayDeducations.GetEmpwiseDeduction(al_emp_serial, dDate);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetEmpwiseDeduction)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("get-DeductionDatewise")]
        public async Task<IActionResult> GetDeductionDatewise(int CompID, string DeductionType, DateTime StartDate, DateTime EndDate)
        {
            try
            {
                var data = await _globalMaster.dgPayDeducations.GetDeductionDatewise(CompID, DeductionType, StartDate, EndDate);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDeductionDatewise)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet]
        public IActionResult GetDgPayDeducations()
        {
            try
            {
                var data = _globalMaster.dgPayDeducations.GetAll().Select(data => DgPayDeducation.DbToCustomModel(data)).ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgPayDeducations)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDgPayDeducation(int id)
        {
            try
            {
                var data = await _globalMaster.dgPayDeducations.GetFirstOrDefaultAsync(x => x.d_serial == id);
                var nData = DgPayDeducation.DbToCustomModel(data);
                return Ok(nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgPayDeducation)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDgPayDeducation(int id, DgPayDeducation obj)
        {
            try
            {
                var nData = DgPayDeducation.CustonToDbModel(obj);
                await _globalMaster.dgPayDeducations.UpdateAsync(nData);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PutDgPayDeducation)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostDgPayDeducation(DgPayDeducationPostPayload obj)
        {
            try
            {
                var response = await _globalMaster.dgPayDeducations.SaveEmpDeducation(obj);
                if (response.IsSuccess==true)
                {
                    return CustomResult(response.Message, HttpStatusCode.OK);
                }
                return CustomResult(response.Message, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PostDgPayDeducation)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDgPayDeducation(int id)
        {
            try
            {
                var obj = await _globalMaster.dgPayDeducations.GetFirstOrDefaultAsync(c => c.d_serial == id);
                if (obj == null)
                {
                    _logger.LogError($"Invalid Delete Attempt In {nameof(DeleteDgPayDeducation)}");
                    return BadRequest("Submitted Data Is Invalid");
                }
                await _globalMaster.dgPayDeducations.DeleteAsync(obj);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteDgPayDeducation)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
    }
}
