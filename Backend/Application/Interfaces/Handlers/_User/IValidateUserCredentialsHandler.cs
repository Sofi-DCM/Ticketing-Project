
namespace Application.Interfaces.Handlers._User
{
    public interface IValidateUserCredentialsHandler
    {
        Task<int> HandleAsync(string name, string password);
    }
}
