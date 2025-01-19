using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using static TaskForceApp.Pages.ExpenseTracker.IndexModel;

namespace TaskForceApp.Pages.ExpenseTracker
{
    public class EditIncomeModel : PageModel
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
                    string sqlQuery = "SELECT ID, description, income, account FROM ExpenseTracker WHERE ID=@ID AND expense=0";
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
            string income = Request.Form["income"];
            string account = Request.Form["account"];
            string description = Request.Form["description"];
            try
            {
                string connectionString = "Data Source=DESKTOP-2042M6B\\SQLEXPRESS;Initial Catalog=TaskForceDB;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string sqlQuery = "UPDATE ExpenseTracker SET income=@income, account=@account, description=@description WHERE ID=@ID"; ;
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@income", income);
                        cmd.Parameters.AddWithValue("@account", account);
                        cmd.Parameters.AddWithValue("@description", description);
                        cmd.Parameters.AddWithValue("@ID", ID);
                        cmd.ExecuteNonQuery();
                    }
                }
                SuccessMessage = "Income Edited successfully.";
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
