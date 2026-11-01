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

        public DbSet<Exam> Exams => Set<Exam>();
        public DbSet<Subject> Subjects => Set<Subject>();

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

            modelBuilder.Entity<Role>()
    .HasIndex(r => r.Name)
    .IsUnique();


            modelBuilder.Entity<Exam>()
       .HasOne(e => e.Institute)
       .WithMany()
       .HasForeignKey(e => e.InstituteId)
       .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Exam>()
                .HasIndex(e => e.Code)
                .IsUnique();

            modelBuilder.Entity<Exam>()
                .Property(e => e.Name)
                .HasMaxLength(150)
                .IsRequired();

            modelBuilder.Entity<Exam>()
                .Property(e => e.Code)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<Exam>()
                .Property(e => e.Description)
                .HasMaxLength(500);



            modelBuilder.Entity<Subject>(entity =>
            {
                entity.ToTable("Subjects", table =>
                {
                    table.HasCheckConstraint(
                        "CK_Subject_MaximumMarks",
                        "[MaximumMarks] > 0");

                    table.HasCheckConstraint(
                        "CK_Subject_PassingMarks",
                        "[PassingMarks] >= 0 AND [PassingMarks] <= [MaximumMarks]");

                    table.HasCheckConstraint(
                        "CK_Subject_DisplayOrder",
                        "[DisplayOrder] >= 0");
                });

                entity.Property(s => s.Name)
                    .HasMaxLength(150)
                    .IsRequired();

                entity.Property(s => s.Code)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(s => s.Description)
                    .HasMaxLength(500);

                entity.Property(s => s.MaximumMarks)
                    .HasPrecision(18, 2);

                entity.Property(s => s.PassingMarks)
                    .HasPrecision(18, 2);

                entity.HasOne(s => s.Exam)
                    .WithMany(e => e.Subjects)
                    .HasForeignKey(s => s.ExamId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(s => new { s.ExamId, s.Code })
                    .IsUnique()
                    .HasFilter("[IsDeleted] = 0");

                entity.HasIndex(s => new { s.ExamId, s.Name })
                    .IsUnique()
                    .HasFilter("[IsDeleted] = 0");
            });


            // Seed initial roles
            modelBuilder.Entity<Role>().HasData(
    new Role
    {
        Id = 1,
        Name = "SuperAdmin",
        Description = "Super Administrator with full system access",
        IsActive = true,
        IsDeleted = false,
        CreatedOn = new DateTime(2026, 1, 1)
    },
    new Role
    {
        Id = 2,
        Name = "Admin",
        Description = "Institute administrator",
        IsActive = true,
        IsDeleted = false,
        CreatedOn = new DateTime(2026, 1, 1)
    },
    new Role
    {
        Id = 3,
        Name = "Teacher",
        Description = "Teacher who evaluates examination booklets",
        IsActive = true,
        IsDeleted = false,
        CreatedOn = new DateTime(2026, 1, 1)
    }
);
        }
    }
}
