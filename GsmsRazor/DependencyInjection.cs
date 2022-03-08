using BusinessObjectLibrary;
using DataAccessLibrary.Implementations;
using DataAccessLibrary.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GsmsRazor
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepository(this IServiceCollection services, IConfiguration configuration)
        {
            #region DbContext
            services.AddDbContext<GsmsContext>(options =>
                {
                    options.UseSqlServer(configuration.GetConnectionString("GsmsDb"));
                }
            );
            #endregion

            #region Repository
            services.AddScoped<IGenericRepository<Category>, GenericRepository<Category>>();
            services.AddScoped<IGenericRepository<Customer>, GenericRepository<Customer>>();
            services.AddScoped<IGenericRepository<Employee>, GenericRepository<Employee>>();
            services.AddScoped<IGenericRepository<ImportOrder>, GenericRepository<ImportOrder>>();
            services.AddScoped<IGenericRepository<ImportOrderDetail>, GenericRepository<ImportOrderDetail>>();
            services.AddScoped<IGenericRepository<Product>, GenericRepository<Product>>();
            services.AddScoped<IGenericRepository<Receipt>, GenericRepository<Receipt>>();
            services.AddScoped<IGenericRepository<ReceiptDetail>, GenericRepository<ReceiptDetail>>();
            services.AddScoped<IGenericRepository<Store>, GenericRepository<Store>>();
            #endregion

            #region UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            #endregion



            return services;
        }
    }
}
