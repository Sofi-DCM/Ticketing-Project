
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        //-------- Commands --------
        public Task<int> InsertUserAsync(User user);
        public Task<bool> ExistsByNameAsync(string name);
        public Task<bool> ExistsByEmailAsync(string email);

        //-------- Queries --------
        public Task<User?> GetUserById(int id);
        public Task<User?> GetUserByNameAsync(string name);
        Task<bool> ExistsByIdAsync(int userId, CancellationToken ct = default);
    }
}
