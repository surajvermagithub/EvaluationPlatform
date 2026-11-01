namespace CheckMate.Application.DTOs.Subjects
{
    public class SubjectResponse
    {
        public int Id { get; set; }
        public int ExamId { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; }

        public decimal MaximumMarks { get; set; }
        public decimal PassingMarks { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
