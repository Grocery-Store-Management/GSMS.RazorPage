using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObjectLibrary;
using DataAccessLibrary.Interfaces;
using DataAccessLibrary.BusinessEntity;

namespace GsmsRazor.Pages.CustomerPage
{
    public class CreateModel : PageModel
    {
        private readonly CustomerBusinessEntity _entity;


        public CreateModel(CustomerBusinessEntity entity)
        {
            _entity = entity;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Customer Customer { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            await _entity.AddAsync(Customer);
            return RedirectToPage("./Index");
        }
    }
}
