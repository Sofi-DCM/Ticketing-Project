
namespace Application.UseCase._User.Commands.CreateUser
{
    public class CreateUserCommand
    {
        public string Name { get; set; } 
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
}
