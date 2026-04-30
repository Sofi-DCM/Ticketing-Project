
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface ISectorRepository
    {
        public Task<IList<int>> InsertAsync(ICollection<Sector> sectors);
        Task<ICollection<Sector>> GetSectorsByEventIdAsync(int eventId, CancellationToken ct = default);
        Task<bool> EventExistsAsync(int eventId, CancellationToken ct = default);
    }
}
