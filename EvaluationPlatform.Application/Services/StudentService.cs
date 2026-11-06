using CheckMate.Application.DTOs.Students;
using CheckMate.Application.Exceptions;
using CheckMate.Application.Interfaces.Repositories;
using CheckMate.Application.Interfaces.Services;
using CheckMate.Domain.Entities;

namespace CheckMate.Application.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IInstituteRepository _instituteRepository;

        public StudentService(
            IStudentRepository studentRepository,
            IUserRepository userRepository,
            IInstituteRepository instituteRepository)
        {
            _studentRepository = studentRepository;
            _userRepository = userRepository;
            _instituteRepository = instituteRepository;
        }

        public async Task CreateStudentAsync(
            int adminUserId,
            CreateStudentRequest request)
        {
            var admin = await GetAdminAsync(adminUserId);

            var instituteId = admin.InstituteId!.Value;

            var institute = await _instituteRepository.GetByIdAsync(instituteId);

            if (institute == null)
            {
                throw new NotFoundException("Institute not found.");
            }

            // Generate admission number using institute code and India calendar year.
            var admissionYear = DateTime.UtcNow.AddHours(5.5).Year;

            var prefix =
                $"{institute.Code.Trim().ToUpperInvariant()}-{admissionYear}-";

            var lastSequence =
                await _studentRepository.GetLatestAdmissionSequenceAsync(
                    instituteId,
                    prefix);

            var nextSequence = lastSequence + 1;

            var admissionNumber = $"{prefix}{nextSequence:D3}";

            ValidateDateOfBirth(request.DateOfBirth);

            var student = new Student
            {
                InstituteId = instituteId,
                FullName = request.FullName.Trim(),
                AdmissionNumber = admissionNumber,
                Email = request.Email?.Trim(),
                Mobile = request.Mobile.Trim(),
                DateOfBirth = request.DateOfBirth,
                Gender = request.Gender?.Trim(),
                Address = request.Address?.Trim(),

                CreatedBy = admin.Id,
                CreatedOn = DateTime.UtcNow,
                IsActive = true,
                IsDeleted = false
            };

            await _studentRepository.AddAsync(student);
        }

        public async Task<List<StudentResponse>> GetStudentsAsync(
            int adminUserId)
        {
            var admin = await GetAdminAsync(adminUserId);

            var students = await _studentRepository.GetByInstituteIdAsync(
                admin.InstituteId!.Value);

            return students.Select(MapToResponse).ToList();
        }

        public async Task<StudentResponse> GetStudentByIdAsync(
            int adminUserId,
            int studentId)
        {
            var admin = await GetAdminAsync(adminUserId);

            var student = await GetStudentForAdminAsync(
                studentId,
                admin.InstituteId!.Value);

            return MapToResponse(student);
        }

        public async Task UpdateStudentAsync(
            int adminUserId,
            int studentId,
            UpdateStudentRequest request)
        {
            var admin = await GetAdminAsync(adminUserId);

            var student = await GetStudentForAdminAsync(
                studentId,
                admin.InstituteId!.Value);

            ValidateDateOfBirth(request.DateOfBirth);

            student.FullName = request.FullName.Trim();
            student.Email = request.Email?.Trim();
            student.Mobile = request.Mobile.Trim();
            student.DateOfBirth = request.DateOfBirth;
            student.Gender = request.Gender?.Trim();
            student.Address = request.Address?.Trim();

            // AdmissionNumber is intentionally not updated.
            student.UpdatedBy = admin.Id;
            student.UpdatedOn = DateTime.UtcNow;

            await _studentRepository.UpdateAsync(student);
        }

        public async Task DeactivateStudentAsync(
            int adminUserId,
            int studentId)
        {
            var admin = await GetAdminAsync(adminUserId);

            var student = await GetStudentForAdminAsync(
                studentId,
                admin.InstituteId!.Value);

            if (!student.IsActive)
            {
                throw new ConflictException("Student is already inactive.");
            }

            student.IsActive = false;
            student.UpdatedBy = admin.Id;
            student.UpdatedOn = DateTime.UtcNow;

            await _studentRepository.UpdateAsync(student);
        }

        public async Task ActivateStudentAsync(
            int adminUserId,
            int studentId)
        {
            var admin = await GetAdminAsync(adminUserId);

            var student = await GetStudentForAdminAsync(
                studentId,
                admin.InstituteId!.Value);

            if (student.IsActive)
            {
                throw new ConflictException("Student is already active.");
            }

            student.IsActive = true;
            student.UpdatedBy = admin.Id;
            student.UpdatedOn = DateTime.UtcNow;

            await _studentRepository.UpdateAsync(student);
        }

        private async Task<User> GetAdminAsync(int adminUserId)
        {
            var admin = await _userRepository.GetByIdAsync(adminUserId);

            if (admin == null ||
                !admin.IsActive ||
                admin.Role.Name != "Admin" ||
                admin.InstituteId == null)
            {
                throw new UnauthorizedException(
                    "Only an active institute Admin can perform this operation.");
            }

            return admin;
        }

        private async Task<Student> GetStudentForAdminAsync(
            int studentId,
            int instituteId)
        {
            var student =
                await _studentRepository.GetByIdAndInstituteIdAsync(
                    studentId,
                    instituteId);

            if (student == null)
            {
                throw new NotFoundException("Student not found.");
            }

            return student;
        }

        private static void ValidateDateOfBirth(DateTime? dateOfBirth)
        {
            if (dateOfBirth.HasValue &&
                dateOfBirth.Value.Date > DateTime.UtcNow.AddHours(5.5).Date)
            {
                throw new ConflictException(
                    "Date of birth cannot be in the future.");
            }
        }

        private static StudentResponse MapToResponse(Student student)
        {
            return new StudentResponse
            {
                Id = student.Id,
                InstituteId = student.InstituteId,
                FullName = student.FullName,
                AdmissionNumber = student.AdmissionNumber,
                Email = student.Email,
                Mobile = student.Mobile,
                DateOfBirth = student.DateOfBirth,
                Gender = student.Gender,
                Address = student.Address,
                IsActive = student.IsActive,
                CreatedOn = student.CreatedOn
            };
        }
    }
}
