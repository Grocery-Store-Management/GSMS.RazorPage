using BusinessObjectLibrary;
using DataAccessLibrary.Interfaces;
using GsmsLibrary;
using System;
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

        public async Task<Store> AddStoreAsync(Store newStore)
        {
            newStore.Id = GsmsUtils.CreateGuiId();
            newStore.CreatedDate = DateTime.Now;
            newStore.IsDeleted = false;
            await work.Stores.AddAsync(newStore);
            await work.Save();
            return newStore;
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

        public async Task<IEnumerable<Store>> GetStoresAsync()
        {
            IEnumerable<Store> stores = await work.Stores.GetAllAsync();
            stores = from store in stores
                     where store.IsDeleted == false
                     select store;
            return stores;
        }

    }
}
