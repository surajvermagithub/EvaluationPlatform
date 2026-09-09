namespace CheckMate.Application.Interfaces.Services
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);
    }
}
