using BLL.Interfaces;
using BOL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Payroll_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DesignationsController : ControllerBase
    {
        private readonly IGlobalMaster _globalMaster;
        private readonly ILogger<DesignationsController> _logger;
        public DesignationsController(IGlobalMaster globalMaster, ILogger<DesignationsController> logger)
        {
            _globalMaster = globalMaster;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetDgPayDesignations()
        {
            try
            {
                var data = _globalMaster.designations.GetAll().Select(data => DgPayDesignation.DbToCustomModel(data)).OrderBy(o => o.DecName).ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgPayDesignations)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDgPayDesignation(int id)
        {
            try
            {
                var data = await _globalMaster.designations.GetFirstOrDefaultAsync(x => x.dec_id == id);
                var nData = DgPayDesignation.DbToCustomModel(data);
                return Ok(nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgPayDesignation)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDgPayDesignation(int id, DgPayDesignation obj)
        {
            try
            {
                var nData = DgPayDesignation.CustonToDbModel(obj);
                await _globalMaster.designations.UpdateAsync(nData);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PutDgPayDesignation)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost]
        public async Task<IActionResult> PostDgPayDesignation(DgPayDesignation obj)
        {
            try
            {
                var nData = DgPayDesignation.CustonToDbModel(obj);
                await _globalMaster.designations.AddAsync(nData);
                return CreatedAtAction("GetDgPayDesignation", new { id = nData.dec_id }, nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PostDgPayDesignation)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDgPayDesignation(int id)
        {
            try
            {
                var obj = await _globalMaster.designations.GetFirstOrDefaultAsync(c => c.dec_id == id);
                if (obj == null)
                {
                    _logger.LogError($"Invalid Delete Attempt In {nameof(DeleteDgPayDesignation)}");
                    return BadRequest("Submitted Data Is Invalid");
                }
                await _globalMaster.designations.DeleteAsync(obj);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteDgPayDesignation)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        //New Action 5/04/2024
        [HttpGet("GetDesigWiseBgtList")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDesigWiseBgtList(int companyId)
        {
            try
            {
                var response = await _globalMaster.designations.GetDesigWiseBgtList(companyId);
                if (response.IsSuccess)
                {
                    return Ok(response);
                }
                return NotFound(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDesigWiseBgtList)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpPost("AddOrEditDesignationBgt")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddOrEditDesignationBgt(DesignationBgtPayload obj)
        {
            try
            {
                var response = await _globalMaster.designations.AddOrEditDesignationBgt(obj);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(AddOrEditDesignationBgt)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpGet("GetDesignationWiseBudgetAll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDesignationWiseBudgetAll(int companyId)
        {
            try
            {
                var response = await _globalMaster.designations.GetDesignationWiseBudgetAll(companyId);
                if (response.IsSuccess)
                {
                    return Ok(response);
                }
                return NotFound(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDesignationWiseBudgetAll)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpGet("GetSelectedDesignationWiseBudget")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSelectedDesignationWiseBudget(int bgtId)
        {
            try
            {
                var response = await _globalMaster.designations.GetSelectedDesignationWiseBudget(bgtId);
                if (response.IsSuccess)
                {
                    return Ok(response);
                }
                return NotFound(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetSelectedDesignationWiseBudget)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpPost("AddOrEditDesignationWiseBudget")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddOrEditDesignationWiseBudget(DesignationBgt_Payload obj)
        {
            try
            {
                var response = await _globalMaster.designations.AddOrEditDesignationWiseBudget(obj);
                if (response.IsSuccess)
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(AddOrEditDesignationWiseBudget)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
    }
}
