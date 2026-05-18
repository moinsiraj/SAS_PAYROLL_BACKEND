using BLL.Interfaces;
using BOL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Payroll_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeductionController : ControllerBase
    {
        private readonly IGlobalMaster _globalMaster;
        private readonly ILogger<DeductionController> _logger;
        public DeductionController(IGlobalMaster globalMaster, ILogger<DeductionController> logger)
        {
            _globalMaster = globalMaster;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetDgPayDeductionsdes()
        {
            try
            {
                var data = _globalMaster.deduction.GetAll().Select(data => DgPayDeductionsde.DbToCustomModel(data)).ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgPayDeductionsdes)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDgPayDeductionsde(int id)
        {
            try
            {
                var data = await _globalMaster.deduction.GetFirstOrDefaultAsync(x => x.d_code == id);
                var nData = DgPayDeductionsde.DbToCustomModel(data);
                return Ok(nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgPayDeductionsde)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDgPayDeductionsde(int id, DgPayDeductionsde obj)
        {
            try
            {
                var nData = DgPayDeductionsde.CustonToDbModel(obj);
                await _globalMaster.deduction.UpdateAsync(nData);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PutDgPayDeductionsde)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost]
        public async Task<IActionResult> PostDgPayDeductionsde(DgPayDeductionsde obj)
        {
            try
            {
                var nData = DgPayDeductionsde.CustonToDbModel(obj);
                await _globalMaster.deduction.AddAsync(nData);
                return CreatedAtAction("GetDgPayDeductionsde", new { id = nData.d_code }, nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PostDgPayDeductionsde)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDgPayDeductionsde(int id)
        {
            try
            {
                var obj = await _globalMaster.deduction.GetFirstOrDefaultAsync(c => c.d_code == id);
                if (obj == null)
                {
                    _logger.LogError($"Invalid Delete Attempt In {nameof(DeleteDgPayDeductionsde)}");
                    return BadRequest("Submitted Data Is Invalid");
                }
                await _globalMaster.deduction.DeleteAsync(obj);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteDgPayDeductionsde)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
    }
}
