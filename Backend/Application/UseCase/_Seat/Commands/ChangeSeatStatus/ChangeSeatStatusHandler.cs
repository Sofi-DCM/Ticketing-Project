
using Application.Interfaces.Handlers._Seat;
using Application.Interfaces.Repositories;

namespace Application.UseCase._Seat.Commands.ChangeSeatStatus
{
    public class ChangeSeatStatusHandler : IChangeSeatStatusHandler
    {
        private readonly ISeatRepository _repository;

        public ChangeSeatStatusHandler(ISeatRepository repository)
        {
            _repository = repository;
        }

        public async Task HandleAsync(ChangeSeatStatusCommand command, CancellationToken ct)
        {
            if (!await _repository.ExistsByIdAsync(command.SeatId))
                throw new KeyNotFoundException($"No existe un asiento con id : {command.SeatId}");

            await _repository.PatchSeatStateAsync(command, ct);
        }
    }
}
