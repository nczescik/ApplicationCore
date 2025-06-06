﻿using AuthService.Application.CQRS.User.Events;
using AuthService.Infrastructure.Security.JWT;
using MediatR;
using Newtonsoft.Json;
using Shared.Application.Messaging.Outbox;

namespace AuthService.Application.CQRS.Authentication.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, string>
    {
        private readonly IJwtTokenGenerator _tokenGenerator;
        private readonly IOutboxRepository _outboxRepository;

        public LoginCommandHandler(
            IJwtTokenGenerator tokenGenerator,
            IOutboxRepository outboxRepository
            )
        {
            _tokenGenerator = tokenGenerator;
            _outboxRepository = outboxRepository;
        }

        public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            //TODO: get user from Projection
            var token = _tokenGenerator.GenerateToken(request.Username, ["User"]);
            var integrationEvent = new UserLoggedInIntegrationEvent(request.Username);
            var outboxMessage = new OutboxMessage
            {
                Id = Guid.NewGuid(),
                EventType = integrationEvent.GetType().AssemblyQualifiedName!,
                Data = JsonConvert.SerializeObject(integrationEvent),
                OccurredOn = DateTime.UtcNow
            };
            await _outboxRepository.AddAsync(outboxMessage, cancellationToken);
            await _outboxRepository.SaveAsync(cancellationToken);

            return token;
        }
    }
}
