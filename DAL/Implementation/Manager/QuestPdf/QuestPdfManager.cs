using BLL.Interfaces.Manager.QuestPdf;
using BLL.Utility;
using BOL.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Implementation.Manager.QuestPdf
{
    public class QuestPdfManager : IQuestPdfManager
    {

        private readonly Dg_Common _dgCommon;
        private readonly SqlConnection _connection;
        public QuestPdfManager(Dg_Common dgCommon)
        {
            _dgCommon = dgCommon;
            _connection = new SqlConnection(Getway.Dg_Payroll);
        }

        public async Task<byte[]> GetQuestPdfTestReport(PostReportViewPayload obj)
        {
            var data = await _dgCommon.get_InformationDataTableAsync("dg_Ecard_Report_v1", _connection, obj);
            var report = GeneratePdfQuest(pageHeaderItem(), pageBodyItem(data), pageFooterItem());
            return report;
        }


        private byte[] GeneratePdfQuest(Action<IContainer> header = null, Action<IContainer> body = null, Action<IContainer> footer = null)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Portrait());
                    page.Margin(20);
                    if (header != null)
                        page.Header().Element(header);

                    if (body != null)
                        page.Content().Element(body);

                    if (footer != null)
                        page.Footer().Element(footer);
                });
            }).GeneratePdf();
        }

        private Action<IContainer> pageHeaderItem()
        {
            return container =>
            {
                container.Text("Hello Header");
            };
        }
        private Action<IContainer> pageBodyItem(DataTable dt)
        {
            return container =>
            {
                container.PaddingVertical(40).Element(e =>
                {
                    e.Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            for (int i = 0; i < 10; i++)
                                columns.RelativeColumn();
                        });
                        table.Header(header =>
                        {
                            header.Cell().Element(CellHeader).Text("Date");
                            header.Cell().Element(CellHeader).Text("Shift");
                            header.Cell().Element(CellHeader).Text("Shift In Time");
                            header.Cell().Element(CellHeader).Text("Shift Out Time");
                            header.Cell().Element(CellHeader).Text("In Time");
                            header.Cell().Element(CellHeader).Text("Out Time");
                            header.Cell().Element(CellHeader).Text("Late");
                            header.Cell().Element(CellHeader).Text("OT Hours");
                            header.Cell().Element(CellHeader).Text("Day Status");
                            header.Cell().Element(CellHeader).Text("Remarks");


                        });
                        foreach (DataRow row in dt.Rows)
                        {
                            table.Cell().Element(CellRow).Text(Convert.ToDateTime(row["at_date"]).ToString("dd-MMM-yy"));
                            table.Cell().Element(CellRow).Text(row["sh_name"].ToString());
                            table.Cell().Element(CellRow).Text(row["sh_InTime"].ToString());
                            table.Cell().Element(CellRow).Text(row["sh_OutTime"].ToString());
                            table.Cell().Element(CellRow).Text(row["at_intime"].ToString());
                            table.Cell().Element(CellRow).Text(row["at_outtime"].ToString());
                            table.Cell().Element(CellRow).Text(row["at_late"].ToString());
                            table.Cell().Element(CellRow).Text(row["at_ot_hrs"].ToString());
                            table.Cell().Element(CellRow).Text(row["at_status"].ToString());
                            table.Cell().Element(CellRow).Text("");


                        }
                    });
                });               
            };
        }
        private Action<IContainer> pageFooterItem()
        {
            return container =>
            {
                container.AlignCenter().Text(x =>
                {
                    x.CurrentPageNumber();
                    x.Span(" / ");
                    x.TotalPages();
                });
            };
        }

        private IContainer CellHeader(IContainer container)
        {
            return container               
                .PaddingVertical(3)
                .BorderBottom(1)
                .BorderColor(Colors.Black)
                .DefaultTextStyle(x => x.SemiBold());
        }

        private IContainer CellRow(IContainer container)
        {
            return container
                .PaddingVertical(2)
                .BorderBottom(0.5f)
                .BorderColor(Colors.Grey.Lighten2)
                .DefaultTextStyle(x => x.FontSize(8));
        }
    }
}
