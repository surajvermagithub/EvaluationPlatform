using CheckMate.Application.DTOs.Subjects;
using CheckMate.Application.Exceptions;
using CheckMate.Application.Interfaces.Repositories;
using CheckMate.Application.Interfaces.Services;
using CheckMate.Domain.Entities;

namespace CheckMate.Application.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly ISubjectRepository _subjectRepository;
        private readonly IExamRepository _examRepository;
        private readonly IUserRepository _userRepository;

        public SubjectService(
            ISubjectRepository subjectRepository,
            IExamRepository examRepository,
            IUserRepository userRepository)
        {
            _subjectRepository = subjectRepository;
            _examRepository = examRepository;
            _userRepository = userRepository;
        }

        private async Task<User> GetAdminAsync(int adminUserId)
        {
            var admin = await _userRepository.GetByIdAsync(adminUserId);

            if (admin == null ||
                admin.Role.Name != "Admin" ||
                !admin.InstituteId.HasValue)
            {
                throw new UnauthorizedException(
                    "Only an institute administrator can manage subjects.");
            }

            return admin;
        }

        private async Task<Exam> GetExamForAdminAsync(
            int adminUserId,
            int examId)
        {
            var admin = await GetAdminAsync(adminUserId);

            var exam = await _examRepository.GetByIdAndInstituteIdAsync(
                examId,
                admin.InstituteId!.Value);

            if (exam == null)
            {
                throw new NotFoundException("Exam not found.");
            }

            return exam;
        }

        private async Task<Subject> GetSubjectForAdminAsync(
            int adminUserId,
            int subjectId)
        {
            var admin = await GetAdminAsync(adminUserId);

            var subject = await _subjectRepository.GetByIdAsync(subjectId);

            if (subject == null ||
                subject.Exam.InstituteId != admin.InstituteId)
            {
                throw new NotFoundException("Subject not found.");
            }

            return subject;
        }

        private static void ValidateMarks(
            decimal maximumMarks,
            decimal passingMarks)
        {
            if (maximumMarks <= 0)
            {
                throw new ConflictException(
                    "Maximum marks must be greater than zero.");
            }

            if (passingMarks < 0 || passingMarks > maximumMarks)
            {
                throw new ConflictException(
                    "Passing marks must be between zero and maximum marks.");
            }
        }

        private static SubjectResponse MapToResponse(Subject subject)
        {
            return new SubjectResponse
            {
                Id = subject.Id,
                ExamId = subject.ExamId,
                Name = subject.Name,
                Code = subject.Code,
                Description = subject.Description,
                MaximumMarks = subject.MaximumMarks,
                PassingMarks = subject.PassingMarks,
                DisplayOrder = subject.DisplayOrder,
                IsActive = subject.IsActive,
                CreatedOn = subject.CreatedOn
            };
        }

        public async Task CreateSubjectAsync(
            int adminUserId,
            CreateSubjectRequest request)
        {
            var exam = await GetExamForAdminAsync(
                adminUserId,
                request.ExamId);

            if (!exam.IsActive)
            {
                throw new ConflictException(
                    "Cannot add subjects to an inactive exam.");
            }

            var name = request.Name.Trim();
            var code = request.Code.Trim().ToUpperInvariant();

            ValidateMarks(request.MaximumMarks, request.PassingMarks);

            if (request.DisplayOrder < 0)
            {
                throw new ConflictException(
                    "Display order cannot be negative.");
            }

            if (await _subjectRepository.ExistsByCodeAsync(
                request.ExamId, code))
            {
                throw new ConflictException(
                    "A subject with this code already exists in this exam.");
            }

            if (await _subjectRepository.ExistsByNameAsync(
                request.ExamId, name))
            {
                throw new ConflictException(
                    "A subject with this name already exists in this exam.");
            }

            var subject = new Subject
            {
                ExamId = request.ExamId,
                Name = name,
                Code = code,
                Description = string.IsNullOrWhiteSpace(request.Description)
                    ? null
                    : request.Description.Trim(),
                MaximumMarks = request.MaximumMarks,
                PassingMarks = request.PassingMarks,
                DisplayOrder = request.DisplayOrder,
                CreatedBy = adminUserId
            };

            await _subjectRepository.AddAsync(subject);
        }

        public async Task<List<SubjectResponse>> GetSubjectsAsync(
            int adminUserId,
            int examId)
        {
            await GetExamForAdminAsync(adminUserId, examId);

            var subjects = await _subjectRepository.GetByExamIdAsync(examId);

            return subjects.Select(MapToResponse).ToList();
        }

        public async Task<SubjectResponse> GetSubjectByIdAsync(
            int adminUserId,
            int subjectId)
        {
            var subject = await GetSubjectForAdminAsync(
                adminUserId, subjectId);

            return MapToResponse(subject);
        }

        public async Task UpdateSubjectAsync(
            int adminUserId,
            int subjectId,
            UpdateSubjectRequest request)
        {
            var subject = await GetSubjectForAdminAsync(
                adminUserId, subjectId);

            if (!subject.IsActive)
            {
                throw new ConflictException(
                    "An inactive subject cannot be updated. Activate it first.");
            }

            var name = request.Name.Trim();
            var code = request.Code.Trim().ToUpperInvariant();

            ValidateMarks(request.MaximumMarks, request.PassingMarks);

            if (request.DisplayOrder < 0)
            {
                throw new ConflictException(
                    "Display order cannot be negative.");
            }

            if (await _subjectRepository.ExistsByCodeAsync(
                subject.ExamId, code, subjectId))
            {
                throw new ConflictException(
                    "A subject with this code already exists in this exam.");
            }

            if (await _subjectRepository.ExistsByNameAsync(
                subject.ExamId, name, subjectId))
            {
                throw new ConflictException(
                    "A subject with this name already exists in this exam.");
            }

            subject.Name = name;
            subject.Code = code;
            subject.Description = string.IsNullOrWhiteSpace(request.Description)
                ? null
                : request.Description.Trim();
            subject.MaximumMarks = request.MaximumMarks;
            subject.PassingMarks = request.PassingMarks;
            subject.DisplayOrder = request.DisplayOrder;
            subject.UpdatedOn = DateTime.UtcNow;
            subject.UpdatedBy = adminUserId;

            await _subjectRepository.UpdateAsync(subject);
        }

        public async Task DeactivateSubjectAsync(
            int adminUserId,
            int subjectId)
        {
            var subject = await GetSubjectForAdminAsync(
                adminUserId, subjectId);

            if (!subject.IsActive)
            {
                throw new ConflictException("Subject is already inactive.");
            }

            subject.IsActive = false;
            subject.UpdatedOn = DateTime.UtcNow;
            subject.UpdatedBy = adminUserId;

            await _subjectRepository.UpdateAsync(subject);
        }

        public async Task ActivateSubjectAsync(
            int adminUserId,
            int subjectId)
        {
            var subject = await GetSubjectForAdminAsync(
                adminUserId, subjectId);

            if (subject.IsActive)
            {
                throw new ConflictException("Subject is already active.");
            }

            var exam = await GetExamForAdminAsync(
                adminUserId, subject.ExamId);

            if (!exam.IsActive)
            {
                throw new ConflictException(
                    "Cannot activate a subject while its exam is inactive.");
            }

            subject.IsActive = true;
            subject.UpdatedOn = DateTime.UtcNow;
            subject.UpdatedBy = adminUserId;

            await _subjectRepository.UpdateAsync(subject);
        }
    }
}
