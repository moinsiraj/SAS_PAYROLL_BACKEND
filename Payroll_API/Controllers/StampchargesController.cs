using BLL.Interfaces;
using BOL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Payroll_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StampchargesController : ControllerBase
    {
        private readonly IGlobalMaster _globalMaster;
        private readonly ILogger<StampchargesController> _logger;
        public StampchargesController(IGlobalMaster globalMaster, ILogger<StampchargesController> logger)
        {
            _globalMaster = globalMaster;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetDgStampcharges()
        {
            try
            {
                var data = _globalMaster.stampcharges.GetAll().Select(x => DgStampcharge.DbToCustomModel(x)).ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgStampcharges)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDgStampcharge(int id)
        {
            try
            {
                var data = await _globalMaster.stampcharges.GetFirstOrDefaultAsync(x => x.Id == id);
                var nData = DgStampcharge.DbToCustomModel(data);
                return Ok(nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgStampcharge)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDgStampcharge(int id, DgStampcharge obj)
        {
            try
            {
                var nData = DgStampcharge.CustomToDbModel(obj);
                await _globalMaster.stampcharges.UpdateAsync(nData);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PutDgStampcharge)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost]
        public async Task<IActionResult> PostDgStampcharge(DgStampcharge obj)
        {
            try
            {
                var nData = DgStampcharge.CustomToDbModel(obj);
                await _globalMaster.stampcharges.AddAsync(nData);
                return CreatedAtAction("GetDgStampcharge", new { id = nData.Id }, nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PostDgStampcharge)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDgStampcharge(int id)
        {
            try
            {
                var obj = await _globalMaster.stampcharges.GetFirstOrDefaultAsync(x => x.Id == id);
                if (obj == null)
                {
                    _logger.LogError($"Invalid Delete Attempt In {nameof(DeleteDgStampcharge)}");
                    return BadRequest("Submitted Data Is Invalid");
                }
                await _globalMaster.stampcharges.DeleteAsync(obj);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteDgStampcharge)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
    }
}
