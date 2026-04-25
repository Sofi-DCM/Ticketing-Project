using Application.Interfaces.Repositories;
using Application.UseCase._User.Commands.CreateUser;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> InsertUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user.Id;
        }

        public async Task<bool> ExistsByNameAsync(string name) =>
            await _context.Users.AnyAsync(u => u.Name == name);

        public async Task<bool> ExistsByEmailAsync(string email) =>
            await _context.Users.AnyAsync(u => u.Email == email);

        public async Task<User?> GetUserById(int id)
        {
           return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
