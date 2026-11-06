using CheckMate.Application.Interfaces.Repositories;
using CheckMate.Domain.Entities;
using CheckMate.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CheckMate.Infrastructure.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ApplicationDbContext _context;

        public StudentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        //public async Task<bool> ExistsByAdmissionNumberAsync(
        //    int instituteId,
        //    string admissionNumber,
        //    int? excludeStudentId = null)
        //{
        //    return await _context.Students.AnyAsync(s =>
        //        s.InstituteId == instituteId &&
        //        s.AdmissionNumber == admissionNumber &&
        //        !s.IsDeleted &&
        //        (!excludeStudentId.HasValue ||
        //         s.Id != excludeStudentId.Value));
        //}


        public async Task<int> GetLatestAdmissionSequenceAsync(
            int instituteId,
            string prefix)
        {
            var admissionNumbers = await _context.Students
                .AsNoTracking()
                .Where(s =>
                    s.InstituteId == instituteId &&
                    s.AdmissionNumber.StartsWith(prefix))
                .Select(s => s.AdmissionNumber)
                .ToListAsync();

            var sequences = admissionNumbers.Select(number =>
                int.TryParse(
                    number[prefix.Length..],
                    out var sequence)
                        ? sequence
                        : 0);

            return sequences.DefaultIfEmpty(0).Max();
        }

        public async Task<Student> AddAsync(Student student)
        {
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();

            return student;
        }

        public async Task<List<Student>> GetByInstituteIdAsync(
            int instituteId)
        {
            return await _context.Students
                .Where(s =>
                    s.InstituteId == instituteId &&
                    !s.IsDeleted)
                .OrderBy(s => s.FullName)
                .ToListAsync();
        }

        public async Task<Student?> GetByIdAndInstituteIdAsync(
            int studentId,
            int instituteId)
        {
            return await _context.Students
                .FirstOrDefaultAsync(s =>
                    s.Id == studentId &&
                    s.InstituteId == instituteId &&
                    !s.IsDeleted);
        }

        public async Task UpdateAsync(Student student)
        {
            _context.Students.Update(student);
            await _context.SaveChangesAsync();
        }
    }
}
