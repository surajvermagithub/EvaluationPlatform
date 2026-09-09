using CheckMate.Application.DTOs.Auth;

namespace CheckMate.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task RegisterAdminAsync(RegisterAdminRequest request);
    }
}
