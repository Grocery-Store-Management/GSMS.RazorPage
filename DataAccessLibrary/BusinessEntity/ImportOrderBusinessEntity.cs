using BusinessObjectLibrary;
using DataAccessLibrary.Interfaces;
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
    }
}
