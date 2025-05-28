
using BuildingBlocks.Domain.Common;

namespace AuthService.Domain.Users.DomainEvents
{
    public record UserCreatedDomainEvent(Guid UserId, string Email, string PasswordHash, DateTime RegisteredAt) : IDomainEvent
    {
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
    }
}