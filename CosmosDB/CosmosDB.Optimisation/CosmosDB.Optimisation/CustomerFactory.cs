using System;
using System.Collections.Generic;

namespace CosmosDB.Optimisation
{
    public class CustomerFactory
    {
        readonly string[] firstNames = { "Adam", "Sarah", "Richard", "Victoria" };
        readonly string[] lastNames = { "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Rodriguez", "Martinez", "Hernandez", "Lopez", "Gonzales", "Wilson", "Anderson", "Thomas", "Taylor", "Moore", "Jackson", "Martin" };

        public IEnumerable<Customer> Generate(int count)
        {
            var random = new Random();

            var customers = new List<Customer>();

            for (var i = 0; i < count; i++)
            {
                customers.Add(new Customer
                {
                    Id = Guid.NewGuid(),
                    FirstName = firstNames[random.Next(0, firstNames.Length)],
                    LastName = lastNames[random.Next(0, lastNames.Length)]
                });
            }

            return customers;
        }
    }
}