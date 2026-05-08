
using Application.Response;

namespace Application.Interfaces.Handlers._User
{
    public interface IGetUserByIdHandler
    {
        public Task<UserResponse> HandleAsync(int id);
    }
}
