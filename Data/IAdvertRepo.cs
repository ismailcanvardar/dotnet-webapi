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
        IEnumerable<Advert> SearchAdverts(AdvertSearchDto advertSearchDto);
        Advert GetAdvert(Guid advertId);
        IEnumerable<Advert> GetMyAdverts(Guid employerId);
    }
}
