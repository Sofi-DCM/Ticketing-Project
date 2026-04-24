using Application.Interfaces.Handlers._AuditLog;
using Application.Interfaces.Handlers._User;
using Application.Interfaces.Repositories;
using Domain.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCase._User.Queries.ValidateUserCredentials
{
    public class ValidateUserCredentialsHandler : IValidateUserCredentialsHandler
    {
        private readonly IUserRepository _repository;

        public ValidateUserCredentialsHandler(IUserRepository repository, ICreateAuditLogHandler createAuditLogHandler)
        {
            _repository=repository;
        }

        public async Task<int> HandleAsync(string name, string password)
        {
            var user = await _repository.GetUserByNameAsync(name)
                ??throw new UnauthorizedException("Credenciales invalidas");

            bool isValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);

            if (!isValid) { throw new UnauthorizedException("Credenciales invalidas"); }

            return user.Id;
        }
    }
}
