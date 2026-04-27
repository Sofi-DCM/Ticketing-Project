using Application.Response;
using Application.UseCase._Reservation.Commands.CreateReservation;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IReservationRepository
    {
        public Task<Guid> InsertReservationAsync(Reservation reservation, CancellationToken ct);

        Task<ICollection<Reservation>> GetExpiredPendingReservationsAsync(CancellationToken ct);

        Task UpdateReservationAsync(Reservation reservation, CancellationToken ct);
    }
}
