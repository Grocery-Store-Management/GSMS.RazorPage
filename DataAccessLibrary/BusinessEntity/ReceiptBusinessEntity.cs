﻿using DataAccessLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DataAccessLibrary.BusinessEntity
{
    public class ReceiptBusinessEntity
    {
        private IUnitOfWork work; 
        public ReceiptBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }    }
}
