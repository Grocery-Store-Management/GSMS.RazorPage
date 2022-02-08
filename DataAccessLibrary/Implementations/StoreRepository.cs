using BusinessObjectLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLibrary.Interfaces;

namespace DataAccessLibrary.Implementations
{
    public class StoreRepository : GenericRepository<Store>, IStoreRepository
    {
        public StoreRepository(GsmsContext context) : base(context)
        {

        }
    }
}
