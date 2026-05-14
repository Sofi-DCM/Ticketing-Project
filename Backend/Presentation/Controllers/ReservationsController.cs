using Application.UseCase._Reservation.Commands.ConfirmPayment;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/v1/reservations")]
    public class ReservationsController : ControllerBase
    {
        private readonly ICreateReservationHandler _createReservationHandler;
        private readonly IConfirmPaymentHandler _confirmPaymentHandler;
        public ReservationsController(ICreateReservationHandler createReservationHandler, IConfirmPaymentHandler confirmPaymentHandler)
        {
            _createReservationHandler = createReservationHandler;
            _confirmPaymentHandler = confirmPaymentHandler;
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateReservationCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _createReservationHandler.HandleAsync(command, cancellationToken);

            return Created(string.Empty, result);
        }
        [HttpPost("{reservationId}/payment")]
        public async Task<IActionResult> ConfirmPayment(Guid reservationId,[FromBody] int userId, CancellationToken cancellationToken)
        {
            await _confirmPaymentHandler.HandleAsync(
                new ConfirmPaymentRequest
                {
                    ReservationId = reservationId,
                    UserId = userId
                },
                cancellationToken);

            return Ok(new
            {
                Message = "Pago confirmado correctamente."
            });
        }
    }
}