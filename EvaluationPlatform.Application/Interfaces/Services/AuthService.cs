using CheckMate.Application.DTOs.Auth;
using CheckMate.Application.Exceptions;
using CheckMate.Application.Interfaces.Repositories;
using CheckMate.Domain.Entities;

namespace CheckMate.Application.Interfaces.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthService(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IJwtTokenService jwtTokenService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtTokenService = jwtTokenService;
        }

        public async Task RegisterAdminAsync(RegisterAdminRequest request)
        {
            bool adminExists =
                await _userRepository.SuperAdminExistsAsync();

            if (adminExists)
            {
                throw new ConflictException(
                    "SuperAdmin already exists.");
            }

            var existingUser =
                await _userRepository.GetByEmailAsync(request.Email);

            if (existingUser != null)
            {
                throw new ConflictException(
                    "Email is already registered.");
            }

            string passwordHash =
                _passwordHasher.HashPassword(request.Password);

            var superAdminRole =
                await _userRepository.GetRoleByNameAsync("SuperAdmin");

            if (superAdminRole == null)
            {
                throw new InvalidOperationException(
                    "SuperAdmin role is not configured.");
            }

            var user = new User
            {
                FullName = request.FullName,
                Email = request.Email,
                Mobile = request.Mobile,
                PasswordHash = passwordHash,
                InstituteId = null,
                RoleId = superAdminRole.Id
            };

            await _userRepository.AddAsync(user);
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var user =
                await _userRepository.GetByEmailAsync(request.Email);

            if (user == null)
            {
                throw new UnauthorizedException(
                    "Invalid email or password.");
            }

            bool passwordValid =
                _passwordHasher.VerifyPassword(
                    request.Password,
                    user.PasswordHash);

            if (!passwordValid)
            {
                throw new UnauthorizedException(
                    "Invalid email or password.");
            }

            string token =
                _jwtTokenService.GenerateToken(user);

            return new LoginResponse
            {
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddMinutes(60)
            };
        }
    }
}
