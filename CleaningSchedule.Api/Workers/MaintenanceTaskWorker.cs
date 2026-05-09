
using CleaningSchedule.Core.Models;
using CleaningSchedule.Core.Services.Abstractions;

namespace CleaningSchedule.Api.Workers
{
    public class MaintenanceTaskWorker(
        ILogger<MaintenanceTask> logger,
        IServiceScopeFactory scopeFactory) : BackgroundService
    {
        private readonly TimeSpan _scheduleTime = new(8, 0, 0); 

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("→ Maintenance Task Worker started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.Now;
                var nextRun = GetNextSundayAtEight(now);

                var delay = TimeSpan.FromSeconds(5);

                logger.LogInformation("→ Next execution scheduled for: {NextRun}", nextRun);

                try
                {
                    await Task.Delay(delay, stoppingToken);

                    logger.LogInformation("→ Executing Sunday task at: {Time}", DateTime.Now);
                    await DoWorkAsync(stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        private DateTime GetNextSundayAtEight(DateTime current)
        {
            var daysUntilSunday = ((int)DayOfWeek.Sunday - (int)current.DayOfWeek + 7) % 7;

            var nextSunday = current.Date.AddDays(daysUntilSunday).Add(_scheduleTime);

            if (nextSunday <= current)
                nextSunday = nextSunday.AddDays(7);

            return nextSunday;
        }

        private async Task DoWorkAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("→ Working...");
            using var scope = scopeFactory.CreateScope();
            var maintenanceTaskService = scope.ServiceProvider.GetRequiredService<IMaintenanceTaskService>();
            await maintenanceTaskService.SendAsync(cancellationToken);
        }
    }
}