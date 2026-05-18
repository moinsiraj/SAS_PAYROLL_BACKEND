using BLL.Interfaces;
using BOL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Payroll_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuildingUnitsController : ControllerBase
    {
        private readonly IGlobalMaster _globalMaster;
        private readonly ILogger<BuildingUnitsController> _logger;
        public BuildingUnitsController(IGlobalMaster globalMaster, ILogger<BuildingUnitsController> logger)
        {
            _globalMaster = globalMaster;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetDgPayBuildingUnits(string userName)
        {
            try
            {
                //var data = _globalMaster.buildingUnit.GetAll().Select(x => DgPayBuildingUnit.DbToCustomModel(x)).ToList();
                var data = await _globalMaster.buildingUnit.GetBuildingUnitsByUser(userName);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgPayBuildingUnits)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDgPayBuildingUnit(int id)
        {
            try
            {
                var data = await _globalMaster.buildingUnit.GetFirstOrDefaultAsync(x => x.Unit_Code == id);
                var nData = DgPayBuildingUnit.DbToCustomModel(data);
                return Ok(nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgPayBuildingUnit)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDgPayBuildingUnit(int id, DgPayBuildingUnit obj)
        {
            try
            {
                var nData = DgPayBuildingUnit.CustomToDbModel(obj);
                await _globalMaster.buildingUnit.UpdateAsync(nData);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PutDgPayBuildingUnit)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost]
        public async Task<IActionResult> PostDgPayBuildingUnit(DgPayBuildingUnit obj)
        {
            try
            {
                var nData = DgPayBuildingUnit.CustomToDbModel(obj);
                await _globalMaster.buildingUnit.AddAsync(nData);
                return CreatedAtAction("GetDgPayBuildingUnit", new { id = nData.Unit_Code, }, nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PostDgPayBuildingUnit)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDgPayBuildingUnit(int id)
        {
            try
            {
                var obj = await _globalMaster.buildingUnit.GetFirstOrDefaultAsync(c => c.Unit_Code == id);
                if (obj == null)
                {
                    _logger.LogError($"Invalid Delete Attempt In {nameof(DeleteDgPayBuildingUnit)}");
                    return BadRequest("Submitted Data Is Invalid");
                }
                await _globalMaster.buildingUnit.DeleteAsync(obj);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteDgPayBuildingUnit)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
    }
}
