
namespace Application.Interfaces.Handlers._Reservation
{
    public interface IExpireReservationsHandler
    {
        Task HandleAsync(CancellationToken ct);
    }
}
