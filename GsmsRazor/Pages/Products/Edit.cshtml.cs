using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObjectLibrary;
using DataAccessLibrary.BusinessEntity;
using DataAccessLibrary.Interfaces;

namespace GsmsRazor.Pages.Products
{
    public class EditModel : PageModel
    {
        private readonly ProductBusinessEntity _products;
        private readonly CategoryBusinessEntity _categories;

        public EditModel(IUnitOfWork work)
        {
            _products = new ProductBusinessEntity(work);
            _categories = new CategoryBusinessEntity(work);
        }

        [BindProperty]
        public Product Product { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product = (await _products.GetAllProductsAsync())
                .FirstOrDefault(p => p.Id.Equals(id));

            if (Product == null)
            {
                return NotFound();
            }

            IEnumerable<Category> categories = await _categories.GetCategoriesAsync();
            ViewData["CategoryId"] = (new SelectList(categories, "Id", "Name"))
                 .Append(new SelectListItem("Add new category", "addNewCategory"));
            ViewData["Category"] = Product.CategoryId;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _products.UpdateProductAsync(Product);
            TempData["UpdateProductSuccessfully"] = true;
            return RedirectToPage("./Index");
        }
    }
}
