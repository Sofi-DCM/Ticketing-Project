namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/v1/reservations")]
    public class ReservationsController : ControllerBase
    {
        private readonly ICreateReservationHandler _createReservationHandler;

        public ReservationsController(ICreateReservationHandler createReservationHandler)
        {
            _createReservationHandler = createReservationHandler;
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateReservationCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _createReservationHandler.HandleAsync(command, cancellationToken);

            return Created(string.Empty, result);
        }
    }
}