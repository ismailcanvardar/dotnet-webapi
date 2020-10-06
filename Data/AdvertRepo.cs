using System;
using System.Collections.Generic;
using System.Linq;
using KariyerAppApi.Dtos;
using KariyerAppApi.Models;
using Microsoft.EntityFrameworkCore;

namespace KariyerAppApi.Data
{
    public class AdvertRepo : IAdvertRepo
    {
        private readonly BaseContext _context;

        public AdvertRepo(BaseContext context)
        {
            _context = context;
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

        public IQueryable GetMyAdverts(Guid employerId)
        {
            return from advert in _context.Adverts.Where(a => a.EmployerId.Equals(employerId)) join employer in _context.Employers on advert.EmployerId equals employer.EmployerId select new { advert, employer };
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

        public IQueryable SearchAdverts(AdvertSearchDto advertSearchDto)
        {
            // If searchcriteria is null and province exists
            if (advertSearchDto.SearchCriteria == null && advertSearchDto.Province != null)
            {
                return from advert in _context.Adverts.Where(a => a.Province.Equals(advertSearchDto.Province)) join employer in _context.Employers on advert.EmployerId equals employer.EmployerId select new { advert, employer };
            }
            // If searchcriteria is null and province and district exists
            else if (advertSearchDto.SearchCriteria == null && advertSearchDto.Province != null && advertSearchDto.District != null)
            {
                return from advert in _context.Adverts.Where(a => a.Province.Equals(advertSearchDto.Province) && a.District.Equals(advertSearchDto.District)) join employer in _context.Employers on advert.EmployerId equals employer.EmployerId select new { advert, employer };
            }
            // If searchcriteria is null and province, district and neighborhood exists
            else if (advertSearchDto.SearchCriteria == null && advertSearchDto.Province != null && advertSearchDto.District != null && advertSearchDto.Neighborhood != null)
            {
                return from advert in _context.Adverts.Where(a => a.Province.Equals(advertSearchDto.Province) && a.District.Equals(advertSearchDto.District) && a.Neighborhood.Equals(advertSearchDto.Neighborhood)) join employer in _context.Employers on advert.EmployerId equals employer.EmployerId select new { advert, employer };
            }
            // If searchcriteria exists and province, district and neighborhood exists
            else if (advertSearchDto.Province != null && advertSearchDto.District != null && advertSearchDto.Neighborhood != null)
            {
                return from advert in _context.Adverts.Where(a => (a.Title.Contains(advertSearchDto.SearchCriteria) || a.Description.Contains(advertSearchDto.SearchCriteria)) && a.Province.Equals(advertSearchDto.Province) && a.Province.Equals(advertSearchDto.Neighborhood)) join employer in _context.Employers on advert.EmployerId equals employer.EmployerId select new { advert, employer };
            }
            // If searchcriteria exists and province and district exists
            else if (advertSearchDto.Province != null && advertSearchDto.District != null)
            {
                return from advert in _context.Adverts.Where(a => (a.Title.Contains(advertSearchDto.SearchCriteria) || a.Description.Contains(advertSearchDto.SearchCriteria)) && a.Province.Equals(advertSearchDto.Province) && a.District.Equals(advertSearchDto.District)) join employer in _context.Employers on advert.EmployerId equals employer.EmployerId select new { advert, employer };
            }
            // If searchcriteria exists and province exists
            else if (advertSearchDto.Province != null)
            {
                return from advert in _context.Adverts.Where(a => (a.Title.Contains(advertSearchDto.SearchCriteria) || a.Description.Contains(advertSearchDto.SearchCriteria)) && a.Province.Equals(advertSearchDto.Province)) join employer in _context.Employers on advert.EmployerId equals employer.EmployerId select new { advert, employer };
            }
            else
            {
                return from advert in _context.Adverts.Where(a => (a.Title.Contains(advertSearchDto.SearchCriteria) || a.Description.Contains(advertSearchDto.SearchCriteria))) join employer in _context.Employers on advert.EmployerId equals employer.EmployerId select new { advert, employer };
            }
        }

        public void UpdateAdvert()
        {
            throw new NotImplementedException();
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public IQueryable GetAdvertWithApplication(Guid advertId)
        {
            var foundAdvert = from advert in _context.Adverts.Where(a => a.AdvertId.Equals(advertId)) join application in _context.Applications on advert.AdvertId equals application.AdvertId select new { advert, application };
            return foundAdvert;
        }

        public IEnumerable<Advert> GetAdvertsOfDefinedEmployer(Guid employerId)
        {
            return _context.Adverts.Where(a => a.EmployerId.Equals(employerId)).ToList();
        }

        public IQueryable GetAdvertsOfDefinedEmployee(Guid employeeId)
        {
            return from application in _context.Applications.Where(a => a.EmployeeId.Equals(employeeId)) join advert in _context.Adverts on application.AdvertId equals advert.AdvertId select new { application, advert };
        }
    }
}
