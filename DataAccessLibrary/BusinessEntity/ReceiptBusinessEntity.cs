using BusinessObjectLibrary;
using DataAccessLibrary.Interfaces;
using GsmsLibrary;
using System;
using System.Collections.Generic;
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
                throw new Exception("Cửa hàng không tồn tại!!");
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
                        throw new Exception("Sản phẩm không tồn tại!!");
                    }
                    if (receiptDetail.Quantity > product.StoredQuantity)
                    {
                        throw new Exception($"Số lượng {product.Name} vượt quá số lượng trong kho ({product.StoredQuantity})!!");
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
    }
}
