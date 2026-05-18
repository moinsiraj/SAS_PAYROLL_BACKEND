using BLL.Interfaces;
using BOL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Payroll_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivedatesController : ControllerBase
    {
        private readonly IGlobalMaster _globalMaster;
        private readonly ILogger<ActivedatesController> _logger;
        public ActivedatesController(IGlobalMaster globalMaster, ILogger<ActivedatesController> logger)
        {
            _globalMaster = globalMaster;
            _logger = logger;

        }

        [HttpGet]
        public async Task<IActionResult> GetDgPayActivedates()
        {
            try
            {
                var data = await _globalMaster.activedate.GetAllAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgPayActivedates)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDgPayActivedate(int id)
        {
            try
            {
                var data = await _globalMaster.activedate.GetFirstOrDefaultAsync(x => x.sl== id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgPayActivedate)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDgPayActivedate(int id,Activedate_DbModel obj)
        {
            try
            {
                await _globalMaster.activedate.UpdateAsync(obj);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PutDgPayActivedate)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost]
        public async Task<IActionResult> PostDgPayActivedate(Activedate_DbModel obj)
        {
            try
            {
                await _globalMaster.activedate.AddAsync(obj);
                return CreatedAtAction("GetDgPayActivedate", new { id = obj.sl }, obj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PostDgPayActivedate)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDgPayActivedate(int id)
        {
            try
            {
                var obj = await _globalMaster.activedate.GetFirstOrDefaultAsync(c => c.sl == id);
                if (obj == null)
                {
                    _logger.LogError($"Invalid Delete Attempt In {nameof(DeleteDgPayActivedate)}");
                    return BadRequest("Submitted Data Is Invalid");
                }
                await _globalMaster.activedate.DeleteAsync(obj);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteDgPayActivedate)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        //Excel Test
        [HttpPost("ExcelTest")]
        public async Task<IActionResult> SaveCountry([FromForm] UploadExcelFile model)
        {
            try
            {
                string message = await _globalMaster.excelText.SaveCountry(model);
                return Ok(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(SaveCountry)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost("GetExcelList")]
        public async Task<IActionResult> GetExcelCountryData([FromForm] UploadExcelFile model)
        {
            try
            {
                var message = await _globalMaster.excelText.GetExcelCountryData(model);
                return Ok(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetExcelCountryData)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpGet("GetConnectionInfoReal")]
        public IActionResult GetConnectionInfoReal()
        {
            try
            {
                var result = new
                {
                    host = "192.168.1.42",
                    user = "sa",
                    password = "dg@INFO2025"
                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        [HttpGet("GetConnectionInfoLocal")]
        public IActionResult GetConnectionInfoLocal()
        {
            try
            {
                var result = new
                {
                    host = "192.168.101.250",
                    user = "sa",
                    password = "dg@INFO"
                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
