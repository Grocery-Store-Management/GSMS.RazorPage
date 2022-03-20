using BusinessObjectLibrary;
using DataAccessLibrary.Interfaces;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<Customer> AddAsync(Customer customer)
        {
            await work.Customers.AddAsync(customer);
            await work.Save();
            return customer;
        }

        public async Task<bool> CheckExists(Customer customer)
        {
            IEnumerable<Customer> res = await work.Customers.GetAllAsync();
            var found = res.ToList().FirstOrDefault(c => c.PhoneNumber == customer.PhoneNumber) != null;
            return found;
        }

    }
}
