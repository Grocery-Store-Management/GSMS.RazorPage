﻿using DataAccessLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DataAccessLibrary.BusinessEntity
{
    public class ProductDetailBusinessEntity
    {
        private IUnitOfWork work; 
        public ProductDetailBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }
    }
}
