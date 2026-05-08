
using Application.Response;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IReservationRepository
    {
        public Task<Guid> InsertReservationAsync(Reservation reservation, CancellationToken ct);

        Task<ICollection<ReservationExpiredInfo>> GetExpiredPendingReservationsAsync(CancellationToken ct);

        Task ExpireReservationsAsync(IEnumerable<Guid> ids, CancellationToken ct);
    }
}
