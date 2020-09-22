using System;
using System.Collections.Generic;
using System.Linq;
using Commander.Models;
using Microsoft.EntityFrameworkCore;

namespace Commander.Data
{
    public class EmployeeRepo : IEmployeeRepo
    {
        private readonly BaseContext _context;

        public EmployeeRepo(BaseContext context)
        {
            _context = context;
        }

        public Employee GetEmployee(Guid employeeId)
        {
            return _context.Employees.FirstOrDefault(p => p.EmployeeId == employeeId);
        }

        public Employee GetEmployeeByEmail(string email)
        {
            var employee = _context.Employees.FirstOrDefault(p => p.Email == email);

            if (employee != null)
            {
                return employee;
            }

            return null;
        }

        public void RegisterEmployee(Employee employee)
        {
             _context.Employees.Add(employee);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public IEnumerable<Employee> SearchEmployees(string searchCriteria, int offset, int limit)
        {
            return _context.Employees.Where(emp => emp.Name.Contains(searchCriteria) || emp.Surname.Contains(searchCriteria)).Take(limit).Skip(offset).ToList();
        }

        public void EmployeeUpdate(Employee employee)
        {
            throw new NotImplementedException();
        }
    }
}