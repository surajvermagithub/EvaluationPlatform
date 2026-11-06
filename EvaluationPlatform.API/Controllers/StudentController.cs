using CheckMate.Application.DTOs.Students;
using CheckMate.Application.Exceptions;
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
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        private int GetCurrentUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userId, out var id))
            {
                throw new UnauthorizedException("Invalid user identity.");
            }

            return id;
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent(
            [FromBody] CreateStudentRequest request)
        {
            await _studentService.CreateStudentAsync(
                GetCurrentUserId(),
                request);

            return Ok(new
            {
                success = true,
                message = "Student registered successfully."
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetStudents()
        {
            var students = await _studentService.GetStudentsAsync(
                GetCurrentUserId());

            return Ok(new
            {
                success = true,
                data = students
            });
        }

        [HttpGet("{studentId:int}")]
        public async Task<IActionResult> GetStudent(int studentId)
        {
            var student = await _studentService.GetStudentByIdAsync(
                GetCurrentUserId(),
                studentId);

            return Ok(new
            {
                success = true,
                data = student
            });
        }

        [HttpPut("{studentId:int}")]
        public async Task<IActionResult> UpdateStudent(
            int studentId,
            [FromBody] UpdateStudentRequest request)
        {
            await _studentService.UpdateStudentAsync(
                GetCurrentUserId(),
                studentId,
                request);

            return Ok(new
            {
                success = true,
                message = "Student updated successfully."
            });
        }

        [HttpPatch("{studentId:int}/deactivate")]
        public async Task<IActionResult> DeactivateStudent(int studentId)
        {
            await _studentService.DeactivateStudentAsync(
                GetCurrentUserId(),
                studentId);

            return Ok(new
            {
                success = true,
                message = "Student deactivated successfully."
            });
        }

        [HttpPatch("{studentId:int}/activate")]
        public async Task<IActionResult> ActivateStudent(int studentId)
        {
            await _studentService.ActivateStudentAsync(
                GetCurrentUserId(),
                studentId);

            return Ok(new
            {
                success = true,
                message = "Student activated successfully."
            });
        }
    }
}
