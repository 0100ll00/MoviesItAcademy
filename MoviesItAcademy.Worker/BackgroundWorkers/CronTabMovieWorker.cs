using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NCrontab;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MoviesItAcademy.Worker.BackgroundWorkers
{
    public class CronTabMovieWorker : BackgroundService
    {
        private readonly ILogger<CronTabMovieWorker> _logger;
        private CrontabSchedule _schedule;
        private DateTime _nextRun;
        private readonly IServiceProvider _serviceProvider;

        private string Schedule => "*/1 * * * *";

        public CronTabMovieWorker(ILogger<CronTabMovieWorker> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _schedule = CrontabSchedule.Parse(Schedule, new CrontabSchedule.ParseOptions() { IncludingSeconds = false });
            _nextRun = _schedule.GetNextOccurrence(DateTime.Now);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker running at rate: once per minute.");
            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.Now;
                if (now > _nextRun)
                {
                    _nextRun = _schedule.GetNextOccurrence(now);
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var service = scope.ServiceProvider.GetRequiredService<ApiClient>();
                        _logger.LogInformation("Attempting to remove expired items...");
                        await service.MoveMoviesToArchive();
                        await service.CancelExpiredBookings();
                        _logger.LogInformation($"Done. Next removal time: {_nextRun}");
                    }
                }
            }
        }
    }
}
