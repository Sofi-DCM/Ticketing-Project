
using Application.Interfaces.Handlers._Seat;
using Application.Interfaces.Repositories;
using Application.Response;
using Domain.Exceptions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Application.UseCase._Seat.Queries.GetSeatsBySector
{
    public class GetSeatsBySectorHandler : IGetSeatsBySectorHandler
    {
        private readonly ISeatRepository _repository;

        public GetSeatsBySectorHandler(ISeatRepository repository)
        {
            _repository = repository;
        }
        public async Task<List<SeatStatusDto>> HandleAsync(int sectorId, bool? onlyRow, CancellationToken ct = default)
        {
            if (sectorId <= 0)
                throw new ArgumentException("Los id deben ser positivos");

            var exists = await _repository.SectorExistsAsync(sectorId, ct);
            if (!exists)
                throw new NotFoundException($"El sector {sectorId} no existe.");

            bool filterOnlyRow = onlyRow ?? false;

            var seats = await _repository.GetSeatsBySectorAsync(sectorId, filterOnlyRow, ct);

            return seats.Select(s => new SeatStatusDto
            {
                Id = s.Id,
                RowIdentifier = s.RowIdentifier,
                SeatNumber = s.SeatNumber,
                Status = s.Status
            }).ToList();
        }
    }
}
