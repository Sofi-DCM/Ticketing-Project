using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Handlers._Seat
{
    public interface IChangeSeatStatusHandler
    {
        Task HandleAsync(Guid seatId, CancellationToken ct);
    }
}
