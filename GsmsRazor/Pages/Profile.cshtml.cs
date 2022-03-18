using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObjectLibrary;
using DataAccessLibrary.BusinessEntity;
using DataAccessLibrary.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GsmsRazor.Pages
{
    public class ProfileModel : PageModel
    {
        private readonly EmployeeBusinessEntity _employeeEntity;
        private readonly StoreBusinessEntity _storeEntity;

        public ProfileModel(IUnitOfWork work)
        {
            _employeeEntity = new EmployeeBusinessEntity(work);
            _storeEntity = new StoreBusinessEntity(work);
        }

        [BindProperty]
        public bool IsCheck { get; set; }

        [BindProperty]
        public Employee Employee { get; set; }

        public async Task<IActionResult> OnGetAsync(string id, string UpdateMessage)
        {
            if (UpdateMessage != null)
            {
                ViewData["UpdateMessage"] = UpdateMessage;
            }
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

            return RedirectToPage("/Profile", new { UpdateMessage = "Update profile success!" , id = Employee.Id});
        }
    }
}

