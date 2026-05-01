
using Application.Interfaces.Repositories;
using Application.UseCase._Event.Queries.GetActiveEvents;
using Domain.Entities;
using Domain.Constants;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly AppDbContext _context;

        public EventRepository(AppDbContext context)
        {
            _context=context;
        }

        public async Task<(ICollection<Event>?,int)> GetActiveEventsAsync(GetActiveEventsQuery queryDto, CancellationToken ct = default)
        {
            var query = _context.Events
                .AsNoTracking()
                .Where(e => e.Status == EventStatusConstants.Active);

            query = queryDto.SortBy switch 
            {
                SortEventsBy.DateAsc => query.OrderBy(e => e.EventDate).ThenByDescending(e => e.Id),
                SortEventsBy.DateDesc => query.OrderByDescending(e => e.EventDate).ThenByDescending(e => e.Id),
                SortEventsBy.NameAsc => query.OrderBy(e => e.Name).ThenByDescending(e => e.Id),
                SortEventsBy.NameDesc => query.OrderByDescending(e => e.Name).ThenByDescending(e => e.Id),
                _ => query.OrderByDescending(e => e.Id) // Newest (default)
            };

            var TotalCount = await query.CountAsync(ct);
            var events = await query
                .Skip((queryDto.PageNumber - 1) * queryDto.PageSize)
                .Take(queryDto.PageSize)
                .ToListAsync(ct);

            return (events, TotalCount);
        }

        public async Task<int> InsertEventAsync(Event newEvent, CancellationToken ct)
        {
            await _context.Events.AddAsync(newEvent, ct);

            await _context.SaveChangesAsync(ct);

            return newEvent.Id;
        }
    }
}
