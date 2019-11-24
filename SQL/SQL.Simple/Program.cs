using System;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace SQL.Simple
{
    class Program
    {
        public static IConfigurationRoot _configuration;

        static async Task Main(string[] args)
        {
            Startup();

            var sql = "SELECT FirstName, LastName FROM SalesLT.Customer";

            using(var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using(var command = new SqlCommand(sql, conn))
                {
                    conn.Open();

                    var reader = await command.ExecuteReaderAsync();

                    while(reader.Read())
                    {
                        Console.WriteLine($"{reader["FirstName"]} {reader["LastName"]}");
                    }

                    conn.Close();
                }
            }
        }

        public static void Startup()
        {
           var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            
            _configuration = builder.Build();
        }
    }
}
