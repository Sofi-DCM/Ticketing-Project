using Application.UseCase.Reservations.Commands;
using Application.UseCase.Reservations.Handlers;
using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : ControllerBase
    {
        private readonly CreateReservationCommandHandler _createReservationCommandHandler;

        public ReservationsController(CreateReservationCommandHandler createReservationCommandHandler)
        {
            _createReservationCommandHandler = createReservationCommandHandler;
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateReservationCommand command,
            CancellationToken cancellationToken)
        {
            try
            {
                var result = await _createReservationCommandHandler.Handle(command, cancellationToken);

                return Created(string.Empty, result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ConflictException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
    }
}
