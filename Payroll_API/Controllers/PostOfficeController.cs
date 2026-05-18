using BLL.Interfaces;
using BOL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Payroll_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostOfficeController : ControllerBase
    {
        private readonly IGlobalMaster _globalMaster;
        private readonly ILogger<PostOfficeController> _logger;
        public PostOfficeController(IGlobalMaster globalMaster, ILogger<PostOfficeController> logger)
        {
            _globalMaster = globalMaster;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPostOffice()
        {
            try
            {
                var data = await _globalMaster.postOffice.GetAllAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetAllPostOffice)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostOfficeById(int id)
        {
            try
            {
                var data = await _globalMaster.postOffice.GetFirstOrDefaultAsync(x => x.po_id == id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetPostOfficeById)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePostOffice(int id, PostOffice_DbModel obj)
        {
            try
            {
                await _globalMaster.postOffice.UpdateAsync(obj);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(UpdatePostOffice)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost]
        public async Task<IActionResult> SavePostOfficeData(PostOffice_DbModel obj)
        {
            try
            {
                obj.po_udate = DateTime.Now;
                await _globalMaster.postOffice.AddAsync(obj);
                return CreatedAtAction("GetPostOfficeById", new { id = obj.po_id }, obj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(SavePostOfficeData)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePostOfficeData(int id)
        {
            try
            {
                var obj = await _globalMaster.postOffice.GetFirstOrDefaultAsync(x => x.po_id == id);
                if (obj == null)
                {
                    _logger.LogError($"Invalid Delete Attempt In {nameof(DeletePostOfficeData)}");
                    return BadRequest("Submitted Data Is Invalid");
                }               
                await _globalMaster.postOffice.DeleteAsync(obj);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeletePostOfficeData)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
    }
}
