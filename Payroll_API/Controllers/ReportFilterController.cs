using BLL.Interfaces;
using BOL.Models;
using CoreApiResponse;
using Microsoft.AspNetCore.Mvc;

namespace Payroll_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportFilterController : BaseController
    {
        private readonly IGlobalMaster _globalMaster;
        private readonly ILogger<ReportFilterController> _logger;
        public ReportFilterController(IGlobalMaster globalMaster, ILogger<ReportFilterController> logger)
        {
            _globalMaster = globalMaster;
            _logger = logger;
        }
        
        [HttpPost("GetEmpNoFilter")]
        public async Task<IActionResult> GetFilterEmpDataNew(EmployeeCheckPaylod obj)
        {
            try
            {
                var response = await _globalMaster.reportFilterManager.GetFilterEmpData(obj);
                if (response.IsSuccess)
                {
                    return Ok(new { response.IsSuccess, response.dataTable, response.Message });
                }
                return NotFound(new { response.IsSuccess, response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetFilterEmpDataNew)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpPost("Save_EmpReportParameter")]
        public async Task<IActionResult> Save_EmpReportParameter(ReportParameterModel obj)
        {
            try
            {
                if (obj != null)
                {
                    bool isSuccess = await _globalMaster.reportFilterManager.Save_EmpReportParameter(obj);
                    if (isSuccess)
                    {
                        return Ok();
                    }
                    return BadRequest();
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(Save_EmpReportParameter)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost("Save_LevReportParameter")]
        public async Task<IActionResult> Save_LevReportParameter(ReportParameterModel obj)
        {
            try
            {
                if (obj != null)
                {
                    bool isSuccess = await _globalMaster.reportFilterManager.Save_LevReportParameter(obj);
                    if (isSuccess)
                    {
                        return Ok();
                    }
                    return BadRequest();
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(Save_LevReportParameter)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost("Save_AttReportParameter")]
        public async Task<IActionResult> Save_AttReportParameter(ReportPrintPayload obj)
        {
            try
            {
                if (obj != null)
                {
                    bool isSuccess = await _globalMaster.reportFilterManager.Save_AttReportParameter(obj);
                    if (isSuccess)
                    {
                        return Ok();
                    }
                    return BadRequest();
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(Save_AttReportParameter)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost("Save_SalReportParameter")]
        public async Task<IActionResult> Save_SalReportParameter(ReportParameterModel obj)
        {
            try
            {
                if (obj != null)
                {
                    bool isSuccess = await _globalMaster.reportFilterManager.Save_SalReportParameter(obj);
                    if (isSuccess)
                    {
                        return Ok();
                    }
                    return BadRequest();
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(Save_SalReportParameter)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        //New Action 11/23/2023GetFilterCheckEmpNo
        [HttpGet("GetDepartment_ForRepFilter")]
        public async Task<IActionResult> GetDepartment_ForRepFilter(int companyID)
        {
            try
            {
                var data = await _globalMaster.reportFilterManager.GetDepartmentForRepFiltter(companyID);
                if (data.Rows.Count > 0)
                {
                    return Ok(data);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDepartment_ForRepFilter)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost("GetSections_ForRepFilter")]
        public async Task<IActionResult> GetSections_ForRepFiltter(DepartmenIdtArr obj)
        {
            try
            {
                var data = await _globalMaster.reportFilterManager.GetSectionsForRepFiltter(obj);
                if (data.Rows.Count > 0)
                {
                    return Ok(data);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetSections_ForRepFiltter)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("GetBulding_ForRepFilter")]
        public async Task<IActionResult> GetBulding_ForRepFilter(int companyID)
        {
            try
            {
                var data = await _globalMaster.reportFilterManager.GetBuldingForRepFiltter(companyID);
                if (data.Rows.Count > 0)
                {
                    return Ok(data);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetBulding_ForRepFilter)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost("GetFloor_ForRepFilter")]
        public async Task<IActionResult> GetFloor_ForRepFilter(floorIdPayload obj)
        {
            try
            {
                var data = await _globalMaster.reportFilterManager.GetFloorForRepFiltter(obj);
                if (data.Rows.Count > 0)
                {
                    return Ok(data);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetFloor_ForRepFilter)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost("GetLines_ForRepFilter")]
        public async Task<IActionResult> GetLines_ForRepFilter(lineIdPayload obj)
        {
            try
            {
                var data = await _globalMaster.reportFilterManager.GetLineForRepFiltter(obj);
                if (data.Rows.Count > 0)
                {
                    return Ok(data);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetLines_ForRepFilter)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost("GetShift_ForRepFilter")]
        public async Task<IActionResult> GetShift_ForRepFilter(ShiftIdPayload obj)
        {
            try
            {
                var data = await _globalMaster.reportFilterManager.GetShiftForRepFiltter(obj);
                if (data.Rows.Count > 0)
                {
                    return Ok(data);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetShift_ForRepFilter)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost("GetGrade_ForRepFilter")]
        public async Task<IActionResult> GetGrade_ForRepFilter(GradeIdPayload obj)
        {
            try
            {
                var data = await _globalMaster.reportFilterManager.GetGradeForRepFiltter(obj);
                if (data.Rows.Count > 0)
                {
                    return Ok(data);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetGrade_ForRepFilter)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost("GetSalCat_ForRepFilter")]
        public async Task<IActionResult> GetSalCat_ForRepFilter(SalCatIdPayload obj)
        {
            try
            {
                var data = await _globalMaster.reportFilterManager.GetSalCatForRepFiltter(obj);
                if (data.Rows.Count > 0)
                {
                    return Ok(data);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetSalCat_ForRepFilter)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpPost("GetSection_ForRepFilter")]
        public async Task<IActionResult> GetSection_ForRepFiltter(DepartmenIdtArr obj)
        {
            try
            {
                var data = await _globalMaster.reportFilterManager.GetRepFiltterSection(obj);
                if (data.Rows.Count >0)
                {
                    return Ok(data);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetSection_ForRepFiltter)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost("GetLine_ForRepFilter")]
        public async Task<IActionResult> GetLine_ForRepFiltter(floorIdArr obj)
        {
            try
            {
                var data = await _globalMaster.reportFilterManager.GetRepFiltterLine(obj);
                if (data != null)
                {
                    return Ok(data);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetLine_ForRepFiltter)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost("GetFilterCheckEmpNo")]
        public async Task<IActionResult> GetFilterCheckEmpNo(Rep_EmpIdNoCheckPaylod obj)
        {
            try
            {
                var response = await _globalMaster.reportFilterManager.GetFilterCheckEmpNo(obj);
                if (response.IsSuccess)
                {
                    return Ok(new { response.IsSuccess, response.dataTable, response.Message });
                }
                return NotFound(new { response.IsSuccess, response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetFilterCheckEmpNo)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost("Print_SalReportData")]
        public async Task<IActionResult> Print_SalReportData(ReportPrintPayload obj)
        {
            try
            {
                if (obj != null)
                {
                    bool isSuccess = await _globalMaster.reportFilterManager.Print_SalReportData(obj);
                    if (isSuccess)
                    {
                        return Ok();
                    }
                    return BadRequest();
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(Print_SalReportData)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("GetAllInactiveReason")]
        public async Task<IActionResult> GetAllInactiveReason()
        {
            try
            {
                var data = await _globalMaster.reportFilterManager.GetEmployeeInactiveReason();
                if (data.Rows.Count > 0)
                {
                    return Ok(data);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetAllInactiveReason)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
    }
}