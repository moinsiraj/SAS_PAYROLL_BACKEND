using BLL.Interfaces;
using BOL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Payroll_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoancategoriesController : ControllerBase
    {
        private readonly IGlobalMaster _globalMaster;
        private readonly ILogger<LoancategoriesController> _logger;
        public LoancategoriesController(IGlobalMaster globalMaster, ILogger<LoancategoriesController> logger)
        {
            _globalMaster = globalMaster;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetDgPayLoancategories()
        {
            try
            {
                var data = _globalMaster.loanCategories.GetAll().Select(x => DgPayLoancategory.DbToCustomModel(x)).ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgPayLoancategories)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDgPayLoancategory(int id)
        {
            try
            {
                var data = await _globalMaster.loanCategories.GetFirstOrDefaultAsync(x => x.lc_id == id);
                var nData = DgPayLoancategory.DbToCustomModel(data);
                return Ok(nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgPayLoancategory)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDgPayLoancategory(int id, DgPayLoancategory obj)
        {
            try
            {
                var nData = DgPayLoancategory.CustonToDbModel(obj);
                await _globalMaster.loanCategories.UpdateAsync(nData);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PutDgPayLoancategory)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost]
        public async Task<IActionResult> PostDgPayLoancategory(DgPayLoancategory obj)
        {
            try
            {
                var nData = DgPayLoancategory.CustonToDbModel(obj);
                await _globalMaster.loanCategories.AddAsync(nData);
                return CreatedAtAction("GetDgPayLoancategory", new { id = nData.lc_id }, nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PostDgPayLoancategory)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDgPayLoancategory(int id)
        {
            try
            {
                var obj = await _globalMaster.loanCategories.GetFirstOrDefaultAsync(c => c.lc_id == id);
                if (obj == null)
                {
                    _logger.LogError($"Invalid Delete Attempt In {nameof(DeleteDgPayLoancategory)}");
                    return BadRequest("Submitted Data Is Invalid");
                }
                await _globalMaster.loanCategories.DeleteAsync(obj);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteDgPayLoancategory)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
    }
}
