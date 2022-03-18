using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObjectLibrary;
using DataAccessLibrary.BusinessEntity;
using DataAccessLibrary.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GsmsRazor.Pages.Employees
{
    public class DetailsModel : PageModel
    {
        private readonly EmployeeBusinessEntity _employeeEntity;

        public DetailsModel(IUnitOfWork work)
        {
            _employeeEntity = new EmployeeBusinessEntity(work);
        }

        public Employee Employee { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Employee = await _employeeEntity.GetEmployeeAsync(id);

            if (Employee == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
