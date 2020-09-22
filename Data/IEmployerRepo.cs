using System;
using System.Collections.Generic;
using KariyerAppApi.Models;

namespace KariyerAppApi.Data
{
    public interface IEmployerRepo
    {
        bool SaveChanges();
        IEnumerable<Employer> SearchEmployers(string searchCriteria, int offset, int limit);
        Employer GetEmployer(Guid employerId);
        void RegisterEmployer(Employer employer);
        Employer GetEmployerByEmail(string email);
        void EmployerUpdate(Employer employer);
    }
}
