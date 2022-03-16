using BusinessObjectLibrary;
using DataAccessLibrary.BusinessEntity;
using DataAccessLibrary.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Web;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

namespace GsmsRazor.Pages
{
    [Authorize(Roles = "Employee")]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private IUnitOfWork _work;
        private CustomerBusinessEntity _entity;

        [BindProperty]
        public string name { get; set; }

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
    }
}
