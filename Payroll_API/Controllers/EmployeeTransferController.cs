using BLL.Interfaces;
using BOL.Models;
using CoreApiResponse;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Payroll_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeTransferController : BaseController
    {
        private readonly IGlobalMaster _globalMaster;
        private readonly ILogger<EmployeeTransferController> _logger;
        public EmployeeTransferController(IGlobalMaster globalMaster, ILogger<EmployeeTransferController> logger)
        {
            _globalMaster = globalMaster;
            _logger = logger;
        }

        [HttpPost("SaveEmployeeTransfer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SaveEmployeeTransfer(EmployeeTransferModel obj)
        {
            try
            {
                var response = await _globalMaster.employeeTransfer.SaveEmployeeTransfer(obj);
                if (response.IsSuccess)
                {
                    return CustomResult(response.Message, HttpStatusCode.OK);
                }
                return CustomResult(response.Message, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(SaveEmployeeTransfer)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("GetEmployeeTransferInfo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]       
        public async Task<IActionResult> GetEmployeeTransferInfo(int companyID, int employeeID)
        {
            try
            {
                var response = await _globalMaster.employeeTransfer.GetEmployeeTransferList(companyID, employeeID);
                if (response.Rows.Count > 0)
                {
                    return CustomResult("Data Loaded !!", response, HttpStatusCode.OK);
                }
                return CustomResult("Data Not Loaded !!", HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetEmployeeTransferInfo)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }
       
        [HttpGet("GetEmployeeTransferToCompany")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetEmployeeTransferToCompany(int fCompanyID)
        {
            try
            {
                var response = await _globalMaster.employeeTransfer.GetTransferToCompany(fCompanyID);
                if (response.Rows.Count > 0)
                {
                    return CustomResult("Data Loaded !!", response, HttpStatusCode.OK);
                }
                return CustomResult("Data Not Loaded !!", HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetEmployeeTransferToCompany)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut("UpdateEmployeeTransferApprove")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateEmployeeTransferApprove(int transID, string userName)
        {
            try
            {
                var response = await _globalMaster.employeeTransfer.ApproveEmployeeTransfer(transID, userName);
                if (response.IsSuccess==true)
                {
                    return CustomResult(response.Message, HttpStatusCode.OK);
                }
                return CustomResult(response.Message, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(UpdateEmployeeTransferApprove)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete("DeleteEmployeeTransfer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteEmployeeTransfer(int transID)
        {
            try
            {
                var response = await _globalMaster.employeeTransfer.DeleteEmployeeTransfer(transID);
                if (response.IsSuccess == true)
                {
                    return CustomResult(response.Message, HttpStatusCode.OK);
                }
                return CustomResult(response.Message, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteEmployeeTransfer)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }
    }
}
