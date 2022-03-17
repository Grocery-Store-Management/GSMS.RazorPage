using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BusinessObjectLibrary;
using BusinessObjectLibrary.ViewModel;
using DataAccessLibrary.BusinessEntity;
using DataAccessLibrary.Interfaces;
using GsmsLibrary;
using GsmsRazor.Server;
using GsmsRazor.SessionUtil;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using QRCoder;

namespace GsmsRazor.Pages
{
    [Authorize(Roles = "Cashier, Store Owner")]
    public class SaleModel : PageModel
    {
        private readonly ProductBusinessEntity _productEntity;
        private readonly ReceiptBusinessEntity _receiptEntity;
        private readonly CustomerBusinessEntity _customerEntity;
        private SignalRHub _hub;
        private const int pageSize = 3;

        public SaleModel(IUnitOfWork work, IHubContext<SignalRHub> contextR)
        {
            _productEntity = new ProductBusinessEntity(work);
            _receiptEntity = new ReceiptBusinessEntity(work);
            _customerEntity = new CustomerBusinessEntity(work);
            _hub = new SignalRHub(contextR);
        }


        [BindProperty]
        public Customer Customer { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                TempData["RegError"] = "These problems occured while trying to register: ";
                return Page();
            }

            Customer.Id = new Guid().ToString();
            Customer.CreatedDate = DateTime.Now;

            await _customerEntity.AddAsync(Customer);
            return RedirectToPage("./Index");
        }

        [BindProperty]
        public IEnumerable<Product> ProductList { get; set; }

        public async Task<IActionResult> OnGet(
            string searchString,
            int? pageIndex)
        {
            if (!pageIndex.HasValue)
            {
                pageIndex = 1;
            }
            int countProduct = _productEntity
                .GetProductsAsync(null, searchString, SortType.ASC, 0, pageSize).Result.Count(); //pageIndex = 0 to get all
            ProductList = await _productEntity.GetProductsAsync(null, searchString, SortType.ASC, pageIndex.Value, pageSize);

            decimal pageCount = Math.Ceiling((decimal)countProduct / pageSize);
            ViewData["pageCount"] = pageCount;
            ViewData["currentPage"] = pageIndex;
            ViewData["searchString"] = searchString;

            //Calculate total price
            List<CartItem> cart = null;
            cart = HttpContext.Session.GetData<List<CartItem>>("CART");
            if (cart != null)
            {
                decimal totalPrice = 0;
                foreach (CartItem item in cart)
                {
                    totalPrice += item.Quantity * item.Price;
                }
                ViewData["totalPrice"] = string.Format("{0:0}", totalPrice);
            }
            return Page();
        }

        private int ExistedProductInCart(List<CartItem> cart, string ProductId)
        {
            for (var i = 0; i < cart.Count; i++)
            {
                if (cart[i].ProductId == ProductId)
                {
                    return i;
                }
            }
            return -1;
        }

        public async Task<IActionResult> OnGetAddToCart(string Id, int? receiptPageIndex)
        {
            List<CartItem> cart = null;
            cart = HttpContext.Session.GetData<List<CartItem>>("CART");
            //Add to cart
            if (!string.IsNullOrEmpty(Id))
            {
                Product product = await _productEntity.GetProductAsync(Id);

                if (product != null)
                {
                    if (cart == null)
                    {
                        cart = new List<CartItem>();
                        cart.Add(new CartItem
                        {
                            ProductId = product.Id,
                            ProductName = product.Name,
                            Price = product.Price,
                            Quantity = 1
                        });
                    }
                    else
                    {
                        int index = ExistedProductInCart(cart, product.Id);
                        if (index == -1)
                        {
                            cart.Add(new CartItem
                            {
                                ProductId = product.Id,
                                ProductName = product.Name,
                                Price = product.Price,
                                Quantity = 1
                            });
                        }
                        else
                        {
                            cart[index].Quantity++;
                        }
                    }
                    HttpContext.Session.SetData("CART", cart);
                }
            }

            return new JsonResult(cart);
        }

