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

namespace GsmsRazor.Pages.ImportOrders
{
    public class IndexModel : PageModel
    {
        private readonly ImportOrderBusinessEntity _importOrders;

        public IndexModel(IUnitOfWork work)
        {
            _importOrders = new ImportOrderBusinessEntity(work);
        }

        public IList<ImportOrder> ImportOrders { get;set; }

        public async Task OnGetAsync(
            [FromQuery] string searchString,
            [FromQuery] int? sPage
            )
        {
            int pageSize = 10;
            int pageNumber = (sPage ?? 1);
            ImportOrders = (await _importOrders.GetImportOrdersAsync(null, null, 
                searchString, null, GsmsLibrary.SortType.DESC, pageNumber, pageSize))
                .ToList();
            int pageCount = (int)Math.Ceiling((decimal)(await _importOrders.GetAllImportOrdersAsync()).Count() / pageSize);

            ViewData["PageNumber"] = pageNumber;
            ViewData["PageCount"] = pageCount;
        }
    }
}
