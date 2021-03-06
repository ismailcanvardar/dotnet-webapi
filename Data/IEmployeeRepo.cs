using System;
using System.Collections.Generic;
using System.Linq;
using KariyerAppApi.Dtos;
using KariyerAppApi.Models;

namespace KariyerAppApi.Data
{
    public interface IEmployeeRepo
    {
        bool SaveChanges();
        IEnumerable<Employee> SearchEmployees(string searchCriteria, int offset, int limit);
        IQueryable GetEmployee(Guid employeeId);
        Employee RegisterEmployee(Employee user);
        Employee GetEmployeeByEmail(string email);
        bool EmployeeUpdate(EmployeeUpdateDto employeeUpdateDto);
    }
}