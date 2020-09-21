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
        bool RemoveAdvert(string externalId);
        void UpdateAdvert();
        bool ApplyToAdvert(Application attendance);
        IEnumerable<Advert> SearchAdverts(AdvertSearchDto advertSearchDto);
        Advert GetAdvert(string externalId);
        Application CancelApplication(string externalId, string employeeId);
        IEnumerable<Application> GetApplicationsByAdvert(string advertExternalId);
        bool IsEmployeeApplied(string employeeId, string advertId);
        void ManageApplicantCount(string advertExternalId, ApplicantCountOperation applicantCountOperation);
        void PickEmployee(PickedEmployee pickedEmployee);
        void UnpickEmployee(PickedEmployee pickedEmployee);
        PickedEmployee GetPickedEmployee(string pickedEmployeeExternalId);
        PickedEmployee GetPickedEmployeeByAdvertAndEmployee(string advertExternalId, string employeeExternalId);
        IEnumerable<Advert> GetMyAdverts(string employerExternalId);
        IEnumerable<Application> GetMyApplications(string employeeExternalId);
    }

    public enum ApplicantCountOperation
    {
        Increment,
        Decrement
    }
}
