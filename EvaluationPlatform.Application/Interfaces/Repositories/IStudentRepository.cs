using CheckMate.Domain.Entities;

namespace CheckMate.Application.Interfaces.Repositories
{
    public interface IStudentRepository
    {
        Task<int> GetLatestAdmissionSequenceAsync(
          int instituteId,
          string prefix);

        Task<Student> AddAsync(Student student);

        Task<List<Student>> GetByInstituteIdAsync(int instituteId);

        Task<Student?> GetByIdAndInstituteIdAsync(
            int studentId,
            int instituteId);

        Task UpdateAsync(Student student);
    }
}
