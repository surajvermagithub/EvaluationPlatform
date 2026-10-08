using CheckMate.Application.DTOs.Exam;
using CheckMate.Application.Exceptions;
using CheckMate.Application.Interfaces.Repositories;
using CheckMate.Application.Interfaces.Services;
using CheckMate.Domain.Entities;

namespace CheckMate.Application.Services
{
    public class ExamService : IExamService
    {
        private readonly IUserRepository _userRepository;
        private readonly IExamRepository _examRepository;

        public ExamService(
            IUserRepository userRepository,
            IExamRepository examRepository)
        {
            _userRepository = userRepository;
            _examRepository = examRepository;
        }

        public async Task CreateExamAsync(
            int adminUserId,
            CreateExamRequest request)
        {
            var admin =
                await _userRepository
                    .GetByIdAsync(adminUserId);

            if (admin == null)
            {
                throw new UnauthorizedException(
                    "Authenticated user was not found.");
            }

            if (admin.Role.Name != "Admin")
            {
                throw new UnauthorizedException(
                    "Only institute administrators can create exams.");
            }

            if (!admin.InstituteId.HasValue)
            {
                throw new InvalidOperationException(
                    "Admin is not associated with an institute.");
            }

            string code = request.Code
                .Trim()
                .ToUpperInvariant();

            bool codeExists =
                await _examRepository
                    .ExistsByCodeAsync(code);

            if (codeExists)
            {
                throw new ConflictException(
                    "Exam code is already registered.");
            }

            var exam = new Exam
            {
                InstituteId = admin.InstituteId.Value,

                Name = request.Name.Trim(),

                Code = code,

                Description =
                    string.IsNullOrWhiteSpace(request.Description)
                        ? null
                        : request.Description.Trim(),

                ExamDate = request.ExamDate,

                CreatedBy = adminUserId
            };

            await _examRepository.AddAsync(exam);
        }

        public async Task<List<ExamResponse>> GetExamsAsync(
            int adminUserId)
        {
            var admin =
                await _userRepository
                    .GetByIdAsync(adminUserId);

            if (admin == null)
            {
                throw new UnauthorizedException(
                    "Authenticated user was not found.");
            }

            if (admin.Role.Name != "Admin")
            {
                throw new UnauthorizedException(
                    "Only institute administrators can view exams.");
            }

            if (!admin.InstituteId.HasValue)
            {
                throw new InvalidOperationException(
                    "Admin is not associated with an institute.");
            }

            var exams =
                await _examRepository
                    .GetByInstituteIdAsync(
                        admin.InstituteId.Value);

            return exams.Select(e => new ExamResponse
            {
                Id = e.Id,
                Name = e.Name,
                Code = e.Code,
                Description = e.Description,
                ExamDate = e.ExamDate,
                IsActive = e.IsActive,
                CreatedOn = e.CreatedOn
            }).ToList();
        }

        public async Task<ExamResponse> GetExamByIdAsync(
            int adminUserId,
            int examId)
        {
            var admin =
                await _userRepository
                    .GetByIdAsync(adminUserId);

            if (admin == null)
            {
                throw new UnauthorizedException(
                    "Authenticated user was not found.");
            }

            if (admin.Role.Name != "Admin")
            {
                throw new UnauthorizedException(
                    "Only institute administrators can view exams.");
            }

            if (!admin.InstituteId.HasValue)
            {
                throw new InvalidOperationException(
                    "Admin is not associated with an institute.");
            }

            var exam =
                await _examRepository
                    .GetByIdAndInstituteIdAsync(
                        examId,
                        admin.InstituteId.Value);

            if (exam == null)
            {
                throw new NotFoundException(
                    "Exam not found.");
            }

            return new ExamResponse
            {
                Id = exam.Id,
                Name = exam.Name,
                Code = exam.Code,
                Description = exam.Description,
                ExamDate = exam.ExamDate,
                IsActive = exam.IsActive,
                CreatedOn = exam.CreatedOn
            };
        }

        public async Task DeactivateExamAsync(
            int adminUserId,
            int examId)
        {
            var admin =
                await _userRepository
                    .GetByIdAsync(adminUserId);

            if (admin == null)
            {
                throw new UnauthorizedException(
                    "Authenticated user was not found.");
            }

            if (admin.Role.Name != "Admin")
            {
                throw new UnauthorizedException(
                    "Only institute administrators can deactivate exams.");
            }

            if (!admin.InstituteId.HasValue)
            {
                throw new InvalidOperationException(
                    "Admin is not associated with an institute.");
            }

            var exam =
                await _examRepository
                    .GetByIdAndInstituteIdAsync(
                        examId,
                        admin.InstituteId.Value);

            if (exam == null)
            {
                throw new NotFoundException(
                    "Exam not found.");
            }

            exam.IsActive = false;
            exam.UpdatedOn = DateTime.UtcNow;
            exam.UpdatedBy = adminUserId;

            await _examRepository.UpdateAsync(exam);
        }

        public async Task ActivateExamAsync(
            int adminUserId,
            int examId)
        {
            var admin =
                await _userRepository
                    .GetByIdAsync(adminUserId);

            if (admin == null)
            {
                throw new UnauthorizedException(
                    "Authenticated user was not found.");
            }

            if (admin.Role.Name != "Admin")
            {
                throw new UnauthorizedException(
                    "Only institute administrators can activate exams.");
            }

            if (!admin.InstituteId.HasValue)
            {
                throw new InvalidOperationException(
                    "Admin is not associated with an institute.");
            }

            var exam =
                await _examRepository
                    .GetByIdAndInstituteIdAsync(
                        examId,
                        admin.InstituteId.Value);

            if (exam == null)
            {
                throw new NotFoundException(
                    "Exam not found.");
            }

            exam.IsActive = true;
            exam.UpdatedOn = DateTime.UtcNow;
            exam.UpdatedBy = adminUserId;

            await _examRepository.UpdateAsync(exam);
        }
    }
}
