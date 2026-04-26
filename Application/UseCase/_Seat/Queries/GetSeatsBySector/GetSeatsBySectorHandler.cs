using Application.Interfaces.Handlers._Seat;
using Application.Interfaces.Repositories;
using Application.Response;
using Domain.Exceptions;

namespace Application.UseCase._Seat.Queries.GetSeatsBySector
{
    public class GetSeatsBySectorHandler : IGetSeatsBySectorHandler
    {
        private readonly ISeatRepository _repository;

        public GetSeatsBySectorHandler(ISeatRepository repository)
        {
            _repository = repository;
        }
        public async Task<List<SeatStatusDto>> HandleAsync(int sectorId, CancellationToken ct = default)
        {
            var exists = await _repository.SectorExistsAsync(sectorId, ct);
            if (!exists)
                throw new NotFoundException($"El sector {sectorId} no existe.");

            var seats = await _repository.GetSeatsBySectorAsync(sectorId, ct);

            return seats.Select(s => new SeatStatusDto
            {
                SectorId = s.Id,
                RowIdentifier = s.RowIdentifier,
                SeatNumber = s.SeatNumber,
                Status = s.Status
            }).ToList();
        }
    }
}
