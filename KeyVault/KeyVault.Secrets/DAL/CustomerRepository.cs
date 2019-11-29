using System.Collections.Generic;
using KeyVault.Secrets.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace KeyVault.Secrets.DAL
{
    public class CustomerRepository : Repository
    {
        public CustomerRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public List<Customer> GetCustomers()
        {
            var customers = new List<Customer>();

            var SqlCommand = "SELECT TOP(10) CustomerId, Title, FirstName, MiddleName, LastName, EmailAddress FROM SalesLT.Customer";

            using(var connection = GetDatabaseConnection("AdventureWorksDbConnectionString"))
            {
                using (var command = new SqlCommand(SqlCommand, connection))
                {
                    connection.Open();

                    var reader = command.ExecuteReader();

                    while(reader.Read())
                    {
                        customers.Add(new Customer {
                            CustomerId = (int)reader["CustomerId"],
                            Title = reader["Title"].ToString(),
                            FirstName = reader["FirstName"].ToString(),
                            MiddleName = reader["MiddleName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            EmailAddress = reader["EmailAddress"].ToString(),
                        });
                    }

                    connection.Close();
                }
            }

            return customers;
        }
    }
}