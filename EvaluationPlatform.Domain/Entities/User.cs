using CheckMate.Domain.Common;

namespace CheckMate.Domain.Entities
{
    public class User : BaseEntity
    {
        public int? InstituteId { get; set; }

        public int RoleId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Mobile { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        // Navigation Properties
        public Institute? Institute { get; set; }

        public Role Role { get; set; } = null!;
    }
}
