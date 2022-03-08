using DataAccessLibrary.Interfaces;

namespace DataAccessLibrary.BusinessEntity
{
    public class BrandBusinessEntity
    {
        private IUnitOfWork work;
        public BrandBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }
    }
}
