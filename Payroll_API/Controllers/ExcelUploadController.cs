using BLL.Interfaces;
using BOL.Models;
using CoreApiResponse;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Payroll_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExcelUploadController : BaseController
    {
        private readonly IGlobalMaster _globalMaster;
        private readonly ILogger<ExcelUploadController> _logger;
        public ExcelUploadController(IGlobalMaster globalMaster, ILogger<ExcelUploadController> logger)
        {
            _globalMaster = globalMaster;
            _logger = logger;
        }

        [HttpGet("GetExcelUploadType")]
        public async Task<IActionResult> GetExcelUploadType()
        {
            try
            {
                var data = await _globalMaster.excelText.GetExcelUploadType();
                if (data.Rows.Count > 0)
                {
                    return CustomResult("Data Loaded !!",data,HttpStatusCode.OK);
                }
                return CustomResult("Data Not Found !!", HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetExcelUploadType)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost("SaveExeclUploadData")]
        public async Task<IActionResult> SaveExeclUploadData([FromForm] UploadExcelFile model)
        {
            try
            {
                var message = await _globalMaster.excelText.SaveUploadExcelToDB(model);
                if (message.Count > 0)
                {
                    return CustomResult(message, HttpStatusCode.OK);
                }
                return CustomResult(message, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(SaveExeclUploadData)}");
                return CustomResult("Internal Server Error, Please Try Again Later!",HttpStatusCode.InternalServerError);
            }
        }
    }
}
