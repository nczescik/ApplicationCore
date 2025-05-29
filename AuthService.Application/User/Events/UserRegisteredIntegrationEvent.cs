using BuildingBlocks.Application.Common;

namespace AuthService.Application.User.Events
{
    public record UserRegisteredIntegrationEvent(string username) : IIntegrationEvent
    {
        public DateTime OccuredOn { get; } = DateTime.UtcNow;
    }
}
