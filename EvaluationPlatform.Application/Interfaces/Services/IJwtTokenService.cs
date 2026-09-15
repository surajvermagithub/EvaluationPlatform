using CheckMate.Domain.Entities;

namespace CheckMate.Application.Interfaces.Services
{
    public interface IJwtTokenService
    {
        string GenerateToken(User user);
    }
}
