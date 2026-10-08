using CheckMate.Domain.Entities;

namespace CheckMate.Application.Interfaces.Repositories
{
    public interface IExamRepository
    {
        Task<bool> ExistsByCodeAsync(string code);

        Task<Exam> AddAsync(Exam exam);

        Task<List<Exam>> GetByInstituteIdAsync(
            int instituteId);

        Task<Exam?> GetByIdAndInstituteIdAsync(
            int examId,
            int instituteId);

        Task UpdateAsync(Exam exam);
    }
}
