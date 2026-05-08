
using Application.Interfaces.Repositories;
using Application.Response;
using Domain.Constants;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly AppDbContext _context;
        public ReservationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> InsertReservationAsync(Reservation reservation, CancellationToken ct)
        {
            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync(ct);

            return reservation.Id;
        }

        public async Task<ICollection<ReservationExpiredInfo>> GetExpiredPendingReservationsAsync(CancellationToken ct)
        {
            return await _context.Reservations
                .Where(r => r.Status == ReservationConstants.Pending
                            && r.ExpiresAt <= DateTime.UtcNow)
                .Select(r => new ReservationExpiredInfo { Id = r.Id, UserId = r.UserId, SeatId = r.SeatId })
                .ToListAsync(ct);
        }

        public async Task ExpireReservationsAsync(IEnumerable<Guid> ids, CancellationToken ct) 
        {
            await _context.Reservations
                .Where(r => ids.Contains(r.Id))
                .ExecuteUpdateAsync(setters => setters.SetProperty(r => r.Status, ReservationConstants.Expired), ct);
        }
    }
}
