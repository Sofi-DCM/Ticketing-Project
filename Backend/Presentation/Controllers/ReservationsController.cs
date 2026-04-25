using Application.UseCase._Reservation.Commands.CreateReservation;
using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
