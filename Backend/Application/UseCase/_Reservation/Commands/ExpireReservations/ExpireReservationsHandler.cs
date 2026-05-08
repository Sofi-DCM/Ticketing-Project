
using Application.Interfaces.Handlers._AuditLog;
using Application.Interfaces.Handlers._Reservation;
using Application.Interfaces.Repositories;
using Application.Response;
using Application.UseCase._AuditLog.Commands.CreateAuditLog;
using Application.UseCase._Reservation.Commands.CreateReservation;
using Domain.Constants;
using Domain.Entities;
using System.Text.Json;

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

            if (expiredReservations.Count == 0) return;

            var reservationsIds = expiredReservations.Select(r => r.Id).ToList();
            var seatsIds = expiredReservations.Select(r => r.SeatId).ToList();
            var auditLogsCommands = expiredReservations.Select(MapToAuditLogCommand).ToList();

            await _reservationRepository.ExpireReservationsAsync(reservationsIds, ct);
        
            await _seatRepository.ReleaseSeatsAsync(seatsIds, ct);
           
            await _createAuditLogHandler.HandleAsync(auditLogsCommands, ct);
        }

        private static CreateAuditLogCommand MapToAuditLogCommand(ReservationExpiredInfo reservation) 
        {
            return new CreateAuditLogCommand
            {
                Action = AuditLogConstants.Actions.ReserveExpired,
                EntityType = AuditLogConstants.Entities.Seat,
                EntityId = reservation.SeatId.ToString(),
                Details = JsonSerializer.Serialize(new { reservation.UserId, reservation.SeatId, reservation.Id })
            };
        }
    }
}
