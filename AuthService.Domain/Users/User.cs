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

        public User() { }

        public static User Create(string username, string email, string password)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = username,
                Email = email,
                //PasswordHash = passwordHash
            };

            user.RaiseEvent(new UserCreatedDomainEvent(user.Id, user.Username, user.Email, user.PasswordHash, DateTime.UtcNow));
            return user;
        }

        protected override void Apply(IDomainEvent @event)
        {
            switch (@event)
            {
                case UserCreatedDomainEvent e:
                    Id = e.UserId;
                    Email = e.Email;
                    Username = e.Username;
                    PasswordHash = e.PasswordHash;
                    RegisteredAt = e.RegisteredAt;
                    break;
            }
        }
    }
}