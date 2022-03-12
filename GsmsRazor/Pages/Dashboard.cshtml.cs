using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GsmsRazor.Pages
{
    public class DashboardModel : PageModel
    {
        public IActionResult OnGet()
        {
            return Page();
        }
    }
}
