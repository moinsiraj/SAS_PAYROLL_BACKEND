using BLL.Interfaces;
using BOL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Payroll_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly IGlobalMaster _globalMaster;
        private readonly ILogger<DepartmentsController> _logger;
        public DepartmentsController(IGlobalMaster globalMaster, ILogger<DepartmentsController> logger)
        {
            _globalMaster = globalMaster;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDepartments(string userName)
        {
            try
            {
                var data = await _globalMaster.departments.GetDepartmentByUserName(userName);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetAllDepartments)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAllDepartment(int id)
        {
            try
            {
                var data = await _globalMaster.departments.GetFirstOrDefaultAsync(x => x.nUserDept == id);
                var nData = DgPayDepartment.DbToCustomModel(data);
                return Ok(nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetAllDepartment)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDgPayDepartment(int id, DgPayDepartment obj)
        {
            try
            {
                var nData = DgPayDepartment.CustomToDbModel(obj);
                await _globalMaster.departments.UpdateAsync(nData);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PutDgPayDepartment)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost]
        public async Task<IActionResult> PostDgPayDepartment(DgPayDepartment obj)
        {
            try
            {
                var nData = DgPayDepartment.CustomToDbModel(obj);
                await _globalMaster.departments.AddAsync(nData);
                return CreatedAtAction("GetAllDepartment", new { id = nData.nUserDept }, nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PostDgPayDepartment)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDgPayDepartment(int id)
        {
            try
            {
                var obj = await _globalMaster.departments.GetFirstOrDefaultAsync(c => c.nUserDept == id);
                if (obj == null)
                {
                    _logger.LogError($"Invalid Delete Attempt In {nameof(DeleteDgPayDepartment)}");
                    return BadRequest("Submitted Data Is Invalid");
                }
                await _globalMaster.departments.DeleteAsync(obj);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteDgPayDepartment)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
    }
}
