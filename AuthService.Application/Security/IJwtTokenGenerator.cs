namespace AuthService.Infrastructure.Security.JWT
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(string username, string[] role);
    }
}
