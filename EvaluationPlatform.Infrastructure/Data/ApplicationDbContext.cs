
using Microsoft.EntityFrameworkCore;
using CheckMate.Domain.Entities;

namespace CheckMate.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();

        public DbSet<Role> Roles => Set<Role>();

        public DbSet<Institute> Institutes => Set<Institute>();
    
    }
}
