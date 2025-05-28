using AuthService.Domain.Users.DomainEvents;
using BuildingBlocks.Domain.Common;

namespace AuthService.Domain.Users
{
    public class User : AggregateRoot
    {
        public string Username { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
        public DateTime RegisteredAt { get; private set; }

        private User() { }

        public static User Register(string email, string passwordHash)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = email,
                PasswordHash = passwordHash
            };
            user.RaiseEvent(new UserRegisteredDomainEvent(user.Id, user.Email, user.PasswordHash, DateTime.UtcNow));
            return user;
        }

        protected override void Apply(IEvent @event)
        {
            switch (@event)
            {
                case UserRegisteredDomainEvent e:
                    Id = e.UserId;
                    Email = e.Email;
                    PasswordHash = e.PasswordHash;
                    RegisteredAt = e.RegisteredAt;
                    break;
            }
        }
    }
}