using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TaskForceApp.Pages.ExpenseTracker
{
    public class LogoutModel : PageModel
    {
        public void OnGet()
        {
            HttpContext.Session.Clear(); // Clear the session
            Response.Redirect("/Account/index"); // Redirect to the login page 
        }
    }
}
