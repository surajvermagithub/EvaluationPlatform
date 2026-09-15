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

        public InstituteService(
            IInstituteRepository instituteRepository)
        {
            _instituteRepository = instituteRepository;
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
    }
}
