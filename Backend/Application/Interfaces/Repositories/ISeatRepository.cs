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
    }
}
