using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObjectLibrary;
using DataAccessLibrary.BusinessEntity;
using DataAccessLibrary.Interfaces;
using GsmsLibrary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GsmsRazor.Pages.Products
{
    public class CreateProductModel : PageModel
    {
        private readonly ProductBusinessEntity _products;
        private readonly CategoryBusinessEntity _categories;

        public CreateProductModel(IUnitOfWork work)
        {
            _products = new ProductBusinessEntity(work);
            _categories = new CategoryBusinessEntity(work);
        }

        public async Task<IActionResult> OnGetAsync()
        {
            IEnumerable<Category> categories = await _categories.GetCategoriesAsync();
            ViewData["CategoryId"] = new SelectList(categories, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public Product Product { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            IEnumerable<Category> categories = await _categories.GetCategoriesAsync();
            ViewData["CategoryId"] = new SelectList(categories, "Id", "Name");
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _products.AddProductAsync(Product);
            TempData["CreateProductSuccessfully"] = true;
            return RedirectToPage("./Index");
        }

    }
}
