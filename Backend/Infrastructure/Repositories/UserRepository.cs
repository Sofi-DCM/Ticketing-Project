using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


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

        public async Task<User?> GetUserByNameAsync(string name) {
            return await _context.Users.FirstOrDefaultAsync(u => u.Name == name);
        }

        public async Task<bool> ExistsByIdAsync(int userId, CancellationToken ct = default)
        {
            return await _context.Users
                .AnyAsync(u => u.Id == userId, ct);
        }
    }
}
