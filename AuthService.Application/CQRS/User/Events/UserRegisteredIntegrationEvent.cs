using BuildingBlocks.Application.Common;

namespace AuthService.Application.CQRS.User.Events
{
    public record UserRegisteredIntegrationEvent(string username) : IIntegrationEvent
    {
        public DateTime OccuredOn { get; } = DateTime.UtcNow;
    }
}
