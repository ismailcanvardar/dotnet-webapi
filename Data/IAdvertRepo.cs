using System;
using System.Collections.Generic;
using System.Linq;
using KariyerAppApi.Dtos;
using KariyerAppApi.Models;

namespace KariyerAppApi.Data
{
    public interface IAdvertRepo
    {
        bool SaveChanges();
        bool CreateAdvert(Advert advert);
        bool RemoveAdvert(Guid advertId);
        void UpdateAdvert();
        IQueryable SearchAdverts(AdvertSearchDto advertSearchDto);
        Advert GetAdvert(Guid advertId);
        IQueryable GetAdvertWithApplication(Guid advertId);
        IQueryable GetMyAdverts(Guid employerId);
    }
}
