using Application.Response;
using Application.UseCase._Reservation.Commands.CreateReservation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Handlers._Reservation
{
    public interface ICreateReservationHandler
    {
        public Task<ReservationResponseDto> HandleAsync(CreateReservationCommand command, CancellationToken cancellationToken);
    }
}
