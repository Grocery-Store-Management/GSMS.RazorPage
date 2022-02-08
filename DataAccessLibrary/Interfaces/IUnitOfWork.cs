using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        #region Repositories
        IBrandRepository Brands { get; }
        ICategoryRepository Categories { get; }
        ICustomerRepository Customers { get; }
        IEmployeeRepository Employees { get; }
        IImportOrderDetailRepository ImportOrderDetails { get; }
        IImportOrderRepository ImportOrders { get; }
        IProductDetailRepository ProductDetails { get; }
        IProductRepository Products { get; }
        IReceiptDetailRepository ReceiptDetails { get; }
        IReceiptRepository Receipts { get; }
        IStoreRepository Stores { get; }
        #endregion

        /// <summary>
        /// Save changes to database
        /// </summary>
        /// <returns></returns>
        int Save();
    }
}
