using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// oficia como DtoRequest para la creacion de usuarios
namespace Application.UseCase._User.Commands.CreateUser
{
    public class CreateUserCommand
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
}
