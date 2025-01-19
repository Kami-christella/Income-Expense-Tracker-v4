using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TaskForceApp.Pages.ExpenseTracker
{
    public class AllInvoiceModel : PageModel
    {
        public IActionResult OnGet()
        {
            var AllReport = new AllReport();
            var document = AllReport.GetInvoice();
            //document.Save("Invoice.pdf");

            MemoryStream stream = new MemoryStream();
            document.Save(stream);

            Response.ContentType = "application/pdf";
            Response.Headers.Add("content-length", stream.Length.ToString());
            byte[] bytes = stream.ToArray();
            stream.Close();

            return File(bytes, "application/pdf", "All Transactions.pdf");

        }
    }
}
