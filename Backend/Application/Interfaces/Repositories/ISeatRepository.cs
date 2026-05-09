
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface ISeatRepository
    {
        public Task<bool> ExistsByIdAsync(Guid seatId);
        public Task PatchSeatStateAsync(Guid seatId, CancellationToken ct);
        Task<ICollection<Seat>> GetSeatsBySectorAsync(int sectorId, bool onlyRow, CancellationToken ct = default);
        Task ReleaseSeatsAsync(IEnumerable<Guid> seatsIds, CancellationToken ct);
        Task<bool> SectorExistsAsync(int sectorId, CancellationToken ct);
        public Task CreateSeatsAsync(ICollection<Seat> seats);
    }
}
