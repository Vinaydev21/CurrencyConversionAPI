using CurrencyConversion.DTO;
using CurrencyConversion.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyConversion.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyController : Controller
    {
        private readonly ICurrencyService _service;
        private readonly ILogger<CurrencyController> _logger;

        public CurrencyController(ICurrencyService service, ILogger<CurrencyController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [Authorize]
        [HttpGet("GetCurrencyRates")]
        public async Task<IActionResult> GetCurrencyRates()
        {
            _logger.LogInformation("GetCurrencyRates called.");
            try
            {
                await _service.GetCurrencyRates();
                _logger.LogInformation("Fetched and updated currency rates from Nationalbanken.");

                var rates = await _service.GetAllRatesAsync();

                if (rates == null || !rates.Any())
                {
                    _logger.LogWarning("No currency rates found.");
                    return NotFound("No currency rates found.");
                }

                _logger.LogInformation("Returning currency rates to client.");
                return Ok(rates);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching currency rates.");
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }
        }

        [Authorize]
        [HttpPost("ConvertCurrency")]
        public async Task<IActionResult> ConvertCurrency([FromBody] CurrencyConversionRequestDto dto)
        {
            _logger.LogInformation("ConvertCurrency called with input: {@Dto}", dto);
            if (dto == null)
            {
                _logger.LogWarning("Conversion request was null.");
                return BadRequest("Conversion request cannot be null.");
            }
            try
            {
                var result = await _service.ConvertAsync(dto);

                if (result == null)
                {
                    _logger.LogWarning("Conversion failed. Rate for currency {Currency} not found.", dto.FromCurrency);
                    return NotFound("Conversion could not be completed. Rate may not exist.");
                }

                _logger.LogInformation("Currency converted successfully: {@Result}", result);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during currency conversion.");
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }
        }



    }
}
