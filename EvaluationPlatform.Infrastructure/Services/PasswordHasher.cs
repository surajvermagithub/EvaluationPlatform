using CheckMate.Application.Interfaces.Services;
using Microsoft.AspNetCore.Identity;

namespace CheckMate.Infrastructure.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        private readonly PasswordHasher<object> _hasher = new();

        public string HashPassword(string password)
        {
            return _hasher.HashPassword(null!, password);
        }
    }
}
