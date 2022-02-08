using DataAccessLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.BusinessEntity
{
    public class CustomerBusinessEntity
    {
        private IUnitOfWork work;
        public CustomerBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }
    }
}
