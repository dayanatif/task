namespace TSManager.Services
{
    public class ExpiryNotificationHostedService : BackgroundService
    {
        private readonly IServiceProvider _sp;
        private readonly ILogger<ExpiryNotificationHostedService> _logger;

        public ExpiryNotificationHostedService(IServiceProvider sp, ILogger<ExpiryNotificationHostedService> logger)
        {
            _sp = sp;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await WaitUntilNextSydneyTime(0, 39, stoppingToken);

                    using var scope = _sp.CreateScope();
                    var runner = scope.ServiceProvider.GetRequiredService<ExpiryNotificationRunner>();

                    await runner.RunAsync(stoppingToken);
                }
                catch (TaskCanceledException)
                {
                    // App is stopping - this is normal
                }
                catch (OperationCanceledException)
                {
                    // App is stopping - this is normal
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Expiry notification job failed.");
                    await Task.Delay(TimeSpan.FromMinutes(2), stoppingToken);
                }

            }
        }
        

        private static async Task WaitUntilNextSydneyTime(int hour, int minute, CancellationToken ct)
        {
            
            var tz = TimeZoneInfo.FindSystemTimeZoneById("Australia/Sydney");
            var nowUtc = DateTime.UtcNow;
            var nowLocal = TimeZoneInfo.ConvertTimeFromUtc(nowUtc, tz);

            var nextLocal = new DateTime(nowLocal.Year, nowLocal.Month, nowLocal.Day, hour, minute, 0);
            if (nowLocal >= nextLocal) nextLocal = nextLocal.AddDays(1);

            var nextUtc = TimeZoneInfo.ConvertTimeToUtc(nextLocal, tz);
            var delay = nextUtc - nowUtc;

            if (delay < TimeSpan.FromSeconds(5)) delay = TimeSpan.FromSeconds(5);
            await Task.Delay(delay, ct);

            
        }
    }
}
