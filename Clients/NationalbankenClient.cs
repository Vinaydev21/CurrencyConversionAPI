using CurrencyConversion.Model;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Xml.Serialization;

namespace CurrencyConversion.Clients
{
    public class NationalbankenClient : INationalBankenClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _endpoint;
        private readonly ILogger<NationalbankenClient> _logger;

        public NationalbankenClient(HttpClient httpClient, IOptions<AppSettings> options)
        {
            _httpClient = httpClient;
            if (options?.Value?.Nationalbanken?.Endpoint == null)
            {
                _logger.LogError("Nationalbanken endpoint is not configured.");
                throw new ArgumentNullException(nameof(options), "Nationalbanken endpoint is not configured.");
            }
               
            _endpoint = options.Value.Nationalbanken.Endpoint;
        }

        public async Task<List<CurrencyRate>> FetchRatesFromNationalBanken()
        {
            try
            {
                var response = await _httpClient.GetAsync(_endpoint);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Failed to fetch data from Nationalbanken. Status Code: {StatusCode}", response.StatusCode);
                    throw new HttpRequestException($"Failed to fetch currency rates. Status code: {response.StatusCode}");
                }

                await using var responseStream = await response.Content.ReadAsStreamAsync();
                var serializer = new XmlSerializer(typeof(ExchangeRatesResponse));
                var data = (ExchangeRatesResponse)serializer.Deserialize(responseStream)!;

                var date = DateTime.Now;

                return data.DailyRates.Currencies.Select(c => new CurrencyRate
                {
                    CurrencyCode = c.Code,
                    Rate = decimal.Parse(c.RateRaw.Replace(",", "."), CultureInfo.InvariantCulture),
                    RetrievedDate = date
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch or parse currency rates from Nationalbanken.");
                throw;
            }
        }
    }
}
