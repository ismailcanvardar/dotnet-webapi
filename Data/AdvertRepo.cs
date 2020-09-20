﻿using System;
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
            var isApplied = _context.Applications.FirstOrDefault(a => a.EmployeeId == attendance.EmployeeId && a.EmployerId == attendance.EmployerId);

            if (isApplied != null)
            {
                return false;
            }

            _context.Applications.Add(attendance);
            return true;
        }

        public bool CancelApplication(string externalId)
        {
            var application = _context.Applications.FirstOrDefault(a => a.ExternalId == externalId);

            if (application == null)
            {
                return false;
            }

            _context.Applications.Remove(application);
            return true;
        }

        public bool CreateAdvert(Advert advert)
        {
            _context.Adverts.Add(advert);
            return true;
        }

        public Advert GetAdvert(string externalId)
        {
            var foundAdvert = _context.Adverts.FirstOrDefault(advert => advert.ExternalId == externalId);
            return foundAdvert;
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

        public void UpdateAdvert()
        {
            throw new NotImplementedException();
        }
    }
}