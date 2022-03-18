using System;
using System.Collections.Generic;
using System.IO;
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
    public class ProductIndexModel : PageModel
    {
        private readonly ProductBusinessEntity _products;
        private readonly CategoryBusinessEntity _categories;

        public ProductIndexModel(IUnitOfWork work)
        {
            _products = new ProductBusinessEntity(work);
            _categories = new CategoryBusinessEntity(work);
        }

        public IList<Product> Products { get; set; }
        public async Task OnGetAsync(
            [FromQuery] string searchString,
            [FromQuery] int? sPage)
        {
            IEnumerable<Category> categories = await _categories.GetCategoriesAsync();
            ViewData["CategoryId"] = (new SelectList(categories, "Id", "Name"))
                .Append(new SelectListItem("Add new category", "addNewCategory"));
            Products = (await _products.GetAllProductsAsync()).ToList();
            
            if (!string.IsNullOrEmpty(searchString))
            {
                ViewData["Search"] = searchString;
                Products = Products.Where(
                    p => p.Name.ToLower().Contains(searchString.ToLower().Trim())).ToList();
            }

            int pageSize = 10;
            int pageNumber = (sPage ?? 1);
            int pageCount = (int)Math.Ceiling((decimal)Products.Count / pageSize);

            ViewData["PageNumber"] = pageNumber;
            ViewData["PageCount"] = pageCount;

            Products = Products.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).ToList();
        }

        [BindProperty]
        public Product NewProduct { get; set; }

        public async Task<IActionResult> OnPostCreateProductAsync()
        {
            IEnumerable<Category> categories = await _categories.GetCategoriesAsync();
            ViewData["CategoryId"] = (new SelectList(categories, "Id", "Name"))
                .Append(new SelectListItem("Add new category", "addNewCategory"));
            Products = (await _products.GetAllProductsAsync()).ToList();
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _products.AddProductAsync(NewProduct);
            TempData["CreateProductSuccessfully"] = true;
            NewProduct = new Product();
            return RedirectToPage("./Index");
        }

        //[BindProperty]
        //public Category NewCategory { get; set; }

        public async Task<IActionResult> OnPostCreateCategoryAsync()
        {
            Category category = await GsmsUtils.ConvertRequestBody<Category>(Request);

            if (category != null && !string.IsNullOrEmpty(category.Name))
            {
                 await _categories.AddCategoryAsync(category);
            }

            return new JsonResult(category);
        }

        public async Task<IActionResult> OnPostDeleteAsync(string deletedProductId)
        {
            //Product product = await GsmsUtils.ConvertRequestBody<Product>(Request);

            if (!string.IsNullOrEmpty(deletedProductId))
            {
                await _products.DeleteProductAsync(deletedProductId);
            }
            TempData["DeleteProductSuccessfully"] = true;
            return RedirectToPage("./Index");
        }
    }
}
