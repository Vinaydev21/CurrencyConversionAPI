namespace CurrencyConversion.Model
{
    public class CurrencyConversionHistoryDbInputModel
    {
        public string FromCurrency { get; set; }
        public decimal FromAmount { get; set; }
        public decimal ConvertedAmount { get; set; }
        public DateTime ConversionDate { get; set; }
        public decimal FromRate { get; set; }
    }
}
