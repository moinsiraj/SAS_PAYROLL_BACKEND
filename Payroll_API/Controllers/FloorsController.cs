using BLL.Interfaces;
using BOL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Payroll_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FloorsController : ControllerBase
    {
        private readonly IGlobalMaster _globalMaster;
        private readonly ILogger<FloorsController> _logger;
        public FloorsController(IGlobalMaster globalMaster, ILogger<FloorsController> logger)
        {
            _globalMaster = globalMaster;
            _logger = logger;
        }

        [HttpGet]
        public async Task< IActionResult> GetDgPayFloors(string userName)
        {
            try
            {
                //var data = _globalMaster.floors.GetAll().Select(x => DgPayFloor.DbToCustomModel(x)).ToList();
                var data = await _globalMaster.floors.GetFloorByUserName(userName);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgPayFloors)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDgPayFloor(int id)
        {
            try
            {
                var data = await _globalMaster.floors.GetFirstOrDefaultAsync(x => x.nFloor == id);
                var nData = DgPayFloor.DbToCustomModel(data);
                return Ok(nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgPayFloor)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDgPayFloor(int id, DgPayFloor obj)
        {
            try
            {
                var nData = DgPayFloor.CustomToDbModel(obj);
                await _globalMaster.floors.UpdateAsync(nData);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PutDgPayFloor)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost]
        public async Task<IActionResult> PostDgPayFloor(DgPayFloor obj)
        {
            try
            {
                var nData = DgPayFloor.CustomToDbModel(obj);
                await _globalMaster.floors.AddAsync(nData);
                return CreatedAtAction("GetDgPayFloor", new { id = nData.nFloor }, nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PostDgPayFloor)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDgPayFloor(int id)
        {
            try
            {
                var obj = await _globalMaster.floors.GetFirstOrDefaultAsync(c => c.nFloor == id);
                if (obj == null)
                {
                    _logger.LogError($"Invalid Delete Attempt In {nameof(DeleteDgPayFloor)}");
                    return BadRequest("Submitted Data Is Invalid");
                }
                await _globalMaster.floors.DeleteAsync(obj);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteDgPayFloor)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
    }
}
