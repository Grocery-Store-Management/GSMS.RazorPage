using DataAccessLibrary.Interfaces;
namespace DataAccessLibrary.BusinessEntity
{
    public class ReceiptDetailBusinessEntity
    {
        private IUnitOfWork work;
        public ReceiptDetailBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }
    }
}
