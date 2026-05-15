using Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCase._Seat.Commands.ChangeSeatStatus
{
    public class ChangeSeatStatusCommand
    {
        public Guid SeatId { get; set; }
        public string OriginalStatus { get; set; } = SeatStatusConstants.Available;
        public string PatchStatus { get; set; } = SeatStatusConstants.Reserved;
    }
}
