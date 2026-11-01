using System.ComponentModel.DataAnnotations;

namespace CheckMate.Application.DTOs.Subjects
{
    public class CreateSubjectRequest
    {
        [Required]
        public int ExamId { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Code { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [Range(typeof(decimal), "0.01", "9999999999999999")]
        public decimal MaximumMarks { get; set; }

        [Range(typeof(decimal), "0", "9999999999999999")]
        public decimal PassingMarks { get; set; }

        [Range(0, int.MaxValue)]
        public int DisplayOrder { get; set; }
    }
}
