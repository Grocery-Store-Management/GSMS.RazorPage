﻿using BusinessObjectLibrary;
using DataAccessLibrary.Implementations;
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
        GenericRepository<Category> Categories { get; }
        GenericRepository<Customer> Customers { get; }
        GenericRepository<Employee> Employees { get; }
        GenericRepository<ImportOrderDetail> ImportOrderDetails { get; }
        GenericRepository<ImportOrder> ImportOrders { get; }
        GenericRepository<Product> Products { get; }
        GenericRepository<ReceiptDetail> ReceiptDetails { get; }
        GenericRepository<Receipt> Receipts { get; }
        GenericRepository<Store> Stores { get; }
        #endregion

        /// <summary>
        /// Save changes to database
        /// </summary>
        /// <returns></returns>
        int Save();
    }
}
