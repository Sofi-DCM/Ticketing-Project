using Application.Interfaces.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Payments
{
    public class SimulatedPayment: IPaymentSimulator
    {
        public async Task<bool> ProcessPaymentAsync(decimal amount, CancellationToken ct)
        {
            await Task.Delay(1000, ct);
            return true;
        }
    }
}
