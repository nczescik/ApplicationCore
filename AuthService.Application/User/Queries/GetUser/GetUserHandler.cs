using AuthService.Infrastructure.Persistance.Users;
using MediatR;

namespace AuthService.Application.User.Queries.GetUser
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDto>
    {
        private readonly IUserRepository _userRepository;

        public GetUserQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {request.UserId} not found.");

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email
            };
        }
    }
}
