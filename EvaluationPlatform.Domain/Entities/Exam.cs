using CheckMate.Domain.Common;

namespace CheckMate.Domain.Entities
{
    public class Exam : BaseEntity
    {
        public int InstituteId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime ExamDate { get; set; }

        public Institute Institute { get; set; } = null!;
    }
}
