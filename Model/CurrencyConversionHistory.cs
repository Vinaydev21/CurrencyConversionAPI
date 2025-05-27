namespace CurrencyConversion.Model
{
    public class CurrencyConversionHistory
    {
        public int Id { get; set; }
        public string FromCurrency { get; set; }
        public decimal FromAmount { get; set; }
        public decimal ConvertedAmount { get; set; }
        public DateTime ConversionDate { get; set; }
    }
}
