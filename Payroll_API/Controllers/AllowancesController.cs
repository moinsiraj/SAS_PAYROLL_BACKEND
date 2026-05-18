using BLL.Interfaces;
using BLL.Utility;
using BOL.Models;
using CoreApiResponse;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Net;

namespace Payroll_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AllowancesController : BaseController
    {
        private readonly IGlobalMaster _globalMaster;        
        private readonly ILogger<AllowancesController> _logger;
        public AllowancesController(IGlobalMaster globalMaster,Dg_Common dgCommon, ILogger<AllowancesController> logger)
        {
            _globalMaster = globalMaster;           
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetDgPayAllowances()
        {
            try
            {
                var data = _globalMaster.allowance.GetAll().Select(x => DgPayAllowance.DbToCustomModel(x)).ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgPayAllowances)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDgPayAllowance(int id)
        {
            try
            {
                var data = await _globalMaster.allowance.GetFirstOrDefaultAsync(x => x.al_serial == id);
                var nData = DgPayAllowance.DbToCustomModel(data);
                return Ok(nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgPayAllowance)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDgPayAllowance(int id, DgPayAllowance obj)
        {
            try
            {
                var nData = DgPayAllowance.CustonToDbModel(obj);
                await _globalMaster.allowance.UpdateAsync(nData);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PutDgPayAllowance)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostDgPayAllowance(DgPayAllowancePostPayload obj)
        {
            try
            {               
                var response = await _globalMaster.allowance.SaveEmpAllowance(obj);
                if (response.IsSuccess==true)
                {
                    return CustomResult(response.Message, HttpStatusCode.OK);
                }
                return CustomResult(response.Message, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PostDgPayAllowance)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDgPayAllowance(int id)
        {
            try
            {
                var obj = await _globalMaster.allowance.GetFirstOrDefaultAsync(c => c.al_serial == id);
                if (obj == null)
                {
                    _logger.LogError($"Invalid Delete Attempt In {nameof(DeleteDgPayAllowance)}");
                    return BadRequest("Submitted Data Is Invalid");
                }
                await _globalMaster.allowance.DeleteAsync(obj);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteDgPayAllowance)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("get-AllowanceType")]
        public async Task<IActionResult> GetAllowanceType(string AllowanceType)
        {
            try
            {
                var data = await _globalMaster.allowance.GetAllowanceType(AllowanceType);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetAllowanceType)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("get-EmpwiseAllowance")]
        public async Task<IActionResult> GetEmpwiseAllowance(decimal al_emp_serial, string alDate)
        {
            try
            {
                var data = await _globalMaster.allowance.GetEmpWiseAllowance(al_emp_serial, alDate);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetEmpwiseAllowance)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("get-AllowanceDatewise")]
        public async Task<IActionResult> GetAllowanceDatewise(int CompID, string AllowanceType, DateTime StartDate, DateTime EndDate)
        {
            try
            {
                var data = await _globalMaster.allowance.GetAllowanceDateWise(CompID, AllowanceType, StartDate, EndDate);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetEmpwiseAllowance)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
    }
}
