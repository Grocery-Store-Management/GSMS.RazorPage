using DataAccessLibrary.Interfaces;
namespace DataAccessLibrary.BusinessEntity
{
    public class EmployeeBusinessEntity
    {
        private IUnitOfWork work;
        public EmployeeBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }
    }
}
