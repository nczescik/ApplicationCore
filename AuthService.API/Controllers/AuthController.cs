using AuthService.API.Security;
using AuthService.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using AuthService.Application.Events;
using Shared.Infrastructure.Messaging.RabbitMQ;

namespace AuthService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IJwtTokenGenerator _tokenGenerator;
        private readonly IEventPublisher _eventPublisher;

        public AuthController(IJwtTokenGenerator tokenGenerator, IEventPublisher eventPublisher)
        {
            _tokenGenerator = tokenGenerator;
            _eventPublisher = eventPublisher;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var token = _tokenGenerator.GenerateToken(request.Username, "User");
            _eventPublisher.Publish(new UserLoggedInEvent(request.Username, DateTime.UtcNow));
            return Ok(new { token });
        }
    }
}
