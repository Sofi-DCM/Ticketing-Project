using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCase._Reservation.Commands.CreateReservation
{
    public class CreateReservationCommand
    {
        public int UserId { get; set; }
        public Guid SeatId { get; set; }
    }
}
