
using Application.Response;

namespace Application.Interfaces.Handlers._Seat
{
    public interface IGetSeatsBySectorHandler
    {
        Task<List<SeatStatusDto>> HandleAsync(int sectorId, bool? onlyRow, CancellationToken ct = default);
    }
}
