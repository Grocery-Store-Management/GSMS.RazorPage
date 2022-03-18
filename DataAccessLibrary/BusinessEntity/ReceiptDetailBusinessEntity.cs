using BusinessObjectLibrary;
using DataAccessLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLibrary.BusinessEntity
{
    public class ReceiptDetailBusinessEntity
    {
        private IUnitOfWork work;
        public ReceiptDetailBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }

        public async Task<IEnumerable<ReceiptDetail>> GetReceiptDetailsAsync()
        {
            return await work.ReceiptDetails.GetAllAsync();
        }

        public async Task<IEnumerable<ReceiptDetail>> GetReceiptDetailsByReceiptIdAsync(string receiptId)
        {
            IEnumerable<ReceiptDetail> receiptDetails;

            Receipt receipt = await work.Receipts.GetAsync(receiptId);
            if (receipt == null || receipt.IsDeleted == true)
            {
                throw new Exception("Receipt does not exist!");
            }
            string sql = "select * from ReceiptDetail where ReceiptId = '" + receiptId + "'";
            receiptDetails = await work.ReceiptDetails.ExecuteQueryAsync(sql);
            return receiptDetails;
        }

        public decimal GetReceiptDailyRevenue(IEnumerable<ReceiptDetail> receiptDetails, int saleDay, int saleMonth, int saleYear)
        {
            decimal revenue = 0;
            //IEnumerable<ReceiptDetail> receiptDetails = await work.ReceiptDetails.GetAllAsync();
            receiptDetails = receiptDetails
                .Where(r => r.CreatedDate.Day == saleDay && r.CreatedDate.Month == saleMonth && r.CreatedDate.Year == saleYear);
            foreach(ReceiptDetail receiptDetail in receiptDetails)
            {
                revenue += receiptDetail.Price * receiptDetail.Quantity;
            }
            return revenue;
        }
    }
}
