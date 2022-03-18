using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObjectLibrary;
using DataAccessLibrary.BusinessEntity;
using DataAccessLibrary.Interfaces;
using GsmsRazor.SessionUtil;
using GsmsLibrary;
using Microsoft.AspNetCore.Authorization;

namespace GsmsRazor.Pages.ImportOrders
{
    [Authorize(Roles = "Store Owner")]

    public class CreateModel : PageModel
    {
        private readonly ImportOrderBusinessEntity _importOrders;
        private readonly StoreBusinessEntity _stores;
        private readonly ProductBusinessEntity _products;

        public CreateModel(IUnitOfWork work)
        {
            _importOrders = new ImportOrderBusinessEntity(work);
            _stores = new StoreBusinessEntity(work);
            _products = new ProductBusinessEntity(work);
        }

        public IList<Product> Products { get; set; }
        public async Task<IActionResult> OnGetAsync(
           [FromQuery] string searchString,
           [FromQuery] int? sPage)
        {
            IEnumerable<Store> stores = await _stores.GetStoresAsync();
            ViewData["StoreId"] = (new SelectList(stores, "Id", "Name"))
                .Append(new SelectListItem("Add new Store", "addNewStore"));

            Products = (await _products.GetAllProductsAsync()).ToList();

            if (!string.IsNullOrEmpty(searchString))
            {
                ViewData["Search"] = searchString;
                Products = Products.Where(
                    p => p.Name.ToLower().Contains(searchString.ToLower().Trim())).ToList();
            }

            int pageSize = 5;
            int pageNumber = (sPage ?? 1);
            int pageCount = (int)Math.Ceiling((decimal)Products.Count / pageSize);

            ViewData["PageNumber"] = pageNumber;
            ViewData["PageCount"] = pageCount;

            Products = Products.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).ToList();
            return Page();
        }

        [BindProperty]
        public ImportOrder ImportOrder { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            IEnumerable<Store> stores = await _stores.GetStoresAsync();
            ViewData["StoreId"] = (new SelectList(stores, "Id", "Name"))
                .Append(new SelectListItem("Add new Store", "addNewStore"));
            if (!ModelState.IsValid)
            {
                return Page();
            }
            List<ImportOrderDetail> cart = HttpContext.Session.GetData<List<ImportOrderDetail>>("ImportOrderCart");
            ImportOrder.ImportOrderDetails = cart;
            await _importOrders.AddImportOrderAsync(ImportOrder);
            TempData["CreateImportOrderSuccessfully"] = true;
            HttpContext.Session.SetData("ImportOrderCart", null);
            return RedirectToPage("./Index");
        }

        private bool IsExistedInCart(List<ImportOrderDetail> cart, string productId)
        {
            return cart.Any(item => item.ProductId.Equals(productId));
        }

        public async Task<IActionResult> OnPostAddToCartAsync(string productId)
        {
            List<ImportOrderDetail> cart = HttpContext.Session.GetData<List<ImportOrderDetail>>("ImportOrderCart");

            if (!string.IsNullOrEmpty(productId))
            {
                Product product = await _products.GetProductAsync(productId);
                if (product != null)
                {
                    if (cart == null)
                    {
                        cart = new List<ImportOrderDetail>();
                        cart.Add(new ImportOrderDetail
                        {
                            ProductId = productId,
                            Name = product.Name,
                            Price = product.Price,
                            Quantity = 1,
                            Distributor = ""
                        });
                    } else
                    {
                        if (IsExistedInCart(cart, productId))
                        {
                            int index = cart.FindIndex(iod => iod.ProductId.Equals(productId));
                            cart[index].Quantity++;
                        } else
                        {
                            cart.Add(new ImportOrderDetail
                            {
                                ProductId = productId,
                                Name = product.Name,
                                Price = product.Price,
                                Quantity = 1,
                                Distributor = ""
                            });
                        }
                    }
                    HttpContext.Session.SetData("ImportOrderCart", cart);
                }
            }

            return new JsonResult(cart);
        }

        public IActionResult OnPostRemoveFromCartAsync(string productId)
        {
            List<ImportOrderDetail> cart = HttpContext.Session.GetData<List<ImportOrderDetail>>("ImportOrderCart");
            if (!string.IsNullOrEmpty(productId))
            {
                if (cart != null)
                {
                    cart.RemoveAll(iod => iod.ProductId.Equals(productId));
                }
                HttpContext.Session.SetData("ImportOrderCart", cart);
            }
            return new JsonResult(cart);
        }

        public IActionResult OnPostSaveCartItem(string productId, decimal price, int quantity, string distributor)
        {
            List<ImportOrderDetail> cart = HttpContext.Session.GetData<List<ImportOrderDetail>>("ImportOrderCart");
            if (!string.IsNullOrEmpty(productId))
            {
                if (cart != null)
                {
                    ImportOrderDetail iod = cart.FirstOrDefault(i => i.ProductId.Equals(productId));
                    if (iod != null)
                    {
                        iod.Price = price;
                        iod.Quantity = quantity;
                        iod.Distributor = distributor ?? "";
                    }
                }
                HttpContext.Session.SetData("ImportOrderCart", cart);
            }
            return new JsonResult(cart);
        }

        public async Task<IActionResult> OnPostCreateStoreAsync()
        {
            Store store = await GsmsUtils.ConvertRequestBody<Store>(Request);
            if (store != null && !string.IsNullOrEmpty(store.Name))
            {
                await _stores.AddStoreAsync(store);
            }
            return new JsonResult(store);
        }
    }
}
