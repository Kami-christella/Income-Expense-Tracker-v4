using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace TaskForceApp.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }

        public void OnGet()
        {
            // This method is left intentionally blank
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                ErrorMessage = "All fields are required.";
                return Page();
            }

            try
            {
                string hashedPassword = HashPassword(Password);

                string connectionString = "Data Source=DESKTOP-2042M6B\\SQLEXPRESS;Initial Catalog=TaskForceDB;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string sqlQuery = "SELECT count(1) FROM Users WHERE email=@Email AND Password=@Password";
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@Email", Email);
                        cmd.Parameters.AddWithValue("@Password", hashedPassword);

                        int count = 0;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                count = Convert.ToInt32(reader[0]);
                                //HttpContext.Session.SetInt32("UserID", userID);
                                //HttpContext.Session.SetString("UserRole", role); 
                                HttpContext.Session.SetString("Email", Email);
                            }
                        }

                        if (count > 0)
                        {
                            SuccessMessage = "Login successful.";
                            // Redirect based on role
                            return RedirectToPage("/ExpenseTracker/Index");
                        }
                        else
                        {
                            ErrorMessage = "Invalid username or password.";
                            return Page();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
