using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObjectLibrary;
using BusinessObjectLibrary.ViewModel;
using DataAccessLibrary.BusinessEntity;
using DataAccessLibrary.Interfaces;
using GsmsLibrary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GsmsRazor.Pages.Employees
{
    public class CreateModel : PageModel
    {
        private readonly EmployeeBusinessEntity _employeeEntity;
        private readonly StoreBusinessEntity _storeEntity;
        public CreateModel(IUnitOfWork work)
        {
            _employeeEntity = new EmployeeBusinessEntity(work);
            _storeEntity = new StoreBusinessEntity(work);
        }

        [BindProperty]
        public EmployeeVM Employee { get; set; }

        public async Task<IActionResult> OnGet()
        {
            IEnumerable<Store> stores = await _storeEntity.GetStoresAsync();
            ViewData["Stores"] = stores;
            List<string> Role = new List<string>
            {
                "Cashier",
                "Store Owner"
            };
            ViewData["Role"] = new SelectList(Role);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Employee employee = new Employee
            {
                Name = Employee.Name,
                Password = Employee.Password,
                StoreId = Employee.StoreId,
                Role = Employee.Role
            };

            await _employeeEntity.AddEmployeeAsync(employee);
            return RedirectToPage("./Index");
        }
    }

}
