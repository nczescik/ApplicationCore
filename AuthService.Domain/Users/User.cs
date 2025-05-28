using AuthService.Domain.Users.Events;
using BuildingBlocks.Domain.Common;

namespace AuthService.Domain.Users
{
    public class User : AggregateRoot<Guid>
    {
        public string Username { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
        public DateTime RegisteredAt { get; private set; }

        private User() { }

        public static User Register(Guid id, string email, string passwordHash)
        {
            var user = new User();
            user.RaiseEvent(new UserRegisteredEvent(id, email, passwordHash, DateTime.UtcNow));
            return user;
        }

        protected override void Apply(IEvent @event)
        {
            switch (@event)
            {
                case UserRegisteredEvent e:
                    Id = e.UserId;
                    Email = e.Email;
                    PasswordHash = e.PasswordHash;
                    RegisteredAt = e.RegisteredAt;
                    break;
            }
        }
    }
}