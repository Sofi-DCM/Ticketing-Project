using Application.Interfaces.Handlers._User;
using Application.UseCase._User.Queries.ValidateUserCredentials;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace Presentation.Controllers
{
    [Route("api/v1/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ICreateUserHandler _createUserHandler;
        private readonly IGetUserByIdHandler _getUserByIdHandler;
        private readonly IValidateUserCredentialsHandler _validateUserCredentialsHandler;

        public UserController(
            ICreateUserHandler createUserHandler, 
            IGetUserByIdHandler getUserByIdHandler, 
            IValidateUserCredentialsHandler validateUserCredentialsHandler)
        {
            _createUserHandler=createUserHandler;
            _getUserByIdHandler=getUserByIdHandler;
            _validateUserCredentialsHandler=validateUserCredentialsHandler;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
        {
            var userCreatedId = await _createUserHandler.HandleAsync(command);

            return CreatedAtAction(nameof(GetUserById), new { id = userCreatedId }, new { id = userCreatedId });
        }

        [HttpGet("validate")]
        public async Task<IActionResult> ValidateUserCredentials(string name, string password)
        {
            var userId = await _validateUserCredentialsHandler.HandleAsync(name, password);

            return Ok(userId);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id) 
        {
            var response = await _getUserByIdHandler.HandleAsync(id);

            return Ok(response);
        }


    }
}
