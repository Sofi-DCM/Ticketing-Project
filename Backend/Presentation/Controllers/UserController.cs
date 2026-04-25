namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ICreateUserHandler _createUserHandler;
        private readonly IGetUserByIdHandler _getUserByIdHandler;

        public UserController(
            ICreateUserHandler createUserHandler, 
            IGetUserByIdHandler getUserByIdHandler)
        {
            _createUserHandler=createUserHandler;
            _getUserByIdHandler=getUserByIdHandler;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
        {
            var userCreatedId = await _createUserHandler.HandleAsync(command);

            return CreatedAtAction(nameof(GetUserById), new { id = userCreatedId }, new { id = userCreatedId });
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id) 
        {
            var response = await _getUserByIdHandler.HandleAsync(id);

            return Ok(response);
        }


    }
}
