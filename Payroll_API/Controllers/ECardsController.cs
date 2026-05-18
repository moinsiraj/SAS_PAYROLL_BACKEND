using BLL.Interfaces;
using BOL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Payroll_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ECardsController : ControllerBase
    {
        private readonly IGlobalMaster _globalMaster;
        private readonly ILogger<ECardsController> _logger;
        public ECardsController(IGlobalMaster globalMaster, ILogger<ECardsController> logger)
        {
            _globalMaster = globalMaster;
            _logger = logger;
        }

        [HttpGet("get-ECardddddddd")]
        public async Task<IActionResult> GeECardddddddd(int comID, int emp_id, DateTime monthdate)
        {
            try
            {
                var data = await _globalMaster.eCards.GeECard(comID, emp_id, monthdate);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GeECardddddddd)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpGet("get-ECarsum")]
        public async Task<IActionResult> GeECarsum(string CompId, int Empid, DateTime EDate)
        {
            try
            {
                var data = await _globalMaster.eCards.GeECarsum(CompId, Empid, EDate);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GeECarsum)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetECards()
        {
            try
            {
                var data = await _globalMaster.eCards.GetAllAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetECards)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetECard(int id)
        {
            try
            {
                var data = await _globalMaster.eCards.GetFirstOrDefaultAsync(data => data.Id == id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetECard)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutECard(int id, ECard_DbModel obj)
        {
            try
            {
                await _globalMaster.eCards.UpdateAsync(obj);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PutECard)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost]
        public async Task<IActionResult> PostECard(ECard_DbModel obj)
        {
            try
            {
                await _globalMaster.eCards.AddAsync(obj);
                return CreatedAtAction("GetECard", new { id = obj.Id }, obj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PostECard)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteECard(int id)
        {
            try
            {
                var obj = await _globalMaster.eCards.GetFirstOrDefaultAsync(c => c.Id == id);
                if (obj == null)
                {
                    _logger.LogError($"Invalid Delete Attempt In {nameof(DeleteECard)}");
                    return BadRequest("Submitted Data Is Invalid");
                }
                await _globalMaster.eCards.DeleteAsync(obj);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteECard)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
    }
}
