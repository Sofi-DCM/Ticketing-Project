using Application.Interfaces.Handlers._AuditLog;
using Application.Interfaces.Handlers._User;
using Application.Interfaces.Repositories;
using Application.UseCase._AuditLog.Commands.CreateAuditLog;
using BCrypt.Net;
using Domain.Entities;
using Domain.Constants;
using System.Data;
using System.Text.Json;

namespace Application.UseCase._User.Commands.CreateUser
{
    public class CreateUserHandler : ICreateUserHandler
    {
        private readonly IUserRepository _repository;
        private readonly ICreateAuditLogHandler _createAuditLogHandler;

        public CreateUserHandler(IUserRepository repository, ICreateAuditLogHandler createAuditLogHandler)
        {
            _repository=repository;
            _createAuditLogHandler=createAuditLogHandler;
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

            await _createAuditLogHandler.HandleAsync(MapToAuditLogCommand(IdUserCreated, command));

            return IdUserCreated;
        }

        private static CreateAuditLogCommand MapToAuditLogCommand(int userId, CreateUserCommand cmd)
        {
            return new CreateAuditLogCommand
            {
                UserId = userId,
                Action = AuditLogConstants.Actions.CreateUser,
                EntityType = AuditLogConstants.Entities.User,
                EntityId = userId.ToString(),
                Details = JsonSerializer.Serialize(new { cmd.Name, cmd.Email })
            };
        }
    }
}
