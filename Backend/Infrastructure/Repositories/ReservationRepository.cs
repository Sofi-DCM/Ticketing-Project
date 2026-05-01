
using Application.Interfaces.Repositories;
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

        public async Task<ICollection<Reservation>> GetExpiredPendingReservationsAsync(CancellationToken ct)
        {
            return await _context.Reservations
                .Where(r => r.Status == ReservationConstants.Pending
                            && r.ExpiresAt <= DateTime.UtcNow)
                .ToListAsync(ct);
        }

        public async Task UpdateReservationAsync(Reservation reservation, CancellationToken ct)
        {
            _context.Reservations.Update(reservation);
            await _context.SaveChangesAsync(ct);
        }
    }
}
