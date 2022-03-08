using DataAccessLibrary.Interfaces;
namespace DataAccessLibrary.BusinessEntity
{
    public class StoreBusinessEntity
    {
        private IUnitOfWork work;
        public StoreBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }
    }
}
