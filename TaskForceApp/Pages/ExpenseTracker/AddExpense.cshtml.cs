using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using static TaskForceApp.Pages.Dashboard.IndexModel;
using static TaskForceApp.Pages.ExpenseTracker.IndexModel;

namespace TaskForceApp.Pages.ExpenseTracker
{
    public class AddExpenseModel : PageModel
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

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
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
                                int income = reader.GetInt32(0);
                                int expense = reader.GetInt32(1);

                                inc += income;
                                exp += expense;
                            }
                        }
                    }

                    totalAccountBalance = inc - exp;

                    if (totalAccountBalance >= expenseAmount)
                    {
                        string insertQuery = "UPDATE ExpenseTracker SET expense=@expense,remaining=@remaining, account=@account, description=@description WHERE ID=@ID";
                        using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                        {
                            cmd.Parameters.AddWithValue("@expense", expenseAmount);
                            cmd.Parameters.AddWithValue("@remaining", totalAccountBalance - expenseAmount);
                            cmd.Parameters.AddWithValue("@account", account);
                            cmd.Parameters.AddWithValue("@description", description);
                            cmd.ExecuteNonQuery();
                        }

                        SuccessMessage = "Expense recorded.";
                        TempData["SuccessMessage"] = SuccessMessage;
                        return RedirectToPage("/ExpenseTracker/AllExpense");
                    }
                    else
                    {
                        ErrorMessage = "You don't have enough money to complete the transaction on your " + account + " account.";
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
