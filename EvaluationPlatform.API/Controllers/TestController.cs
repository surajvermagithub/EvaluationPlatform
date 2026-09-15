using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CheckMate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [Authorize]
        [HttpGet("protected")]
        public IActionResult Protected()
        {
            return Ok(new
            {
                message = "You are authenticated.",
                userId = User.FindFirst(
                    System.Security.Claims.ClaimTypes.NameIdentifier)?.Value,
                name = User.Identity?.Name,
                role = User.FindFirst(
                    System.Security.Claims.ClaimTypes.Role)?.Value
            });
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpGet("superadmin")]
        public IActionResult SuperAdminOnly()
        {
            return Ok(new
            {
                message = "Only SuperAdmin can access this endpoint."
            });
        }
    }
}
