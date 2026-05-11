using Application.UseCase._Reservation.Commands.ConfirmPayment;

namespace Application.Interfaces.Handlers._Reservation
{
    public interface IConfirmPaymentHandler
    {
        Task HandleAsync(ConfirmPaymentRequest request, CancellationToken ct);
    }
}
