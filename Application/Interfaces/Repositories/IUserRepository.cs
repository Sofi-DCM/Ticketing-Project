using Application.UseCase._User.Commands.CreateUser;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
