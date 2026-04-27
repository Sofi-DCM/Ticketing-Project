using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface ISectorRepository
    {
        public Task<IList<int>> InsertAsync(ICollection<Sector> sectors);
        Task<ICollection<Sector>> GetSectorsByEventIdAsync(int eventId, CancellationToken ct = default);
    }
}
