using BusinessObjectLibrary;
using DataAccessLibrary.Interfaces;
using GsmsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLibrary.BusinessEntity
{
    public class ImportOrderBusinessEntity
    {
        private IUnitOfWork work;
        public ImportOrderBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }

        public async Task<IEnumerable<ImportOrder>> GetImportOrdersBySaleMonthYearAsync(int saleMonth, int saleYear)
        {
            IEnumerable<ImportOrder> importOrders = await work.ImportOrders.GetAllAsync();
            importOrders = importOrders.Where(i => i.IsDeleted == false && i.CreatedDate.Month == saleMonth && i.CreatedDate.Year == saleYear);
            return importOrders;
        }
        public async Task<ImportOrder> AddImportOrderAsync(ImportOrder newImportOrder)
        {
            List<ImportOrderDetail> importOrderDetails = newImportOrder.ImportOrderDetails.ToList();
            Store store = await work.Stores.GetAsync(newImportOrder.StoreId);
            if (store == null)
            {
                throw new Exception("Store is not existed!!");
            }
            newImportOrder.Id = GsmsUtils.CreateGuiId();
            newImportOrder.CreatedDate = DateTime.Now;
            newImportOrder.IsDeleted = false;
            if (newImportOrder.ImportOrderDetails != null && newImportOrder.ImportOrderDetails.Count > 0)
            {
                foreach (ImportOrderDetail importOrderDetail in newImportOrder.ImportOrderDetails)
                {
                    importOrderDetail.Id = GsmsUtils.CreateGuiId();
                    Product product = await work.Products.GetAsync(importOrderDetail.ProductId);                 
                    if (product == null || product.IsDeleted == true)
                    {
                        throw new Exception("Product is not existed!!");
                    }
                    importOrderDetail.OrderId = newImportOrder.Id;

                    // TODO Code
                    importOrderDetail.Name = product.Name;
                    importOrderDetail.Price = product.Price;

                    //await work.ImportOrderDetails.AddAsync(importOrderDetail);
                    product.StoredQuantity += importOrderDetail.Quantity;
                    work.Products.Update(product);
                }
            }
            await work.ImportOrders.AddAsync(newImportOrder);
            await work.Save();
            //newImportOrder.ImportOrderDetails = importOrderDetails;
            return newImportOrder;
        }

        public async Task<IEnumerable<ImportOrder>> GetAllImportOrdersAsync()
        {
            IEnumerable<ImportOrder> importOrders = await work.ImportOrders.GetAllAsync("Store", "ImportOrderDetails");
            importOrders = from order in importOrders
                           where order.IsDeleted == false
                           select order;
            return importOrders;
        }
        public async Task<IEnumerable<ImportOrder>> GetImportOrdersAsync(
            DateTime? startDate,
            DateTime? endDate,
            string? searchByName,
            SortType? sortByName,
            SortType? sortByDate,
            int page,
            int pageSize)
        {
            IEnumerable<ImportOrder> importOrders = await work.ImportOrders.GetAllAsync("Store");
            importOrders = importOrders.Where(i => i.IsDeleted == false);
            if (startDate.HasValue && endDate.HasValue)
            {
                importOrders = importOrders.Where(i => i.CreatedDate >= startDate && i.CreatedDate <= endDate);
            }
            if (!string.IsNullOrEmpty(searchByName))
            {
                importOrders = importOrders.Where(i => i.Name.ToLower().Contains(searchByName.Trim().ToLower()));
            }
            if (sortByName.HasValue)
            {
                importOrders = GsmsUtils.Sort(importOrders, i => i.Name, sortByName.Value);
            }
            else if (sortByDate.HasValue)
            {
                importOrders = GsmsUtils.Sort(importOrders, i => i.CreatedDate, sortByDate.Value);
            }
            else if (!sortByName.HasValue && !sortByDate.HasValue)
            {
                importOrders = GsmsUtils.Sort(importOrders, i => i.CreatedDate, SortType.DESC);
            }
            importOrders = GsmsUtils.Paging(importOrders, page, pageSize);
            //foreach (ImportOrder importOrder in importOrders)
            //{
            //    string sql = "select * from ImportOrderDetail where ImportOrderId = '" + importOrder.Id + "'";
            //    IEnumerable<ImportOrderDetail> details = await work.ImportOrderDetails.ExecuteQueryAsync(sql);
            //    importOrder.ImportOrderDetails = details.ToList();
            //}
            return importOrders;
        }

        public async Task<ImportOrder> GetAsync(string id)
        {
            ImportOrder importOrder = await work.ImportOrders.GetAsync(id);
            if (importOrder != null && importOrder.IsDeleted == true)
            {
                return null;
            }
            return importOrder;
        }
    }
}
