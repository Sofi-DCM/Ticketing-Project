using Application.DTOs;
using Application.Interfaces;
using Application.UseCase.Reservations.Commands;
using Domain.Entities;
using Domain.Exceptions;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly AppDbContext _context;
        public ReservationRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ReservationResponseDto> CreateReservationAsync(CreateReservationCommand command, CancellationToken cancellationToken)
        {
            var seat = await _context.Seats.FirstOrDefaultAsync(s => s.Id == command.SeatId, cancellationToken);
            if (seat == null)
            {
                throw new Exception("Seat not found");
            }
            if (seat.Status != "Available")
            {
                _context.AuditLogs.Add(new AuditLog
                {
                    Id = Guid.NewGuid(),
                    UserId = command.UserId,
                    Action = "RESERVE_ATTEMPT_FAILED",
                    EntityType = "Seat",
                    EntityId = command.SeatId.ToString(),
                    Details = $"La butaca no está disponible. Estado actual: {seat.Status}",
                    CreatedAt = DateTime.UtcNow
                });
                await _context.SaveChangesAsync(cancellationToken);

                throw new ConflictException("La butaca ya no está disponible.");
            }
            var reservation = new Reservation
            {
                Id = Guid.NewGuid(),
                UserId = command.UserId,
                SeatId = seat.Id,
                Status = "Pending",
                ReservedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(5)
            };
            seat.Status = "Reserved";
            seat.Version++;

            _context.Reservations.Add(reservation);

            _context.AuditLogs.Add(new AuditLog
            {
                Id = Guid.NewGuid(),
                UserId = command.UserId,
                Action = "RESERVE_SUCCESS",
                EntityType = "Reservation",
                EntityId = reservation.Id.ToString(),
                Details = $"Reserva creada para la butaca {seat.Id}. Expira a las {reservation.ExpiresAt:O}",
                CreatedAt = DateTime.UtcNow
            });
            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new ConflictException("Otro usuario reservó la butaca al mismo tiempo.");
            }

            return new ReservationResponseDto
            {
                ReservationId = reservation.Id,
                SeatId = reservation.SeatId,
                SeatStatus = seat.Status,
                ReservationStatus = reservation.Status,
                ReservedAt = reservation.ReservedAt,
                ExpiresAt = reservation.ExpiresAt
            };
        }
    }
}
