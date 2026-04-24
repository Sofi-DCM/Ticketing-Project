using Application.Interfaces.Handlers._User;
using Application.Interfaces.Repositories;
using Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCase._User.Queries.GetUserById
{
    public class GetUserByIdHandler : IGetUserByIdHandler
    {
        private readonly IUserRepository _repository;

        public GetUserByIdHandler(IUserRepository repository)
        {
            _repository=repository;
        }

        public async Task<UserResponse> HandleAsync(int id)
        {
            var response = await _repository.GetUserById(id)
                ??throw new KeyNotFoundException($"el usuario con Id : {id} no existe");

            return new UserResponse { Name = response.Name, Email = response.Email,};
        }
    }
}
