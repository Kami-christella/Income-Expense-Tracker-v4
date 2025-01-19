using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using static TaskForceApp.Pages.ExpenseTracker.IndexModel;

namespace TaskForceApp.Pages.ExpenseTracker
{
    public class EditExpenseModel : PageModel
    {
        [BindProperty]
        public string description { get; set; }

        [BindProperty]
        public string expense { get; set; }

        [BindProperty]
        public string account { get; set; }

        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }

        public List<AllMomoAc> ListMomoAc = new List<AllMomoAc>();
        public List<AllSavingAc> ListSavingAc = new List<AllSavingAc>();
        public List<AllBankAc> ListBankAc = new List<AllBankAc>();

        public AllInfo AllInfo = new AllInfo();

        public void OnGet()
        {
            string ID = Request.Query["ID"];
            try
            {
                string connectionString = "Data Source=DESKTOP-2042M6B\\SQLEXPRESS;Initial Catalog=TaskForceDB;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string sqlQuery = "SELECT ID, description, expense, account FROM ExpenseTracker WHERE ID=@ID";
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
            string expense = Request.Form["expense"];
            string account = Request.Form["account"];
            string description = Request.Form["description"];

            int inc = 0;
            int exp = 0;
            int totalAccountBalance = 0;

            if (!ModelState.IsValid || string.IsNullOrEmpty(description) || string.IsNullOrEmpty(expense) || string.IsNullOrEmpty(account))
            {
                ErrorMessage = "All fields are required.";
                return Page();
            }

            int expenseAmount;
            if (!int.TryParse(expense, out expenseAmount))
            {
                ErrorMessage = "Invalid expense amount.";
                return Page();
            }

            try
            {
                string connectionString = "Data Source=DESKTOP-2042M6B\\SQLEXPRESS;Initial Catalog=TaskForceDB;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string sqlQuery = $"SELECT income, expense FROM ExpenseTracker WHERE account=@account";
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@account", account);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                inc += reader.GetInt32(0);
                                exp += reader.GetInt32(1);
                            }
                        }
                    }

                    totalAccountBalance = inc - exp;

                    if (totalAccountBalance >= expenseAmount)
                    {
                        string updateQuery = "UPDATE ExpenseTracker SET expense=@expense, account=@account, description=@description WHERE ID=@ID";
                        using (SqlCommand cmd = new SqlCommand(updateQuery, con))
                        {
                            cmd.Parameters.AddWithValue("@expense", expenseAmount);
                            cmd.Parameters.AddWithValue("@account", account);
                            cmd.Parameters.AddWithValue("@description", description);
                            cmd.Parameters.AddWithValue("@ID", ID);
                            cmd.ExecuteNonQuery();
                        }

                        SuccessMessage = "Expense updated successfully.";
                        TempData["SuccessMessage"] = SuccessMessage;
                        return RedirectToPage("/ExpenseTracker/AllExpense");
                    }
                    else
                    {
                        ErrorMessage = "The total account balance is insufficient to edit the expense.";
                        return Page();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }
        }

        public class AllMomoAc
        {
            public int income { get; set; }
            public int expense { get; set; }
        }

        public class AllSavingAc
        {
            public int income { get; set; }
            public int expense { get; set; }
        }

        public class AllBankAc
        {
            public int income { get; set; }
            public int expense { get; set; }
        }
    }
}
