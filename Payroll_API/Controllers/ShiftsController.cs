using BLL.Interfaces;
using BOL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Payroll_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShiftsController : ControllerBase
    {
        private readonly IGlobalMaster _globalMaster;
        private readonly ILogger<ShiftsController> _logger;
        public ShiftsController(IGlobalMaster globalMaster, ILogger<ShiftsController> logger)
        {
            _globalMaster = globalMaster;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetDgPayShifts()
        {
            try
            {
                var data = _globalMaster.shifts.GetAll().Select(x => DgPayShift.DbToCustomModel(x)).ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgPayShifts)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDgPayShift(int id)
        {
            try
            {
                var data = await _globalMaster.shifts.GetFirstOrDefaultAsync(x => x.sh_code == id);
                var nData = DgPayShift.DbToCustomModel(data);
                return Ok(nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgPayShift)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDgPayShift(int id, DgPayShift obj)
        {
            try
            {
                var nData = DgPayShift.CustomToDbModel(obj);
                await _globalMaster.shifts.UpdateAsync(nData);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PutDgPayShift)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost("SaveShiftInfo")]
        public async Task<IActionResult> PostDgPayShift(DgPayShift obj)
        {
            try
            {
                var isExists = _globalMaster.shifts.GetAll().Where(x => x.sh_name == obj.ShName && x.sh_comp == obj.ShComp).Any();
                if (isExists)
                {
                    return BadRequest(new { isSuccess = false, statusCode = 400, message = "Shift Name Already Exists !!" });
                }
                var nData = DgPayShift.CustomToDbModel(obj);
                bool flag = await _globalMaster.shifts.AddAsync(nData);
                if (flag)
                {
                    var resData = _globalMaster.shifts.GetAll().Where(x => x.sh_comp == obj.ShComp).ToList();
                    return Ok(new {isSuccess = flag,statusCode = 200, message = "Save Successfully !!", data = resData });
                }
                return BadRequest(new { isSuccess = flag, statusCode = 400, message = "Save Fail !!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PostDgPayShift)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDgPayShift(int id)
        {
            try
            {
                var obj = await _globalMaster.shifts.GetFirstOrDefaultAsync(x => x.sh_code == id);
                if (obj == null)
                {
                    _logger.LogError($"Invalid Delete Attempt In {nameof(DeleteDgPayShift)}");
                    return BadRequest("Submitted Data Is Invalid");
                }
                await _globalMaster.shifts.DeleteAsync(obj);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteDgPayShift)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("GetCompShiftInfo")]
        public IActionResult GetShiftByComp(int? compid)
        {
            try
            {
                var data = _globalMaster.shifts.GetAll().Where(x => x.sh_comp == compid).ToList();
                if (data != null)
                {
                    return Ok(new { isSuccess = true, statusCode = 200, data = data });
                }
                return NotFound(new { isSuccess = true, statusCode = 200, message = "No Data Found !!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetShiftByComp)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
    }
}
