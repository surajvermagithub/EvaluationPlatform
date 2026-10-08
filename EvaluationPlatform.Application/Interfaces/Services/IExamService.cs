using CheckMate.Application.DTOs.Exam;

namespace CheckMate.Application.Interfaces.Services
{
    public interface IExamService
    {
        Task CreateExamAsync(
            int adminUserId,
            CreateExamRequest request);

        Task<List<ExamResponse>> GetExamsAsync(
            int adminUserId);

        Task<ExamResponse> GetExamByIdAsync(
            int adminUserId,
            int examId);

        Task DeactivateExamAsync(
            int adminUserId,
            int examId);

        Task ActivateExamAsync(
            int adminUserId,
            int examId);
    }
}
