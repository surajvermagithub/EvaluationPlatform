using System.ComponentModel.DataAnnotations;

namespace CheckMate.Application.DTOs.Exam
{
    public class CreateExamRequest
    {
        [Required(ErrorMessage = "Exam name is required.")]
        [MaxLength(150, ErrorMessage = "Exam name cannot exceed 150 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Exam code is required.")]
        [MaxLength(50, ErrorMessage = "Exam code cannot exceed 50 characters.")]
        public string Code { get; set; } = string.Empty;

        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Exam date is required.")]
        public DateTime ExamDate { get; set; }
    }
}
