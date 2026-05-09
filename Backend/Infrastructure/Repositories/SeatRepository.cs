
using Application.Interfaces.Repositories;
using Domain.Constants;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class SeatRepository : ISeatRepository
    {
        private readonly AppDbContext _context;
        public SeatRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistsByIdAsync(Guid seatId) =>
            await _context.Seats.AnyAsync(s => s.Id == seatId);

        public async Task<ICollection<Seat>> GetSeatsBySectorAsync(int sectorId, bool onlyRow, CancellationToken ct = default)
        {
            return await _context.Seats
                .AsNoTracking()
                .Where(s => s.SectorId == sectorId)
                .Where(s => onlyRow == false || s.RowIdentifier == "A")
                .OrderBy(s => s.RowIdentifier)
                .ThenBy(s => s.SeatNumber)
                .ToListAsync(ct);
        }

        public async Task PatchSeatStateAsync(Guid seatId, CancellationToken ct)
        {
            var seat = await _context.Seats
                .FirstOrDefaultAsync(s => s.Id == seatId && s.Status == SeatStatusConstants.Available, ct);

            if(seat == null) throw new DbUpdateConcurrencyException();

            seat.Status = SeatStatusConstants.Reserved;
            seat.Version++;

            await _context.SaveChangesAsync();
        }

        public async Task ReleaseSeatsAsync(IEnumerable<Guid> seatsIds, CancellationToken ct)
        {
            if (seatsIds == null || !seatsIds.Any()) return;
            await _context.Seats
                .Where(s => seatsIds.Contains(s.Id) && s.Status == SeatStatusConstants.Reserved)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(s => s.Status, SeatStatusConstants.Available)
                    .SetProperty(s => s.Version, s => s.Version + 1), ct);
        }

        public async Task<bool> SectorExistsAsync(int sectorId, CancellationToken ct)
        {
            return await _context.Sectors
                .AnyAsync(s => s.Id == sectorId, ct);
        }

        public async Task CreateSeatsAsync(ICollection<Seat> seats) 
        {
            await _context.Seats.AddRangeAsync(seats);
            await _context.SaveChangesAsync();
        }

    }
}
