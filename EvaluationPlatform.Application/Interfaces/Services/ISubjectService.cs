using CheckMate.Application.DTOs.Subjects;

namespace CheckMate.Application.Interfaces.Services
{
    public interface ISubjectService
    {
        Task CreateSubjectAsync(
            int adminUserId,
            CreateSubjectRequest request);

        Task<List<SubjectResponse>> GetSubjectsAsync(
            int adminUserId,
            int examId);

        Task<SubjectResponse> GetSubjectByIdAsync(
            int adminUserId,
            int subjectId);

        Task UpdateSubjectAsync(
            int adminUserId,
            int subjectId,
            UpdateSubjectRequest request);

        Task DeactivateSubjectAsync(
            int adminUserId,
            int subjectId);

        Task ActivateSubjectAsync(
            int adminUserId,
            int subjectId);
    }
}
