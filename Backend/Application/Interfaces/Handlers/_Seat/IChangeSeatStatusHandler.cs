
using Application.UseCase._Seat.Commands.ChangeSeatStatus;

namespace Application.Interfaces.Handlers._Seat
{
    public interface IChangeSeatStatusHandler
    {
        Task HandleAsync(ChangeSeatStatusCommand command, CancellationToken ct);
    }
}
