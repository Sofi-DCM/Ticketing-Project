
using Application.UseCase._Seat.Commands.ChangeSeatStatus;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface ISeatRepository
    {
        public Task<bool> ExistsByIdAsync(Guid seatId);
        public Task PatchSeatStateAsync(ChangeSeatStatusCommand command, CancellationToken ct);
        Task<ICollection<Seat>> GetSeatsBySectorAsync(int sectorId, bool onlyRow, CancellationToken ct = default);
        Task ReleaseSeatsAsync(IEnumerable<Guid> seatsIds, CancellationToken ct);
        Task<bool> SectorExistsAsync(int sectorId, CancellationToken ct);
        public Task CreateSeatsAsync(ICollection<Seat> seats);
    }
}
