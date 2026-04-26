using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Handlers._Reservation
{
    public interface IExpireReservationsHandler
    {
        Task HandleAsync(CancellationToken ct);
    }
}
