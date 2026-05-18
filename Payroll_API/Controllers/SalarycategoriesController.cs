using BLL.Interfaces;
using BOL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Payroll_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalarycategoriesController : ControllerBase
    {
        private readonly IGlobalMaster _globalMaster;
        private readonly ILogger<SalarycategoriesController> _logger;
        public SalarycategoriesController(IGlobalMaster globalMaster, ILogger<SalarycategoriesController> logger)
        {
            _globalMaster = globalMaster;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetDgPaySalarycategories()
        {
            try
            {
                var data = _globalMaster.salaryCategories.GetAll().Select(x => DgPaySalarycategory.DbToCustomModel(x)).ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgPaySalarycategories)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDgPaySalarycategory(int id)
        {
            try
            {
                var data = await _globalMaster.salaryCategories.GetFirstOrDefaultAsync(x => x.cat_id == id);
                var nData = DgPaySalarycategory.DbToCustomModel(data);
                return Ok(nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgPaySalarycategory)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDgPaySalarycategory(int id, DgPaySalarycategory obj)
        {
            try
            {
                var nData = DgPaySalarycategory.CustomToDbModel(obj);
                await _globalMaster.salaryCategories.UpdateAsync(nData);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PutDgPaySalarycategory)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost]
        public async Task<IActionResult> PostDgPaySalarycategory(DgPaySalarycategory obj)
        {
            try
            {
                var nData = DgPaySalarycategory.CustomToDbModel(obj);
                await _globalMaster.salaryCategories.AddAsync(nData);
                return CreatedAtAction("GetDgPaySalarycategory", new { id = nData.cat_id }, nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PostDgPaySalarycategory)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDgPaySalarycategory(int id)
        {
            try
            {
                var obj = await _globalMaster.salaryCategories.GetFirstOrDefaultAsync(x => x.cat_id == id);
                if (obj == null)
                {
                    _logger.LogError($"Invalid Delete Attempt In {nameof(DeleteDgPaySalarycategory)}");
                    return BadRequest("Submitted Data Is Invalid");
                }
                await _globalMaster.salaryCategories.DeleteAsync(obj);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteDgPaySalarycategory)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
    }
}
