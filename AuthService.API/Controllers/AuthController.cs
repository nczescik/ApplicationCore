using AuthService.API.Security;
using AuthService.Application.DTOs;
using AuthService.Application.Events;
using AuthService.Infrastructure.Users;
using Microsoft.AspNetCore.Mvc;
using Shared.Infrastructure.Messaging.RabbitMQ;
using UserEntity = AuthService.Domain.Users.User;

namespace AuthService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IJwtTokenGenerator _tokenGenerator;
        private readonly IEventPublisher _eventPublisher;
        private readonly IUserRepository _userRepository;

        public AuthController(
            IJwtTokenGenerator tokenGenerator, 
            IEventPublisher eventPublisher,
            IUserRepository userRepository
            )
        {
            _tokenGenerator = tokenGenerator;
            _eventPublisher = eventPublisher;
            _userRepository = userRepository;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var token = _tokenGenerator.GenerateToken(request.Username, "User");
            _eventPublisher.Publish(new UserLoggedInIntegrationEvent(request.Username));
            return Ok(new { token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var user = UserEntity.Create(request.Username, request.Email, request.Password);
            await _userRepository.SaveAsync(user);

            _eventPublisher.Publish(new UserRegisteredIntegrationEvent(request.Username));
            return Ok();
        }
    }
}
