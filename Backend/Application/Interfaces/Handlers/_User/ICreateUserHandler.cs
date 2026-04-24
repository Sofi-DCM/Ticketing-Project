using Application.Response;
using Application.UseCase._User.Commands.CreateUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Handlers._User
{
    public interface ICreateUserHandler
    {
        public Task<int> HandleAsync(CreateUserCommand command);
    }
}
