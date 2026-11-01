using CheckMate.Domain.Common;

namespace CheckMate.Domain.Entities
{
    public class Subject : BaseEntity
    {
        public int ExamId { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; }

        public decimal MaximumMarks { get; set; }
        public decimal PassingMarks { get; set; }

        public int DisplayOrder { get; set; }

        public Exam Exam { get; set; } = null!;
    }
}
