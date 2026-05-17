
using Application.Interfaces.Repositories;
using Application.Response;
using Application.UseCase._User.Queries.GetUserById;
using Azure.Core;
using Domain.Constants;
using Domain.Entities;
using Domain.Exceptions;
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

        public async Task<Reservation?> GetByIdWithSeatAsync(Guid reservationId, CancellationToken ct)
        {
            return await _context.Reservations
                .Include(r => r.Seat)
                    .ThenInclude(s => s.Sector)
                .FirstOrDefaultAsync(
                    r => r.Id == reservationId,
                    ct);
        }

        public async Task<IList<UserReservationResponse>> GetReservationsByUserIdAsync(int userId, CancellationToken ct)
        {
            return await _context.Reservations
                .Where(r => r.UserId == userId && r.ExpiresAt > DateTime.UtcNow && r.Status == ReservationConstants.Pending)
                .Select(r => new UserReservationResponse { 
                    ReservationId = r.Id,
                    SeatName = r.Seat.RowIdentifier + r.Seat.SeatNumber,
                    EventName = r.Seat.Sector.Event.Name,
                    SectorName = r.Seat.Sector.Name,
                    SectorPrice = r.Seat.Sector.Price,
                    ExpiresAt = r.ExpiresAt
                }).ToListAsync(ct);
        }

        public async Task<Guid?> CancelReservationAsync(Guid id, int userId, CancellationToken ct)
        {
            var reservation = await _context.Reservations.FirstOrDefaultAsync(r => r.Id == id);
            if (reservation != null)
            {
                if (reservation.UserId != userId)
                    throw new UnauthorizedException("La reserva no pertenece a ese usuario");

                reservation.Status = ReservationConstants.Expired;
                await _context.SaveChangesAsync();
                return reservation.SeatId;
            }
            return null;
        }
    }
}
