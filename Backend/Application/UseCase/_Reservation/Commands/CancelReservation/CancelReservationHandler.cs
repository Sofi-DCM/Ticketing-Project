using Application.Interfaces;
using Application.Interfaces.Handlers._AuditLog;
using Application.Interfaces.Handlers._Reservation;
using Application.Interfaces.Handlers._Seat;
using Application.Interfaces.Repositories;
using Application.UseCase._AuditLog.Commands.CreateAuditLog;
using Application.UseCase._Seat.Commands.ChangeSeatStatus;
using Application.UseCase._User.Queries.GetUserById;
using Domain.Constants;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Application.UseCase._Reservation.Commands.CancelReservation
{
    public class CancelReservationHandler : ICancelReservationHandler
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IChangeSeatStatusHandler _seatChangeStatusHandler;
        private readonly ICreateAuditLogHandler _createAuditLogHandler;
        private readonly IUnitOfWork _unitOfWork;

        public CancelReservationHandler(
            IReservationRepository reservationRepository,
            IChangeSeatStatusHandler seatStatusHandler,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            ICreateAuditLogHandler createAuditLogHandler)
        {
            _reservationRepository = reservationRepository;
            _seatChangeStatusHandler = seatStatusHandler;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _createAuditLogHandler = createAuditLogHandler;
        }
        public async Task HandleAsync(Guid id, int userId, CancellationToken ct)
        {
            if (userId <= 0)
                throw new ArgumentException("Los id deben ser positivos");

            if (!await _userRepository.ExistsByIdAsync(userId, ct))
                throw new NotFoundException($"No existe un usuario con id: {userId}");

            using var transaction = await _unitOfWork.BeginTransactionAsync();
            try { 
                var seatId = await _reservationRepository.CancelReservationAsync(id, userId, ct);
                if (seatId == null)
                    throw new KeyNotFoundException($"No existe una reservacion con id : {id}");

                var command = new ChangeSeatStatusCommand
                {
                    SeatId = (Guid)seatId,
                    OriginalStatus = SeatStatusConstants.Reserved,
                    PatchStatus = SeatStatusConstants.Available,
                };
                await _seatChangeStatusHandler.HandleAsync(command, ct);
                
                await _unitOfWork.SaveChangesAsync();
                await transaction.CommitAsync();
                
                await _createAuditLogHandler.HandleAsync(MapToAuditLogCommand(userId, (Guid)seatId, id));
            }
            catch (DbUpdateConcurrencyException)
            {
                await transaction.RollbackAsync();
                throw new ConflictException("La reserva ya fue cancelada");
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        private static CreateAuditLogCommand MapToAuditLogCommand( int userId, Guid seatId, Guid reservationId) 
        {
            return new CreateAuditLogCommand
            {
                UserId = userId,
                Action = AuditLogConstants.Actions.ReserveExpired,
                EntityType = AuditLogConstants.Entities.Seat,
                EntityId = seatId.ToString(),
                Details = JsonSerializer.Serialize(new { userId, seatId, reservationId })
            };
        }
    }
}
