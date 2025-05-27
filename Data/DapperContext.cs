using Microsoft.Data.SqlClient;
using System.Data;

namespace CurrencyConversion.Data
{
    public class DapperContext : IDapperContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;

            try
            {
                _connectionString = _configuration.GetConnectionString("DefaultConnection");

                if (string.IsNullOrWhiteSpace(_connectionString))
                    throw new ArgumentException("Database connection string is missing or empty.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DapperContext Error] Failed to load connection string: {ex.Message}");
                throw;
            }
        }

        public IDbConnection CreateConnection()
        {
            try
            {
                return new SqlConnection(_connectionString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DapperContext Error] Failed to create DB connection: {ex.Message}");
                throw;
            }
        }
    }
}
