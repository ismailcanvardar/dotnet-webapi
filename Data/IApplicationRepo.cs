using System;
using System.Collections.Generic;
using System.Linq;
using KariyerAppApi.Models;

namespace KariyerAppApi.Data
{
    public interface IApplicationRepo
    {
        bool IsApplied(Guid advertId, Guid employeeId);
        bool SaveChanges();
        bool ApplyToAdvert(Application application);
        Application CancelApplication(Guid applicationId);
        IQueryable GetApplicationsByAdvert(Guid advertId);
        bool IsEmployeeApplied(Guid employeeId, Guid advertId);
        void ManageApplicantCount(Guid advertId, ApplicantCountOperation applicantCountOperation);
        IQueryable GetMyApplications(Guid employeeId);
        IEnumerable<Application> GetApplicationsOfDefinedEmployee(Guid employeeId);
    }

    public enum ApplicantCountOperation
    {
        Increment,
        Decrement
    }
}
