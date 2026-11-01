using CheckMate.Application.Interfaces.Repositories;
using CheckMate.Domain.Entities;
using CheckMate.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CheckMate.Infrastructure.Repositories
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly ApplicationDbContext _context;

        public SubjectRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistsByCodeAsync(
            int examId,
            string code,
            int? excludeSubjectId = null)
        {
            return await _context.Subjects.AnyAsync(s =>
                s.ExamId == examId &&
                s.Code == code &&
                !s.IsDeleted &&
                (!excludeSubjectId.HasValue || s.Id != excludeSubjectId.Value));
        }

        public async Task<bool> ExistsByNameAsync(
            int examId,
            string name,
            int? excludeSubjectId = null)
        {
            return await _context.Subjects.AnyAsync(s =>
                s.ExamId == examId &&
                s.Name == name &&
                !s.IsDeleted &&
                (!excludeSubjectId.HasValue || s.Id != excludeSubjectId.Value));
        }

        public async Task<Subject> AddAsync(Subject subject)
        {
            await _context.Subjects.AddAsync(subject);
            await _context.SaveChangesAsync();

            return subject;
        }

        public async Task<List<Subject>> GetByExamIdAsync(int examId)
        {
            return await _context.Subjects
                .Where(s => s.ExamId == examId && !s.IsDeleted)
                .OrderBy(s => s.DisplayOrder)
                .ThenBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<Subject?> GetByIdAsync(int subjectId)
        {
            return await _context.Subjects
                .Include(s => s.Exam)
                .FirstOrDefaultAsync(s =>
                    s.Id == subjectId &&
                    !s.IsDeleted);
        }

        public async Task UpdateAsync(Subject subject)
        {
            _context.Subjects.Update(subject);
            await _context.SaveChangesAsync();
        }
    }
}
