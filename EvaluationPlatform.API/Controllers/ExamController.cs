using CheckMate.Application.DTOs.Exam;
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
    public class ExamController : ControllerBase
    {
        private readonly IExamService _examService;

        public ExamController(
            IExamService examService)
        {
            _examService = examService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateExam(
            CreateExamRequest request)
        {
            int adminUserId = GetCurrentUserId();

            await _examService.CreateExamAsync(
                adminUserId,
                request);

            return Ok(new
            {
                success = true,
                message = "Exam created successfully."
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetExams()
        {
            int adminUserId = GetCurrentUserId();

            var exams =
                await _examService
                    .GetExamsAsync(adminUserId);

            return Ok(new
            {
                success = true,
                data = exams
            });
        }

        [HttpGet("{examId:int}")]
        public async Task<IActionResult> GetExam(
            int examId)
        {
            int adminUserId = GetCurrentUserId();

            var exam =
                await _examService
                    .GetExamByIdAsync(
                        adminUserId,
                        examId);

            return Ok(new
            {
                success = true,
                data = exam
            });
        }

        [HttpPatch("{examId:int}/deactivate")]
        public async Task<IActionResult> DeactivateExam(
            int examId)
        {
            int adminUserId = GetCurrentUserId();

            await _examService
                .DeactivateExamAsync(
                    adminUserId,
                    examId);

            return Ok(new
            {
                success = true,
                message = "Exam deactivated successfully."
            });
        }

        [HttpPatch("{examId:int}/activate")]
        public async Task<IActionResult> ActivateExam(
            int examId)
        {
            int adminUserId = GetCurrentUserId();

            await _examService
                .ActivateExamAsync(
                    adminUserId,
                    examId);

            return Ok(new
            {
                success = true,
                message = "Exam activated successfully."
            });
        }

        private int GetCurrentUserId()
        {
            string? userId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            if (!int.TryParse(
                    userId,
                    out int parsedUserId))
            {
                throw new UnauthorizedAccessException(
                    "Invalid authenticated user.");
            }

            return parsedUserId;
        }
    }
}
