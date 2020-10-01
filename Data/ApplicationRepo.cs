using System;
using System.Collections.Generic;
using System.Linq;
using KariyerAppApi.Models;

namespace KariyerAppApi.Data
{
    public class ApplicationRepo : IApplicationRepo
    {
        private readonly BaseContext _context;

        public ApplicationRepo(BaseContext context)
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

        public void ManageApplicantCount(Guid advertId, ApplicantCountOperation applicantCountOperation)
        {
            var advert = _context.Adverts.FirstOrDefault(a => a.AdvertId == advertId);

            if (applicantCountOperation == ApplicantCountOperation.Increment)
            {
                advert.TotalApplicantCount += 1;
            }
            else if (applicantCountOperation == ApplicantCountOperation.Decrement)
            {
                advert.TotalApplicantCount -= 1;
            }

            SaveChanges();
        }

        public IEnumerable<Application> GetApplicationsByAdvert(Guid advertId)
        {
            var applications = _context.Applications.Where(app => app.AdvertId == advertId).ToList();
            return applications;
        }

        public IEnumerable<Application> GetMyApplications(Guid employeeId)
        {
            var applications = _context.Applications.Where(app => app.EmployeeId.Equals(employeeId)).OrderByDescending(app => app.CreatedAt).ToList();
            return applications;
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public bool IsApplied(Guid advertId, Guid employeeId)
        {
            var application = _context.Applications.FirstOrDefault(app => app.AdvertId.Equals(advertId) && app.EmployeeId.Equals(employeeId));

            if (application != null)
            {
                return true;
            }

            return false;
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
    }
}
