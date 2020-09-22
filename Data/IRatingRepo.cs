using System;
using KariyerAppApi.Models;

namespace KariyerAppApi.Data
{
    public interface IRatingRepo
    {
        bool SaveChanges();
        void RateEmployee(Rating rating);
        Rating GetRating(Guid employeeId, Guid advertId);
    }
}
