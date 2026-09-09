using CheckMate.Application.DTOs.Auth;
using CheckMate.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CheckMate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterAdmin(
            RegisterAdminRequest request)
        {
            await _authService.RegisterAdminAsync(request);

            return Ok(new
            {
                message = "SuperAdmin registered successfully."
            });
        }
    }
}
