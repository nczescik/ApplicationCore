namespace AuthService.Application.Events
{
    public record UserLoggedInEvent(string Username, DateTime Timestamp);
}
