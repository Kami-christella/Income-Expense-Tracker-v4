using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using static TaskForceApp.Pages.ExpenseTracker.IndexModel;

namespace TaskForceApp.Pages.ExpenseTracker
{
    public class DeleteExpenseModel : PageModel
    {
        public AllInfo AllInfo = new AllInfo();
        public string ErrorMessage = "";
        public string SuccessMessage = "";

        public void OnGet()
        {
            string ID = Request.Query["ID"];
            try
            {
                string connectionString = "Data Source=DESKTOP-2042M6B\\SQLEXPRESS;Initial Catalog=TaskForceDB;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string sqlQuery = "SELECT ID, description, expense, account FROM ExpenseTracker WHERE ID=@ID AND income=0"  ;
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@ID", ID);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                AllInfo.ID = reader.GetInt32(0);
                                AllInfo.description = reader.GetString(1);
                                AllInfo.expense = reader.GetInt32(2);
                                AllInfo.account = reader.GetString(3);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        public IActionResult OnPost()
        {
            string ID = Request.Form["ID"];
            try
            {
                string connectionString = "Data Source=DESKTOP-2042M6B\\SQLEXPRESS;Initial Catalog=TaskForceDB;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string sqlQuery = "DELETE FROM ExpenseTracker WHERE ID=@ID";
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@ID", ID);
                        cmd.ExecuteNonQuery();
                    }
                }
                SuccessMessage = "Expense deleted successfully.";
                return RedirectToPage("/ExpenseTracker/AllExpense");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }
        }
    }
}
