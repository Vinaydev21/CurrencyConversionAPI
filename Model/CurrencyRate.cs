namespace CurrencyConversion.Model
{
    public class CurrencyRate
    {
        public string CurrencyCode { get; set; }
        public decimal Rate { get; set; }
        public DateTime RetrievedDate { get; set; }
    }
}
