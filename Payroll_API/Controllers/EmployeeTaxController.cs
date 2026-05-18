using BLL.Interfaces;
using BLL.Utility;
using BOL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Payroll_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeTaxController : ControllerBase
    {
        private readonly IGlobalMaster _globalMaster;
        private readonly ILogger<EmployeeTaxController> _logger;
        private readonly Dg_Common _dgCommon;
        public EmployeeTaxController(IGlobalMaster globalMaster, ILogger<EmployeeTaxController> logger, Dg_Common dgCommon)
        {
            _globalMaster = globalMaster;
            _logger = logger;
            _dgCommon = dgCommon;
        }

        [HttpGet("GetCompanyForTax")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCompanyForTax(string userName)
        {
            try
            {
                var response = await _globalMaster.employeeTax.GetCompanyForTax(userName);
                if (response.IsSuccess)
                {
                    return Ok(new { isSuccess = response.IsSuccess, statusCode = 200, data = response.dataTable });
                }
                return NotFound(new { isSuccess = response.IsSuccess, statusCode = 404, response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetCompanyForTax)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpGet("GetTaxYearInfo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTaxYearInfo()
        {
            try
            {
                var response = await _globalMaster.employeeTax.GetTaxYearInfo();
                if (response.IsSuccess)
                {
                    return Ok(new { isSuccess = response.IsSuccess, statusCode = 200, data = response.dataTable });
                }
                return NotFound(new { isSuccess = response.IsSuccess, statusCode = 404, response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetTaxYearInfo)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpGet("GetEmployeeInfoForTax")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetEmployeeInfoForTax(int compid, string empPrefix=null)
        {
            try
            {
                var response = await _globalMaster.employeeTax.GetEmployeeInfoForTax(compid, empPrefix);
                if (response.IsSuccess)
                {
                    return Ok(new { isSuccess = response.IsSuccess, statusCode = 200, data = response.dataTable });
                }
                return NotFound(new { isSuccess = response.IsSuccess, statusCode = 404, response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetEmployeeInfoForTax)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpGet("GetExistingTaxInfo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetExistingTaxInfo(int taxYear, int empSL)
        {
            try
            {
                var response = await _globalMaster.employeeTax.GetExistingTaxInfo(taxYear, empSL);
                if (response.IsSuccess)
                {
                    return Ok(new { isSuccess = response.IsSuccess, statusCode = 200, data = response.dataTable });
                }
                return NotFound(new { isSuccess = response.IsSuccess, statusCode = 404, response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetExistingTaxInfo)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpPost("SaveEmployeeTaxInfo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SaveEmployeeTaxInfo(EmployeeTaxModel obj)
        {
            try
            {
                var response = await _globalMaster.employeeTax.SaveEmployeeTaxInfo(obj);
                if (response.IsSuccess)
                {
                    return Ok(new { isSuccess = response.IsSuccess, statusCode = 200, message = response.Message });
                }
                return BadRequest(new { isSuccess = response.IsSuccess, statusCode = 400, message = response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(SaveEmployeeTaxInfo)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpPost("GetEmployeeTaxInfoReport")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetEmployeeTaxInfoReport(EmployeeTaxReport obj)
        {
            try
            {
                var response = await _globalMaster.employeeTax.GetEmployeeTaxInfoReport(obj);
                if (string.IsNullOrEmpty(response.message))
                {
                    return File(response.reportByte, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(obj.reportType));
                }
                return BadRequest(new { response.message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetEmployeeTaxInfoReport)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpGet("GetEmployeeInfoFromTax")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetEmployeeInfoFromTax(int compid, int taxYear, string empPrefix = null)
        {
            try
            {
                var response = await _globalMaster.employeeTax.GetEmployeeInfoFromTax(compid, taxYear, empPrefix);
                if (response.IsSuccess)
                {
                    return Ok(new { isSuccess = response.IsSuccess, statusCode = 200, data = response.dataTable });
                }
                return NotFound(new { isSuccess = response.IsSuccess, statusCode = 404, response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetEmployeeInfoFromTax)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpGet("GetBankInfoForTax")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBankInfoForTax()
        {
            try
            {
                var response = await _globalMaster.employeeTax.GetBankInfoForTax();
                if (response.IsSuccess)
                {
                    return Ok(new { isSuccess = response.IsSuccess, statusCode = 200, data = response.dataTable });
                }
                return NotFound(new { isSuccess = response.IsSuccess, statusCode = 404, response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetBankInfoForTax)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpGet("GetBankInfoBranchForTax")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBankInfoBranchForTax(int bankID)
        {
            try
            {
                var response = await _globalMaster.employeeTax.GetBankInfoBranchForTax(bankID);
                if (response.IsSuccess)
                {
                    return Ok(new { isSuccess = response.IsSuccess, statusCode = 200, data = response.dataTable });
                }
                return NotFound(new { isSuccess = response.IsSuccess, statusCode = 404, response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetBankInfoBranchForTax)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpPost("SaveTaxChallan")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SaveTaxChallan([FromForm] EmployeeTaxChallan obj)
        {
            try
            {
                var response = await _globalMaster.employeeTax.SaveTaxChallan(obj);
                if (response.IsSuccess)
                {
                    return Ok(new { isSuccess = response.IsSuccess, statusCode = 200, message = response.Message, data = response.dataTable });
                }
                return BadRequest(new { isSuccess = response.IsSuccess, statusCode = 400, message = response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(SaveTaxChallan)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpGet("GetEmpTaxChallanInfo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetEmpTaxChallanInfo(int compid, int empSerial, int taxYear)
        {
            try
            {
                var response = await _globalMaster.employeeTax.GetEmpTaxChallanInfo(compid, empSerial, taxYear);
                if (response.IsSuccess)
                {
                    return Ok(new { isSuccess = response.IsSuccess, statusCode = 200, data = response.dataTable });
                }
                return NotFound(new { isSuccess = response.IsSuccess, statusCode = 404, response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetEmpTaxChallanInfo)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpDelete("DeleteTaxChallan")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteTaxChallan(int chlnid)
        {
            try
            {
                var response = await _globalMaster.employeeTax.DeleteTaxChallan(chlnid);
                if (response.IsSuccess)
                {
                    return Ok(new { isSuccess = response.IsSuccess, statusCode = 200, message = response.Message, data = response.dataTable });
                }
                return BadRequest(new { isSuccess = response.IsSuccess, statusCode = 400, message = response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteTaxChallan)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpPut("UpdateTaxChallan")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateTaxChallan([FromForm] EmployeeTaxChallan obj)
        {
            try
            {
                var response = await _globalMaster.employeeTax.UpdateTaxChallan(obj);
                if (response.IsSuccess)
                {
                    return Ok(new { isSuccess = response.IsSuccess, statusCode = 200, message = response.Message, data = response.dataTable });
                }
                return BadRequest(new { isSuccess = response.IsSuccess, statusCode = 400, message = response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(UpdateTaxChallan)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpPost("GetEmployeeTaxChallanInfoReport")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetEmployeeTaxChallanInfoReport(EmployeeTaxReport obj)
        {
            try
            {
                var response = await _globalMaster.employeeTax.GetEmployeeTaxChallanInfoReport(obj);
                if (string.IsNullOrEmpty(response.message))
                {
                    return File(response.reportByte, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(obj.reportType));
                }
                return BadRequest(new { response.message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetEmployeeTaxChallanInfoReport)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpGet("GetAllBankBranch")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllBankBranch(int bankid)
        {
            try
            {
                var response = await _globalMaster.employeeTax.GetAllBankBranch(bankid);
                if (response.IsSuccess)
                {
                    return Ok(new { isSuccess = response.IsSuccess, statusCode = 200, data = response.dataTable });
                }
                return NotFound(new { isSuccess = response.IsSuccess, statusCode = 404, message = response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetAllBankBranch)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpPost("AddOrEditBankBranch")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddOrEditBankBranch(BankBranchPayload obj)
        {
            try
            {
                var response = await _globalMaster.employeeTax.AddOrEditBankBranch(obj);
                if (response.IsSuccess)
                {
                    return Ok(new { isSuccess = response.IsSuccess, statusCode = 200, message = response.Message, data = response.dataTable });
                }
                return BadRequest(new { isSuccess = response.IsSuccess, statusCode = 400, message = response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(AddOrEditBankBranch)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
    }
}
