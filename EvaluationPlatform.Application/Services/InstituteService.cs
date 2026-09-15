using CheckMate.Application.DTOs.Institute;
using CheckMate.Application.Exceptions;
using CheckMate.Application.Interfaces.Repositories;
using CheckMate.Application.Interfaces.Services;
using CheckMate.Domain.Entities;

namespace CheckMate.Application.Services
{
    public class InstituteService : IInstituteService
    {
      

        private readonly IInstituteRepository _instituteRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public InstituteService(
            IInstituteRepository instituteRepository,
            IUserRepository userRepository,
            IPasswordHasher passwordHasher)
        {
            _instituteRepository = instituteRepository;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task CreateInstituteAsync(
            CreateInstituteRequest request)
        {
            bool instituteExists =
                await _instituteRepository
                    .ExistsByCodeAsync(request.Code);

            if (instituteExists)
            {
                throw new ConflictException(
                    "Institute code is already registered.");
            }

            var institute = new Institute
            {
                Name = request.Name,
                Code = request.Code,
                Email = request.Email,
                Mobile = request.Mobile,
                AddressLine1 = request.AddressLine1,
                AddressLine2 = request.AddressLine2,
                City = request.City,
                State = request.State,
                Country = request.Country,
                PinCode = request.PinCode
            };

            await _instituteRepository.AddAsync(institute);
        }

        public async Task CreateInstituteAdminAsync(
    CreateInstituteAdminRequest request)
        {
            var institute =
                await _instituteRepository
                    .GetByIdAsync(request.InstituteId);

            if (institute == null)
            {
                throw new NotFoundException(
     "Institute not found.");
            }

            var existingUser =
                await _userRepository
                    .GetByEmailAsync(request.Email);

            if (existingUser != null)
            {
                throw new ConflictException(
                    "Email is already registered.");
            }

            var adminRole =
                await _userRepository
                    .GetRoleByNameAsync("Admin");

            if (adminRole == null)
            {
                throw new InvalidOperationException(
                    "Admin role is not configured.");
            }

            var passwordHash =
                _passwordHasher.HashPassword(request.Password);

            var admin = new User
            {
                FullName = request.FullName,
                Email = request.Email,
                Mobile = request.Mobile,
                PasswordHash = passwordHash,
                InstituteId = institute.Id,
                RoleId = adminRole.Id
            };

            await _userRepository.AddAsync(admin);
        }
    }
}
