﻿using AuthService.Application.CQRS.User.Events;
using MediatR;
using Newtonsoft.Json;
using Shared.Application.Messaging.Outbox;
using System.Transactions;

namespace AuthService.Application.CQRS.User.Commands.CreateUser
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

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                await _userRepository.SaveAsync(user, cancellationToken);

                var integrationEvent = new UserRegisteredIntegrationEvent(user.Username);
                var outboxMessage = new OutboxMessage
                {
                    Id = Guid.NewGuid(),
                    EventType = integrationEvent.GetType().AssemblyQualifiedName!,
                    Data = JsonConvert.SerializeObject(integrationEvent),
                    OccurredOn = DateTime.UtcNow
                };

                await _outboxRepository.AddAsync(outboxMessage, cancellationToken);
                await _outboxRepository.SaveAsync(cancellationToken);

                scope.Complete();
            }

            return user.Id;
        }
    }
}
