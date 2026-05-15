using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Handlers._Reservation
{
    public interface ICancelReservationHandler
    {
        public Task HandleAsync(Guid id, int userId, CancellationToken ct);
    }
}
