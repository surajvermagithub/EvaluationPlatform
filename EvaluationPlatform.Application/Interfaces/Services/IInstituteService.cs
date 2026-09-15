using CheckMate.Application.DTOs.Institute;

namespace CheckMate.Application.Interfaces.Services
{
    public interface IInstituteService
    {
        Task CreateInstituteAsync(CreateInstituteRequest request);
    }
}
