using BLL.Interfaces;
using BOL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Payroll_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttbonusController : ControllerBase
    {
        private readonly IGlobalMaster _globalMaster;
        private readonly ILogger<AttbonusController> _logger;
        public AttbonusController(IGlobalMaster globalMaster, ILogger<AttbonusController> logger)
        {
            _globalMaster = globalMaster;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetDgPayAttbonus(int companyID)
        {
            try
            {
                var data = _globalMaster.attBonus.GetAll().Where(x => x.attb_compid == companyID).Select(n => DgPayAttbonu.DbToCustomModel(n)).ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgPayAttbonus)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDgPayAttbonu(int id)
        {
            try
            {
                var data = await _globalMaster.attBonus.GetFirstOrDefaultAsync(x => x.attb_serial == id);
                var nData = DgPayAttbonu.DbToCustomModel(data);
                return Ok(nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgPayAttbonu)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDgPayAttbonu(int id, DgPayAttbonu obj)
        {
            try
            {
                var nData = DgPayAttbonu.CustonToDbModel(obj);
                nData.attb_udate = DateTime.Now;
                await _globalMaster.attBonus.UpdateAsync(nData);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PutDgPayAttbonu)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost]
        public async Task<IActionResult> PostDgPayAttbonu(DgPayAttbonu obj)
        {
            try
            {
                var isExistsData = _globalMaster.attBonus.GetFirstOrDefault(x => x.attb_compid==obj.AttbCompid && x.attb_designation==obj.AttbDesignation);
                if (isExistsData == null)
                {
                    var nData = DgPayAttbonu.CustonToDbModel(obj);
                    nData.attb_udate = DateTime.Now;
                    await _globalMaster.attBonus.AddAsync(nData);
                    return CreatedAtAction("GetDgPayAttbonu", new { id = nData.attb_serial }, nData);
                }
                return BadRequest("Attendance Bouns Already Extsts !!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PostDgPayAttbonu)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDgPayAttbonu(int id)
        {
            try
            {
                var obj = await _globalMaster.attBonus.GetFirstOrDefaultAsync(c => c.attb_serial == id);
                if (obj == null)
                {
                    _logger.LogError($"Invalid Delete Attempt In {nameof(DeleteDgPayAttbonu)}");
                    return BadRequest("Submitted Data Is Invalid");
                }
                await _globalMaster.attBonus.DeleteAsync(obj);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteDgPayAttbonu)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
    }
}
