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
        public IBrandRepository Brands { get; }
        public ICategoryRepository Categories { get; }
        public ICustomerRepository Customers { get; }
        public IEmployeeRepository Employees { get; }
        public IImportOrderDetailRepository ImportOrderDetails { get; }
        public IImportOrderRepository ImportOrders { get; }
        public IProductDetailRepository ProductDetails { get; }
        public IProductRepository Products { get; }
        public IReceiptDetailRepository ReceiptDetails { get; }
        public IReceiptRepository Receipts { get; }
        public IStoreRepository Stores { get; }
        #endregion

        public UnitOfWork(GsmsContext context,
            IBrandRepository brandRepository, ICategoryRepository categoryRepository,
            ICustomerRepository customerRepository, IEmployeeRepository employeeRepository,
            IImportOrderDetailRepository importOrderDetailRepository,
            IImportOrderRepository importOrderRepository,
            IProductDetailRepository productDetailRepository,
            IProductRepository productRepository,
            IReceiptDetailRepository receiptDetailRepository,
            IReceiptRepository receiptRepository,
            IStoreRepository storeRepository
            )
        {
            this.context = context;
            this.Brands = brandRepository;
            this.Categories = categoryRepository;
            this.Customers = customerRepository;
            this.Employees = employeeRepository;
            this.ImportOrderDetails = importOrderDetailRepository;
            this.ImportOrders = importOrderRepository;
            this.ProductDetails = productDetailRepository;
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
