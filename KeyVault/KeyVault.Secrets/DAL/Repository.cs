using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace KeyVault.Secrets.DAL
{
    public class Repository
    {
        private readonly IConfiguration _configuration;

        public Repository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public SqlConnection GetDatabaseConnection(string connectionName)
        {
            return new SqlConnection(_configuration[connectionName]);
        }
    }
}