        public IActionResult OnGetQuantityChange(string productId, int quantity)
        {
            List<CartItem> cart = null;
            cart = HttpContext.Session.GetData<List<CartItem>>("CART");
            if (cart != null)
            {
                foreach (CartItem item in cart)
                {
                    if (item.ProductId.Equals(productId))
                    {
                        item.Quantity = quantity;
                    }
                }
                HttpContext.Session.SetData("CART", cart);
            }
            return new JsonResult(cart);
        }

        public IActionResult OnGetRemoveFromCart(string productId)
        {
            //Remove from cart
            List<CartItem> cart = null;
            cart = HttpContext.Session.GetData<List<CartItem>>("CART");
            if (!string.IsNullOrEmpty(productId))
            {
                if (cart != null)
                {
                    foreach (CartItem item in cart)
                    {
                        if (item.ProductId.Equals(productId))
                        {
                            cart.Remove(item);
                            break;
                        }
                    }
                    HttpContext.Session.SetData("CART", cart);
                }

            }
            return new JsonResult(cart);
        }

        public async Task<IActionResult> OnGetInvoiceExport(string type)
        {
            int countProduct = _productEntity
                        .GetProductsAsync(null, null, SortType.ASC, 0, pageSize).Result.Count(); //pageIndex = 0 to get all
            ProductList = await _productEntity.GetProductsAsync(null, null, SortType.ASC, 1, pageSize);

            decimal pageCount = Math.Ceiling((decimal)countProduct / pageSize);
            ViewData["pageCount"] = pageCount;
            ViewData["currentPage"] = 1;
            ViewData["searchString"] = null;

            List<CartItem> cart = null;
            cart = HttpContext.Session.GetData<List<CartItem>>("CART");
            Receipt addedReceipt = null;
            string encodedBytes = "";

            //Save Receipt
            try
            {
                if (cart != null)
                {
                    List<ReceiptDetail> receiptDetails = new List<ReceiptDetail>();
                    foreach (CartItem item in cart)
                    {
                        receiptDetails.Add(new ReceiptDetail
                        {
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                            Price = item.Price,
                            Name = item.ProductName
                        });
                    }
                    Receipt receipt = new Receipt
                    {
                        StoreId = HttpContext.Session.GetString("STORE_ID"),
                        EmployeeId = HttpContext.Session.GetString("UID"),
                        ReceiptDetails = receiptDetails
                    };
                    addedReceipt = await _receiptEntity.AddReceiptAsync(receipt);
                } else
                {
                    throw new Exception("Receipt is empty, please add at least one product!!!");
                }
            }
            catch (Exception ex)
            {
                ViewData["Error"] = ex.Message;
                return Page();
            }

            //Calculate points
            decimal totalPrice = 0;
            int points = 0;

            if (cart != null)
            {
                foreach (CartItem item in cart)
                {
                    totalPrice += item.Price * item.Quantity;
                }
                if (!string.IsNullOrEmpty(type))
                {
                    if (type.Equals("InvoiceExport"))
                    {
                        points = (int)(totalPrice / 1000);
                        ViewData["QRType"] = "Accumulate";
                    } else if (type.Equals("PayByPoints"))
                    {
                        points = (int)(totalPrice / 1000) * (-1);
                        ViewData["QRType"] = "PointPay";
                    }
                }
                //QR Code
                string ip = SocketListener.GetLocalIPAddress().ToString();
                string qrText = $"{ip}${{\"createdDate\":\"Mar 3, 2022 16:09:43\",\"id\":,\"isDeleted\":false,\"password\":\"12345678\",\"phoneNumber\":\"0978665441\",\"point\":\"{points}\"}}";
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrText,
                QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                Bitmap qrCodeImage = qrCode.GetGraphic(20);
                encodedBytes = Convert.ToBase64String(BitmapToBytes(qrCodeImage));
                ViewData["encodedBytes"] = encodedBytes;
                SocketListener socketListener = new SocketListener(_hub);
                Thread thread = new Thread(new ThreadStart(() =>
                {
                    socketListener.StartServer();
                }));
                thread.Start();

                HttpContext.Session.Remove("CART");
            }
            return Page();
        }

        private static Byte[] BitmapToBytes(Bitmap img)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }
    }
}
