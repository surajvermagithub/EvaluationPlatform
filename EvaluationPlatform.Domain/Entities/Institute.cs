using CheckMate.Domain.Common;

namespace CheckMate.Domain.Entities
{
    public class Institute : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Mobile { get; set; } = string.Empty;

        public string AddressLine1 { get; set; } = string.Empty;

        public string? AddressLine2 { get; set; }

        public string City { get; set; } = string.Empty;

        public string State { get; set; } = string.Empty;

        public string Country { get; set; } = string.Empty;

        public string PinCode { get; set; } = string.Empty;

        // Navigation Property
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
