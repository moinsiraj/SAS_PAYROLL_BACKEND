using BLL.Interfaces;
using BOL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Payroll_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AllowancesdesController : ControllerBase
    {
        private readonly IGlobalMaster _globalMaster;
        private readonly ILogger<AllowancesdesController> _logger;
        public AllowancesdesController(IGlobalMaster globalMaster, ILogger<AllowancesdesController> logger)
        {
            _globalMaster = globalMaster;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetDgPayAllowancesdes()
        {
            try
            {
                var data = _globalMaster.allowancesde.GetAll().Select(x => DgPayAllowancesde.DbToCustomModel(x)).ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgPayAllowancesdes)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDgPayAllowancesde(int id)
        {
            try
            {
                var data = await _globalMaster.allowancesde.GetFirstOrDefaultAsync(x => x.al_code == id);
                var nData = DgPayAllowancesde.DbToCustomModel(data);
                return Ok(nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgPayAllowancesde)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDgPayAllowancesde(int id, DgPayAllowancesde obj)
        {
            try
            {
                var nData = DgPayAllowancesde.CustonToDbModel(obj);
                await _globalMaster.allowancesde.UpdateAsync(nData);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PutDgPayAllowancesde)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost]
        public async Task<IActionResult> PostDgPayAllowancesde(DgPayAllowancesde obj)
        {
            try
            {
                var nData = DgPayAllowancesde.CustonToDbModel(obj);
                await _globalMaster.allowancesde.AddAsync(nData);
                return CreatedAtAction("GetDgPayAllowancesde", new { id = nData.al_code }, nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PostDgPayAllowancesde)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDgPayAllowancesde(int id)
        {
            try
            {
                var obj = _globalMaster.allowancesde.GetFirstOrDefault(c => c.al_code == id);
                if (obj == null)
                {
                    _logger.LogError($"Invalid Delete Attempt In {nameof(DeleteDgPayAllowancesde)}");
                    return BadRequest("Submitted Data Is Invalid");
                }
                await _globalMaster.allowancesde.DeleteAsync(obj);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteDgPayAllowancesde)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
    }
}
