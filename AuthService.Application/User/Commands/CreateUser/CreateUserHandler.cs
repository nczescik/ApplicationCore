using MediatR;
using Newtonsoft.Json;
using Shared.Infrastructure.Messaging.Outbox;
using Shared.Infrastructure.Messaging.Outbox.Repository;
using AuthService.Application.User.Events;
using AuthService.Application.User.Commands.CreateUser;
using AuthService.Infrastructure.Persistance.Users;

namespace AuthService.Application.Users.Commands.CreateUser
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly IUserRepository _userRepository;
        private readonly IOutboxRepository _outboxRepository;

        public CreateUserHandler(
            IUserRepository userRepository,
            IOutboxRepository outboxRepository
            )
        {
            _userRepository = userRepository;
            _outboxRepository = outboxRepository;
        }

        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = Domain.Users.User.Create(request.Username, request.Email, request.Password);

            //TODO: transaction scope
            await _userRepository.SaveAsync(user, cancellationToken);

            var integrationEvent = new UserRegisteredIntegrationEvent(user.Username);
            var outboxMessage = new OutboxMessage
            {
                Id = Guid.NewGuid(),
                EventType = integrationEvent.GetType().AssemblyQualifiedName!,
                Data = JsonConvert.SerializeObject(integrationEvent),
                OccurredOn = DateTime.UtcNow
            };

            await _outboxRepository.AddAsync(outboxMessage);
            await _outboxRepository.SaveAsync();
            // end of transaction scope

            return user.Id;
        }
    }
}
