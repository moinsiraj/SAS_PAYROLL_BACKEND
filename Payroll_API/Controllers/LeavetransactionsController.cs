using BLL.Interfaces;
using BOL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Payroll_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeavetransactionsController : ControllerBase
    {
        private readonly IGlobalMaster _globalMaster;
        private readonly ILogger<LeavetransactionsController> _logger;
        public LeavetransactionsController(IGlobalMaster globalMaster, ILogger<LeavetransactionsController> logger)
        {
            _globalMaster = globalMaster;
            _logger = logger;
        }

        [HttpGet("get_leave_info_comdatewise")]
        public async Task<IActionResult> Getleave_info_comdatewise(int CompID, DateTime Sdate, DateTime Edate)
        {
            try
            {
                var data = await _globalMaster.leavetransactions.Getleave_info_comdatewise(CompID, Sdate, Edate);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(Getleave_info_comdatewise)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpGet("get_EmployeeNo")]
        public async Task<IActionResult> GetEmployeeNo(int compID, int levYear)
        {
            try
            {
                var data = await _globalMaster.leavetransactions.GetEmployeeNo(compID, levYear);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetEmployeeNo)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpGet]
        public IActionResult GetDgPayLeavetransactions()
        {
            try
            {
                var data = _globalMaster.leavetransactions.GetAll().Select(x => DgPayLeavetransaction.DbToCustomModel(x)).ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgPayLeavetransactions)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDgPayLeavetransaction(DateTime id)
        {
            try
            {
                var data = await _globalMaster.leavetransactions.GetFirstOrDefaultAsync(x => x.ltr_date == id);
                var nData = DgPayLeavetransaction.DbToCustomModel(data);
                return Ok(nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgPayLeavetransaction)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDgPayLeavetransaction(DateTime id, DgPayLeavetransaction obj)
        {
            try
            {
                var nData = DgPayLeavetransaction.CustomToDbModel(obj);
                await _globalMaster.leavetransactions.UpdateAsync(nData);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PutDgPayLeavetransaction)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveLeaveTransaction(LeaveTransactionPayload obj)
        {
            try
            {
                var response = await _globalMaster.leavetransactions.SaveLeaveLeavetransaction(obj);
                if (response.IsSuccess)
                {
                    return Ok(new {response.IsSuccess,response.Message});
                }
                return BadRequest(new { response.IsSuccess, response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(SaveLeaveTransaction)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpPost("Leavepost_bulk")]
        public async Task<IActionResult> SaveLeaveTransaction_Bulk(LeaveTransaction_BulkPayload obj)
        {
            try
            {
                var response = await _globalMaster.leavetransactions.SaveLeaveLeavetransaction_Bulk(obj);
                if (response.IsSuccess)
                {
                    return Ok(new { response.IsSuccess, response.Message });
                }
                return BadRequest(new { response.IsSuccess, response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(SaveLeaveTransaction)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpDelete("{id}/{userName}")]
        public async Task<IActionResult> deleteTransInfo(int id, string userName)
        {
            try
            {
                bool isDelete = await _globalMaster.leavetransactions.deleteLevTrans(id, userName);
                if (!isDelete)
                {
                    _logger.LogError($"Invalid Delete Attempt In {nameof(deleteTransInfo)}");
                    return BadRequest("Submitted Data Is Invalid");
                }
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(deleteTransInfo)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        //Leave Online
        [HttpGet("GetRecommenderAndDPTPerson")]
        public async Task<IActionResult> GetRecommenderAndDPTPerson(string prefixText = null)
        {
            try
            {
                var response = await _globalMaster.leavetransactions.GetRecommenderAndDPTPerson(WebUtility.UrlDecode(prefixText));
                if (response.IsSuccess)
                {
                    return Ok(new { isSuccess = response.IsSuccess, statusCode = 200, data = response.dataTable });
                }
                return NotFound(new { isSuccess = response.IsSuccess, statusCode = 404, message = response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetRecommenderAndDPTPerson)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpGet("GetEmployeeSlByCompEmp_online")]
        public async Task<IActionResult> GetEmployeeSlByCompEmp_online(int compid, int empno)
        {
            try
            {
                var response = await _globalMaster.leavetransactions.GetEmployeeSlByCompEmp_online(compid, empno);
                if (response.IsSuccess)
                {
                    return Ok(new { isSuccess = response.IsSuccess, statusCode = 200, data = response.dataTable });
                }
                return NotFound(new { isSuccess = response.IsSuccess, statusCode = 404, message = response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetEmployeeSlByCompEmp_online)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpGet("GetEmployeeLeaveOnlineBalance")]
        public async Task<IActionResult> GetEmployeeLeaveOnlineBalance(int empSerial, int year)
        {
            try
            {
                var response = await _globalMaster.leavetransactions.GetEmployeeLeaveOnlineBalance(empSerial, year);
                if (response.IsSuccess)
                {
                    return Ok(new { isSuccess = response.IsSuccess, statusCode = 200, data = response.dataTable });
                }
                return NotFound(new { isSuccess = response.IsSuccess, statusCode = 404, message = response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetEmployeeLeaveOnlineBalance)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpPost("SaveEmployeeLeave_online")]
        public async Task<IActionResult> SaveEmployeeLeave_online(LeaveTransactionOnlinePayload obj)
        {
            try
            {
                var response = await _globalMaster.leavetransactions.SaveEmployeeLeave_online(obj);
                if (response.IsSuccess)
                {
                    return Ok(new { isSuccess = response.IsSuccess, statusCode = 200, message = response.Message, data = response.dataTable });
                }
                return BadRequest(new { isSuccess = response.IsSuccess, statusCode = 400, message = response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(SaveEmployeeLeave_online)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpGet("GetEmployeeLeaveAddInfo_online")]
        public async Task<IActionResult> GetEmployeeLeaveAddInfo_online(int compid, string userName)
        {
            try
            {
                var response = await _globalMaster.leavetransactions.GetEmployeeLeaveAddInfo_online(compid, userName);
                if (response.IsSuccess)
                {
                    return Ok(new { isSuccess = response.IsSuccess, statusCode = 200, data = response.dataTable });
                }
                return NotFound(new { isSuccess = response.IsSuccess, statusCode = 404, message = response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetEmployeeLeaveAddInfo_online)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpDelete("DeleteEmployeeLeave_online")]
        public async Task<IActionResult> DeleteEmployeeLeave_online(int compid, int leaveID, string userName)
        {
            try
            {
                var response = await _globalMaster.leavetransactions.DeleteEmployeeLeave_online(compid, leaveID, userName);
                if (response.IsSuccess)
                {
                    return Ok(new { isSuccess = response.IsSuccess, statusCode = 200, message = response.Message, data = response.dataTable });
                }
                return BadRequest(new { isSuccess = response.IsSuccess, statusCode = 400, message = response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteEmployeeLeave_online)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        // Approval
        [HttpGet("GetLeaveTransferPersonList")]
        public async Task<IActionResult> GetLeaveTransferPersonList(int transEmpSerial)
        {
            try
            {
                var response = await _globalMaster.leavetransactions.GetLeaveTransferPersonList(transEmpSerial);
                if (response.IsSuccess)
                {
                    return Ok(new { isSuccess = response.IsSuccess, statusCode = 200, data = response.dataTable });
                }
                return NotFound(new { isSuccess = response.IsSuccess, statusCode = 404, message = response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetLeaveTransferPersonList)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpPut("UpdateLeaveTransferPerson")]
        public async Task<IActionResult> UpdateLeaveTransferPerson(LeaveOnlineApp obj)
        {
            try
            {
                var response = await _globalMaster.leavetransactions.UpdateLeaveTransferPerson(obj);
                if (response.IsSuccess)
                {
                    return Ok(new { isSuccess = response.IsSuccess, statusCode = 200, message = response.Message, data = response.dataTable });
                }
                return NotFound(new { isSuccess = response.IsSuccess, statusCode = 400, message = response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetLeaveRecommenderPersonList)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpGet("GetLeaveRecommenderPersonList")]
        public async Task<IActionResult> GetLeaveRecommenderPersonList(int transEmpSerial)
        {
            try
            {
                var response = await _globalMaster.leavetransactions.GetLeaveRecommenderPersonList(transEmpSerial);
                if (response.IsSuccess)
                {
                    return Ok(new { isSuccess = response.IsSuccess, statusCode = 200, data = response.dataTable });
                }
                return NotFound(new { isSuccess = response.IsSuccess, statusCode = 404, message = response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetLeaveRecommenderPersonList)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpPut("UpdateLeaveRecommenderPerson")]
        public async Task<IActionResult> UpdateLeaveRecommenderPerson(LeaveOnlineApp obj)
        {
            try
            {
                var response = await _globalMaster.leavetransactions.UpdateLeaveRecommenderPerson(obj);
                if (response.IsSuccess)
                {
                    return Ok(new { isSuccess = response.IsSuccess, statusCode = 200, message = response.Message, data = response.dataTable });
                }
                return NotFound(new { isSuccess = response.IsSuccess, statusCode = 400, message = response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(UpdateLeaveRecommenderPerson)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpGet("GetLeaveDeptHeadPersonList")]
        public async Task<IActionResult> GetLeaveDeptHeadPersonList(int transEmpSerial)
        {
            try
            {
                var response = await _globalMaster.leavetransactions.GetLeaveDeptHeadPersonList(transEmpSerial);
                if (response.IsSuccess)
                {
                    return Ok(new { isSuccess = response.IsSuccess, statusCode = 200, data = response.dataTable });
                }
                return NotFound(new { isSuccess = response.IsSuccess, statusCode = 404, message = response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetLeaveDeptHeadPersonList)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpPut("UpdateLeaveDeptHeadPerson")]
        public async Task<IActionResult> UpdateLeaveDeptHeadPerson(LeaveOnlineApp obj)
        {
            try
            {
                var response = await _globalMaster.leavetransactions.UpdateLeaveDeptHeadPerson(obj);
                if (response.IsSuccess)
                {
                    return Ok(new { isSuccess = response.IsSuccess, statusCode = 200, message = response.Message, data = response.dataTable });
                }
                return NotFound(new { isSuccess = response.IsSuccess, statusCode = 400, message = response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(UpdateLeaveDeptHeadPerson)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpGet("GetLeaveHrList")]
        public async Task<IActionResult> GetLeaveHrList(string userName)
        {
            try
            {
                var response = await _globalMaster.leavetransactions.GetLeaveHrList(userName);
                if (response.IsSuccess)
                {
                    return Ok(new { isSuccess = response.IsSuccess, statusCode = 200, data = response.dataTable });
                }
                return NotFound(new { isSuccess = response.IsSuccess, statusCode = 404, message = response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetLeaveHrList)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpPut("UpdateLeaveHrPerson")]
        public async Task<IActionResult> UpdateLeaveHrPerson(LeaveOnlineApp obj)
        {
            try
            {
                var response = await _globalMaster.leavetransactions.UpdateLeaveHrPerson(obj);
                if (response.IsSuccess)
                {
                    return Ok(new { isSuccess = response.IsSuccess, statusCode = 200, message = response.Message, data = response.dataTable });
                }
                return NotFound(new { isSuccess = response.IsSuccess, statusCode = 400, message = response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(UpdateLeaveHrPerson)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpGet("GetLeaveAppList")]
        public async Task<IActionResult> GetLeaveAppList(string userName)
        {
            try
            {
                var response = await _globalMaster.leavetransactions.GetLeaveAppList(userName);
                if (response.IsSuccess)
                {
                    return Ok(new { isSuccess = response.IsSuccess, statusCode = 200, data = response.dataTable });
                }
                return NotFound(new { isSuccess = response.IsSuccess, statusCode = 404, message = response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetLeaveAppList)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpPut("UpdateLeaveAppPerson")]
        public async Task<IActionResult> UpdateLeaveAppPerson(LeaveOnlineApp obj)
        {
            try
            {
                var response = await _globalMaster.leavetransactions.UpdateLeaveAppPerson(obj);
                if (response.IsSuccess)
                {
                    return Ok(new { isSuccess = response.IsSuccess, statusCode = 200, message = response.Message, data = response.dataTable });
                }
                return NotFound(new { isSuccess = response.IsSuccess, statusCode = 400, message = response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(UpdateLeaveAppPerson)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpGet("GetLeaveAppListAll")]
        public async Task<IActionResult> GetLeaveAppListAll(string userName)
        {
            try
            {
                var response = await _globalMaster.leavetransactions.GetLeaveAppListAll(userName);
                if (response.IsSuccess)
                {
                    return Ok(new { isSuccess = response.IsSuccess, statusCode = 200, data = response.dataTable });
                }
                return NotFound(new { isSuccess = response.IsSuccess, statusCode = 404, message = response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetLeaveAppListAll)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
    }
}
