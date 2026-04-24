using Application.UseCase._Event.Queries.GetActiveEvents;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IEventRepository
    {
        public Task<(ICollection<Event>? events , int total)> GetActiveEventsAsync(GetActiveEventsQuery query, CancellationToken ct = default);
    }
}
