using BLL.Interfaces;
using BOL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Payroll_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThanasController : ControllerBase
    {
        private readonly IGlobalMaster _globalMaster;
        private readonly ILogger<ThanasController> _logger;
        public ThanasController(IGlobalMaster globalMaster, ILogger<ThanasController> logger)
        {
            _globalMaster = globalMaster;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetDgPayThanas()
        {
            try
            {
                var data = _globalMaster.thanas.GetAll().Select(x => DgPayThana.DbToCustomModel(x)).ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgPayThanas)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDgPayThana(int id)
        {
            try
            {
                var data = await _globalMaster.thanas.GetFirstOrDefaultAsync(x => x.th_id == id);
                var nData = DgPayThana.DbToCustomModel(data);
                return Ok(nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgPayThana)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDgPayThana(int id, DgPayThana obj)
        {
            try
            {
                var nData = DgPayThana.CustomToDbModel(obj);
                await _globalMaster.thanas.UpdateAsync(nData);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PutDgPayThana)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost]
        public async Task<IActionResult> PostDgPayThana(DgPayThana obj)
        {
            try
            {
                var nData = DgPayThana.CustomToDbModel(obj);
                await _globalMaster.thanas.AddAsync(nData);
                return CreatedAtAction("GetDgPayThana", new { id = nData.th_id }, nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PostDgPayThana)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDgPayThana(int id)
        {
            try
            {
                var obj = await _globalMaster.thanas.GetFirstOrDefaultAsync(x => x.th_id == id);
                if (obj == null)
                {
                    _logger.LogError($"Invalid Delete Attempt In {nameof(DeleteDgPayThana)}");
                    return BadRequest("Submitted Data Is Invalid");
                }
                await _globalMaster.thanas.DeleteAsync(obj);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteDgPayThana)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
    }
}
