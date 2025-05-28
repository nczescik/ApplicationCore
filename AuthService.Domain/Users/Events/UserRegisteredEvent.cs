
using BuildingBlocks.Domain.Common;

namespace AuthService.Domain.Users.Events
{
    public record UserRegisteredEvent(Guid UserId, string Email, string PasswordHash, DateTime RegisteredAt) : IEvent
    {
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
    }
}