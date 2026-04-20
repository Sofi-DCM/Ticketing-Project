using Application.Interfaces.Handlers._User;
using Application.Interfaces.Repositories;
using BCrypt.Net;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCase._User.Commands.CreateUser
{
    public class CreateUserHandler : ICreateUserHandler
    {
        private readonly IUserRepository _repository;

        public CreateUserHandler(IUserRepository repository)
        {
            _repository=repository;
        }

        public async Task<int> HandleAsync(CreateUserCommand command)
        {
            if (await _repository.ExistsByNameAsync(command.Name))
                throw new DuplicateNameException("Ya existe un usuario con ese nombre");

            if (await _repository.ExistsByEmailAsync(command.Email))
                throw new DuplicateNameException("Ya existe un usuario con ese email");

            var newUser = new User 
            {
                Name = command.Name,
                Email = command.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(command.PasswordHash),
            };

            var IdUserCreated = await _repository.InsertUserAsync(newUser);

            return IdUserCreated;
        }
    }
}
