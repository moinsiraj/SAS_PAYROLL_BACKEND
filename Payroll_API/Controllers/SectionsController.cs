using BLL.Interfaces;
using BOL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Payroll_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectionsController : ControllerBase
    {
        private readonly IGlobalMaster _globalMaster;
        private readonly ILogger<SectionsController> _logger;
        public SectionsController(IGlobalMaster globalMaster, ILogger<SectionsController> logger)
        {
            _globalMaster = globalMaster;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetDgPaySections(string userName)
        {
            try
            {
                var data = await _globalMaster.sections.GetSectionByUserPermission(userName);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgPaySections)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDgPaySection(int id)
        {
            try
            {
                var data = await _globalMaster.sections.GetFirstOrDefaultAsync(x => x.nSectionID == id);
                var nData = DgPaySection.DbToCustomModel(data);
                return Ok(nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgPaySection)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDgPaySection(int id, DgPaySection obj)
        {
            try
            {
                var nData = DgPaySection.CustonToDbModel(obj);
                await _globalMaster.sections.UpdateAsync(nData);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PutDgPaySection)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost]
        public async Task<IActionResult> PostDgPaySection(DgPaySection obj)
        {
            try
            {
                var nData = DgPaySection.CustonToDbModel(obj);
                await _globalMaster.sections.AddAsync(nData);
                return CreatedAtAction("GetDgPaySection", new { id = nData.nSectionID }, nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PostDgPaySection)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDgPaySection(int id)
        {
            try
            {
                var obj = await _globalMaster.sections.GetFirstOrDefaultAsync(x => x.nSectionID == id);
                if (obj == null)
                {
                    _logger.LogError($"Invalid Delete Attempt In {nameof(DeleteDgPaySection)}");
                    return BadRequest("Submitted Data Is Invalid");
                }
                await _globalMaster.sections.DeleteAsync(obj);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteDgPaySection)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
    }
}
