using System.Collections.Generic;
using Commander.Models;

namespace Commander.Data
{
    public interface IUserRepo
    {
        bool SaveChanges();
        IEnumerable<User> SearchUsersByUsername(string username, int offset);
        User GetUserByUsername(string username);
        void RegisterUser(User user);
        User LoginUser(string email, string password);
        void UserUpdate(User user);
    }
}