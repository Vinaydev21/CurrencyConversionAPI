using System.ComponentModel.DataAnnotations;

namespace CurrencyConversion.DTO
{
    public class CurrencyConversionRequestDto
    {
        public string FromCurrency { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }
    }
}
