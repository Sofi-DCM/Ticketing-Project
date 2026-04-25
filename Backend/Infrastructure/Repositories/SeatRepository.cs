using Application.Interfaces.Repositories;
using Domain.Constants;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class SeatRepository : ISeatRepository
    {
        private readonly AppDbContext _context;
        public SeatRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistsByIdAsync(Guid seatId) =>
            await _context.Seats.AnyAsync(s => s.Id == seatId);

        public async Task<bool> PatchSeatStateAsync(Guid seatId, CancellationToken ct)
        {
            int rowsAffected = await _context.Seats
                .Where(s => s.Id == seatId && s.Status == SeatStatusConstants.Available)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(s => s.Status, SeatStatusConstants.Reserved), ct);
            //a futuro se busca el where tambien con version y tambien se incrementa

            // Si rowsAffected es 0, significa que el estado no coincidio por eso no se modifico
            return rowsAffected > 0;
        }
    }
}
