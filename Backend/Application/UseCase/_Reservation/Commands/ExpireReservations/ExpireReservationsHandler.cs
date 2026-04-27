using Application.Interfaces.Handlers._AuditLog;
using Application.Interfaces.Handlers._Reservation;
using Application.Interfaces.Repositories;
using Application.UseCase._AuditLog.Commands.CreateAuditLog;
using Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.UseCase._Reservation.Commands.ExpireReservations
{
    public class ExpireReservationsHandler : IExpireReservationsHandler
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly ISeatRepository _seatRepository;
        private readonly ICreateAuditLogHandler _createAuditLogHandler;

        public ExpireReservationsHandler(
            IReservationRepository reservationRepository,
            ISeatRepository seatRepository,
            ICreateAuditLogHandler createAuditLogHandler)
        {
            _reservationRepository = reservationRepository;
            _seatRepository = seatRepository;
            _createAuditLogHandler = createAuditLogHandler;
        }

        public async Task HandleAsync(CancellationToken ct)
        {
            var expiredReservations =
                await _reservationRepository.GetExpiredPendingReservationsAsync(ct);

            foreach (var reservation in expiredReservations)
            {
                reservation.Status = ReservationConstants.Expired;

                await _reservationRepository.UpdateReservationAsync(reservation, ct);

                await _seatRepository.ReleaseSeatAsync(reservation.SeatId, ct);

                await _createAuditLogHandler.HandleAsync(new CreateAuditLogCommand
                {
                    Action = AuditLogConstants.Actions.ReserveExpired,
                    EntityType = AuditLogConstants.Entities.Seat,
                    EntityId = reservation.SeatId.ToString(),
                    Details = JsonSerializer.Serialize(new { reservation.UserId, reservation.SeatId, reservation.Id })
                });
            }
        }
    }
}
