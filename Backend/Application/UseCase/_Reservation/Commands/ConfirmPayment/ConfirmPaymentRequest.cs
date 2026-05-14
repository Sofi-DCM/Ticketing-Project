
namespace Application.UseCase._Reservation.Commands.ConfirmPayment
{
    public class ConfirmPaymentRequest
    {
        public Guid ReservationId { get; set; }
        public int UserId { get; set; }
    }
}
