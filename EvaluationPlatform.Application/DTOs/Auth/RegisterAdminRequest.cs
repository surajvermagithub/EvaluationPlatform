namespace CheckMate.Application.DTOs.Auth
{
    public class RegisterAdminRequest
    {
        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Mobile { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}
