using Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Handlers._Seat
{
    public interface IGetSeatsBySectorHandler
    {
        Task<List<SeatStatusDto>> HandleAsync(int sectorId, bool? onlyRow, CancellationToken ct = default);
    }
}
