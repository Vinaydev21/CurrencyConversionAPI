using CurrencyConversion.Model;

namespace CurrencyConversion.Repositories
{
    public interface ICurrencyRepository
    {
        Task UpsertCurrencyRatesAsync(IEnumerable<CurrencyRate> rates);
        Task<CurrencyRate> GetRateByCurrencyCodeAsync(string currencyCode);
        Task<IEnumerable<CurrencyRate>> GetAllRatesAsync();
        Task SaveConversionAsync(CurrencyConversionHistoryDbInputModel conversion);
    }
}
