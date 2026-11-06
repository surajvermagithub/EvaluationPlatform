namespace CheckMate.Application.DTOs.Students
{
    public class StudentResponse
    {
        public int Id { get; set; }
        public int InstituteId { get; set; }

        public string FullName { get; set; } = string.Empty;
        public string AdmissionNumber { get; set; } = string.Empty;

        public string? Email { get; set; }
        public string Mobile { get; set; } = string.Empty;

        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }

        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
