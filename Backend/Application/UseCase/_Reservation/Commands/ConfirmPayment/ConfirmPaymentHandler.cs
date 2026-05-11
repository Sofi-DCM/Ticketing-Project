using Application.Interfaces;
using Application.Interfaces.Handlers._AuditLog;
using Application.Interfaces.Handlers._Reservation;
using Application.Interfaces.Payments;
using Application.Interfaces.Repositories;
using Application.UseCase._AuditLog.Commands.CreateAuditLog;
using Domain.Constants;
using Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Application.UseCase._Reservation.Commands.ConfirmPayment
{
    public class ConfirmPaymentHandler : IConfirmPaymentHandler
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly ICreateAuditLogHandler _createAuditLogHandler;
        private readonly IPaymentSimulator _paymentSimulator;
        private readonly IUnitOfWork _unitOfWork;

        public ConfirmPaymentHandler(
            IReservationRepository reservationRepository,
            ICreateAuditLogHandler createAuditLogHandler,
            IPaymentSimulator paymentSimulator,
            IUnitOfWork unitOfWork)
        {
            _reservationRepository = reservationRepository;
            _createAuditLogHandler = createAuditLogHandler;
            _paymentSimulator = paymentSimulator;
            _unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(ConfirmPaymentRequest request, CancellationToken ct)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                var reservation = await _reservationRepository
                    .GetByIdWithSeatAsync(request.ReservationId, ct);

                if (reservation == null)
                    throw new NotFoundException("No existe la reserva.");

                if (reservation.Status == ReservationConstants.Paid)
                    throw new BadRequestException("La reserva ya fue pagada.");

                if (reservation.Status == ReservationConstants.Expired)
                    throw new BadRequestException("La reserva ya expiró.");


                var paymentApproved = await _paymentSimulator.ProcessPaymentAsync(reservation.Seat.Sector.Price, ct);

                if (!paymentApproved)
                    throw new BadRequestException("El pago fue rechazado.");

                // Confirmar compra
                reservation.Status = ReservationConstants.Paid;
                reservation.Seat.Status = SeatStatusConstants.Sold;
                reservation.Seat.Version++;

                // Auditoría
                await _createAuditLogHandler.HandleAsync(
                    new CreateAuditLogCommand
                    {
                        UserId = reservation.UserId,
                        Action = "PAYMENT_CONFIRMED",
                        EntityType = "Reservation",
                        EntityId = reservation.Id.ToString(),
                        Details = JsonSerializer.Serialize(new
                        {
                            reservation.UserId,
                            reservation.SeatId,
                            reservation.Id,
                            Payment = "Simulated"
                        })
                    });

                await _unitOfWork.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                await transaction.RollbackAsync();
                throw;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
