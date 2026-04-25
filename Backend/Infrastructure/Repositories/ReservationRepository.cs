using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;
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

        public async Task<Guid> InsertReservationAsync(Reservation reservation, CancellationToken ct)
        {
            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync(ct);

            return reservation.Id;
        }
    }
}
