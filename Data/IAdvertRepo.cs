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
        bool CancelApplication(string externalId);
    }
}
