using AuthService.API.Security;
using AuthService.Application.DTOs;
using AuthService.Application.User.Events;
using AuthService.Infrastructure.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shared.Infrastructure.Messaging.Outbox;
using Shared.Infrastructure.Messaging.Outbox.Repository;
using Shared.Infrastructure.Messaging.RabbitMQ;
using UserEntity = AuthService.Domain.Users.User;

namespace AuthService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IJwtTokenGenerator _tokenGenerator;
        private readonly IUserRepository _userRepository;
        private readonly IOutboxRepository _outboxRepository;
        private readonly IMediator _mediator;   

        public AuthController(
            IJwtTokenGenerator tokenGenerator, 
            IEventPublisher eventPublisher,
            IUserRepository userRepository,
            IOutboxRepository outboxRepository,
            IMediator mediator
            )
        {
            _tokenGenerator = tokenGenerator;
            _userRepository = userRepository;
            _outboxRepository = outboxRepository;
            _mediator = mediator;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var token = _tokenGenerator.GenerateToken(request.Username, "User");
            //_eventPublisher.Publish(new UserLoggedInIntegrationEvent(request.Username));
            return Ok(new { token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var user = UserEntity.Create(request.Username, request.Email, request.Password);

            //TODO: transaction scope in CQRS handler
            await _userRepository.SaveAsync(user);

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

            return Ok(user.Id);
        }
    }
}
