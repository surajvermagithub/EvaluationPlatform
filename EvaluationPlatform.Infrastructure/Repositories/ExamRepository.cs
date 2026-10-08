using CheckMate.Application.Interfaces.Repositories;
using CheckMate.Domain.Entities;
using CheckMate.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CheckMate.Infrastructure.Repositories
{
    public class ExamRepository : IExamRepository
    {
        private readonly ApplicationDbContext _context;

        public ExamRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistsByCodeAsync(
            string code)
        {
            return await _context.Exams
                .AnyAsync(e =>
                    e.Code == code &&
                    !e.IsDeleted);
        }

        public async Task<Exam> AddAsync(
            Exam exam)
        {
            await _context.Exams.AddAsync(exam);

            await _context.SaveChangesAsync();

            return exam;
        }

        public async Task<List<Exam>> GetByInstituteIdAsync(
            int instituteId)
        {
            return await _context.Exams
                .Where(e =>
                    e.InstituteId == instituteId &&
                    !e.IsDeleted)
                .OrderByDescending(e => e.ExamDate)
                .ToListAsync();
        }

        public async Task<Exam?> GetByIdAndInstituteIdAsync(
            int examId,
            int instituteId)
        {
            return await _context.Exams
                .FirstOrDefaultAsync(e =>
                    e.Id == examId &&
                    e.InstituteId == instituteId &&
                    !e.IsDeleted);
        }

        public async Task UpdateAsync(
            Exam exam)
        {
            _context.Exams.Update(exam);

            await _context.SaveChangesAsync();
        }
    }
}
