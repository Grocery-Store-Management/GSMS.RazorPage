using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BusinessObjectLibrary;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GsmsRazor.Pages
{
    public class LoginModel : PageModel
    {
        private readonly GsmsContext _context;

        public LoginModel(GsmsContext context)
        {
            _context = context;
        }

        [BindProperty]
        [Required]
        public string Username { get; set; }

        [BindProperty]
        [Required]
        public string Password { get; set; }


        public async Task<IActionResult> OnGetLogout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToPage("/Login");
        }
        public async Task<IActionResult> OnPost()
        {
            //    Employee _account = _context.Employees.SingleOrDefault(m => m.Id == Username && m.Password == Password);

            //    if (_account != null)
            //    {
            //        var claims = new List<Claim>
            //            {
            //                new Claim(ClaimTypes.Role, "Staff"),
            //            };

            //        var claimsIdentity = new ClaimsIdentity(
            //            claims, CookieAuthenticationDefaults.AuthenticationScheme);

            //        var authProperties = new AuthenticationProperties
            //        {
            //            IsPersistent = true
            //        };

            //        await HttpContext.SignInAsync(
            //            CookieAuthenticationDefaults.AuthenticationScheme,
            //            new ClaimsPrincipal(claimsIdentity),
            //            authProperties);

            //        return RedirectToPage("./Index");
            //    }

            //    return Page();
            return RedirectToPage("./Index");
        }

    }
}
