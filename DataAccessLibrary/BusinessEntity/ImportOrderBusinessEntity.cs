using DataAccessLibrary.Interfaces;
namespace DataAccessLibrary.BusinessEntity
{
    public class ImportOrderBusinessEntity
    {
        private IUnitOfWork work;
        public ImportOrderBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }
    }
}
