
using Application.Response;
using Application.UseCase._Event.Queries.GetActiveEvents;

namespace Application.Interfaces.Handlers._Event
{
    public interface IGetActiveEventsHandler
    {
        public Task <PagedEventsResponse> HandleAsync (GetActiveEventsQuery query, CancellationToken ct = default);
    }
}
