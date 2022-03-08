using BusinessObjectLibrary;
using DataAccessLibrary.BusinessEntity;
using DataAccessLibrary.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace GsmsRazor.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private IUnitOfWork _work;
        private CustomerBusinessEntity _entity;

        public IndexModel(ILogger<IndexModel> logger, IUnitOfWork work)
        {
            _work = work;
            _logger = logger;
            _entity = new CustomerBusinessEntity(work);

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

            Customer.Id = new Guid().ToString();
            Customer.CreatedDate = DateTime.Now;

            Debug.WriteLine(Customer);
            Debug.WriteLine("owo");


            await _entity.AddAsync(Customer);
            return RedirectToPage("./Index");
        }
    }
}
