using BLL.Interfaces;
using BLL.Utility;
using BOL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Payroll_API.Controllers.QuestPdf
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestPdfController : ControllerBase
    {
        private readonly IGlobalMaster _globalMaster;
        private readonly Dg_Common _dgCommon;
        private readonly ILogger<QuestPdfController> _logger;

        public QuestPdfController(IGlobalMaster globalMaster, Dg_Common dgCommon, ILogger<QuestPdfController> logger)
        {
            _globalMaster = globalMaster;
            _dgCommon = dgCommon;
            _logger = logger;
        }

        [HttpPost("GetQuestPdfTestReport")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetQuestPdfTestReport(PostReportViewPayload obj)
        {
            try
            {
                var response = await _globalMaster.questPdfManager.GetQuestPdfTestReport(obj);
                return File(response, MediaTypeNames.Application.Octet, _dgCommon.GetContentType(obj.reportType));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetQuestPdfTestReport)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
    }
}
