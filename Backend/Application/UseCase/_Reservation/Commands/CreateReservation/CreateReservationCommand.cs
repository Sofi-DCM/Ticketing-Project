
namespace Application.UseCase._Reservation.Commands.CreateReservation
{
    public class CreateReservationCommand
    {
        public int UserId { get; set; }
        public Guid SeatId { get; set; }
    }
}
