using CheckMate.Application.DTOs.Auth;
using CheckMate.Application.Interfaces.Repositories;
using CheckMate.Domain.Entities;

namespace CheckMate.Application.Interfaces.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public AuthService(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task RegisterAdminAsync(RegisterAdminRequest request)
        {
            // 1. Check whether SuperAdmin already exists
            bool adminExists = await _userRepository.SuperAdminExistsAsync();

            if (adminExists)
            {
                throw new InvalidOperationException(
                    "SuperAdmin already exists.");
            }

            // 2. Check whether email is already used
            var existingUser = await _userRepository
                .GetByEmailAsync(request.Email);

            if (existingUser != null)
            {
                throw new InvalidOperationException(
                    "Email is already registered.");
            }

            // 3. Hash password
            string passwordHash =
                _passwordHasher.HashPassword(request.Password);

            var superAdminRole =
    await _userRepository.GetRoleByNameAsync("SuperAdmin");

            if (superAdminRole == null)
            {
                throw new InvalidOperationException(
                    "SuperAdmin role is not configured.");
            }

            // 4. Create User entity
         
            var user = new User
            {
                FullName = request.FullName,
                Email = request.Email,
                Mobile = request.Mobile,
                PasswordHash = passwordHash,
                InstituteId = null,
                RoleId = superAdminRole.Id
            };

            // 5. Save user
            await _userRepository.AddAsync(user);
        }


    }
}
