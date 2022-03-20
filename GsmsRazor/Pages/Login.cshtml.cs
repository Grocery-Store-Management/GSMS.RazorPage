using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BusinessObjectLibrary;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GsmsRazor.Pages
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly GsmsContext _context;
        public HttpContext httpContext;

        [BindProperty]
        [MaxLength(60, ErrorMessage = "This string is too long.")]
        public string Username { get; set; }
        [BindProperty]
        [MaxLength(60, ErrorMessage = "This string is too long.")]
        public string Password { get; set; }

        public LoginModel(GsmsContext context)
        {
            _context = context;
        }
        public IActionResult OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToPage("./Sale");
            }

            ViewData["Error"] = null;
            return Page();
        }
        //public async Task<IActionResult> OnGetLogout()
        //{
        //    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        //    HttpContext.Session.Clear();
        //    return RedirectToPage("/");
        //}
        //public async Task<IActionResult> OnPostLogout()
        //{
        //    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        //    HttpContext.Session.Clear();
        //    return RedirectToPage("/");
        //}

        public async Task<IActionResult> OnPost()
        {
            Employee _emp = _context.Employees
                .SingleOrDefault(e => e.Name == Username && e.Password == Password);
            if (_emp != null)
            {
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Role, _emp.Role),
                        new Claim("ID", _emp.Id),
                        new Claim("ACTIVE", (!_emp.IsDeleted).ToString())
                    };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                // Session variables
                HttpContext.Session.SetString("UID", _emp.Id);
                HttpContext.Session.SetString("NAME", _emp.Name);
                HttpContext.Session.SetString("STORE_ID", _emp.StoreId);
                HttpContext.Session.SetString("ROLE", _emp.Role);



                return RedirectToPage("./Index");
            }
            else
            {
                ViewData.Add("Error", "Username or password is incorrect!");
            }

            return Page();
        }

    }
}
