using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TSManager.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        public IActionResult OnGet()
        {
            return NotFound(); // disables registration page
        }

        public IActionResult OnPost()
        {
            return NotFound();
        }
    }
}
