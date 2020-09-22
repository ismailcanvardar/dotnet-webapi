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

        public bool ApplyToAdvert(Application application)
        {
            _context.Applications.Add(application);
            return true;
        }

        public Application CancelApplication(Guid applicationId, Guid employeeId)
        {
            var application = _context.Applications.FirstOrDefault(a => a.ApplicationId == applicationId && a.EmployeeId == employeeId);

            if (application == null)
            {
                return null;
            }

            _context.Applications.Remove(application);
            return application;
        }

        public bool CreateAdvert(Advert advert)
        {
            _context.Adverts.Add(advert);
            return true;
        }

        public Advert GetAdvert(Guid advertId)
        {
            var foundAdvert = _context.Adverts.FirstOrDefault(advert => advert.AdvertId == advertId);
            return foundAdvert;
        }

        public IEnumerable<Application> GetMyApplications(Guid employeeId)
        {
            var applications = _context.Applications.Where(app => app.EmployeeId.Equals(employeeId)).OrderByDescending(app => app.CreatedAt).ToList();
            return applications;
        }

        public IEnumerable<Advert> GetMyAdverts(Guid employerId)
        {
            var adverts = _context.Adverts.Where(ad => ad.EmployerId.Equals(employerId)).OrderByDescending(ad => ad.CreatedAt).ToList();
            return adverts;
        }

        public PickedEmployee GetPickedEmployee(Guid pickedEmployeeId)
        {
            PickedEmployee pickedEmployee = _context.PickedEmployees.FirstOrDefault(pe => pe.PickedEmployeeId == pickedEmployeeId);
            return pickedEmployee;
        }

        public bool IsEmployeeApplied(Guid employeeId, Guid advertId)
        {
            // Check if user applied earlier
            var application = _context.Applications.FirstOrDefault(a => a.AdvertId == advertId && a.EmployeeId == employeeId);
            if (application != null)
            {
                return true;
            }

            return false;
        }

        public void ManageApplicantCount(Guid advertId, ApplicantCountOperation applicantCountOperation)
        {
            var advert = _context.Adverts.FirstOrDefault(a => a.AdvertId == advertId);

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
            _context.PickedEmployees.Add(pickedEmployee);
        }

        public bool RemoveAdvert(Guid advertId)
        {
            var advert = _context.Adverts.FirstOrDefault(advert => advert.AdvertId == advertId);

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

        public IEnumerable<Application> GetApplicationsByAdvert(Guid advertId)
        {
            var applications = _context.Applications.Where(app => app.AdvertId == advertId).ToList();
            return applications;
        }

        public PickedEmployee GetPickedEmployeeByAdvertAndEmployee(Guid advertId, Guid employeeId)
        {
            var pickedEmployee = _context.PickedEmployees.FirstOrDefault(pe => pe.AdvertId.Equals(advertId) && pe.EmployeeId.Equals(employeeId));
            return pickedEmployee;
        }
    }
}
