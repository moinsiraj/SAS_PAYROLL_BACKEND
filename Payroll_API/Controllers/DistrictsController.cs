using BLL.Interfaces;
using BOL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Payroll_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistrictsController : ControllerBase
    {
        private readonly IGlobalMaster _globalMaster;
        private readonly ILogger<DistrictsController> _logger;
        public DistrictsController(IGlobalMaster globalMaster, ILogger<DistrictsController> logger)
        {
            _globalMaster = globalMaster;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetDgPayDistricts()
        {
            try
            {
                var data = _globalMaster.districts.GetAll().Select(data => DgPayDistrict.DbToCustomModel(data)).ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgPayDistricts)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDgPayDistrict(int id)
        {
            try
            {
                var data = await _globalMaster.districts.GetFirstOrDefaultAsync(x => x.di_id == id);
                var nData = DgPayDistrict.DbToCustomModel(data);
                return Ok(nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgPayDistrict)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDgPayDistrict(int id, DgPayDistrict obj)
        {
            try
            {
                var nData = DgPayDistrict.CustonToDbModel(obj);
                await _globalMaster.districts.UpdateAsync(nData);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PutDgPayDistrict)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost]
        public async Task<IActionResult> PostDgPayDistrict(DgPayDistrict obj)
        {
            try
            {
                var nData = DgPayDistrict.CustonToDbModel(obj);
                await _globalMaster.districts.UpdateAsync(nData);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PostDgPayDistrict)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDgPayDistrict(int id)
        {
            try
            {
                var obj = await _globalMaster.districts.GetFirstOrDefaultAsync(c => c.di_id == id);
                if (obj == null)
                {
                    _logger.LogError($"Invalid Delete Attempt In {nameof(DeleteDgPayDistrict)}");
                    return BadRequest("Submitted Data Is Invalid");
                }
                await _globalMaster.districts.DeleteAsync(obj);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteDgPayDistrict)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
    }
}
