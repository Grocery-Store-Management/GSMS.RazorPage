using DataAccessLibrary.Interfaces;
namespace DataAccessLibrary.BusinessEntity
{
    public class ReceiptBusinessEntity
    {
        private IUnitOfWork work;
        public ReceiptBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }
    }
}
