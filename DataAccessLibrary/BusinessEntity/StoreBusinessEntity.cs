using BusinessObjectLibrary;
using DataAccessLibrary.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLibrary.BusinessEntity
{
    public class StoreBusinessEntity
    {
        private IUnitOfWork work;
        public StoreBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }

        public async Task<IEnumerable<Store>> GetStoresAsync()
        {
            IEnumerable<Store> stores = await work.Stores.GetAllAsync();
            return stores.Where(s => !s.IsDeleted);
        }

        public async Task<Store> GetStoreAsync(string id)
        {
            Store store = await work.Stores.GetAsync(id);
            if (store != null && store.IsDeleted == true)
            {
                return null;
            }
            return store;
        }
    }
}
