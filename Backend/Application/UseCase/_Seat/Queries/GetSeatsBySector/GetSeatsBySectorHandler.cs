using Application.Interfaces.Handlers._Seat;
using Application.Interfaces.Repositories;
using Application.Response;

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
