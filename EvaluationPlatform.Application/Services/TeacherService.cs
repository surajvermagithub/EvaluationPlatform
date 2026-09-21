using CheckMate.Application.DTOs.Teacher;
using CheckMate.Application.Exceptions;
using CheckMate.Application.Interfaces.Repositories;
using CheckMate.Application.Interfaces.Services;
using CheckMate.Domain.Entities;

namespace CheckMate.Application.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public TeacherService(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task CreateTeacherAsync(
            int adminUserId,
            CreateTeacherRequest request)
        {
            var admin =
                await _userRepository.GetByIdAsync(adminUserId);

            if (admin == null)
            {
                throw new UnauthorizedException(
                    "Authenticated user was not found.");
            }

            if (admin.Role.Name != "Admin")
            {
                throw new UnauthorizedException(
                    "Only institute administrators can create teachers.");
            }

            if (!admin.InstituteId.HasValue)
            {
                throw new InvalidOperationException(
                    "Admin is not associated with an institute.");
            }

            bool emailExists =
                await _userRepository
                    .ExistsByEmailAsync(request.Email);

            if (emailExists)
            {
                throw new ConflictException(
                    "Email is already registered.");
            }

            var teacherRole =
                await _userRepository
                    .GetRoleByNameAsync("Teacher");

            if (teacherRole == null)
            {
                throw new InvalidOperationException(
                    "Teacher role is not configured.");
            }

            string passwordHash =
                _passwordHasher.HashPassword(request.Password);

            var teacher = new User
            {
                FullName = request.FullName,
                Email = request.Email,
                Mobile = request.Mobile,
                PasswordHash = passwordHash,

                // Critical tenant isolation:
                InstituteId = admin.InstituteId.Value,

                RoleId = teacherRole.Id,

                CreatedBy = adminUserId
            };

            await _userRepository.AddAsync(teacher);
        }

        public async Task<List<TeacherResponse>> GetTeachersAsync(
            int adminUserId)
        {
            var admin =
                await _userRepository.GetByIdAsync(adminUserId);

            if (admin == null)
            {
                throw new UnauthorizedException(
                    "Authenticated user was not found.");
            }

            if (admin.Role.Name != "Admin")
            {
                throw new UnauthorizedException(
                    "Only institute administrators can view teachers.");
            }

            if (!admin.InstituteId.HasValue)
            {
                throw new InvalidOperationException(
                    "Admin is not associated with an institute.");
            }

            var teachers =
                await _userRepository
                    .GetTeachersByInstituteIdAsync(
                        admin.InstituteId.Value);

            return teachers.Select(t => new TeacherResponse
            {
                Id = t.Id,
                FullName = t.FullName,
                Email = t.Email,
                Mobile = t.Mobile,
                IsActive = t.IsActive,
                CreatedOn = t.CreatedOn
            }).ToList();
        }

        public async Task DeactivateTeacherAsync(
            int adminUserId,
            int teacherId)
        {
            var admin =
                await _userRepository.GetByIdAsync(adminUserId);

            if (admin == null)
            {
                throw new UnauthorizedException(
                    "Authenticated user was not found.");
            }

            if (admin.Role.Name != "Admin")
            {
                throw new UnauthorizedException(
                    "Only institute administrators can deactivate teachers.");
            }

            if (!admin.InstituteId.HasValue)
            {
                throw new InvalidOperationException(
                    "Admin is not associated with an institute.");
            }

            var teacher =
                await _userRepository
                    .GetTeacherByIdAndInstituteIdAsync(
                        teacherId,
                        admin.InstituteId.Value);

            if (teacher == null)
            {
                throw new NotFoundException(
                    "Teacher not found.");
            }

            teacher.IsActive = false;
            teacher.UpdatedOn = DateTime.UtcNow;
            teacher.UpdatedBy = adminUserId;

            await _userRepository.UpdateAsync(teacher);
        }

        public async Task ActivateTeacherAsync(
            int adminUserId,
            int teacherId)
        {
            var admin =
                await _userRepository.GetByIdAsync(adminUserId);

            if (admin == null)
            {
                throw new UnauthorizedException(
                    "Authenticated user was not found.");
            }

            if (admin.Role.Name != "Admin")
            {
                throw new UnauthorizedException(
                    "Only institute administrators can activate teachers.");
            }

            if (!admin.InstituteId.HasValue)
            {
                throw new InvalidOperationException(
                    "Admin is not associated with an institute.");
            }

            var teacher =
                await _userRepository
                    .GetTeacherByIdAndInstituteIdAsync(
                        teacherId,
                        admin.InstituteId.Value);

            if (teacher == null)
            {
                throw new NotFoundException(
                    "Teacher not found.");
            }

            teacher.IsActive = true;
            teacher.UpdatedOn = DateTime.UtcNow;
            teacher.UpdatedBy = adminUserId;

            await _userRepository.UpdateAsync(teacher);
        }
    }
}
