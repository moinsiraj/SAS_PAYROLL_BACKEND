using BOL.Models;

namespace BLL.Interfaces.Manager.QuestPdf
{
    public interface IQuestPdfManager
    {
        Task<byte[]> GetQuestPdfTestReport(PostReportViewPayload obj);
    }
}
