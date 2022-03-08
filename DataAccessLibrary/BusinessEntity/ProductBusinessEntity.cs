using DataAccessLibrary.Interfaces;
namespace DataAccessLibrary.BusinessEntity
{
    public class ProductBusinessEntity
    {
        private IUnitOfWork work;
        public ProductBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }
    }
}
