using CheckMate.Application.DTOs.Students;

namespace CheckMate.Application.Interfaces.Services
{
    public interface IStudentService
    {
        Task CreateStudentAsync(
            int adminUserId,
            CreateStudentRequest request);

        Task<List<StudentResponse>> GetStudentsAsync(
            int adminUserId);

        Task<StudentResponse> GetStudentByIdAsync(
            int adminUserId,
            int studentId);

        Task UpdateStudentAsync(
            int adminUserId,
            int studentId,
            UpdateStudentRequest request);

        Task DeactivateStudentAsync(
            int adminUserId,
            int studentId);

        Task ActivateStudentAsync(
            int adminUserId,
            int studentId);
    }
}
