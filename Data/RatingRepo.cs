using System;
using System.Linq;
using KariyerAppApi.Models;

namespace KariyerAppApi.Data
{
    public class RatingRepo : IRatingRepo
    {
        private readonly BaseContext _context;

        public RatingRepo(BaseContext context)
        {
            _context = context;
        }

        public Rating GetRating(Guid employeeId, Guid advertId)
        {
            return _context.Ratings.FirstOrDefault(r => r.EmployeeId.Equals(employeeId) && r.AdvertId.Equals(advertId));
        }

        public void RateEmployee(Rating rating)
        {
            var employee = _context.Employees.FirstOrDefault(employee => employee.EmployeeId.Equals(rating.EmployeeId));
            employee.RatingCount = (employee.RatingCount + rating.GivenRating) / 2;
            employee.TotalRating += 1;
            _context.Ratings.Add(rating);
            SaveChanges();
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}
