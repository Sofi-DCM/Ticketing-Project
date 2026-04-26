using Application.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface ISeatRepository
    {
        public Task<bool> ExistsByIdAsync(Guid seatId);
        public Task<bool> PatchSeatStateAsync(Guid seatId, CancellationToken ct);
        Task<ICollection<Seat>> GetSeatsBySectorAsync(int sectorId, CancellationToken ct = default);
        Task ReleaseSeatAsync(Guid seatId, CancellationToken ct);
        Task<bool> SectorExistsAsync(int sectorId, CancellationToken ct);
        public Task CreateSeatsAsync(ICollection<Seat> seats);
    }
}
