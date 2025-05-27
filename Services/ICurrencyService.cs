using CurrencyConversion.DTO;

namespace CurrencyConversion.Services
{
    public interface ICurrencyService
    {
        Task GetCurrencyRates();
        Task<IEnumerable<CurrencyRateDto>> GetAllRatesAsync();
        Task<CurrencyConversionResponseDto> ConvertAsync(CurrencyConversionRequestDto dto);
    }
}
