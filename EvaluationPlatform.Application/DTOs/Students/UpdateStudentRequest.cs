using System.ComponentModel.DataAnnotations;

namespace CheckMate.Application.DTOs.Students
{
    public class UpdateStudentRequest
    {
        [Required]
        [StringLength(150, MinimumLength = 2)]
        public string FullName { get; set; } = string.Empty;

 

        [EmailAddress]
        [StringLength(150)]
        public string? Email { get; set; }

        [Required]
        [RegularExpression(
            @"^\+?[0-9]{7,15}$",
            ErrorMessage = "Enter a valid mobile number.")]
        public string Mobile { get; set; } = string.Empty;

        public DateTime? DateOfBirth { get; set; }

        [StringLength(30)]
        public string? Gender { get; set; }

        [StringLength(500)]
        public string? Address { get; set; }
    }
}
