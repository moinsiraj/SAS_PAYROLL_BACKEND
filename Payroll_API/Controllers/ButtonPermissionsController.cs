using BLL.Interfaces;
using BOL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Payroll_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ButtonPermissionsController : ControllerBase
    {
        private readonly IGlobalMaster _globalMaster;
        private readonly ILogger<ButtonPermissionsController> _logger;
        public ButtonPermissionsController(IGlobalMaster globalMaster, ILogger<ButtonPermissionsController> logger)
        {
            _globalMaster = globalMaster;
            _logger = logger;
        }

        [HttpGet("userwise_Buttonlist")]
        public async Task<IActionResult> Getuserwisebuttonlist(string b_user)
        {
            try
            {
                var data = await _globalMaster.buttonPermission.GetUserWiseButtonList(b_user);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(Getuserwisebuttonlist)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet]
        public IActionResult GetDgButtonPermissions()
        {
            try
            {
                var data = _globalMaster.buttonPermission.GetAll().Select(n => DgButtonPermission.DbToCustomModel(n)).ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgButtonPermissions)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDgButtonPermission(int id)
        {
            try
            {
                var data = await _globalMaster.buttonPermission.GetFirstOrDefaultAsync(x => x.b_sl == id);
                var nData = DgButtonPermission.DbToCustomModel(data);
                return Ok(nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgButtonPermission)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDgButtonPermission(int id, DgButtonPermission obj)
        {
            try
            {
                var nData = DgButtonPermission.CustonToDbModel(obj);
                await _globalMaster.buttonPermission.UpdateAsync(nData);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PutDgButtonPermission)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost]
        public async Task<IActionResult> PostDgButtonPermission(DgButtonPermission obj)
        {
            try
            {
                var nData = DgButtonPermission.CustonToDbModel(obj);
                await _globalMaster.buttonPermission.AddAsync(nData);
                return CreatedAtAction("GetDgButtonPermission", new { id = nData.b_sl }, nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PostDgButtonPermission)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDgButtonPermission(int id)
        {
            try
            {
                var obj = await _globalMaster.buttonPermission.GetFirstOrDefaultAsync(c => c.b_sl == id);
                if (obj == null)
                {
                    _logger.LogError($"Invalid Delete Attempt In {nameof(DeleteDgButtonPermission)}");
                    return BadRequest("Submitted Data Is Invalid");
                }
                await _globalMaster.buttonPermission.DeleteAsync(obj);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteDgButtonPermission)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
    }
}
