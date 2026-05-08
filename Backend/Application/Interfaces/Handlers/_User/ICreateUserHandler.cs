
using Application.UseCase._User.Commands.CreateUser;

namespace Application.Interfaces.Handlers._User
{
    public interface ICreateUserHandler
    {
        public Task<int> HandleAsync(CreateUserCommand command);
    }
}
