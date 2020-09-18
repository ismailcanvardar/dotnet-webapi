using Commander.Models;
using Microsoft.EntityFrameworkCore;

namespace Commander.Data
{
    public class BaseContext : DbContext
    {
        public BaseContext(DbContextOptions<BaseContext> opt) : base(opt)
        {

        }

        public DbSet<Command> Commands { get; set; }

        public DbSet<User> Users { get; set; }
    }
}