namespace CurrencyConversion.Data
{
    public static class StoredProcedures
    {
        public const string GetRateByCurrencyCode = "GetRateByCurrencyCode";
        public const string GetAllCurrencyRates = "GetAllCurrencyRates";
        public const string InsertConversionHistory = "InsertConversionHistory";
        public const string UpsertCurrencyRates = "sp_UpsertCurrencyRate"; 
    }
}
