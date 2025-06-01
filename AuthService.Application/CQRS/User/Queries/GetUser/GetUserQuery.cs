using MediatR;

namespace AuthService.Application.CQRS.User.Queries.GetUser
{
    public class GetUserQuery : IRequest<UserDto>
    {
        public Guid UserId { get; set; }
    }
}