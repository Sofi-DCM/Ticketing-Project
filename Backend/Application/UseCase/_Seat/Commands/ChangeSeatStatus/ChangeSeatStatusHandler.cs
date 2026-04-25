using Application.Interfaces.Handlers._Seat;
using Application.Interfaces.Repositories;

namespace Application.UseCase._Seat.Commands.ChangeSeatStatus
{
    public class ChangeSeatStatusHandler : IChangeSeatStatusHandler
    {
        // InvalidOperationException lanzar si el asiento no es available
        private readonly ISeatRepository _repository;

        public ChangeSeatStatusHandler(ISeatRepository repository)
        {
            _repository = repository;
        }

        public async Task HandleAsync(Guid seatId, CancellationToken ct)
        {
            if (!await _repository.ExistsByIdAsync(seatId))
                throw new KeyNotFoundException($"No existe un asiento con id : {seatId}");

            if (!await _repository.PatchSeatStateAsync(seatId, ct))
                throw new InvalidOperationException($"El asiento con Id : {seatId} ya esta reservado");
        }
    }
}
