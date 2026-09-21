using CheckMate.Application.DTOs.Teacher;

namespace CheckMate.Application.Interfaces.Services
{
    public interface ITeacherService
    {
        Task CreateTeacherAsync(
            int adminUserId,
            CreateTeacherRequest request);

        Task<List<TeacherResponse>> GetTeachersAsync(
            int adminUserId);

        Task DeactivateTeacherAsync(
            int adminUserId,
            int teacherId);

        Task ActivateTeacherAsync(
            int adminUserId,
            int teacherId);
    }
}
