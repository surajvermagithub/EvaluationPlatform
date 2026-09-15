using System.ComponentModel.DataAnnotations;

namespace CheckMate.Application.DTOs.Institute
{
    public class CreateInstituteRequest
    {
        [Required(ErrorMessage = "Institute name is required.")]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Institute code is required.")]
        [MaxLength(50)]
        public string Code { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mobile is required.")]
        public string Mobile { get; set; } = string.Empty;

        [Required(ErrorMessage = "Address line 1 is required.")]
        [MaxLength(250)]
        public string AddressLine1 { get; set; } = string.Empty;

        [MaxLength(250)]
        public string? AddressLine2 { get; set; }

        [Required(ErrorMessage = "City is required.")]
        [MaxLength(100)]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "State is required.")]
        [MaxLength(100)]
        public string State { get; set; } = string.Empty;

        [Required(ErrorMessage = "Country is required.")]
        [MaxLength(100)]
        public string Country { get; set; } = string.Empty;

        [Required(ErrorMessage = "Pin code is required.")]
        [MaxLength(20)]
        public string PinCode { get; set; } = string.Empty;
    }
}
