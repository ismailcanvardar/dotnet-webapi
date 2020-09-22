using System;
using System.Collections.Generic;
using Commander.Dtos;
using Commander.Models;

namespace Commander.Data
{
    public interface IAdvertRepo
    {
        bool SaveChanges();
        bool CreateAdvert(Advert advert);
        bool RemoveAdvert(Guid advertId);
        void UpdateAdvert();
        bool ApplyToAdvert(Application application);
        IEnumerable<Advert> SearchAdverts(AdvertSearchDto advertSearchDto);
        Advert GetAdvert(Guid advertId);
        Application CancelApplication(Guid applicationId, Guid employeeId);
        IEnumerable<Application> GetApplicationsByAdvert(Guid advertId);
        bool IsEmployeeApplied(Guid employeeId, Guid advertId);
        void ManageApplicantCount(Guid advertId, ApplicantCountOperation applicantCountOperation);
        void PickEmployee(PickedEmployee pickedEmployee);
        void UnpickEmployee(PickedEmployee pickedEmployee);
        PickedEmployee GetPickedEmployee(Guid pickedEmployeeId);
        PickedEmployee GetPickedEmployeeByAdvertAndEmployee(Guid advertId, Guid employeeId);
        IEnumerable<Advert> GetMyAdverts(Guid employerId);
        IEnumerable<Application> GetMyApplications(Guid employeeId);
    }

    public enum ApplicantCountOperation
    {
        Increment,
        Decrement
    }
}
