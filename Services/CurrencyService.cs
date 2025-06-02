using CurrencyConversion.Clients;
using CurrencyConversion.DTO;
using CurrencyConversion.Model;
using CurrencyConversion.Repositories;

namespace CurrencyConversion.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyRepository _repo;
        private readonly INationalBankenClient _client;
        private readonly ILogger<CurrencyService> _logger;

        public CurrencyService(ICurrencyRepository repo, INationalBankenClient client, ILogger<CurrencyService> logger)
        {
            _repo = repo;
            _client = client;
            _logger = logger;
        }

        public async Task GetCurrencyRates()
        {
            _logger.LogInformation("Starting GetCurrencyRates...");
            try
            {
                var rates = await _client.FetchRatesFromNationalBanken();

                if (rates == null || rates.Count == 0)
                {
                    _logger.LogWarning("Fetched currency rates are null or empty.");
                    throw new InvalidOperationException("Fetched currency rates are null or empty.");
                }

                await _repo.UpsertCurrencyRatesAsync(rates);
                _logger.LogInformation("Currency rates successfully fetched and upserted.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred in GetCurrencyRates.");
                throw;
            }
        }

        public async Task<IEnumerable<CurrencyRateDto>> GetAllRatesAsync()
        {
            _logger.LogInformation("Retrieving all currency rates.");
            try
            {
                var rates = await _repo.GetAllRatesAsync();

                if (rates == null || !rates.Any())
                {
                    _logger.LogWarning("No currency rates found in database.");
                    return Enumerable.Empty<CurrencyRateDto>();
                }

                var result = rates.Select(r => new CurrencyRateDto
                {
                    CurrencyCode = r.CurrencyCode,
                    Rate = r.Rate
                });

                _logger.LogInformation("Currency rates retrieved successfully.");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve currency rates.");
                throw;
            }
        }

        public async Task<CurrencyConversionResponseDto> ConvertAsync(CurrencyConversionRequestDto dto)
        {
            _logger.LogInformation("Converting currency for request: {@Dto}", dto);
            try
            {
                if (dto == null || string.IsNullOrWhiteSpace(dto.FromCurrency) || dto.Amount <= 0)
                {
                    _logger.LogWarning("Invalid conversion request received: {@Dto}", dto);
                    throw new ArgumentException("Invalid conversion request. Please provide a valid currency and amount.");
                }

                var rate = await _repo.GetRateByCurrencyCodeAsync(dto.FromCurrency);

                if (rate == null)
                {
                    _logger.LogWarning("Exchange rate for '{Currency}' not found.", dto.FromCurrency);
                    throw new InvalidOperationException($"Exchange rate for '{dto.FromCurrency}' not found.");
                }

                var converted = dto.Amount * rate.Rate;

                var conversion = new CurrencyConversionHistoryDbInputModel
                {
                    FromCurrency = dto.FromCurrency,
                    FromAmount = dto.Amount,
                    ConvertedAmount = converted,
                    ConversionDate = DateTime.Now,
                    FromRate = rate.Rate,
                };

                await _repo.SaveConversionAsync(conversion);
                _logger.LogInformation("Currency conversion successful for {Currency}. Amount: {Amount} → {Converted}",
                dto.FromCurrency, dto.Amount, converted);

                return new CurrencyConversionResponseDto
                {
                    FromCurrency = dto.FromCurrency,
                    FromAmount = dto.Amount,
                    ConvertedAmount = converted
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during currency conversion.");
                throw;
            }
        }

    }
}
