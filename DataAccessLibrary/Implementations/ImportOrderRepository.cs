using BusinessObjectLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLibrary.Interfaces;

namespace DataAccessLibrary.Implementations
{
    public class ImportOrderRepository : GenericRepository<ImportOrder>, IImportOrderRepository
    {
        public ImportOrderRepository(GsmsContext context) : base(context)
        {

        }
    }
}
