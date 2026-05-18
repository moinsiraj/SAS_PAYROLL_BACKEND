using BLL.Interfaces;
using BOL.Models;
using CoreApiResponse;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Payroll_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManualAttendancesController : BaseController
    {
        private readonly IGlobalMaster _globalMaster;
        private readonly ILogger<ManualAttendancesController> _logger;
        public ManualAttendancesController(IGlobalMaster globalMaster, ILogger<ManualAttendancesController> logger)
        {
            _globalMaster = globalMaster;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetDgPayAttendances()
        {
            try
            {
                var data = _globalMaster.manualAttendances.GetAll().Select(x => DgPayAttendance.DbToCustomModel(x)).ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgPayAttendances)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDgPayAttendance(int id)
        {
            try
            {
                var data = await _globalMaster.manualAttendances.GetFirstOrDefaultAsync(x => x.at_emp_serial == id);
                var nData = DgPayAttendance.DbToCustomModel(data);
                return Ok(nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgPayAttendance)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDgPayAttendance(int id, DgPayAttendance obj)
        {
            try
            {
                var nData = DgPayAttendance.CustomToDbModel(obj);
                await _globalMaster.manualAttendances.UpdateAsync(nData);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PutDgPayAttendance)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost]
        public async Task<IActionResult> PostDgPayAttendance(DgPayAttendance obj)
        {
            try
            {
                var nData = DgPayAttendance.CustomToDbModel(obj);
                await _globalMaster.manualAttendances.AddAsync(nData);
                return CreatedAtAction("GetDgPayAttendance", new { id = nData.at_emp_serial }, nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PostDgPayAttendance)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDgPayAttendance(int id)
        {
            try
            {
                var obj = await _globalMaster.manualAttendances.GetFirstOrDefaultAsync(c => c.at_emp_serial == id);
                if (obj == null)
                {
                    _logger.LogError($"Invalid Delete Attempt In {nameof(DeleteDgPayAttendance)}");
                    return BadRequest("Submitted Data Is Invalid");
                }
                await _globalMaster.manualAttendances.DeleteAsync(obj);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteDgPayAttendance)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpGet("get-ManualAttendance")]
        public async Task<IActionResult> ManualAttendance(int Emp_serial, DateTime date, decimal intime, DateTime outdate, decimal outtime)
        {
            try
            {
                var data = await _globalMaster.manualAttendances.ManualAttendance(Emp_serial, date, intime, outdate, outtime);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(ManualAttendance)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("get-ManualAttendance_select")]
        public async Task<IActionResult> ManualAttendance_select(int Emp_serial, DateTime date)
        {
            try
            {
                var data = await _globalMaster.manualAttendances.ManualAttendance_select(Emp_serial, date);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(ManualAttendance_select)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("get-ManualAttendance_viw")]
        public async Task<IActionResult> ManualAttendance_viw(int comp, DateTime Sdate, DateTime Edate, int IND)
        {
            try
            {
                var data = await _globalMaster.manualAttendances.ManualAttendance_viw(comp, Sdate, Edate, IND);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(ManualAttendance_viw)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("get-ManualAttendancefilter")]
        public async Task<IActionResult> ManualAttendancefilter(int? Compid = null, int? Department = null, int? section = null, int? Building = null, int? Floor = null, int? Line = null,int? salCat=null)
        {
            try
            {
                var data = await _globalMaster.manualAttendances.ManualAttendancefilter(Compid, Department, section, Building, Floor, Line, salCat);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(ManualAttendancefilter)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("get-shiftlist")]
        public async Task<IActionResult> shiftlist(int CompID)
        {
            try
            {
                var data = await _globalMaster.manualAttendances.shiftlist(CompID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(shiftlist)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost("get_seleted_AttList")]
        public async Task<IActionResult> Get_mAttList(List<ManualAttPara> para)
        {
            try
            {
                var data = await _globalMaster.manualAttendances.GetManualAttList(para);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(Get_mAttList)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost("ManualAtt_Save")]
        public async Task<IActionResult> Save_ManualAtt(List<ManualAttSavePara> para)
        {
            try
            {
                var response = await _globalMaster.manualAttendances.SaveManualAtt(para);
                if (response.IsSuccess)
                {
                    return Ok(new { isSuccess = response.IsSuccess, statusCode = 200, message = response.Message });
                }
                return BadRequest(new { isSuccess = response.IsSuccess, statusCode = 400, message = response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(Save_ManualAtt)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost("UpdateMenualAttendanceAbsent")]
        public async Task<IActionResult> UpdateMenualAttendanceAbsent(List<AttendanceAbsentPaylod> objArr)
        {
            try
            {
                string message = await _globalMaster.manualAttendances.UpdateMenualAttendanceAbsent(objArr);
                if (message == "Save Successfully !!")
                {
                    return CustomResult(message, System.Net.HttpStatusCode.OK);
                }
                return CustomResult(message, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(UpdateMenualAttendanceAbsent)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost("GetEmployeeListForAttDel")]
        public async Task<IActionResult> GetEmployeeListForAttDel(GetEmployeeListPayload obj)
        {
            try
            {
                var data = await _globalMaster.manualAttendances.GetEmployeeListForAttDel(obj);
                if (data !=null)
                {
                    return CustomResult("Data Loaded !!", data, HttpStatusCode.OK);
                }
                return CustomResult("Data Not Found !!", HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetEmployeeListForAttDel)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpPost("GetEmpFromAttendanceForOutTimeChange")]
        public async Task<IActionResult> GetEmpFromAttendanceForOutTimeChange(GetEmpAttendanceListPayload obj)
        {
            try
            {
                var response = await _globalMaster.manualAttendances.GetEmpFromAttendanceForOutTimeChange(obj);
                if (response.IsSuccess)
                {
                    return Ok(response);
                }
                return NotFound(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetEmpFromAttendanceForOutTimeChange)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpPost("SaveEmpAttendanceChange")]
        public IActionResult SaveEmpAttendanceChange(EmpAttendanceChangeSavePayload obj)
        {
            try
            {
                var response = _globalMaster.manualAttendances.SaveEmpAttendanceChange(obj);
                if (response.IsSuccess)
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(SaveEmpAttendanceChange)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpPost("GetEmpFromAttendanceForInTimeChange")]
        public async Task<IActionResult> GetEmpFromAttendanceForInTimeChange(GetEmpAttendanceListPayload obj)
        {
            try
            {
                var response = await _globalMaster.manualAttendances.GetEmpFromAttendanceForInTimeChange(obj);
                if (response.IsSuccess)
                {
                    return Ok(response);
                }
                return NotFound(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetEmpFromAttendanceForInTimeChange)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpPost("SaveEmpAttendanceInTimeChange")]
        public IActionResult SaveEmpAttendanceInTimeChange(EmpAttendanceChangeSavePayload obj)
        {
            try
            {
                var response = _globalMaster.manualAttendances.SaveEmpAttendanceInTimeChange(obj);
                if (response.IsSuccess)
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(SaveEmpAttendanceInTimeChange)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
    }
}
