using Application.Interfaces.Handlers._Event;
using Application.Interfaces.Repositories;
using Application.Response;
using Domain.Entities;
using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCase._Event.Queries.GetActiveEvents
{
    public class GetActiveEventsHandler : IGetActiveEventsHandler
    {
        private readonly IEventRepository _repository;

        public GetActiveEventsHandler(IEventRepository repository)
        {
            _repository=repository;
        }

        public async Task<PagedEventsResponse> HandleAsync(GetActiveEventsQuery query, CancellationToken ct = default)
        {
            if (query.PageNumber < 1)
                throw new BadRequestException("La pagina solicitada no puede ser menor a 1");

            if (query.PageSize < 1 || query.PageSize > 50)
                throw new BadRequestException("El tamaño de página debe estar entre 1 y 50.");

            var response = await _repository.GetActiveEventsAsync(query, ct);

            return MapToPagedEventsResponse(response, query);
        }

        public static PagedEventsResponse MapToPagedEventsResponse((ICollection<Event>? events, int total) response, GetActiveEventsQuery query) 
        {
            var lastPageNumber = (int)Math.Ceiling(response.total / (double)query.PageSize);

            return new PagedEventsResponse
            {
                Events = response.events?.Select(e => new ShortEventResponse
                        {
                            Id = e.Id,
                            Name = e.Name,
                            EventDate = e.EventDate,
                            Venue = e.Venue,
                        }).ToList() ?? new List<ShortEventResponse>(),
                TotalEventsInBd = response.total,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                SortBy = query.SortBy.ToString(),
                HasPreviousPage = query.PageNumber > 1,
                HasNextPage = query.PageNumber < lastPageNumber,
            };
        }
    }
}
