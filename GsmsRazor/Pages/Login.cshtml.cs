using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BusinessObjectLibrary;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GsmsRazor.Pages
{
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
            Console.WriteLine(Username);
            Customer _cus = _context.Customers
                .SingleOrDefault(c => c.PhoneNumber == Username && c.Password == Password);

            if (_cus != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Role, "Customer"),
                    new Claim("ID", _cus.Id),
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
                HttpContext.Session.SetString("UID", _cus.Id);
                HttpContext.Session.SetString("PHONE", _cus.PhoneNumber);
                HttpContext.Session.SetString("ROLE", "Customer");

                return RedirectToPage("./Dashboard");
            }
            else
            {
                Employee _emp = _context.Employees
                    .SingleOrDefault(e => e.Name == Username && e.Password == Password);
                if (_emp != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Role, _emp.Role),
                        new Claim("ID", _emp.Id)
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
            }

            return Page();
        }

    }
}
