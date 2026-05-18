using BLL.Interfaces;
using BOL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Payroll_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenurightsController : ControllerBase
    {
        private readonly IGlobalMaster _globalMaster;
        private readonly ILogger<MenurightsController> _logger;
        public MenurightsController(IGlobalMaster globalMaster, ILogger<MenurightsController> logger)
        {
            _globalMaster = globalMaster;
            _logger = logger;
        }

        [HttpGet("Company-wise-userlist")]
        public async Task<IActionResult> GetCompanywiseuserlist(int compid)
        {
            try
            {
                var data = await _globalMaster.menurights.GetCompanywiseuserlist(compid);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetCompanywiseuserlist)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("userwise_menulist")]
        public async Task<IActionResult> Getuserwisemenulist(string m_user)
        {
            try
            {
                var data = await _globalMaster.menurights.Getuserwisemenulist(m_user);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(Getuserwisemenulist)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet]
        public IActionResult GetDgMenurights()
        {
            try
            {
                var data = _globalMaster.menurights.GetAll().Select(x => DgMenuright.DbToCustomModel(x)).ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgMenurights)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDgMenuright(int id)
        {
            try
            {
                var data = await _globalMaster.menurights.GetFirstOrDefaultAsync(x => x.m_sl == id);
                var nData = DgMenuright.DbToCustomModel(data);
                return Ok(nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgMenuright)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDgMenuright(int id, DgMenuright obj)
        {
            try
            {
                var nData = DgMenuright.CustomToDbModel(obj);
                await _globalMaster.menurights.UpdateAsync(nData);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PutDgMenuright)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost]
        public async Task<IActionResult> PostDgMenuright(DgMenuright obj)
        {
            try
            {
                var nData = DgMenuright.CustomToDbModel(obj);
                await _globalMaster.menurights.AddAsync(nData);
                return CreatedAtAction("", new { id = nData.m_sl }, nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PostDgMenuright)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDgMenuright(int id)
        {
            try
            {
                var obj = await _globalMaster.menurights.GetFirstOrDefaultAsync(c => c.m_sl == id);
                if (obj == null)
                {
                    _logger.LogError($"Invalid Delete Attempt In {nameof(DeleteDgMenuright)}");
                    return BadRequest("Submitted Data Is Invalid");
                }
                await _globalMaster.menurights.DeleteAsync(obj);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteDgMenuright)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
    }
}
