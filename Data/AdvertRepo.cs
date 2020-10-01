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

        public IEnumerable<Advert> GetMyAdverts(Guid employerId)
        {
            var adverts = _context.Adverts.Where(ad => ad.EmployerId.Equals(employerId)).OrderByDescending(ad => ad.CreatedAt).ToList();
            return adverts;
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

        public IEnumerable<Advert> SearchAdverts(AdvertSearchDto advertSearchDto)
        {
            if (advertSearchDto.Province != "" && advertSearchDto.District != "" && advertSearchDto.Neighborhood != "")
            {
               return _context.Adverts.Where(advert => advert.Province.Equals(advertSearchDto.Province) && advert.District.Equals(advertSearchDto.District) && advert.Neighborhood.Equals(advertSearchDto.Neighborhood)).ToList();
            } else if (advertSearchDto.Province != "" && advertSearchDto.District != "")
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

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}
