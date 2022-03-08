using DataAccessLibrary.Interfaces;

namespace DataAccessLibrary.BusinessEntity
{
    public class CategoryBusinessEntity
    {
        private IUnitOfWork work;
        public CategoryBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }
    }
}
