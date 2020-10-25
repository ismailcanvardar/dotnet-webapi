using System;
using System.Collections.Generic;
using System.Linq;
using KariyerAppApi.Models;
using Microsoft.EntityFrameworkCore;

namespace KariyerAppApi.Data
{
    public class EmployeeRepo : IEmployeeRepo
    {
        private readonly BaseContext _context;

        public EmployeeRepo(BaseContext context)
        {
            _context = context;
        }

        public IQueryable GetEmployee(Guid employeeId)
        {
            return from employee in _context.Employees.Where(e => e.EmployeeId.Equals(employeeId)) join aboutEmployee in _context.AboutEmployees on employee.EmployeeId equals aboutEmployee.EmployeeId select new { employee, aboutEmployee };
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

        public Employee RegisterEmployee(Employee employee)
        {
            _context.Employees.Add(employee);
            return employee;
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