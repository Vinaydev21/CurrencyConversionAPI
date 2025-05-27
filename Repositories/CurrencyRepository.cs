using CurrencyConversion.Data;
using CurrencyConversion.Model;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace CurrencyConversion.Repositories
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly IDapperContext _context;
        private readonly ILogger<CurrencyRepository> _logger;

        public CurrencyRepository(IDapperContext context, ILogger<CurrencyRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task UpsertCurrencyRatesAsync(IEnumerable<CurrencyRate> rates)
        {
            try
            {
                if (rates == null || !rates.Any())
                {
                    throw new ArgumentException("Currency rates data is null or empty.");
                }

                using var connection = _context.CreateConnection();

                foreach (var rate in rates)
                {
                    await connection.ExecuteAsync(StoredProcedures.UpsertCurrencyRates, new
                    {
                        rate.CurrencyCode,
                        rate.Rate,
                        rate.RetrievedDate
                    }, commandType: CommandType.StoredProcedure);
                }
                _logger.LogInformation("Successfully upserted {Count} currency rates.", rates.Count());
            }            
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in UpsertCurrencyRatesAsync.");
                throw;
            }
        }

        public async Task<CurrencyRate> GetRateByCurrencyCodeAsync(string currencyCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(currencyCode))
                {
                    throw new ArgumentException("Currency code must not be null or empty.");
                }

                var parameters = new { CurrencyCode = currencyCode };

                var result = await _context.CreateConnection().QueryFirstOrDefaultAsync<CurrencyRate>(
                    StoredProcedures.GetRateByCurrencyCode,
                    parameters,
                    commandType: CommandType.StoredProcedure);

                if (result == null)
                {
                    _logger.LogWarning("No exchange rate found for currency code: {CurrencyCode}", currencyCode);
                    throw new ArgumentException($"No exchange rate found for currency code: {currencyCode}");
                }

                return result;
            }          
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in GetRateByCurrencyCodeAsync.");
                throw;
            }
        }


        public async Task<IEnumerable<CurrencyRate>> GetAllRatesAsync()
        {
            try
            {
                var connection = _context.CreateConnection();

                var result = await connection.QueryAsync<CurrencyRate>(
                    StoredProcedures.GetAllCurrencyRates,
                    commandType: CommandType.StoredProcedure);

                _logger.LogInformation("Retrieved {Count} currency rates.", result.Count());
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SQL error while fetching all currency rates: {ex.Message}");
                throw;
            }           
        }

        public async Task SaveConversionAsync(CurrencyConversionHistoryDbInputModel conversion)
        {
            try
            {
                var connection = _context.CreateConnection();

                await connection.ExecuteAsync(
                    StoredProcedures.InsertConversionHistory,
                    conversion,
                    commandType: CommandType.StoredProcedure);
                _logger.LogInformation("Successfully saved conversion from {Currency} with amount {Amount}.",
              conversion.FromCurrency, conversion.FromAmount);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SQL error while saving conversion: {ex.Message}");
                throw;
            }           
        }

    }
}

