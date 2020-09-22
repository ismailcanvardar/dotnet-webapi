using System;
using System.Collections.Generic;
using KariyerAppApi.Models;

namespace KariyerAppApi.Data
{
    public interface IApplicationRepo
    {
        bool SaveChanges();
        bool ApplyToAdvert(Application application);
        Application CancelApplication(Guid applicationId, Guid employeeId);
        IEnumerable<Application> GetApplicationsByAdvert(Guid advertId);
        bool IsEmployeeApplied(Guid employeeId, Guid advertId);
        void ManageApplicantCount(Guid advertId, ApplicantCountOperation applicantCountOperation);
        IEnumerable<Application> GetMyApplications(Guid employeeId);
    }

    public enum ApplicantCountOperation
    {
        Increment,
        Decrement
    }
}
