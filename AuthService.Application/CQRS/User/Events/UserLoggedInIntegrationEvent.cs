using BuildingBlocks.Application.Common;

namespace AuthService.Application.CQRS.User.Events
{
    public record UserLoggedInIntegrationEvent(string Username) : IIntegrationEvent
    {
        public DateTime OccuredOn { get; } = DateTime.UtcNow;
    }
}
