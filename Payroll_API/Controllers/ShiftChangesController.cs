using BLL.Interfaces;
using BOL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Payroll_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShiftChangesController : ControllerBase
    {
        private readonly IGlobalMaster _globalMaster;
        private readonly ILogger<ShiftChangesController> _logger;
        public ShiftChangesController(IGlobalMaster globalMaster, ILogger<ShiftChangesController> logger)
        {
            _globalMaster = globalMaster;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetDgPayShiftChanges()
        {
            try
            {
                var data = _globalMaster.shiftChanges.GetAll().Select(x => DgPayShiftChange.DbToCustomModel(x)).ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgPayShiftChanges)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDgPayShiftChange(int id)
        {
            try
            {
                var data = await _globalMaster.shiftChanges.GetFirstOrDefaultAsync(x => x.sc_serial == id);
                var nData = DgPayShiftChange.DbToCustomModel(data);
                return Ok(nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgPayShiftChange)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDgPayShiftChange(int id, DgPayShiftChange obj)
        {
            try
            {
                var nData = ShiftChange_DbModel.CustomToDbModel(obj);
                await _globalMaster.shiftChanges.UpdateAsync(nData);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PutDgPayShiftChange)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost]
        public async Task<IActionResult> PostDgPayShiftChange(DgPayShiftChange obj)
        {
            try
            {
                var nData = ShiftChange_DbModel.CustomToDbModel(obj);
                await _globalMaster.shiftChanges.AddAsync(nData);
                return CreatedAtAction("GetDgPayShiftChange", new { id = nData.sc_serial }, nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PostDgPayShiftChange)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDgPayShiftChange(int id)
        {
            try
            {
                var obj = await _globalMaster.shiftChanges.GetFirstOrDefaultAsync(x => x.sc_serial == id);
                if (obj == null)
                {
                    _logger.LogError($"Invalid Delete Attempt In {nameof(DeleteDgPayShiftChange)}");
                    return BadRequest("Submitted Data Is Invalid");
                }
                await _globalMaster.shiftChanges.DeleteAsync(obj);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteDgPayShiftChange)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpGet("FilterBase_employeelist")]
        public async Task<IActionResult> FilterBase_employeelist(int? Compid = null, int? Department = null, int? section = null, int? Building = null, int? Floor = null, int? Line = null, int? Shift = null, int? Grade = null, int? salcat = null)
        {
            try
            {
                var data = await _globalMaster.shiftChanges.FilterBase_employeelist(Compid, Department, section, Building, Floor, Line, Shift, Grade, salcat);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(FilterBase_employeelist)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("get_shift_info")]
        public async Task<IActionResult> ShiftChanges_Batch(int CompID, int emp_no)
        {
            try
            {
                var data = await _globalMaster.shiftChanges.ShiftChanges_Batch(CompID, emp_no);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(ShiftChanges_Batch)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("get_ShiftSearch")]
        public async Task<IActionResult> ShiftSearch(int compid, DateTime s_date, DateTime E_date)
        {
            try
            {
                var data = await _globalMaster.shiftChanges.ShiftSearch(compid, s_date, E_date);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(ShiftSearch)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("get-Shift-list")]
        public async Task<IActionResult> GetShift(int compid)
        {
            try
            {
                var data = await _globalMaster.shiftChanges.GetShift(compid);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetShift)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("get-shiftlist_alldata")]
        public async Task<IActionResult> Getshiftlist_alldata(int compid)
        {
            try
            {
                var data = await _globalMaster.shiftChanges.Getshiftlist_alldata(compid);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(Getshiftlist_alldata)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("get-ShiftChanges_individual")]
        public async Task<IActionResult> ShiftChanges_Batch(int emp_serial, int oi_shift_OLD, int oi_shift, DateTime effectDate, String User, DateTime Udate)
        {
            try
            {
                var data = await _globalMaster.shiftChanges.ShiftChanges_Batch(emp_serial, oi_shift_OLD, oi_shift, effectDate, User, Udate);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(ShiftChanges_Batch)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("get-ShiftChanges_Batch")]
        public async Task<IActionResult> ShiftChanges_Batch(int oi_shift, DateTime effectDate, string User, DateTime Udate, int comid)
        {
            try
            {
                var data = await _globalMaster.shiftChanges.ShiftChanges_Batch(oi_shift, effectDate, User, Udate, comid);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(ShiftChanges_Batch)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("get-FilterBase_employee")]
        public async Task<IActionResult> employee_Info(int? Compid = null, int? Department = null, 
            int? section = null, int? Building = null, int? Floor = null, int? Line = null, int? Shift = null, 
            int? Grade = null, int? salcat = null, int? Newshift = null, DateTime? EffectDate = null)
        {
            try
            {
                var data = await _globalMaster.shiftChanges.employee_Info(Compid, Department, section, Building, Floor, Line, Shift, Grade, salcat, Newshift, EffectDate);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(employee_Info)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost("Dg_Save_ShiftRostaring")]
        public async Task<IActionResult> Dg_Save_ShiftRostaring_info(List<ShiftRostaring> srg)
        {
            try
            {
                await _globalMaster.shiftChanges.Dg_Save_ShiftRostaring_info(srg);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(Dg_Save_ShiftRostaring_info)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost("Dg_Save_ShiftRostaring2")]
        public IActionResult Dg_Save_ShiftRostaring_info2(List<ShiftRostaring2> srg)
        {
            try
            {
                var data = _globalMaster.shiftChanges.Dg_Save_ShiftRostaring_info2(srg);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(Dg_Save_ShiftRostaring_info2)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost("SaveShiftRusterAutoEmpWise")]
        public IActionResult SaveShiftRusterAutoEmpWise(ShiftRostaringAuto obj)
        {
            try
            {
                var data = _globalMaster.shiftChanges.Save_AutoShiftRostaring(obj);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(SaveShiftRusterAutoEmpWise)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpPost("SaveShiftRusterAutoEmpWiseProcess")]
        public IActionResult SaveShiftRusterAutoEmpWiseProcess(ShiftRostaringAuto obj)
        {
            try
            {
                var data = _globalMaster.shiftChanges.Save_AutoShiftRosterProcess(obj);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(SaveShiftRusterAutoEmpWiseProcess)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("GetShiftGroupList")]
        public async Task<IActionResult> GetShiftGroupList(int companyID)
        {
            try
            {
                var data = await _globalMaster.shiftChanges.GetShiftGroupList(companyID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(Getrosterhistory)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Shift-rosterhistory")]
        public async Task<IActionResult> Getrosterhistory(int comid)
        {
            try
            {
                var data = await _globalMaster.shiftChanges.Getrosterhistory(comid);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(Getrosterhistory)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Shift_rosterhistory_fromdateto_todate")]
        public async Task<IActionResult> GetRosterInfoFdateTdateWise(int comid, string from_date = null, string to_date = null)
        {
            try
            {
                var data = await _globalMaster.shiftChanges.GetRosterInfoFdateTdateWise(comid, from_date, to_date);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetRosterInfoFdateTdateWise)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        [HttpGet("Get_ShiftRosterhistory_shiftName")]
        public async Task<IActionResult> Get_ShiftRosterhistory_shiftName(int compID,int deptID=0,int secID=0)
        {
            try
            {
                var data = await _globalMaster.shiftChanges.GetShiftName(compID, deptID, secID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(Get_ShiftRosterhistory_shiftName)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
    }
}