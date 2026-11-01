using CheckMate.Application.DTOs.Subjects;
using CheckMate.Application.Exceptions;
using CheckMate.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CheckMate.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _subjectService;

        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
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
        public async Task<IActionResult> CreateSubject(
            [FromBody] CreateSubjectRequest request)
        {
            await _subjectService.CreateSubjectAsync(
                GetCurrentUserId(), request);

            return Ok(new
            {
                success = true,
                message = "Subject created successfully."
            });
        }

        [HttpGet("exam/{examId:int}")]
        public async Task<IActionResult> GetSubjects(int examId)
        {
            var subjects = await _subjectService.GetSubjectsAsync(
                GetCurrentUserId(), examId);

            return Ok(new
            {
                success = true,
                data = subjects
            });
        }

        [HttpGet("{subjectId:int}")]
        public async Task<IActionResult> GetSubject(int subjectId)
        {
            var subject = await _subjectService.GetSubjectByIdAsync(
                GetCurrentUserId(), subjectId);

            return Ok(new
            {
                success = true,
                data = subject
            });
        }

        [HttpPut("{subjectId:int}")]
        public async Task<IActionResult> UpdateSubject(
            int subjectId,
            [FromBody] UpdateSubjectRequest request)
        {
            await _subjectService.UpdateSubjectAsync(
                GetCurrentUserId(), subjectId, request);

            return Ok(new
            {
                success = true,
                message = "Subject updated successfully."
            });
        }

        [HttpPatch("{subjectId:int}/deactivate")]
        public async Task<IActionResult> DeactivateSubject(int subjectId)
        {
            await _subjectService.DeactivateSubjectAsync(
                GetCurrentUserId(), subjectId);

            return Ok(new
            {
                success = true,
                message = "Subject deactivated successfully."
            });
        }

        [HttpPatch("{subjectId:int}/activate")]
        public async Task<IActionResult> ActivateSubject(int subjectId)
        {
            await _subjectService.ActivateSubjectAsync(
                GetCurrentUserId(), subjectId);

            return Ok(new
            {
                success = true,
                message = "Subject activated successfully."
            });
        }
    }
}
