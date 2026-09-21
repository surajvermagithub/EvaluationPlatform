using CheckMate.Application.DTOs.Teacher;
using CheckMate.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CheckMate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherService _teacherService;

        public TeacherController(
            ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTeacher(
            CreateTeacherRequest request)
        {
            int adminUserId = GetCurrentUserId();

            await _teacherService
                .CreateTeacherAsync(
                    adminUserId,
                    request);

            return Ok(new
            {
                success = true,
                message = "Teacher created successfully."
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetTeachers()
        {
            int adminUserId = GetCurrentUserId();

            var teachers =
                await _teacherService
                    .GetTeachersAsync(adminUserId);

            return Ok(new
            {
                success = true,
                data = teachers
            });
        }

        [HttpPatch("{teacherId}/deactivate")]
        public async Task<IActionResult> DeactivateTeacher(
            int teacherId)
        {
            int adminUserId = GetCurrentUserId();

            await _teacherService
                .DeactivateTeacherAsync(
                    adminUserId,
                    teacherId);

            return Ok(new
            {
                success = true,
                message = "Teacher deactivated successfully."
            });
        }

        [HttpPatch("{teacherId}/activate")]
        public async Task<IActionResult> ActivateTeacher(
            int teacherId)
        {
            int adminUserId = GetCurrentUserId();

            await _teacherService
                .ActivateTeacherAsync(
                    adminUserId,
                    teacherId);

            return Ok(new
            {
                success = true,
                message = "Teacher activated successfully."
            });
        }

        private int GetCurrentUserId()
        {
            string? userId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            if (!int.TryParse(userId, out int parsedUserId))
            {
                throw new UnauthorizedAccessException(
                    "Invalid authenticated user.");
            }

            return parsedUserId;
        }
    }
}
