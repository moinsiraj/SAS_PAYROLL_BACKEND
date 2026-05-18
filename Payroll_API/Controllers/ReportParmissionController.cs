using BLL.Interfaces;
using BLL.Utility;
using BOL.Models;
using CoreApiResponse;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Payroll_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportParmissionController : BaseController
    {
        private readonly IGlobalMaster _globalMaster;
        private readonly Dg_Common _dgCommon;
        private readonly ILogger<ReportParmissionController> _logger;
        public ReportParmissionController(IGlobalMaster globalMaster, Dg_Common dgCommon, ILogger<ReportParmissionController> logger)
        {
            _globalMaster = globalMaster;
            _dgCommon = dgCommon;
            _logger = logger;
        }

        [HttpGet("GetUserWise_ReportList")]
        public async Task<IActionResult> GetUserWise_ReportPermission(string userName,string reportType)
        {
            try
            {
                var data = await _globalMaster.reportPermission.GetUserWise_reportlist(userName, reportType);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetUserWise_ReportPermission)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpGet("GetReportList")]
        public async Task<IActionResult> GetUserWiseTotalReportList(string userName)
        {
            try
            {
                var data = await _globalMaster.reportPermission.GetReportListByCatagory(userName);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetUserWiseTotalReportList)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpPost("UpdateReportPermission")]
        public async Task<IActionResult> UpdateReportPermission(ReportPermissionUpdate obj)
        {
            try
            {
                bool isSuccess = await _globalMaster.reportPermission.UpdateUserWiseReportPermission(obj);
                if (isSuccess)
                {
                    return NoContent();
                }
                return Problem("fail");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(UpdateReportPermission)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        //New Action
        [HttpGet("GetTotalReportList")]
        public async Task<IActionResult> GetTotalReportList()
        {
            try
            {
                var listData = await _globalMaster.reportPermission.GetTotalReportList();
                //var dataTable = _dgCommon.ListToDataTable(listData.ToList());
                return Ok(listData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetTotalReportList)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpGet("GetReportUserDropdown")]
        public async Task<IActionResult> GetReportUserDropdown(int compid)
        {
            try
            {
                var data = await _globalMaster.reportPermission.GetReportUserDropdown(compid);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetReportUserDropdown)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpGet("GetPermittedReportByUser")]
        public async Task<IActionResult> GetPermittedReport(int compid,string userName)
        {
            try
            {
                var data = await _globalMaster.reportPermission.GetPermissionReportByUser(compid, userName);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetPermittedReport)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpGet("GetReportPermissionGroupList")]
        public async Task<IActionResult> GetReportPermissionGroup(int compid)
        {
            try
            {
                var data = await _globalMaster.reportPermission.GetReportPermissionGroup(compid);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetReportPermissionGroup)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpPost("SaveReportPermission")]
        public async Task<IActionResult> SaveReportPermission(ReportPermissionPayload obj)
        {
            try
            {
                string isSuccess = await _globalMaster.reportPermission.Save_Pay_ReportPermission(obj);
                if (isSuccess== "User Wise Save Successfully !!" || isSuccess == "Group Wise Save Successfully !!")
                {
                    return CustomResult(isSuccess, HttpStatusCode.OK);
                }
                return CustomResult(isSuccess, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(SaveReportPermission)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpGet("GetUserAndGroupWise_reportlist")]
        public async Task<IActionResult> GetUserAndGroupWise_reportlist(int compid,string userName, string reportType)
        {
            try
            {
                var data = await _globalMaster.reportPermission.GetUserAndGroupWise_reportlist(compid,userName, reportType);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetUserAndGroupWise_reportlist)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpPost("SaveReportPermissionGroup")]
        public async Task<IActionResult> SaveReportPermissionGroup(ReportPermissionGroupPayload obj)
        {
            try
            {
                string isSuccess = await _globalMaster.reportPermission.Save_Pay_ReportGroup(obj);
                if (isSuccess == "Submitted Successfully !!")
                {
                    return CustomResult(isSuccess, HttpStatusCode.OK);
                }
                return CustomResult(isSuccess, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(SaveReportPermissionGroup)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpGet("GetPermissionReportByGroupID")]
        public async Task<IActionResult> GetPermissionReportByGroupID(int compid, int groupId)
        {
            try
            {
                var data = await _globalMaster.reportPermission.GetPermissionReportByGroupID(compid, groupId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetPermissionReportByGroupID)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
    }
}