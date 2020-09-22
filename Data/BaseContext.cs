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

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Employer> Employers { get; set; }

        public DbSet<Advert> Adverts { get; set; }

        public DbSet<Application> Applications { get; set; }

        public DbSet<Rating> Ratings { get; set; }

        public DbSet<PickedEmployee> PickedEmployees { get; set; }
    }
}