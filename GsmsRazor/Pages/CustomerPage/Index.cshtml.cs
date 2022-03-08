using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObjectLibrary;

namespace GsmsRazor.Pages.CustomerPage
{
    public class IndexModel : PageModel
    {
        private readonly BusinessObjectLibrary.GsmsContext _context;

        public IndexModel(BusinessObjectLibrary.GsmsContext context)
        {
            _context = context;
        }

        public IList<Customer> Customer { get;set; }

        public async Task OnGetAsync()
        {
            Customer = await _context.Customers.ToListAsync();
        }
    }
}
