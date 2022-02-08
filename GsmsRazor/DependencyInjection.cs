using BusinessObjectLibrary;
using DataAccessLibrary.BusinessEntity;
using DataAccessLibrary.Implementations;
using DataAccessLibrary.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GsmsRazor
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepository(this IServiceCollection services)
        {
            #region Repository
            services.AddTransient<IBrandRepository, BrandRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<ICustomerRepository, CustomerRepository>();
            services.AddTransient<IEmployeeRepository, EmployeeRepository>();
            services.AddTransient<IImportOrderDetailRepository, ImportOrderDetailRepository>();
            services.AddTransient<IImportOrderRepository, ImportOrderRepository>();
            services.AddTransient<IProductDetailRepository, ProductDetailRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IReceiptDetailRepository, ReceiptDetailRepository>();
            services.AddTransient<IReceiptRepository, ReceiptRepository>();
            services.AddTransient<IStoreRepository, StoreRepository>();
            #endregion

            #region UnitOfWork
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            #endregion

            #region DbContext
            services.AddDbContext<GsmsContext>();
            #endregion

            #region BusinessEntity
            services.AddTransient<BrandBusinessEntity>();
            services.AddTransient<CategoryBusinessEntity>();
            services.AddTransient<CustomerBusinessEntity>();
            services.AddTransient<EmployeeBusinessEntity>();
            services.AddTransient<ImportOrderBusinessEntity>();
            services.AddTransient<ImportOrderDetailBusinessEntity>();
            services.AddTransient<ProductBusinessEntity>();
            services.AddTransient<ProductDetailBusinessEntity>();
            services.AddTransient<ReceiptBusinessEntity>();
            services.AddTransient<ReceiptDetailBusinessEntity>();
            services.AddTransient<StoreBusinessEntity>();
            #endregion

            return services;
        }
    }
}
