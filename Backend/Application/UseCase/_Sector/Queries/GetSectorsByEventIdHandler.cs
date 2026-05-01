
using Application.Interfaces.Handlers._Sector;
using Application.Interfaces.Repositories;
using Application.Response;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Application.UseCase._Sector.Queries
{
    public class GetSectorsByEventIdHandler : IGetSectorsByEventIdHandler
    {
        private readonly ISectorRepository _repository;

        public GetSectorsByEventIdHandler(ISectorRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<SectorResponseDto>> HandleAsync(int eventId, CancellationToken ct = default)
        {
            if (eventId <= 0)
                throw new ArgumentException("Los id deben ser positivos");

            var eventExists = await _repository.EventExistsAsync(eventId);

            if (!eventExists)
            {
                throw new KeyNotFoundException($"No existe un evento con id {eventId}");
            }

            var sectors = await _repository.GetSectorsByEventIdAsync(eventId, ct);

            return sectors.Select(s => new SectorResponseDto
            {
                Id = s.Id,
                EventId = s.EventId,
                Name = s.Name,
                Price = s.Price,
                Capacity = s.Capacity
            }).ToList();
        }
    }
}
