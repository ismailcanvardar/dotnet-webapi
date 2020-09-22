using System;
using System.Collections.Generic;
using Commander.Models;

namespace Commander.Data
{
    public interface IEmployeeRepo
    {
        bool SaveChanges();
        IEnumerable<Employee> SearchEmployees(string searchCriteria, int offset, int limit);
        Employee GetEmployee(Guid employeeId);
        void RegisterEmployee(Employee user);
        Employee GetEmployeeByEmail(string email);
        void EmployeeUpdate(Employee employee);
    }
}