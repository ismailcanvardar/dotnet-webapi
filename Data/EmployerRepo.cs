using System;
using System.Collections.Generic;
using System.Linq;
using KariyerAppApi.Models;
using Microsoft.EntityFrameworkCore;

namespace KariyerAppApi.Data
{
    public class EmployerRepo : IEmployerRepo
    {
        private readonly BaseContext _context;

        public EmployerRepo(BaseContext context)
        {
            _context = context;
        }

        public void EmployerUpdate(Employer employer)
        {
            throw new NotImplementedException();
        }

        public Employer GetEmployer(Guid employerId)
        {
           return _context.Employers.FirstOrDefault(p => p.EmployerId == employerId);
        }

        public Employer GetEmployerByEmail(string email)
        {
            return _context.Employers.FirstOrDefault(p => p.Email == email);
        }

        public void RegisterEmployer(Employer employer)
        {
            _context.Employers.Add(employer);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public IEnumerable<Employer> SearchEmployers(string searchCriteria, int offset, int limit)
        {
            return _context.Employers.Where(employer => employer.Name.Contains(searchCriteria)).Take(limit).Skip(offset).ToList();
        }
    }
}
