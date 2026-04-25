using Application.Interfaces.Handlers._Seat;
using Application.Interfaces.Repositories;
using Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCase._Seat.Queries.GetSeatsBySector
{
    public class GetSeatsBySectorHandler : IGetSeatsBySectorHandler
    {
        private readonly ISeatRepository _repository;

        public GetSeatsBySectorHandler(ISeatRepository repository)
        {
            _repository = repository;
        }
        public async Task<List<SeatStatusDto>> HandleAsync (int sectorId)
        {
            return await _repository.GetSeatsBySectorAsync(sectorId);
        }
    }
}
