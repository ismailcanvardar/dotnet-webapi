using System;
using System.Linq;
using KariyerAppApi.Models;

namespace KariyerAppApi.Data
{
    public class AboutEmployeeRepo : IAboutEmployeeRepo
    {
        private readonly BaseContext _context;

        public AboutEmployeeRepo(BaseContext context)
        {
            _context = context;
        }

        public void CreateAboutEmployee(AboutEmployee aboutEmployee)
        {
            _context.AboutEmployees.Add(aboutEmployee);
        }

        public AboutEmployee GetAboutEmployee(Guid employeeId)
        {
            var foundAboutEmployee = _context.AboutEmployees.FirstOrDefault(ae => ae.EmployeeId == employeeId);
            return foundAboutEmployee;
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}
