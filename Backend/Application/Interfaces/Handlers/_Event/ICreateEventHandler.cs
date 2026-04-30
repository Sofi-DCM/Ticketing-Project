
using Application.UseCase._Event.Commands.CreateEvent;

namespace Application.Interfaces.Handlers._Event
{
    public interface ICreateEventHandler
    {
        Task<int> HandleAsync(CreateEventCommand command, CancellationToken ct);
    }
}
