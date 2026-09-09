using CheckMate.Domain.Entities;

namespace CheckMate.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {

        Task<bool> SuperAdminExistsAsync();

        Task<User?> GetByEmailAsync(string email);

        Task<User> AddAsync(User user);
    }
}
