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

        public async Task<bool> PatchSeatStateAsync(Guid seatId, CancellationToken ct)
        {
            int rowsAffected = await _context.Seats
                .Where(s => s.Id == seatId && s.Status == SeatStatusConstants.Available)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(s => s.Status, SeatStatusConstants.Reserved), ct);
            // a futuro se busca el where tambien con version y tambien se incrementa

            // Si rowsAffected es 0, significa que el estado no coincidio por eso no se modifico
            return rowsAffected > 0;
        }

        public async Task ReleaseSeatAsync(Guid seatId, CancellationToken ct)
        {
            await _context.Seats
                .Where(s => s.Id == seatId && s.Status == SeatStatusConstants.Reserved)
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
