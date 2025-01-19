using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace TaskForceApp.Pages.ExpenseTracker
{
    public class AddIncomeModel : PageModel
    {
        [BindProperty]
        public string description { get; set; }

        [BindProperty]
        public string income { get; set; }

        [BindProperty]
        public string account { get; set; }

        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(description) || string.IsNullOrEmpty(income) || string.IsNullOrEmpty(account))
            {
                ErrorMessage = "All fields are required.";
                return Page();
            }

            try
            {
                string currentDate = DateTime.Now.ToString("yyyy-MM-dd");

                string connectionString = "Data Source=DESKTOP-2042M6B\\SQLEXPRESS;Initial Catalog=TaskForceDB;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string sqlQuery = "INSERT INTO ExpenseTracker (income,expense,remaining,account,description,Date) VALUES (@income, 0, 0, @account, @description, @date)";
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@income", income);
                        cmd.Parameters.AddWithValue("@account", account);
                        cmd.Parameters.AddWithValue("@description", description);
                        cmd.Parameters.AddWithValue("@date", currentDate);
                        cmd.ExecuteNonQuery();
                    }
                }
                SuccessMessage = "Income recorded";
                TempData["SuccessMessage"] = SuccessMessage;
                return RedirectToPage("/ExpenseTracker/AllIncome");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }
        }

       
    }
}
