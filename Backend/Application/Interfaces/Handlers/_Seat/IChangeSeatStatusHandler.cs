
namespace Application.Interfaces.Handlers._Seat
{
    public interface IChangeSeatStatusHandler
    {
        Task HandleAsync(Guid seatId, CancellationToken ct);
    }
}
