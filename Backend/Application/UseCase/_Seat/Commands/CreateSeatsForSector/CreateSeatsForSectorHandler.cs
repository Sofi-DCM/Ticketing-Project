
using Application.Interfaces.Handlers._Seat;
using Application.Interfaces.Repositories;
using Domain.Constants;
using Domain.Entities;

namespace Application.UseCase._Seat.Commands.CreateSeatsForSector
{
    public class CreateSeatsForSectorHandler : ICreateSeatsForSectorHandler
    {
        private readonly ISeatRepository _repository;

        public CreateSeatsForSectorHandler(ISeatRepository repository)
        {
            _repository = repository;
        }

        public async Task HandleAsync(IList<CreateSeatsForSectorCommand> commands)
        {
            ICollection<Seat> seats = [];
            foreach (CreateSeatsForSectorCommand command in commands) {
                for (int r = 1; r <= command.RowsAmount; r++) 
                {
                    string rowLetter = GetRowIdentifier(r);

                    for (int c = 1; c <= command.ColumnsAmount; c++) 
                    {
                        seats.Add(new Seat
                        {
                            SectorId = command.SectorId,
                            RowIdentifier = rowLetter,
                            SeatNumber = c,
                            Status = SeatStatusConstants.Available,
                            Version = 1 //por ahora
                        });
                    };
                }
            }

            await _repository.CreateSeatsAsync(seats);
        }

        private static string GetRowIdentifier(int rowNumber)
        {
            string identifier = string.Empty;
            while (rowNumber > 0)
            {
                int modulo = (rowNumber - 1) % 26;
                identifier = Convert.ToChar('A' + modulo) + identifier;
                rowNumber = (rowNumber - modulo) / 26;
            }
            return identifier;
        }
    }
}
