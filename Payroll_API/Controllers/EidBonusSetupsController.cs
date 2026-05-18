using BLL.Interfaces;
using BOL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Payroll_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EidBonusSetupsController : ControllerBase
    {
        private readonly IGlobalMaster _globalMaster;
        private readonly ILogger<EidBonusSetupsController> _logger;
        public EidBonusSetupsController(IGlobalMaster globalMaster, ILogger<EidBonusSetupsController> logger)
        {
            _globalMaster = globalMaster;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetDgPayEidBonusSetups()
        {
            try
            {
                var data = _globalMaster.eidBonusSetups.GetAll().Select(x => DgPayEidBonusSetup.DbToCustomModel(x)).ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgPayEidBonusSetups)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDgPayEidBonusSetup(int id)
        {
            try
            {
                var data = await _globalMaster.eidBonusSetups.GetFirstOrDefaultAsync(data => data.eb_serial == id);
                var nData = DgPayEidBonusSetup.DbToCustomModel(data);
                return Ok(nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgPayEidBonusSetup)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDgPayEidBonusSetup(int id, DgPayEidBonusSetup obj)
        {
            try
            {
                var nData = DgPayEidBonusSetup.CustomToDbModel(obj);
                await _globalMaster.eidBonusSetups.UpdateAsync(nData);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PutDgPayEidBonusSetup)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost]
        public async Task<IActionResult> PostDgPayEidBonusSetup(DgPayEidBonusSetup obj)
        {
            try
            {
                var nData = DgPayEidBonusSetup.CustomToDbModel(obj);
                await _globalMaster.eidBonusSetups.AddAsync(nData);
                return CreatedAtAction("GetDgPayEidBonusSetup", new { id = nData.eb_serial }, nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PostDgPayEidBonusSetup)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDgPayEidBonusSetup(int id)
        {
            try
            {
                var obj = await _globalMaster.eidBonusSetups.GetFirstOrDefaultAsync(c => c.eb_serial == id);
                if (obj == null)
                {
                    _logger.LogError($"Invalid Delete Attempt In {nameof(DeleteDgPayEidBonusSetup)}");
                    return BadRequest("Submitted Data Is Invalid");
                }
                await _globalMaster.eidBonusSetups.DeleteAsync(obj);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteDgPayEidBonusSetup)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpPost("EidBonusProcess")]
        public async Task<IActionResult> EidBonusProcess(EidBonusProcess obj)
        {
            try
            {
                var response = await _globalMaster.eidBonusSetups.EidBonusProcess(obj);
                if (response.IsSuccess)
                {
                    return Ok(new {response.IsSuccess, response.Message});
                }
                return BadRequest(new { response.IsSuccess, response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(EidBonusProcess)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost("EidBounsConfirm")]
        public async Task<IActionResult> EidBounsConfirm(EidBonusProcess obj)
        {
            try
            {
                var response = await _globalMaster.eidBonusSetups.EidBounsConfirm(obj);
                if (response.IsSuccess)
                {
                    return Ok(new { response.IsSuccess, response.Message });
                }
                return BadRequest(new { response.IsSuccess, response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(EidBounsConfirm)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
    }
}