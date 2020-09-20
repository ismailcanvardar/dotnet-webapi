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

        public DbSet<Employer> Employers { get; set; }

        public DbSet<JobPost> JobPosts { get; set; }

        public DbSet<Attendance> Attendances { get; set; }

        public DbSet<Rating> Ratings { get; set; }
    }
}