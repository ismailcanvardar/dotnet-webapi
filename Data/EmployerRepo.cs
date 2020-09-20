using System;
using System.Collections.Generic;
using System.Linq;
using Commander.Models;
using Microsoft.EntityFrameworkCore;

namespace Commander.Data
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

        public Employer GetEmployer(string externalId)
        {
           return _context.Employers.FirstOrDefault(p => p.ExternalId == externalId);
        }

        public Employer GetEmployerByEmail(string email)
        {
            return _context.Employers.FirstOrDefault(p => p.Email == email);
        }

        public void RegisterEmployer(Employer employer)
        {
            if (employer == null)
            {
                throw new ArgumentNullException(nameof(employer));
            }

            var isUserExists = _context.Users.FirstOrDefault(p => p.Email == employer.Email);

            if (isUserExists != null)
            {
                throw new ArgumentNullException(nameof(employer));
            }

            // Adds uuid to user's external id property
            employer.ExternalId = Guid.NewGuid().ToString();

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
