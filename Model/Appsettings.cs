namespace CurrencyConversion.Model
{
    public class AppSettings
    {
        public NationalbankenSettings Nationalbanken { get; set; }
    }

    public class NationalbankenSettings
    {
        public string Endpoint { get; set; }
    }

}
