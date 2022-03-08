using BusinessObjectLibrary;
using DataAccessLibrary.Interfaces;
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
    }
}
