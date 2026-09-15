using System.ComponentModel.DataAnnotations;

namespace CheckMate.Application.DTOs.Institute
{
    public class CreateInstituteAdminRequest
    {
        [Required]
        public int InstituteId { get; set; }

        [Required(ErrorMessage = "Full name is required.")]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mobile is required.")]
        public string Mobile { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8)]
        public string Password { get; set; } = string.Empty;
    }
}
