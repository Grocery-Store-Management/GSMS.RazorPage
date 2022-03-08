using DataAccessLibrary.Interfaces;
namespace DataAccessLibrary.BusinessEntity
{
    public class ImportOrderDetailBusinessEntity
    {
        private IUnitOfWork work;
        public ImportOrderDetailBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }
    }
}
