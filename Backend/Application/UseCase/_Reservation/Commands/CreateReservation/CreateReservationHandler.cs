using Application.Interfaces.Repositories;
using Application.Interfaces.Handlers._AuditLog;
using Application.Interfaces.Handlers._Reservation;
using Application.Interfaces.Handlers._Seat;
using Application.Response;
using Application.UseCase._AuditLog.Commands.CreateAuditLog;
using Application.UseCase._User.Commands.CreateUser;
using Domain.Constants;
using Domain.Entities;
using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCase._Reservation.Commands.CreateReservation
{
    public class CreateReservationHandler : ICreateReservationHandler
    {
        private readonly IReservationRepository _repository;
        private readonly ICreateAuditLogHandler _createAuditLogHandler;
        private readonly IChangeSeatStatusHandler _changeSeatStatusHandler;

        public CreateReservationHandler(
            IReservationRepository reservationRepository,
            ICreateAuditLogHandler createAuditLogHandler,
            IChangeSeatStatusHandler changeSeatStatusHandler)
        {
            _repository = reservationRepository;
            _createAuditLogHandler = createAuditLogHandler;
            _changeSeatStatusHandler = changeSeatStatusHandler;
        }

        public async Task<ReservationResponseDto> HandleAsync(CreateReservationCommand command, CancellationToken ct)
        {
            try
            {
                // enviar a que se modifique el estado de asiento
                // si alguien ya lo modifico el handler de asiento lanza excepcion de conflicto que toma el middleware
                await _changeSeatStatusHandler.HandleAsync(command.SeatId, ct);
                // hacer la reserva en repository
                var newReservation = new Reservation
                {
                    UserId = command.UserId,
                    SeatId = command.SeatId,
                    Status = ReservationConstants.Pending,
                    ReservedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(5)
                };

                var reservationId = await _repository.InsertReservationAsync(newReservation, ct);

                // crear auditoria de conseguido
                await _createAuditLogHandler.HandleAsync(MapToAuditLogCommand(command, true, reservationId));

                return MapToResponseDto(newReservation);
            }
            catch (InvalidOperationException ex) //a futuro es DbUpdateConcurrencyException
            {
                //si se rompe porque el asiento ya esta reservado 
                await _createAuditLogHandler.HandleAsync(MapToAuditLogCommand(command, false));
                throw ex;
            }
        }

        private static CreateAuditLogCommand MapToAuditLogCommand(CreateReservationCommand cmd, bool reserved, Guid? reservationId = null)
        {
            return new CreateAuditLogCommand
            {
                UserId = cmd.UserId,
                Action = reserved ? AuditLogConstants.Actions.ReserveSuccess : AuditLogConstants.Actions.ReserveAttemp,
                EntityType = AuditLogConstants.Entities.Seat,
                EntityId = cmd.SeatId.ToString(),
                Details = reserved
                        ? JsonSerializer.Serialize(new { cmd.UserId, cmd.SeatId, reservationId })
                        : JsonSerializer.Serialize(new { cmd.UserId, cmd.SeatId, Note = "Attempt failed" })
            };
        }

        private static ReservationResponseDto MapToResponseDto(Reservation reservation)
        {
            return new ReservationResponseDto
            {
                ReservationId = reservation.Id,
                SeatId = reservation.SeatId,
                SeatStatus = SeatStatusConstants.Reserved,
                ReservationStatus = reservation.Status,
                ReservedAt = reservation.ReservedAt,
                ExpiresAt = reservation.ExpiresAt
            };
        }
    }
}
