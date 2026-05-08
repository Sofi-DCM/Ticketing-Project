
using Application.UseCase._Event.Queries.GetActiveEvents;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IEventRepository
    {
        public Task<(ICollection<Event>? events , int total)> GetActiveEventsAsync(GetActiveEventsQuery query, CancellationToken ct = default);
        Task<int> InsertEventAsync(Event newEvent, CancellationToken ct);
    }
}
