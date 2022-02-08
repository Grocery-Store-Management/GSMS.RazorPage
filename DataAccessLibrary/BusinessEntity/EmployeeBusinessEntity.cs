using DataAccessLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DataAccessLibrary.BusinessEntity
{
    public class EmployeeBusinessEntity
    {
        private IUnitOfWork work; 
        public EmployeeBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }
     }
}
