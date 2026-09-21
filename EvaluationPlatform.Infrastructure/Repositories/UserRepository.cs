using CheckMate.Application.Interfaces.Repositories;
using CheckMate.Domain.Entities;
using CheckMate.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CheckMate.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SuperAdminExistsAsync()
        {
            return await _context.Users
                .AnyAsync(u => u.Role.Name == "SuperAdmin" && u.IsDeleted == false);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.Role)
        .FirstOrDefaultAsync(u =>
            u.Email == email &&
            !u.IsDeleted &&
            u.IsActive);
        }

        public async Task<User> AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<Role?> GetRoleByNameAsync(string roleName)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(r =>
                    r.Name == roleName &&
                    !r.IsDeleted);
        }

        public async Task<List<User>> GetTeachersByInstituteIdAsync(
    int instituteId)
        {
            return await _context.Users
                .Include(u => u.Role)
                .Where(u =>
                    u.InstituteId == instituteId &&
                    u.Role.Name == "Teacher" &&
                    !u.IsDeleted)
                .OrderBy(u => u.FullName)
                .ToListAsync();
        }

        public async Task<User?> GetTeacherByIdAndInstituteIdAsync(
    int teacherId,
    int instituteId)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u =>
                    u.Id == teacherId &&
                    u.InstituteId == instituteId &&
                    u.Role.Name == "Teacher" &&
                    !u.IsDeleted);
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _context.Users
                .AnyAsync(u =>
                    u.Email == email &&
                    !u.IsDeleted);
        }

        public async Task<User?> GetByIdAsync(int userId)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u =>
                    u.Id == userId &&
                    !u.IsDeleted);
        }
    }
}
