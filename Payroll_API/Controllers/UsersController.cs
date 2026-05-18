using BLL.Interfaces;
using BOL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Payroll_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IGlobalMaster _globalMaster;
        private readonly ILogger<UsersController> _logger;
        public UsersController(IGlobalMaster globalMaster, ILogger<UsersController> logger)
        {
            _globalMaster = globalMaster;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetTblUsers()
        {
            try
            {
                var data = _globalMaster.users.GetAll().Select(x => TblUser.DbToCustomModel(x)).ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetTblUsers)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTblUser(int id)
        {
            try
            {
                var data = await _globalMaster.users.GetFirstOrDefaultAsync(x => x.ID == id);
                var nData = TblUser.DbToCustomModel(data);
                return Ok(nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetTblUser)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTblUser(int id, TblUser obj)
        {
            try
            {
                var nData = TblUser.CustomToDbModel(obj);
                await _globalMaster.users.UpdateAsync(nData);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PutTblUser)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost]
        public async Task<IActionResult> PostTblUser(TblUser obj)
        {
            try
            {
                var nData = TblUser.CustomToDbModel(obj);
                await _globalMaster.users.AddAsync(nData);
                return CreatedAtAction("GetTblUser", new { id = nData.ID }, nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PostTblUser)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTblUser(int id)
        {
            try
            {
                var obj = await _globalMaster.users.GetFirstOrDefaultAsync(x => x.ID == id);
                if (obj == null)
                {
                    _logger.LogError($"Invalid Delete Attempt In {nameof(DeleteTblUser)}");
                    return BadRequest("Submitted Data Is Invalid");
                }
                await _globalMaster.users.DeleteAsync(obj);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteTblUser)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
    }
}
