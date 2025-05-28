using BuildingBlocks.Application.Common;

namespace AuthService.Application.Events
{
    public record UserLoggedInIntegrationEvent(string Username) : IIntegrationEvent
    {
        public DateTime OccuredOn { get; } = DateTime.UtcNow;
    }
}
