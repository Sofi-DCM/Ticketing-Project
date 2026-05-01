using Application.Interfaces.Handlers._Reservation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.BackgroundJobs
{
    public class ReservationExpirationBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public ReservationExpirationBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();

                var handler = scope.ServiceProvider
                    .GetRequiredService<IExpireReservationsHandler>();

                await handler.HandleAsync(stoppingToken);

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
