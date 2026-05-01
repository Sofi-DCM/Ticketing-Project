
using Application.Interfaces;
using Application.Interfaces.Handlers._AuditLog;
using Application.Interfaces.Handlers._Reservation;
using Application.Interfaces.Handlers._Seat;
using Application.Interfaces.Repositories;
using Application.Response;
using Application.UseCase._AuditLog.Commands.CreateAuditLog;
using Domain.Constants;
using Domain.Entities;
using Domain.Exceptions;
using System.Text.Json;

namespace Application.UseCase._Reservation.Commands.CreateReservation
{
    public class CreateReservationHandler : ICreateReservationHandler
    {
        private readonly IReservationRepository _repository;
        private readonly ICreateAuditLogHandler _createAuditLogHandler;
        private readonly IChangeSeatStatusHandler _changeSeatStatusHandler;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateReservationHandler(
            IReservationRepository reservationRepository,
            ICreateAuditLogHandler createAuditLogHandler,
            IChangeSeatStatusHandler changeSeatStatusHandler,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork)
        {
            _repository = reservationRepository;
            _createAuditLogHandler = createAuditLogHandler;
            _changeSeatStatusHandler = changeSeatStatusHandler;
            _userRepository = userRepository;
            _unitOfWork=unitOfWork;
        }

        public async Task<ReservationResponseDto> HandleAsync(CreateReservationCommand command, CancellationToken ct)
        {
            if (!await _userRepository.ExistsByIdAsync(command.UserId, ct))
                throw new NotFoundException($"No existe un usuario con id: {command.UserId}");

            using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                await _changeSeatStatusHandler.HandleAsync(command.SeatId, ct);

                var newReservation = new Reservation
                {
                    UserId = command.UserId,
                    SeatId = command.SeatId,
                    Status = ReservationConstants.Pending,
                    ReservedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(5)
                };

                var reservationId = await _repository.InsertReservationAsync(newReservation, ct);

                await _createAuditLogHandler.HandleAsync(MapToAuditLogCommand(command, true, reservationId));

                await _unitOfWork.SaveChangesAsync();
                await transaction.CommitAsync();

                return MapToResponseDto(newReservation);
            }
            catch (InvalidOperationException) //a futuro es DbUpdateConcurrencyException
            {
                await transaction.RollbackAsync();

                await _createAuditLogHandler.HandleAsync(MapToAuditLogCommand(command, false));
                throw;
            }
            catch (Exception) 
            {
                await transaction.RollbackAsync();
                throw;
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
