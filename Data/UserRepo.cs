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

        public User GetUserByUsername(string username)
        {
            return _context.Users.FirstOrDefault(p => p.Username == username);
        }

        public User LoginUser(string email, string password)
        {
            var user = _context.Users.SingleOrDefault(x => x.Email == email && x.Password == password);

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

            var isUserExists = _context.Users.FirstOrDefault(p => p.Username == user.Username);

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

        public IEnumerable<User> SearchUsersByUsername(string username, int offset)
        {
            var foundUserList = _context.Users.FromSqlRaw($"select * from users where Username like '{username}%' order by Username offset {offset} rows fetch next 5 rows only").ToList();

            return foundUserList;
        }

        public void UserUpdate(User user)
        {
            throw new NotImplementedException();
        }
    }
}