using BuildingBlocks.Application.Common;

namespace AuthService.Application.Events
{
    public record UserRegisteredIntegrationEvent(string username) : IIntegrationEvent
    {
        public DateTime OccuredOn { get; } = DateTime.Now;
    }
}
