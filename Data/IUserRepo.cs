using System.Collections.Generic;
using Commander.Models;

namespace Commander.Data
{
    public interface IUserRepo
    {
        bool SaveChanges();
        IEnumerable<User> SearchUser(string searchCriteria, int offset);
        User GetUser(string externalId);
        void RegisterUser(User user);
        User LoginUser(string email, string password);
        void UserUpdate(User user);
    }
}