using System;
using System.Collections.Generic;
using System.Linq;
using Commander.Models;
using Microsoft.EntityFrameworkCore;

namespace Commander.Data
{
    public class UserRepo : IUserRepo
    {
        private readonly BaseContext _context;

        public UserRepo(BaseContext context)
        {
            _context = context;
        }

        public User GetUser(string externalId)
        {
            throw new NotImplementedException();
        }

        public User LoginUser(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(p => p.Email == email);

            if (user != null)
            {
                return user;
            }

            return null;
        }

        public void RegisterUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var isUserExists = _context.Users.FirstOrDefault(p => p.Email == user.Email);

            if (isUserExists != null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            // Adds uuid to user's external id property
            user.ExternalId = Guid.NewGuid().ToString();

            _context.Users.Add(user);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public IEnumerable<User> SearchUser(string searchCriteria, int offset)
        {
            throw new NotImplementedException();
        }

        public void UserUpdate(User user)
        {
            throw new NotImplementedException();
        }
    }
}