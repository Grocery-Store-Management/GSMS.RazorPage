using BusinessObjectLibrary;
using DataAccessLibrary.Interfaces;
using GsmsLibrary;
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
            IEnumerable<Product> products = await work.Products.GetAllAsync();
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
    }
}
