using CurrencyConversion.Model;

namespace CurrencyConversion.Clients
{
    public interface INationalBankenClient
    {
        Task<List<CurrencyRate>> FetchRatesFromNationalBanken();
    }
}
