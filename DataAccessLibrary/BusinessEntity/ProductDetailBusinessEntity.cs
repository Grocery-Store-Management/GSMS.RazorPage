using DataAccessLibrary.Interfaces;
namespace DataAccessLibrary.BusinessEntity
{
    public class ProductDetailBusinessEntity
    {
        private IUnitOfWork work;
        public ProductDetailBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }
    }
}
