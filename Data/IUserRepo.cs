using System.Collections.Generic;
using Commander.Models;

namespace Commander.Data
{
    public interface IUserRepo
    {
        bool SaveChanges();
        IEnumerable<User> SearchUser(string searchCriteria, int offset, int limit);
        User GetUser(string externalId);
        void RegisterUser(User user);
        User GetUserByEmail(string email);
        void UserUpdate(User user);
    }
}