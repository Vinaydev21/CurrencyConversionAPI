using System.Data;

namespace CurrencyConversion.Data
{
    public interface IDapperContext
    {
        IDbConnection CreateConnection();
    }
}
