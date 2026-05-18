using BLL.Interfaces;
using BOL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Payroll_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly IGlobalMaster _globalMaster;
        private readonly ILogger<CompaniesController> _logger;
        public CompaniesController(IGlobalMaster globalMaster, ILogger<CompaniesController> logger)
        {
            _globalMaster = globalMaster;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetDgPayCompanies()
        {
            try
            {
                var data = _globalMaster.companies.GetAll().Select(n => DgPayCompany.DbToCustomModel(n)).ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgPayCompanies)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDgPayCompany(int id)
        {
            try
            {
                var data = await _globalMaster.companies.GetFirstOrDefaultAsync(x => x.com_id == id);
                var nData = DgPayCompany.DbToCustomModel(data);
                return Ok(nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgPayCompany)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDgPayCompany(int id, DgPayCompany obj)
        {
            try
            {
                var nData = DgPayCompany.CustonToDbModel(obj);
                await _globalMaster.companies.UpdateAsync(nData);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PutDgPayCompany)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost]
        public async Task<IActionResult> PostDgPayCompany(DgPayCompany obj)
        {
            try
            {
                var nData = DgPayCompany.CustonToDbModel(obj);
                await _globalMaster.companies.AddAsync(nData);
                return CreatedAtAction("GetDgPayCompany", new { id = nData.com_id }, nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PostDgPayCompany)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDgPayCompany(int id)
        {
            try
            {
                var obj = await _globalMaster.companies.GetFirstOrDefaultAsync(c => c.com_id == id);
                if (obj == null)
                {
                    _logger.LogError($"Invalid Delete Attempt In {nameof(DeleteDgPayCompany)}");
                    return BadRequest("Submitted Data Is Invalid");
                }
                await _globalMaster.companies.DeleteAsync(obj);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteDgPayCompany)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
    }
}
