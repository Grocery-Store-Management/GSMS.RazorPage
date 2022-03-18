using BusinessObjectLibrary;
using DataAccessLibrary.Interfaces;
using GsmsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLibrary.BusinessEntity
{
    public class EmployeeBusinessEntity
    {
        private IUnitOfWork work;
        public EmployeeBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }

        public async Task<IEnumerable<Employee>> GetEmployeesAsync(string searchString, int page, int pageSize)
        {
            IEnumerable<Employee> employees = await work.Employees.GetAllAsync();
            foreach(var employee in employees)
            {
                employee.Store = await work.Stores.GetAsync(employee.StoreId);
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                employees = employees.Where(e => e.Name.ToLower().Contains(searchString.ToLower()));
            }

            employees = GsmsUtils.Paging(employees, page, pageSize);
            return employees;
        }

        public async Task<Employee> GetEmployeeAsync(string id)
        {
            Employee employee = await work.Employees.GetAsync(id);
            //if (employee != null && employee.IsDeleted == true)
            //{
            //    return null;
            //}
            employee.Store = await work.Stores.GetAsync(employee.StoreId);
            return employee;
        }

        private async Task CheckEmployee(Employee employee)
        {
            Store store = await work.Stores.GetAsync(employee.StoreId);
            if (store == null)
            {
                throw new Exception("Store does not exist!");
            }
        }
        public async Task<Employee> AddEmployeeAsync(Employee newEmployee)
        {
            await CheckEmployee(newEmployee);
            newEmployee.Id = GsmsUtils.CreateGuiId();
            newEmployee.CreatedDate = DateTime.Now;
            newEmployee.IsDeleted = false;
            await work.Employees.AddAsync(newEmployee);
            await work.Save();
            return newEmployee;
        }

        public async Task<Employee> UpdateEmployeeAsync(Employee updatedEmployee)
        {
            Employee employee = await work.Employees.GetAsync(updatedEmployee.Id);
            //if (employee == null || employee.IsDeleted == true)
            //{
            //    throw new Exception("Employee does not exist!");
            //}
            await CheckEmployee(updatedEmployee);
            employee.Name = updatedEmployee.Name;
            employee.Password = updatedEmployee.Password;
            employee.IsDeleted = updatedEmployee.IsDeleted;
            employee.StoreId = updatedEmployee.StoreId;
            employee.Role = updatedEmployee.Role;
            work.Employees.Update(employee);
            await work.Save();
            return employee;
        }
    }
}
