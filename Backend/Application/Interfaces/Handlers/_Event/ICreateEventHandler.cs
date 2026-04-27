using Application.UseCase._Event.Commands.CreateEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Handlers._Event
{
    public interface ICreateEventHandler
    {
        Task<int> HandleAsync(CreateEventCommand command, CancellationToken ct);
    }
}
