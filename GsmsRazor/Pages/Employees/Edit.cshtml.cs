using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObjectLibrary;
using BusinessObjectLibrary.ViewModel;
using DataAccessLibrary.BusinessEntity;
using DataAccessLibrary.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GsmsRazor.Pages.Employees
{
    [Authorize(Roles = "Store Owner")]
    public class EditModel : PageModel
    {
        private readonly EmployeeBusinessEntity _employeeEntity;
        private readonly StoreBusinessEntity _storeEntity;

        public EditModel(IUnitOfWork work)
        {
            _employeeEntity = new EmployeeBusinessEntity(work);
            _storeEntity = new StoreBusinessEntity(work);
        }

        [BindProperty]
        public bool IsCheck { get; set; }

        [BindProperty]
        public Employee Employee { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewData["Id"] = id;

            Employee = await _employeeEntity.GetEmployeeAsync(id);

            if (Employee == null)
            {
                return NotFound();
            }

            IEnumerable<Store> stores = await _storeEntity.GetStoresAsync();
            ViewData["Stores"] = stores;
            List<string> Role = new List<string>
            {
                "Cashier",
                "Store Owner"
            };
            ViewData["Role"] = Role;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (IsCheck)
            {
                Employee.IsDeleted = false;
            }
            else
            {
                Employee.IsDeleted = true;
            }
            await _employeeEntity.UpdateEmployeeAsync(Employee);

            return RedirectToPage("./Index", new { UpdateMessage = "Success"});
        }
    }
}
