using BLL.Interfaces;
using BOL.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Payroll_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradesController : ControllerBase
    {
        private readonly IGlobalMaster _globalMaster;
        private readonly ILogger<GradesController> _logger;
        public GradesController(IGlobalMaster globalMaster, ILogger<GradesController> logger)
        {
            _globalMaster = globalMaster;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetDgPayGrades()
        {
            try
            {
                var data = _globalMaster.grades.GetAll().Select(data => DgPayGrade.DbToCustomModel(data)).ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgPayGrades)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDgPayGrade(int id)
        {
            try
            {
                var data = await _globalMaster.grades.GetFirstOrDefaultAsync(x => x.Grd_id == id);
                var nData = DgPayGrade.DbToCustomModel(data);
                return Ok(nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgPayGrade)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDgPayGrade(int id, DgPayGrade obj)
        {
            try
            {
                var nData = DgPayGrade.CustonToDbModel(obj);
                await _globalMaster.grades.UpdateAsync(nData);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PutDgPayGrade)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost]
        public async Task<IActionResult> PostDgPayGrade(DgPayGrade obj)
        {
            try
            {
                var nData = DgPayGrade.CustonToDbModel(obj);
                await _globalMaster.grades.AddAsync(nData);
                return CreatedAtAction("GetDgPayGrade", new { id = nData.Grd_id }, nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PostDgPayGrade)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDgPayGrade(int id)
        {
            try
            {
                var obj = await _globalMaster.grades.GetFirstOrDefaultAsync(c => c.Grd_id == id);
                if (obj == null)
                {
                    _logger.LogError($"Invalid Delete Attempt In {nameof(DeleteDgPayGrade)}");
                    return BadRequest("Submitted Data Is Invalid");
                }
                await _globalMaster.grades.DeleteAsync(obj);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteDgPayGrade)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
    }
}
