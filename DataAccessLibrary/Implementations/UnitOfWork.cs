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
        public GenericRepository<Category> Categories { get; }
        public GenericRepository<Customer> Customers { get; }
        public GenericRepository<Employee> Employees { get; }
        public GenericRepository<ImportOrderDetail> ImportOrderDetails { get; }
        public GenericRepository<ImportOrder> ImportOrders { get; }
        public GenericRepository<Product> Products { get; }
        public GenericRepository<ReceiptDetail> ReceiptDetails { get; }
        public GenericRepository<Receipt> Receipts { get; }
        public GenericRepository<Store> Stores { get; }
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
            GenericRepository<Category> categoryRepository,
            GenericRepository<Customer> customerRepository, GenericRepository<Employee> employeeRepository,
            GenericRepository<ImportOrderDetail> importOrderDetailRepository,
            GenericRepository<ImportOrder> importOrderRepository,
            GenericRepository<Product> productRepository,
            GenericRepository<ReceiptDetail> receiptDetailRepository,
            GenericRepository<Receipt> receiptRepository,
            GenericRepository<Store> storeRepository
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
