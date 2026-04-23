using Application.DTOs;
using Application.Interfaces;
using Application.UseCase.Reservations.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCase.Reservations.Handlers
{
    public class CreateReservationCommandHandler
    {
        private readonly IReservationRepository _reservationRepository;
        public CreateReservationCommandHandler(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }
        public async Task<ReservationResponseDto> Handle(CreateReservationCommand command, CancellationToken cancellationToken)
        {
            return await _reservationRepository.CreateReservationAsync(command, cancellationToken);
        }
    }
}
