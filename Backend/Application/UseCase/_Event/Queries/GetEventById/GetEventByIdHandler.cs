using Application.Interfaces.Handlers._Event;
using Application.Interfaces.Repositories;
using Application.Response;
using Domain.Entities;

namespace Application.UseCase._Event.Queries.GetEventById
{
    public class GetEventByIdHandler : IGetEventByIdHandler
    {
        private readonly IEventRepository _repository;

        public GetEventByIdHandler(IEventRepository repository)
        {
            _repository = repository;
        }

        public async Task<ShortEventResponse> HandleAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Los id deben ser positivos");

            var response = await _repository.GetEventByIdAsync(id)
                 ?? throw new KeyNotFoundException($"el Evento con Id : {id} no existe");

            return MapToEventResponse(response);
        }

        private static ShortEventResponse MapToEventResponse(Event e)
        {
            return new ShortEventResponse
            {
                Id = e.Id,
                Name = e.Name,
                EventDate = e.EventDate,
                Venue = e.Venue,
            };
        }
        
    }
}
