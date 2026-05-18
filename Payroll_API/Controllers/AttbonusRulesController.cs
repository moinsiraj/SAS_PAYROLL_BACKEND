using BLL.Interfaces;
using BOL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Payroll_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttbonusRulesController : ControllerBase
    {
        private readonly IGlobalMaster _globalMaster;
        private readonly ILogger<AttbonusRulesController> _logger;
        public AttbonusRulesController(IGlobalMaster globalMaster, ILogger<AttbonusRulesController> logger)
        {
            _globalMaster = globalMaster;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetDgPayAttbonusRules()
        {
            try
            {
                var data = _globalMaster.attbonusRule.GetAll().Select(n => DgPayAttbonusRule.DbToCustomModel(n)).ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgPayAttbonusRules)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDgPayAttbonusRule(int id)
        {
            try
            {
                var data = await _globalMaster.attbonusRule.GetFirstOrDefaultAsync(x => x.atbr_serial == id);
                var nData = DgPayAttbonusRule.DbToCustomModel(data);
                return Ok(nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgPayAttbonusRule)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDgPayAttbonusRule(int id, DgPayAttbonusRule obj)
        {
            try
            {
                var nData = DgPayAttbonusRule.CustonToDbModel(obj);
                await _globalMaster.attbonusRule.UpdateAsync(nData);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PutDgPayAttbonusRule)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost]
        public async Task<IActionResult> PostDgPayAttbonusRule(DgPayAttbonusRule obj)
        {
            try
            {
                var nData = DgPayAttbonusRule.CustonToDbModel(obj);
                await _globalMaster.attbonusRule.AddAsync(nData);
                return CreatedAtAction("GetDgPayAttbonusRule", new { id = nData.atbr_serial }, nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PostDgPayAttbonusRule)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDgPayAttbonusRule(int id)
        {
            try
            {
                var obj = await _globalMaster.attbonusRule.GetFirstOrDefaultAsync(c => c.atbr_serial == id);
                if (obj == null)
                {
                    _logger.LogError($"Invalid Delete Attempt In {nameof(DeleteDgPayAttbonusRule)}");
                    return BadRequest("Submitted Data Is Invalid");
                }
                await _globalMaster.attbonusRule.DeleteAsync(obj);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteDgPayAttbonusRule)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
    }
}
