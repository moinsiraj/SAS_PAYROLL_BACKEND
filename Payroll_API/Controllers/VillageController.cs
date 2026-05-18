using BLL.Interfaces;
using BOL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Payroll_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillageController : ControllerBase
    {
        private readonly IGlobalMaster _globalMaster;
        private readonly ILogger<VillageController> _logger;
        public VillageController(IGlobalMaster globalMaster, ILogger<VillageController> logger)
        {
            _globalMaster = globalMaster;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetAllVillage(int page = 1, int pageSize = 10)
        {
            try
            {
                var data = _globalMaster.village.GetAll();
                if (data == null)
                {
                    return NotFound();
                }
                var totalItems = data.Count();
                var pagedProducts = data.Skip((page - 1) * pageSize).Take(pageSize);
                var response = new
                {
                    TotalItems = totalItems,
                    Page = page,
                    PageSize = pageSize,
                    Data = pagedProducts
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetAllVillage)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVillageById(int id)
        {
            try
            {
                var data = await _globalMaster.village.GetFirstOrDefaultAsync(x => x.vill_id == id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetVillageById)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVillage(int id, Village_DbModel obj)
        {
            try
            {
                await _globalMaster.village.UpdateAsync(obj);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(UpdateVillage)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost]
        public async Task<IActionResult> SaveVillage(Village_DbModel obj)
        {
            try
            {
                await _globalMaster.village.AddAsync(obj);
                return CreatedAtAction("GetVillageById", new { id = obj.vill_id }, obj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(SaveVillage)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVillage(int id)
        {
            try
            {
                var obj = await _globalMaster.village.GetFirstOrDefaultAsync(x => x.vill_id == id);
                if (obj == null)
                {
                    _logger.LogError($"Invalid Delete Attempt In {nameof(DeleteVillage)}");
                    return BadRequest("Submitted Data Is Invalid");
                }
                await _globalMaster.village.DeleteAsync(obj);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteVillage)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
    }
}