using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TaskForceApp.Pages
{
    public class CheckSessionModel : PageModel
    {
        public string EmailFromSession { get; set; }
        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }

        public void OnGet()
        {
            try
            {
                EmailFromSession = HttpContext.Session.GetString("Email");

                if (string.IsNullOrEmpty(EmailFromSession))
                {
                    ErrorMessage = "No session value found!";
                }
                else
                {
                    SuccessMessage = "Session value retrieved successfully!";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }
    }
}
