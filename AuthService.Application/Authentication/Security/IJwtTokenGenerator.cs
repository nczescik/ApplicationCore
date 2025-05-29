namespace AuthService.API.Security
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(string username, string role);
    }
}
