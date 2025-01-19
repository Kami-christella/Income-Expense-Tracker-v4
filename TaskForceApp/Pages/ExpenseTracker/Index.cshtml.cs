using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using static TaskForceApp.Pages.ExpenseTracker.IndexModel;

namespace TaskForceApp.Pages.ExpenseTracker
{
    public class IndexModel : PageModel
    {
        public List<AllInfo> ListAll = new List<AllInfo>();
        public List<AllIncome> ListIncome = new List<AllIncome>();
        public List<AllExpense> ListExpense = new List<AllExpense>();

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("Email") == null)
            {
                return RedirectToPage("/Account/Login");
            }

            ListAll.Clear();
            ListIncome.Clear();
            ListExpense.Clear();

            int totalIncome = 0;
            int totalExpense = 0;
            int remaining = 0;

            try
            {
                string connectionString = "Data Source=DESKTOP-2042M6B\\SQLEXPRESS;Initial Catalog=TaskForceDB;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string sqlQuery = "SELECT income, expense, remaining, account, description,ID  FROM ExpenseTracker";
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                AllInfo info = new AllInfo
                                {
                                    income = reader.GetInt32(0),
                                    expense = reader.GetInt32(1),
                                    remaining = reader.GetInt32(2),
                                    account = reader.GetString(3),
                                    description = reader.GetString(4),
                                    ID = reader.GetInt32(5)
                                };

                                ListAll.Add(info);

                             

                                totalIncome += info.income;
                                totalExpense += info.expense;

                              remaining = totalIncome- totalExpense;


                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }

            ViewData["TotalIncome"] = totalIncome;
            ViewData["TotalExpense"] = totalExpense;
            ViewData["remaining"] = remaining;

            //start of expense

            ListExpense.Clear();

            try
            {
                string connectionString = "Data Source=DESKTOP-2042M6B\\SQLEXPRESS;Initial Catalog=TaskForceDB;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string sqlQuery = "SELECT expense, account, description,ID  FROM ExpenseTracker where expense != 0 ";
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                AllExpense info = new AllExpense
                                {
                                   
                                    expense = reader.GetInt32(0),
                                    account = reader.GetString(1),
                                    description = reader.GetString(2)
                                   
                                };

                                ListExpense.Add(info);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
            //end

            //start of income
            ListIncome.Clear();

            try
            {
                string connectionString = "Data Source=DESKTOP-2042M6B\\SQLEXPRESS;Initial Catalog=TaskForceDB;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string sqlQuery = "SELECT income, account, description,ID  FROM ExpenseTracker where income !=0";
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                AllIncome info = new AllIncome
                                {
                                    income = reader.GetInt32(0),
                                    account = reader.GetString(1),
                                    description = reader.GetString(2),
                                    ID=reader.GetInt32(3)
                                 
                                };

                                ListIncome.Add(info);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }

            return Page();

            //end of income
        }



        public class AllInfo
        {
            public int income { get; set; }
            public int expense { get; set; }
            public int remaining { get; set; }
            public string account { get; set; }
            public string description { get; set; }
            public int ID { get; set; }
        }

        public class AllIncome
        {
            public int income { get; set; }
            public string account { get; set; }
            public string description { get; set; }
            public int ID { get; set; }
        }

        public class AllExpense
        {
            public int expense { get; set; }
            public string account { get; set; }
            public string description { get; set; }
            public int ID { get; set; }
        }


    }
}
