using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObjectLibrary;
using DataAccessLibrary.BusinessEntity;
using DataAccessLibrary.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GsmsRazor.Pages.Employees
{
    [Authorize(Roles = "Store Owner")]
    public class IndexModel : PageModel
    {
        private readonly EmployeeBusinessEntity _employeeEntity;

        public IndexModel(IUnitOfWork work)
        {
            _employeeEntity = new EmployeeBusinessEntity(work);
        }

        public IEnumerable<Employee> Employee { get;set; }

        public async Task OnGetAsync(string searchString, int? pageIndex, string CreateMessage, string UpdateMessage)
        {
            int pageSize = 5;
            if (!pageIndex.HasValue)
            {
                pageIndex = 1;
            }
            IEnumerable<Employee> employees = await _employeeEntity.GetEmployeesAsync(null, 0, pageSize); // page = 0 to get all
            int countEmployee = (employees as List<Employee>).Count();
            Employee = await _employeeEntity.GetEmployeesAsync(searchString, pageIndex.Value, pageSize);

            decimal pageCount = Math.Ceiling((decimal)countEmployee / pageSize);
            ViewData["pageCount"] = pageCount;
            ViewData["currentPage"] = pageIndex;
            ViewData["searchString"] = searchString;
            if (!string.IsNullOrEmpty(CreateMessage))
            {
                ViewData["CreateMessage"] = CreateMessage;
            }
            if (!string.IsNullOrEmpty(UpdateMessage))
            {
                ViewData["UpdateMessage"] = UpdateMessage;
            }
        }
    }
}
