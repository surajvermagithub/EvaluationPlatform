using CheckMate.Application.DTOs.Institute;
using CheckMate.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CheckMate.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin")]
    public class InstituteController : ControllerBase
    {
        private readonly IInstituteService _instituteService;

        public InstituteController(
            IInstituteService instituteService)
        {
            _instituteService = instituteService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateInstitute(
            CreateInstituteRequest request)
        {
            await _instituteService.CreateInstituteAsync(request);

            return Ok(new
            {
                success = true,
                message = "Institute created successfully."
            });
        }
    }
}
