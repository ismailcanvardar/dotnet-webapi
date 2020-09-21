using System;
using System.Collections.Generic;
using System.Linq;
using Commander.Dtos;
using Commander.Models;
using Microsoft.EntityFrameworkCore;

namespace Commander.Data
{
    public class AdvertRepo : IAdvertRepo
    {
        private readonly BaseContext _context;

        public AdvertRepo(BaseContext context)
        {
            _context = context;
        }

        public bool ApplyToAdvert(Application attendance)
        {
            attendance.ExternalId = Guid.NewGuid().ToString();
            _context.Applications.Add(attendance);
            return true;
        }

        public Application CancelApplication(string externalId, string employeeId)
        {
            var application = _context.Applications.FirstOrDefault(a => a.ExternalId == externalId && a.EmployeeId == employeeId);

            if (application == null)
            {
                return null;
            }

            _context.Applications.Remove(application);
            return application;
        }

        public bool CreateAdvert(Advert advert)
        {
            advert.ExternalId = Guid.NewGuid().ToString();
            _context.Adverts.Add(advert);
            return true;
        }

        public Advert GetAdvert(string externalId)
        {
            var foundAdvert = _context.Adverts.FirstOrDefault(advert => advert.ExternalId == externalId);
            return foundAdvert;
        }

        public IEnumerable<Application> GetMyApplications(string employeeExternalId)
        {
            var applications = _context.Applications.Where(app => app.EmployeeId.Equals(employeeExternalId)).OrderByDescending(app => app.CreatedAt).ToList();
            return applications;
        }

        public IEnumerable<Advert> GetMyAdverts(string employerExternalId)
        {
            var adverts = _context.Adverts.Where(ad => ad.EmployerId.Equals(employerExternalId)).OrderByDescending(ad => ad.CreatedAt).ToList();
            return adverts;
        }

        public PickedEmployee GetPickedEmployee(string pickedEmployeeExternalId)
        {
            PickedEmployee pickedEmployee = _context.PickedEmployees.FirstOrDefault(pe => pe.ExternalId == pickedEmployeeExternalId);
            return pickedEmployee;
        }

        public bool IsEmployeeApplied(string employeeId, string advertId)
        {
            // Check if user applied earlier
            var application = _context.Applications.FirstOrDefault(a => a.AdvertId == advertId && a.EmployeeId == employeeId);
            if (application != null)
            {
                return true;
            }

            return false;
        }

        public void ManageApplicantCount(string advertExternalId, ApplicantCountOperation applicantCountOperation)
        {
            var advert = _context.Adverts.FirstOrDefault(a => a.ExternalId == advertExternalId);

            if (applicantCountOperation == ApplicantCountOperation.Increment)
            {
                advert.TotalApplicantCount += 1;
            } else if (applicantCountOperation == ApplicantCountOperation.Decrement)
            {
                advert.TotalApplicantCount -= 1;
            }

            SaveChanges();
        }

        public void PickEmployee(PickedEmployee pickedEmployee)
        {
            pickedEmployee.ExternalId = Guid.NewGuid().ToString();
            _context.PickedEmployees.Add(pickedEmployee);
        }

        public bool RemoveAdvert(string externalId)
        {
            var advert = _context.Adverts.FirstOrDefault(advert => advert.ExternalId == externalId);

            if (advert == null)
            {
                return false;
            }

            _context.Adverts.Remove(advert);
            return true;   
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public IEnumerable<Advert> SearchAdverts(AdvertSearchDto advertSearchDto)
        {
            if (advertSearchDto.Province != null && advertSearchDto.District != null && advertSearchDto.Neighborhood != null)
            {
               return _context.Adverts.Where(advert => advert.Province.Equals(advertSearchDto.Province) && advert.District.Equals(advertSearchDto.District) && advert.Neighborhood.Equals(advertSearchDto.Neighborhood)).ToList();
            } else if (advertSearchDto.Province != null && advertSearchDto.District != null)
            {
                return _context.Adverts.Where(advert => advert.Province.Equals(advertSearchDto.Province) && advert.District.Equals(advertSearchDto.District)).ToList();
            } else
            {
                return _context.Adverts.Where(advert => advert.Province.Equals(advertSearchDto.Province)).ToList();
            }
        }

        public void UnpickEmployee(PickedEmployee pickedEmployee)
        {
            _context.PickedEmployees.Remove(pickedEmployee);
        }

        public void UpdateAdvert()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Application> GetApplicationsByAdvert(string advertExternalId)
        {
            var applications = _context.Applications.Where(app => app.AdvertId == advertExternalId).ToList();
            return applications;
        }

        public PickedEmployee GetPickedEmployeeByAdvertAndEmployee(string advertExternalId, string employeeExternalId)
        {
            var pickedEmployee = _context.PickedEmployees.FirstOrDefault(pe => pe.AdvertExternalId.Equals(advertExternalId) && pe.EmployeeExternalId.Equals(employeeExternalId));
            return pickedEmployee;
        }
    }
}
