
using Application.Response;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IReservationRepository
    {
        public Task<Guid> InsertReservationAsync(Reservation reservation, CancellationToken ct);
        Task<ICollection<ReservationExpiredInfo>> GetExpiredPendingReservationsAsync(CancellationToken ct);
        Task ExpireReservationsAsync(IEnumerable<Guid> ids, CancellationToken ct);
        Task<Reservation?> GetByIdWithSeatAsync(Guid reservationId, CancellationToken ct);
        public Task<IList<UserReservationResponse>> GetReservationsByUserIdAsync(int userId, CancellationToken ct);
        public Task<Guid?> CancelReservationAsync(Guid id, int userId, CancellationToken ct);
    }
}
