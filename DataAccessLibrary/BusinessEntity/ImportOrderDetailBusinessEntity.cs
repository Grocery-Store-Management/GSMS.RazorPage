using BusinessObjectLibrary;
using DataAccessLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLibrary.BusinessEntity
{
    public class ImportOrderDetailBusinessEntity
    {
        private IUnitOfWork work;
        public ImportOrderDetailBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }

        public async Task<IEnumerable<ImportOrderDetail>> GetImportOrderDetailsAsync()
        {
            return await work.ImportOrderDetails.GetAllAsync();
        }

        public async Task<IEnumerable<ImportOrderDetail>> GetImportOrderDetailsByImportOrderIdAsync(string importOrderId)
        {
            IEnumerable<ImportOrderDetail> importOrderDetails;

            ImportOrder importOrder = await work.ImportOrders.GetAsync(importOrderId);
            if (importOrder == null || importOrder.IsDeleted == true)
            {
                throw new Exception("Import order does not exist!");
            }
            string sql = "select * from ImportOrderDetail where OrderId = '" + importOrderId + "'";
            importOrderDetails = await work.ImportOrderDetails.ExecuteQueryAsync(sql);
            return importOrderDetails;
        }
    }
}
