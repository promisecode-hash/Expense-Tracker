namespace ExpenseTracker.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(Guid userId, string email, string username);
        bool ValidateToken(string token);
    }

    public interface IPasswordService
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string hash);
    }
}
