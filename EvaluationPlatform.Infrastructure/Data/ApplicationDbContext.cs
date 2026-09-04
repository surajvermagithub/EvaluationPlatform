
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.Institute)
                .WithMany(i => i.Users)
                .HasForeignKey(u => u.InstituteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Institute>()
    .HasIndex(i => i.Code)
    .IsUnique();

            modelBuilder.Entity<Institute>()
    .Property(i => i.Name)
    .HasMaxLength(200)
    .IsRequired();

            modelBuilder.Entity<Institute>()
                .Property(i => i.Code)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<Institute>()
                .Property(i => i.Email)
                .HasMaxLength(150)
                .IsRequired();

            modelBuilder.Entity<Institute>()
                .Property(i => i.Mobile)
                .HasMaxLength(20)
                .IsRequired();

            modelBuilder.Entity<User>()
    .Property(u => u.FullName)
    .HasMaxLength(200)
    .IsRequired();

            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .HasMaxLength(150)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(u => u.Mobile)
                .HasMaxLength(20)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(u => u.PasswordHash)
                .IsRequired();

            modelBuilder.Entity<Role>()
    .Property(r => r.Name)
    .HasMaxLength(50)
    .IsRequired();

            modelBuilder.Entity<Role>()
                .Property(r => r.Description)
                .HasMaxLength(250);
        }

    }
}
