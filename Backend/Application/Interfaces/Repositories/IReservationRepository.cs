
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IReservationRepository
    {
        public Task<Guid> InsertReservationAsync(Reservation reservation, CancellationToken ct);

        Task<ICollection<Reservation>> GetExpiredPendingReservationsAsync(CancellationToken ct);

        Task UpdateReservationAsync(Reservation reservation, CancellationToken ct);
    }
}
