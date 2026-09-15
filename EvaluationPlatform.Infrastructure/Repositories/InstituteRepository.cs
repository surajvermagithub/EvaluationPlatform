using CheckMate.Application.Interfaces.Repositories;
using CheckMate.Domain.Entities;
using CheckMate.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CheckMate.Infrastructure.Repositories
{
    public class InstituteRepository : IInstituteRepository
    {
        private readonly ApplicationDbContext _context;

        public InstituteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistsByCodeAsync(string code)
        {
            return await _context.Institutes
                .AnyAsync(i =>
                    i.Code == code &&
                    !i.IsDeleted);
        }

        public async Task<Institute?> GetByIdAsync(int instituteId)
        {
            return await _context.Institutes
                .FirstOrDefaultAsync(i =>
                    i.Id == instituteId &&
                    !i.IsDeleted &&
                    i.IsActive);
        }

        public async Task<Institute> AddAsync(Institute institute)
        {
            await _context.Institutes.AddAsync(institute);
            await _context.SaveChangesAsync();

            return institute;
        }
    }
}
