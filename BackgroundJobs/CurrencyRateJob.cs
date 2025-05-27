using CurrencyConversion.Services;

namespace CurrencyConversion.BackgroundJobs
{
    public class CurrencyRateJob : BackgroundService
    {
        private readonly IServiceProvider _provider;
        private readonly ILogger<CurrencyRateJob> _logger;

        public CurrencyRateJob(IServiceProvider provider, ILogger<CurrencyRateJob> logger)
        {
            _provider = provider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("CurrencyRateJob started at {StartTime}", DateTime.Now);
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _provider.CreateScope();
                var service = scope.ServiceProvider.GetRequiredService<ICurrencyService>();

                try
                {
                    await service.GetCurrencyRates();
                    _logger.LogInformation("Currency rates updated at {Time}", DateTime.Now);
                }
                catch (HttpRequestException httpEx)
                {
                    _logger.LogError(httpEx, "HTTP error while fetching currency rates.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unexpected error occurred while updating currency rates.");
                }

                await Task.Delay(TimeSpan.FromMinutes(60), stoppingToken);// Job to run every 60 Minutes               
            }
            _logger.LogInformation("CurrencyRateJob stopped at {StopTime}", DateTime.Now);
        }
    }
}
