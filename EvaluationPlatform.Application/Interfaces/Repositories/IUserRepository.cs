using CheckMate.Domain.Entities;

namespace CheckMate.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<bool> SuperAdminExistsAsync();

        Task<User?> GetByEmailAsync(string email);

        Task<bool> ExistsByEmailAsync(string email);
        Task<Role?> GetRoleByNameAsync(string roleName);

        Task<User> AddAsync(User user);

        Task<List<User>> GetTeachersByInstituteIdAsync(int instituteId);

        Task<User?> GetTeacherByIdAndInstituteIdAsync(
            int teacherId,
            int instituteId);

        Task UpdateAsync(User user);

        Task<User?> GetByIdAsync(int userId);
    }
}
