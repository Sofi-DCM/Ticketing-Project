using Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Handlers._Sector
{
    public interface IGetSectorsByEventIdHandler
    {
        Task<List<SectorResponseDto>> HandleAsync(int eventId, CancellationToken ct = default);
    }
}
