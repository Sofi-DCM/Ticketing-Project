using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class SectorRepository : ISectorRepository
    {
        private readonly AppDbContext _context;

        public SectorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IList<int>> InsertAsync(ICollection<Sector> sectors)
        {
            await _context.Sectors.AddRangeAsync(sectors);
            await _context.SaveChangesAsync();

            return sectors.Select(s => s.Id).ToList();
        }

        public async Task<ICollection<Sector>> GetSectorsByEventIdAsync(int eventId, CancellationToken ct = default)
        {
            return await _context.Sectors
                .AsNoTracking()
                .Where(s => s.EventId == eventId)
                .OrderBy(s => s.Id)
                .ToListAsync(ct);
        }
    }
}
