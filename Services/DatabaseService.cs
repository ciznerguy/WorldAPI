using MySql.Data.MySqlClient; 
namespace WorldAPI.Services
{
    public class DatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public MySqlConnection GetConnection() // שינוי ל-MySqlConnection
        {
            return new MySqlConnection(_connectionString); // שימוש ב-MySqlConnection
        }
    }
}
