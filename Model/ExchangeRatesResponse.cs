using System.Xml.Serialization;

namespace CurrencyConversion.Model
{
    [XmlRoot("exchangerates")]
    public class ExchangeRatesResponse
    {
        [XmlElement("dailyrates")]
        public DailyRates DailyRates { get; set; }
    }

    public class DailyRates
    {
        [XmlAttribute("id")]
        public DateTime Date { get; set; }

        [XmlElement("currency")]
        public List<CurrencyRateXml> Currencies { get; set; }
    }

    public class CurrencyRateXml
    {
        [XmlAttribute("code")]
        public string Code { get; set; }

        [XmlAttribute("rate")]
        public string RateRaw { get; set; }
    }
}
