using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace TaskForceApp.Pages.Account
{
    public class SearchIncomeModel : PageModel
    {
        public List<AllIncome> SearchResults { get; set; } = new List<AllIncome>();
        public string SearchString { get; set; }

        public void OnGet(string searchString)
        {
            SearchString = searchString;

            if (!string.IsNullOrEmpty(SearchString))
            {
                string connectionString = "Data Source=DESKTOP-2042M6B\\SQLEXPRESS;Initial Catalog=TaskForceDB;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string sqlQuery = "SELECT income, account, description, ID FROM ExpenseTracker WHERE income !=0 AND (description LIKE @searchString OR account LIKE @searchString)";
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@searchString", "%" + searchString + "%");

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                AllIncome info = new AllIncome
                                {
                                    income = reader.GetInt32(0),
                                    account = reader.GetString(1),
                                    description = reader.GetString(2),
                                    ID = reader.GetInt32(3)
                                };

                                SearchResults.Add(info);
                            }
                        }
                    }
                }
            }
        }

        public class AllIncome
        {
            public int income { get; set; }
            public string account { get; set; }
            public string description { get; set; }
            public int ID { get; set; }
        }
    }
}
