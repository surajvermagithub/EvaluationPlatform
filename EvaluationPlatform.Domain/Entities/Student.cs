using CheckMate.Domain.Common;

namespace CheckMate.Domain.Entities
{
    public class Student : BaseEntity
    {
        public int InstituteId { get; set; }

        public string FullName { get; set; } = string.Empty;
        public string AdmissionNumber { get; set; } = string.Empty;

        public string? Email { get; set; }
        public string Mobile { get; set; } = string.Empty;

        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }

        public Institute Institute { get; set; } = null!;
    }
}
