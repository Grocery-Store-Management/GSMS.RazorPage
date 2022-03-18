using BusinessObjectLibrary;
using DataAccessLibrary.Interfaces;
using GsmsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLibrary.BusinessEntity
{
    public class ProductBusinessEntity
    {
        private IUnitOfWork work;
        public ProductBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            IEnumerable<Product> products = await work.Products.GetAllAsync("Category");
            products = from product in products
                       where product.IsDeleted == false
                       select product;
            return products;
        }

        public async Task<IEnumerable<Product>> GetActiveProductsAsync()
        {
            IEnumerable<Product> products = await work.Products.GetAllAsync();
            return products.Where(p => !p.IsDeleted);
        }

        public async Task<IEnumerable<Product>> GetProductsAsync(
            string categoryId,
            string searchByName,
            SortType? sortByName,
            int page,
            int pageSize
            )
        {
            IEnumerable<Product> products = await work.Products.GetAllAsync();
            products = from product in products
                       where product.IsDeleted == false
                       select product;

            if (!string.IsNullOrEmpty(categoryId))
            {
                products = from product in products
                           where product.CategoryId.Equals(categoryId)
                           select product;
            }

            if (!string.IsNullOrEmpty(searchByName))
            {
                products = from product in products
                           where product.Name.ToLower().Contains(searchByName.ToLower())
                           select product;
            }

            if (sortByName.HasValue)
            {
                products = GsmsUtils.Sort(products, p => p.Name, sortByName.Value);
            }

            products = GsmsUtils.Paging(products, page, pageSize);

            foreach (Product product in products)
            {
                if (product.Category != null)
                {
                    product.Category.Products = null;
                }
                product.ImportOrderDetails = null;
                product.ReceiptDetails = null;
            }

            return products;
        }
        public async Task<Product> GetProductAsync(string id)
        {
            Product product = await work.Products.GetAsync(id);
            if (product != null && product.IsDeleted == true)
            {
                return null;
            }
            return product;
        }

        public async Task<Product> AddProductAsync(Product newProduct)
        {
            await CheckProduct(newProduct);
            newProduct.Id = GsmsUtils.CreateGuiId();
            newProduct.IsDeleted = false;
            //newProduct.Status = 1;
            await UpdateProductStatus(newProduct);

            await work.Products.AddAsync(newProduct);
            await work.Save();
            return newProduct;
        }

        public async Task<Product> UpdateProductAsync(Product updatedProduct)
        {
            Product product = await work.Products.GetAsync(updatedProduct.Id);
            if (product == null || product.IsDeleted == true)
            {
                throw new Exception("Product does not exist!");
            }
            await CheckProduct(updatedProduct);
            product.Name = updatedProduct.Name;
            product.CategoryId = updatedProduct.CategoryId;
            product.IsDeleted = updatedProduct.IsDeleted;
            product.ExpiringDate = updatedProduct.ExpiringDate;
            product.Price = updatedProduct.Price;
            //product.Status = updatedProduct.Status;
            product.StoredQuantity = updatedProduct.StoredQuantity;

            await UpdateProductStatus(product);

            work.Products.Update(product);
            await work.Save();
            return product;
        }

        private async Task UpdateProductStatus(Product product)
        {
            if (product.StoredQuantity == 0)
            {
                product.Status = Status.OUT_OF_STOCK;
            }
            else if (product.StoredQuantity < 10)
            {
                product.Status = Status.ALMOST_OUT_OF_STOCK;
            }
            else
            {
                IEnumerable<ReceiptDetail> receiptDetails = await work.ReceiptDetails.GetAllAsync();
                receiptDetails = from detail in receiptDetails
                                 where detail.ProductId.Equals(product.Id)
                                      && detail.CreatedDate.Month == DateTime.Now.Month
                                 select detail;
                if (receiptDetails.Count() > 50)
                {
                    product.Status = Status.BEST_SELLER;
                }
                else
                {
                    product.Status = Status.AVAILABLE;
                }
            }
        }

        private async Task CheckProduct(Product product)
        {
            Category category = await work.Categories.GetAsync(product.CategoryId);
            if (category == null)
            {
                throw new Exception("Category does not exist!");
            }
            if (product.Price < 0)
            {
                throw new Exception("Product Price must be a positive decimal number!!");
            }
            if (product.StoredQuantity < 0)
            {
                throw new Exception("Stored Quantity must be a positive number!!");
            }
        }

        public async Task DeleteProductAsync(string id)
        {
            Product product = await work.Products.GetAsync(id);
            if (product == null)
            {
                throw new Exception("Product does not exist!");
            }
            //work.Products.Delete(product);
            product.IsDeleted = true;
            work.Products.Update(product);
            await work.Save();
        }
    }
}
