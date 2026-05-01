
using Application.Response;
using Application.UseCase._Reservation.Commands.CreateReservation;

namespace Application.Interfaces.Handlers._Reservation
{
    public interface ICreateReservationHandler
    {
        public Task<ReservationResponseDto> HandleAsync(CreateReservationCommand command, CancellationToken cancellationToken);
    }
}
