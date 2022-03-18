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

        public IList<ImportOrder> ImportOrder { get;set; }

        public async Task OnGetAsync()
        {
            ImportOrder = (await _importOrders.GetImportOrdersAsync(null, null, "", null, null, 0, 0))
                .ToList();
        }
    }
}
