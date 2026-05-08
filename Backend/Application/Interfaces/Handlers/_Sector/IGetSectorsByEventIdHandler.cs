
using Application.Response;

namespace Application.Interfaces.Handlers._Sector
{
    public interface IGetSectorsByEventIdHandler
    {
        Task<List<SectorResponseDto>> HandleAsync(int eventId, CancellationToken ct = default);
    }
}
