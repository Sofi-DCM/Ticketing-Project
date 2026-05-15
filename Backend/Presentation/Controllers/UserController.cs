
namespace Presentation.Controllers
{
    [Route("api/v1/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ICreateUserHandler _createUserHandler;
        private readonly IGetUserByIdHandler _getUserByIdHandler;
        private readonly IValidateUserCredentialsHandler _validateUserCredentialsHandler;
        private readonly IGetUserReservationsByIdHandler _getUserReservationsByIdHandler;

        public UserController(
            ICreateUserHandler createUserHandler,
            IGetUserByIdHandler getUserByIdHandler,
            IValidateUserCredentialsHandler validateUserCredentialsHandler,
            IGetUserReservationsByIdHandler getUserReservationsByIdHandler)
        {
            _createUserHandler=createUserHandler;
            _getUserByIdHandler=getUserByIdHandler;
            _validateUserCredentialsHandler=validateUserCredentialsHandler;
            _getUserReservationsByIdHandler = getUserReservationsByIdHandler;
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

        [HttpGet("{id}/reservations")]
        public async Task<IActionResult> GetUserReservationsById(int id, CancellationToken ct = default)
        {
            var response = await _getUserReservationsByIdHandler.HandleAsync(id, ct);

            return Ok(response);
        }

    }
}
