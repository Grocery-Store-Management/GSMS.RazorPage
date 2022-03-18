using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObjectLibrary;
using DataAccessLibrary.BusinessEntity;
using DataAccessLibrary.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace GsmsRazor.Pages.ImportOrders
{
    [Authorize(Roles = "Store Owner")]
    public class DetailsModel : PageModel
    {
        private readonly ImportOrderBusinessEntity _importOrders;

        public DetailsModel(IUnitOfWork work)
        {
            _importOrders = new ImportOrderBusinessEntity(work);
        }

        public ImportOrder ImportOrder { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ImportOrder = (await _importOrders.GetAllImportOrdersAsync())
                .FirstOrDefault(io => io.Id.Equals(id));

            if (ImportOrder == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
