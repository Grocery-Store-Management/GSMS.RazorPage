using BusinessObjectLibrary;
using DataAccessLibrary.Interfaces;
using GsmsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLibrary.BusinessEntity
{
    public class ReceiptBusinessEntity
    {
        private IUnitOfWork work;
        public ReceiptBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }
        public async Task<Receipt> AddReceiptAsync(Receipt newReceipt)
        {
            Store store = await work.Stores.GetAsync(newReceipt.StoreId);
            if (store == null)
            {
                throw new Exception("Store is not existed!");
            }
            newReceipt.Id = GsmsUtils.CreateGuiId();
            newReceipt.CreatedDate = DateTime.Now;
            newReceipt.IsDeleted = false;
            if (newReceipt.ReceiptDetails.Count > 0)
            {
                foreach (ReceiptDetail receiptDetail in newReceipt.ReceiptDetails)
                {
                    receiptDetail.Id = GsmsUtils.CreateGuiId();
                    Product product = await work.Products.GetAsync(receiptDetail.ProductId);
                    if (product == null || product.IsDeleted == true)
                    {
                        throw new Exception("Product is not existed!");
                    }
                    if (receiptDetail.Quantity > product.StoredQuantity)
                    {
                        throw new Exception($"Purchase of {product.Name} exceed stored quantity ({product.StoredQuantity})!!");
                    }
                    if(receiptDetail.Quantity == product.StoredQuantity)
                    {
                        product.Status = Status.OUT_OF_STOCK;
                    } else if(product.StoredQuantity - receiptDetail.Quantity < 10 && product.StoredQuantity - receiptDetail.Quantity > 0)
                    {
                        product.Status = Status.ALMOST_OUT_OF_STOCK;
                        //product.Status = 3; //almost out of stock
                    }
                    receiptDetail.ReceiptId = newReceipt.Id;
                    receiptDetail.CreatedDate = DateTime.Now;
                    await work.ReceiptDetails.AddAsync(receiptDetail);
                    product.StoredQuantity -= receiptDetail.Quantity;
                    work.Products.Update(product);
                }
            }
            await work.Receipts.AddAsync(newReceipt);
            await work.Save();
            return newReceipt;
        }

        public async Task<IEnumerable<Receipt>> GetReceiptsBySaleMonthYearAsync(int saleMonth, int saleYear)
        {
            IEnumerable<Receipt> receipts = await work.Receipts.GetAllAsync();
            receipts = receipts.Where(i => i.IsDeleted == false && i.CreatedDate.Month == saleMonth && i.CreatedDate.Year == saleYear);
            return receipts;
        }
    }
}
