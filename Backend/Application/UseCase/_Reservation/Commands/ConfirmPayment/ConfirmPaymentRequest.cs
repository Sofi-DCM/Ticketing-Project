using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCase._Reservation.Commands.ConfirmPayment
{
    public class ConfirmPaymentRequest
    {
        public Guid ReservationId { get; set; }
    }
}
