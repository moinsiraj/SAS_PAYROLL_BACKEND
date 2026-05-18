using BLL.Interfaces;
using BLL.Utility;
using BOL.Models;
using CoreApiResponse;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using static BOL.Models.DgEmpIncrementInfo;

namespace Payroll_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpIncrementInfoesController : BaseController
    {
        private readonly IGlobalMaster _globalMaster;
        private readonly Dg_Common _dgCommon;
        private readonly SqlConnection _payCon;
        private readonly ILogger<EmpIncrementInfoesController> _logger;
        public EmpIncrementInfoesController(IGlobalMaster globalMaster, Dg_Common dgCommon, ILogger<EmpIncrementInfoesController> logger)
        {
            _globalMaster = globalMaster;
            _dgCommon = dgCommon;
            _payCon = new SqlConnection(Getway.Dg_Payroll);
            _logger = logger;
        }

        [HttpGet("get-Increment_list")]
        public async Task<IActionResult> Increment_list(int Compid)
        {
            try
            {
                var data = await _globalMaster.incrementInfoes.Increment_list(Compid);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(Increment_list)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpGet("get-Increment_Batch")]
        public async Task<IActionResult> Increment_Batch_list(int com_code, string inc_type, int dependon, decimal inc_gross, decimal inc_basic, decimal inc_grossPrct, decimal inc_BasicPrct, DateTime date, string uid, DateTime cutofdate)
        {
            try
            {
                var data = await _globalMaster.incrementInfoes.Increment_Batch_list(com_code, inc_type, dependon, inc_gross, inc_basic, inc_grossPrct, inc_BasicPrct, date, uid, cutofdate);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(Increment_Batch_list)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpGet]
        public IActionResult GetDgEmpIncrementInfos()
        {
            try
            {
                var data = _globalMaster.incrementInfoes.GetAll().Select(x => DgEmpIncrementInfo.DbToCustomModel(x)).ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgEmpIncrementInfos)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDgEmpIncrementInfo(int id)
        {
            try
            {
                var data = await _globalMaster.incrementInfoes.GetFirstOrDefaultAsync(x => x.inc_id == id);
                var nData = DgEmpIncrementInfo.DbToCustomModel(data);
                return Ok(nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgEmpIncrementInfo)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDgEmpIncrementInfo(int id, DgEmpIncrementInfo obj)
        {
            try
            {
                var nData = DgEmpIncrementInfo.CustomToDbModel(obj);
                await _globalMaster.incrementInfoes.UpdateAsync(nData);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PutDgEmpIncrementInfo)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostDgEmpIncrementInfo(DgEmpIncrementInfo obj)
        {
            try
            {
                var dtSalaryConf = this.GetEmployeeSalaryConf(obj.IncDate.ToString());
                bool isExistsSalConf = dtSalaryConf.Rows.Count > 0 ? true : false;
                if (!isExistsSalConf)
                {
                    var nData = DgEmpIncrementInfo.CustomToDbModel(obj);
                    await _globalMaster.incrementInfoes.AddAsync(nData);
                    return CreatedAtAction("GetDgEmpIncrementInfo", new { id = nData.inc_id }, nData);
                }
                return BadRequest("Salary Already Confirmed This Month(" + Convert.ToDateTime(obj.IncDate).ToString("MMMM-yyyy") + ") !!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PostDgEmpIncrementInfo)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDgEmpIncrementInfo(int id)
        {
            try
            {
                var obj = await _globalMaster.incrementInfoes.GetFirstOrDefaultAsync(c => c.inc_id == id);
                if (obj == null)
                {
                    _logger.LogError($"Invalid Delete Attempt In {nameof(DeleteDgEmpIncrementInfo)}");
                    return BadRequest("Submitted Data Is Invalid");
                }
                await _globalMaster.incrementInfoes.DeleteAsync(obj);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteDgEmpIncrementInfo)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        //New Action 11/30/2023
        [HttpPost("SaveEmployeeIncementData")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SaveEmployeeIncementData(EmployeeIncrementPayload obj)
        {
            try
            {
                var response = await _globalMaster.incrementInfoes.SaveEmployeeIncrementInfo(obj);
                if (response.IsSuccess == true)
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(SaveEmployeeIncementData)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpGet("GetEmpIncrementInfoList")]
        public async Task<IActionResult> GetEmpIncrementInfoList(int compid)
        {
            try
            {
                var data = await _globalMaster.incrementInfoes.GetEmpIncrementInfoList(compid);
                if (data !=null)
                {
                    return CustomResult("Data Loaded !!",data,HttpStatusCode.OK);
                }
                return CustomResult("Data Not Found !!", HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetEmpIncrementInfoList)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpPost("SaveEmpIncrementApprove")]
        public async Task<IActionResult> SaveEmpIncrementApprove(List<EmpIncrementApprovePayload> obj)
        {
            try
            {
                var message = await _globalMaster.incrementInfoes.SaveEmpIncrementApprove(obj);
                if (message == "Approved Successfully !!")
                {
                    return CustomResult(message, HttpStatusCode.OK);
                }
                return CustomResult(message, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(SaveEmpIncrementApprove)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpPost("DeleteEmpIncrement")]
        public async Task<IActionResult> DeleteEmpIncrement(List<EmpIncrementApprovePayload> obj)
        {
            try
            {
                var message = await _globalMaster.incrementInfoes.DeleteEmpIncrement(obj);
                if (message == "Delete Successfully !!")
                {
                    return CustomResult(message, HttpStatusCode.NoContent);
                }
                return CustomResult(message, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteEmpIncrement)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
        private DataTable GetEmployeeSalaryConf(string salDate)
        {
            var data = _dgCommon.get_InformationDataTable("SELECT ss_emp_serial FROM dg_pay_salarysheet WHERE MONTH(ss_date)=MONTH('" + salDate + "') AND YEAR(ss_date)=YEAR('" + salDate + "') AND ss_confirmed=1", _payCon);
            return data;
        }
    }
}
