using BLL.Interfaces;
using BOL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Payroll_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankInfosController : ControllerBase
    {
        private readonly IGlobalMaster _globalMaster;
        private readonly ILogger<BankInfosController> _logger;
        public BankInfosController(IGlobalMaster globalMaster, ILogger<BankInfosController> logger)
        {
            _globalMaster = globalMaster;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetDgPayBankInfos()
        {
            try
            {
                var data = _globalMaster.bankInfos.GetAll().Where(w => w.Showing_permission==true).Select(n => DgPayBankInfo.DbToCustomModel(n)).ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgPayBankInfos)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDgPayBankInfo(int id)
        {
            try
            {
                var data = await _globalMaster.bankInfos.GetFirstOrDefaultAsync(x => x.Bank_Code == id);
                var nData = DgPayBankInfo.DbToCustomModel(data);
                return Ok(nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgPayBankInfo)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDgPayBankInfo(int id, DgPayBankInfo obj)
        {
            try
            {
                var nData = DgPayBankInfo.CustonToDbModel(obj);
                await _globalMaster.bankInfos.UpdateAsync(nData);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PutDgPayBankInfo)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost]
        public async Task<IActionResult> PostDgPayBankInfo(DgPayBankInfo obj)
        {
            try
            {
                var nData = DgPayBankInfo.CustonToDbModel(obj);
                await _globalMaster.bankInfos.AddAsync(nData);
                return CreatedAtAction("GetDgPayBankInfo", new { id = nData.Bank_Code, }, nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PostDgPayBankInfo)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDgPayBankInfo(int id)
        {
            try
            {
                var obj = await _globalMaster.bankInfos.GetFirstOrDefaultAsync(c => c.Bank_Code == id);
                if (obj == null)
                {
                    _logger.LogError($"Invalid Delete Attempt In {nameof(DeleteDgPayBankInfo)}");
                    return BadRequest("Submitted Data Is Invalid");
                }
                await _globalMaster.bankInfos.DeleteAsync(obj);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteDgPayBankInfo)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
    }
}
