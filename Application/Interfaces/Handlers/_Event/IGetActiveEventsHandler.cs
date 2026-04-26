using Application.Response;
using Application.UseCase._Event.Queries.GetActiveEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Handlers._Event
{
    public interface IGetActiveEventsHandler
    {
        public Task <PagedEventsResponse> HandleAsync (GetActiveEventsQuery query, CancellationToken ct = default);
    }
}
