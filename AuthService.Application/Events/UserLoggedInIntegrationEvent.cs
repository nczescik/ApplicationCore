using BuildingBlocks.Application;

namespace AuthService.Application.Events
{
    public record UserLoggedInIntegrationEvent(string Username, DateTime Timestamp) : IIntegrationEvent
    {
        public DateTime OccuredOn { get; } = DateTime.Now;
    }
}
