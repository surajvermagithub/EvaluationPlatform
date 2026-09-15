using CheckMate.Domain.Entities;

namespace CheckMate.Application.Interfaces.Repositories
{
    public interface IInstituteRepository
    {
        Task<bool> ExistsByCodeAsync(string code);

        Task<Institute?> GetByIdAsync(int instituteId);
        Task<Institute> AddAsync(Institute institute);
    }
}
