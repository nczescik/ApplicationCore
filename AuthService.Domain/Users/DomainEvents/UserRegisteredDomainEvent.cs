
using BuildingBlocks.Domain.Common;

namespace AuthService.Domain.Users.DomainEvents
{
    public record UserRegisteredDomainEvent(Guid UserId, string Email, string PasswordHash, DateTime RegisteredAt) : IEvent
    {
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
    }
}