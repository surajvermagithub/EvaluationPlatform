using CheckMate.Domain.Entities;

namespace CheckMate.Application.Interfaces.Repositories
{
    public interface ISubjectRepository
    {
        Task<bool> ExistsByCodeAsync(
            int examId,
            string code,
            int? excludeSubjectId = null);

        Task<bool> ExistsByNameAsync(
            int examId,
            string name,
            int? excludeSubjectId = null);

        Task<Subject> AddAsync(Subject subject);

        Task<List<Subject>> GetByExamIdAsync(int examId);

        Task<Subject?> GetByIdAsync(int subjectId);

        Task UpdateAsync(Subject subject);
    }
}
