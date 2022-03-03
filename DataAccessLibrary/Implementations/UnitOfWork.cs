using BusinessObjectLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLibrary.Interfaces;

namespace DataAccessLibrary.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GsmsContext context;

        #region Repositories
        public IGenericRepository<Category> Categories { get; }
        public IGenericRepository<Customer> Customers { get; }
        public IGenericRepository<Employee> Employees { get; }
        public IGenericRepository<ImportOrderDetail> ImportOrderDetails { get; }
        public IGenericRepository<ImportOrder> ImportOrders { get; }
        public IGenericRepository<Product> Products { get; }
        public IGenericRepository<ReceiptDetail> ReceiptDetails { get; }
        public IGenericRepository<Receipt> Receipts { get; }
        public IGenericRepository<Store> Stores { get; }
        #endregion

        //#region Database context
        //public GsmsContext Context
        //{
        //    get
        //    {
        //        return this.context;
        //    }
        //}
        //#endregion

        public UnitOfWork(GsmsContext context,
            IGenericRepository<Category> categoryRepository,
            IGenericRepository<Customer> customerRepository, IGenericRepository<Employee> employeeRepository,
            IGenericRepository<ImportOrderDetail> importOrderDetailRepository,
            IGenericRepository<ImportOrder> importOrderRepository,
            IGenericRepository<Product> productRepository,
            IGenericRepository<ReceiptDetail> receiptDetailRepository,
            IGenericRepository<Receipt> receiptRepository,
            IGenericRepository<Store> storeRepository
            )
        {
            this.context = context;
            this.Categories = categoryRepository;
            this.Customers = customerRepository;
            this.Employees = employeeRepository;
            this.ImportOrderDetails = importOrderDetailRepository;
            this.ImportOrders = importOrderRepository;
            this.Products = productRepository;
            this.ReceiptDetails = receiptDetailRepository;
            this.Receipts = receiptRepository;
            this.Stores = storeRepository;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
        }

        public int Save()
        {
            return context.SaveChanges();
        }
    }
}
