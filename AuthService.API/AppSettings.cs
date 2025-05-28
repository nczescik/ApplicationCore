namespace AuthService.Settings
{
    public class AuthSettings
    {
        public required string JwtSecret { get; set; }
        public int TokenExpirationMinutes { get; set; }
    }

    public class JwtSettings
    {
        public required string Key { get; set; }
        public required string Issuer { get; set; }
        public required string Audience { get; set; }
        public int ExpirationMinutes { get; set; } = 60;
    }
}