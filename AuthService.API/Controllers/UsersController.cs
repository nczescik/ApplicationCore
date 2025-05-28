using AuthService.API.Security;
using AuthService.Infrastructure.Users;
using Microsoft.AspNetCore.Mvc;
using Shared.Infrastructure.Messaging.RabbitMQ;

namespace AuthService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IJwtTokenGenerator _tokenGenerator;
        private readonly IEventPublisher _eventPublisher;
        private readonly IUserRepository _userRepository;

        public UsersController(
            IJwtTokenGenerator tokenGenerator,
            IEventPublisher eventPublisher,
            IUserRepository userRepository
            )
        {
            _tokenGenerator = tokenGenerator;
            _eventPublisher = eventPublisher;
            _userRepository = userRepository;
        }

        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }
    }
}
