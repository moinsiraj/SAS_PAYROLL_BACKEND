using BLL.Interfaces;
using BOL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Payroll_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly IGlobalMaster _globalMaster;
        private readonly ILogger<LoansController> _logger;
        public LoansController(IGlobalMaster globalMaster, ILogger<LoansController> logger)
        {
            _globalMaster = globalMaster;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetDgPayLoans()
        {
            try
            {
                var data = _globalMaster.loans.GetAll().Select(x => DgPayLoan.DbToCustomModel(x)).ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgPayLoans)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDgPayLoan(int id)
        {
            try
            {
                var data = await _globalMaster.loans.GetFirstOrDefaultAsync(x => x.l_loanserial == id);
                var nData = DgPayLoan.DbToCustomModel(data);
                return Ok(nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgPayLoan)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDgPayLoan(int id, DgPayLoan obj)
        {
            try
            {
                var nData = DgPayLoan.CustomToDbModel(obj);
                await _globalMaster.loans.UpdateAsync(nData);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PutDgPayLoan)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost]
        public async Task<IActionResult> PostDgPayLoan(DgPayLoan obj)
        {
            try
            {
                var nData = DgPayLoan.CustomToDbModel(obj);
                await _globalMaster.loans.AddAsync(nData);
                return CreatedAtAction("GetDgPayLoan", new { id = nData.l_loanserial }, nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PostDgPayLoan)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDgPayLoan(int id)
        {
            try
            {
                var obj = await _globalMaster.loans.GetFirstOrDefaultAsync(c => c.l_loanserial == id);
                if (obj == null)
                {
                    _logger.LogError($"Invalid Delete Attempt In {nameof(DeleteDgPayLoan)}");
                    return BadRequest("Submitted Data Is Invalid");
                }
                await _globalMaster.loans.DeleteAsync(obj);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteDgPayLoan)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
    }
}
