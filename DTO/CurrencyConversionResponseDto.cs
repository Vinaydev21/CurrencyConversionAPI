namespace CurrencyConversion.DTO
{
    public class CurrencyConversionResponseDto
    {
        public string FromCurrency { get; set; }
        public decimal FromAmount { get; set; }
        public decimal ConvertedAmount { get; set; }
        public string ToCurrency => "DKK";
    }
}
