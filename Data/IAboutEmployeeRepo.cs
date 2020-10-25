using System;
using KariyerAppApi.Models;

namespace KariyerAppApi.Data
{
    public interface IAboutEmployeeRepo
    {
        AboutEmployee GetAboutEmployee(Guid employeeId);
        void CreateAboutEmployee(AboutEmployee aboutEmployee);
        bool SaveChanges();
    }
}
