using BLL.Interfaces;
using BOL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Payroll_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DivisionsController : ControllerBase
    {
        private readonly IGlobalMaster _globalMaster;
        private readonly ILogger<DivisionsController> _logger;
        public DivisionsController(IGlobalMaster globalMaster, ILogger<DivisionsController> logger)
        {
            _globalMaster = globalMaster;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetDgPayDivisions()
        {
            try
            {
                var data = _globalMaster.divisions.GetAll().Select(data => DgPayDivision.DbToCustomModel(data)).ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgPayDivisions)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDgPayDivision(int id)
        {
            try
            {
                var data = await _globalMaster.divisions.GetFirstOrDefaultAsync(x => x.div_id == id);
                var nData = DgPayDivision.DbToCustomModel(data);
                return Ok(nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgPayDivision)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDgPayDivision(int id, DgPayDivision obj)
        {
            try
            {
                var nData = DgPayDivision.CustonToDbModel(obj);
                await _globalMaster.divisions.UpdateAsync(nData);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PutDgPayDivision)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost]
        public async Task<IActionResult> PostDgPayDivision(DgPayDivision obj)
        {
            try
            {
                obj.DivGroupid = 11;
                obj.DivUdate = DateTime.Now;
                var nData = DgPayDivision.CustonToDbModel(obj);
                await _globalMaster.divisions.UpdateAsync(nData);
                return CreatedAtAction("GetDgPayDivision", new { id = nData.div_id }, nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PostDgPayDivision)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDgPayDivision(int id)
        {
            try
            {
                var obj = await _globalMaster.divisions.GetFirstOrDefaultAsync(c => c.div_id == id);
                if (obj == null)
                {
                    _logger.LogError($"Invalid Delete Attempt In {nameof(DeleteDgPayDivision)}");
                    return BadRequest("Submitted Data Is Invalid");
                }
                await _globalMaster.divisions.DeleteAsync(obj);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteDgPayDivision)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
    }
}
