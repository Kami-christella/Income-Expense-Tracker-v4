using PdfSharp.Pdf;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using Microsoft.Data.SqlClient;
using MigraDoc.Rendering;

namespace TaskForceApp.Pages.ExpenseTracker
{
    public class Report
    {
        public PdfDocument GetInvoice()
        {
            var document = new Document();
            BuildDocument(document);

            var pdfRenderer = new PdfDocumentRenderer();
            pdfRenderer.Document = document;

            pdfRenderer.RenderDocument();

            return pdfRenderer.PdfDocument;


        }

        private void BuildDocument(Document document)
        {
            Section section = document.AddSection();

            var paragraph = section.AddParagraph();
            paragraph.AddFormattedText("Income Report", TextFormat.Bold);
            paragraph.Format.Alignment = ParagraphAlignment.Justify;
            paragraph.Format.LineSpacing = 5;
            paragraph.AddLineBreak();
            paragraph.AddLineBreak();


            string connectionString = "Data Source=DESKTOP-2042M6B\\SQLEXPRESS;Initial Catalog=TaskForceDB;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string sqlQuery = "SELECT income , account, description  FROM ExpenseTracker where income != 0 ";

                var table = document.LastSection.AddTable();
                table.Borders.Width = 0.5;
                table.AddColumn("4cm");
                table.AddColumn("4cm");
                table.AddColumn("4cm");





                Row row = table.AddRow();
                row.HeadingFormat = true;
                row.Format.Font.Bold = true;
                row.Cells[0].AddParagraph("Description");
                row.Cells[1].AddParagraph("Income Amount");
                row.Cells[2].AddParagraph("Account");

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            row = table.AddRow();
                            row.Cells[0].AddParagraph(reader.GetString(2));
                            row.Cells[1].AddParagraph(reader.GetInt32(0).ToString());
                            row.Cells[2].AddParagraph(reader.GetString(1));

                        }
                    }
                }

            }

        }


    }
}
