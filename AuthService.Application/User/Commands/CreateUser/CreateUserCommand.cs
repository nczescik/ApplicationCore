using MediatR;

namespace AuthService.Application.User.Commands.CreateUser
{
    public record CreateUserCommand(string Username, string Email, string Password) : IRequest<Guid>;
}
