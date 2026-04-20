using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ICreateUserHandler _createUserHandler;

        public UserController(ICreateUserHandler createUserHandler)
        {
            _createUserHandler=createUserHandler;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
        {
            var userCreatedId = await _createUserHandler.HandleAsync(command);

            return CreatedAtAction(nameof(GetUserById), new { id = userCreatedId }, new { id = userCreatedId });
        }


        [HttpGet("{id}")]
        public IActionResult GetUserById([FromQuery] int id) 
        {
            return new JsonResult(new { user = "user" });
        }


    }
}
